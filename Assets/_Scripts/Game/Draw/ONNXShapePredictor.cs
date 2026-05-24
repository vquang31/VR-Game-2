using PDollarGestureRecognizer;
using System.Collections.Generic;
using System.Linq;
using Unity.InferenceEngine;
using UnityEngine;

public class ONNXShapePredictor : MonoBehaviour
{
    [Header("Model")]
    public ModelAsset modelAsset;

    [Header("Input")]
    public Texture2D inputTexture;

    private Worker worker;

    void Start()
    {
        // Load model
        var model = ModelLoader.Load(modelAsset);

        // Create worker (CPU hoặc GPU)
        worker = new Worker(model, BackendType.CPU);
    }

    //string[] labels = { "ellipse", "rectangle", "triangle" };
    private string[] labels = {
        "circle",
        "triangle",
        "z",
        "wave",
        "star",
        "alpha",
        "beta",
        "delta",
        "phi",
        "cronos",
        "omega",
        "zeta",
        "sigma"
    };
// Gọi hàm này để test
[ContextMenu("Run Prediction")]
    public Result RunPrediction(List<Vector2> v2)
    {
        Texture2D texture2D = ConvertToTexture2D(v2);

        inputTexture = texture2D;
        if (inputTexture == null)
        {
            Debug.LogError("Input texture is null!");
            return new Result { GestureClass = "Unknown" };
        }

        // Convert Texture -> Tensor
        Tensor<float> inputTensor = TextureToTensor(inputTexture);

        // Run inference
        worker.Schedule(inputTensor);

        // Get output
        Tensor<float> output = worker.PeekOutput() as Tensor<float>;

        float[] result = output.DownloadToArray();

        // Get index and confidence
        int predictedIndex = 0;
        float maxConfidence = float.MinValue;
        //Debug.Log(result.Length);
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] > maxConfidence)
            {
                maxConfidence = result[i];
                predictedIndex = i;
            }
        }
        Result gestureResult = new Result { GestureClass = labels[predictedIndex].ToString(), Score = maxConfidence };
        Debug.Log($"Predicted class index: {predictedIndex} (confidence: {maxConfidence:0.000})");
        // Dispose
        //if (predictedIndex < labels.Length)
        //{
        //    Debug.Log($"Predicted shape: {labels[predictedIndex]} (confidence: {maxConfidence:0.000})");
        //}
        inputTensor.Dispose();
        output.Dispose();

        return gestureResult;

    }

    //Tensor<float> TextureToTensor(Texture2D texture)
    //{
    //    int width = texture.width;
    //    int height = texture.height;

    //    // Resize nếu model yêu cầu (ví dụ 224x224)
    //    Texture2D resized = Resize(texture, 224, 224);

    //    Color[] pixels = resized.GetPixels();

    //    float[] data = new float[224 * 224 * 3];

    //    for (int i = 0; i < pixels.Length; i++)
    //    {
    //        data[i * 3 + 0] = pixels[i].r;
    //        data[i * 3 + 1] = pixels[i].g;
    //        data[i * 3 + 2] = pixels[i].b;
    //    }

    //    // Shape: (1, H, W, C)
    //    return new Tensor<float>(new TensorShape(1, 224, 224, 3), data);
    //}
    Tensor<float> TextureToTensor(Texture2D texture)
    {
        int width = 224;
        int height = 224;

        Texture2D resized = Resize(texture, width, height);
        Color[] pixels = resized.GetPixels();

        float[] data = new float[1 * 3 * height * width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int pixelIndex = y * width + x;
                Color pixel = pixels[pixelIndex];

                int indexR = 0 * height * width + y * width + x;
                int indexG = 1 * height * width + y * width + x;
                int indexB = 2 * height * width + y * width + x;

                data[indexR] = pixel.r;
                data[indexG] = pixel.g;
                data[indexB] = pixel.b;
            }
        }

        return new Tensor<float>(new TensorShape(1, 3, height, width), data);
    }

    Texture2D Resize(Texture2D source, int width, int height)
    {
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(source, rt);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D result = new Texture2D(width, height);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }


    public Texture2D ConvertToTexture2D(List<Vector2> listPoint, int textureSize = 224)
    {
        Texture2D tex = new Texture2D(textureSize, textureSize, TextureFormat.RGB24, false);

        // Fill background (đen)
        Color bg = Color.black;
        Color drawColor = Color.white;

        Color[] pixels = new Color[textureSize * textureSize];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = bg;

        if (listPoint == null || listPoint.Count < 2)
        {
            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        // ===== 1. Tìm bounding box =====
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (var p in listPoint)
        {
            if (p.x < minX) minX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.x > maxX) maxX = p.x;
            if (p.y > maxY) maxY = p.y;
        }

        float width = maxX - minX;
        float height = maxY - minY;

        // tránh chia 0
        if (width < 1e-5f) width = 1;
        if (height < 1e-5f) height = 1;

        // ===== 2. Scale + padding =====
        float scale = 0.8f * textureSize / Mathf.Max(width, height); // padding 20%
        Vector2 offset = new Vector2(
            (textureSize - width * scale) * 0.5f,
            (textureSize - height * scale) * 0.5f
        );

        // ===== 3. Hàm convert point → pixel =====
        Vector2 ToPixel(Vector2 p)
        {
            float x = (p.x - minX) * scale + offset.x;
            float y = (p.y - minY) * scale + offset.y;
            return new Vector2(x, y);
        }

        // ===== 4. Vẽ line (Bresenham đơn giản) =====
        void DrawLine(Vector2 p0, Vector2 p1)
        {
            int x0 = Mathf.RoundToInt(p0.x);
            int y0 = Mathf.RoundToInt(p0.y);
            int x1 = Mathf.RoundToInt(p1.x);
            int y1 = Mathf.RoundToInt(p1.y);

            int dx = Mathf.Abs(x1 - x0);
            int dy = Mathf.Abs(y1 - y0);

            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;

            int err = dx - dy;

            while (true)
            {
                if (x0 >= 0 && x0 < textureSize && y0 >= 0 && y0 < textureSize)
                {
                    pixels[y0 * textureSize + x0] = drawColor;
                }

                if (x0 == x1 && y0 == y1) break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        // ===== 5. Vẽ toàn bộ stroke =====
        for (int i = 0; i < listPoint.Count - 1; i++)
        {
            Vector2 p0 = ToPixel(listPoint[i]);
            Vector2 p1 = ToPixel(listPoint[i + 1]);

            DrawLine(p0, p1);
        }

        tex.SetPixels(pixels);
        tex.Apply();

        return tex;
    }


    private void OnDestroy()
    {
        worker?.Dispose();
    }
}