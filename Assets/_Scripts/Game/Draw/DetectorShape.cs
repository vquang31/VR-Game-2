using PDollarGestureRecognizer;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//[System.Serializable]
//public class DrawData
//{
//    public Vector3[] points;
//}
public class DetectorShape : Singleton<DetectorShape>
{
    private List<Gesture> trainingSet = new();

    public bool useONNXPredictor = true;    

    private ONNXShapePredictor onnxPredictor;
    protected override void LoadComponents()
    {
        base.LoadComponents();

        // my link to the dataset: Assets/_Resources/DataDetect
        //Load pre-made gestures
        //TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("DataDetect");

        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));


        onnxPredictor = GetComponent<ONNXShapePredictor>();
        //Load user custom gestures
        //string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        //foreach (string filePath in filePaths)
        //    trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
    }
    public Result DetectShape(LineRenderer line)
    {
        List<Vector2> v2 = LineRendererToVector2(line);
        // Run prediction
        if(useONNXPredictor)
            return onnxPredictor.RunPrediction(v2);

        ///
        Result gestureResult = new();
        List<Point> points = GetPoints(line);
        if (points.Count <= 2)
        {
            // fail Casting
            Debug.Log("Not enough points to recognize");
            return gestureResult;
        }
        Gesture candidate = new(points.ToArray());
        gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

        //Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);

        return gestureResult;
    }

    private List<Point> GetPoints(LineRenderer line)
    {
        List<Vector2> v2 = LineRendererToVector2(line);
        return ConvertToPoints(v2);
    }

    private List<Vector2> LineRendererToVector2(LineRenderer line)
    {
        List<Vector2> points = new();

        if (line.positionCount < 2)
            return points;

        // Lấy điểm gốc
        Vector3 p0 = line.GetPosition(0);

        // Trường hợp chỉ có 2 điểm → chiếu đơn giản
        if (line.positionCount == 2)
        {
            Vector3 p1 = line.GetPosition(1);
            Vector3 dir = (p1 - p0).normalized;

            for (int i = 0; i < line.positionCount; i++)
            {
                Vector3 p = line.GetPosition(i);
                Vector3 d = p - p0;

                float x = Vector3.Dot(d, dir);
                points.Add(new Vector2(x, 0));
            }

            return points;
        }

        // Lấy 3 điểm để xác định mặt phẳng
        Vector3 p1_ = line.GetPosition(1);
        Vector3 p2_ = line.GetPosition(2);

        // Tạo hệ trục
        Vector3 u = (p1_ - p0).normalized;
        Vector3 normal = Vector3.Cross(p1_ - p0, p2_ - p0).normalized;
        Vector3 v = Vector3.Cross(normal, u).normalized;

        // Chiếu toàn bộ điểm
        for (int i = 0; i < line.positionCount; i++)
        {
            Vector3 p = line.GetPosition(i);
            Vector3 d = p - p0;

            float x = Vector3.Dot(d, u);
            float y = Vector3.Dot(d, v);

            points.Add(new Vector2(x, y));
        }

        return points;
    }


    private List<Point> ConvertToPoints(List<Vector2> v2)
    {
        int count = v2.Count;
        List<Point> points = new();
        for (int i = 0; i < count; i++)
        {
            Vector2 p = v2[i];
            points.Add(new Point(p.x, p.y, 0));
        }
        return points;
    }



    public static Shape LabelToShape(string shape)
    {
        if (string.IsNullOrEmpty(shape))
            return Shape.Unknown;
        return shapeToMagic.TryGetValue(shape.ToLower(), out var magicName) ? magicName : Shape.Unknown;
    }
    public enum Shape
    {
        Circle,
        Triangle,
        Z,
        Star,
        Wave,
        Alpha,
        Beta,
        Delta,
        Phi,
        Cronos,
        Omega,
        Zeta,
        Sigma,
        Unknown
    }

    public static Dictionary<string, Shape> shapeToMagic = new()
    {
        { "circle", Shape.Circle },
        { "triangle", Shape.Triangle },
        { "z", Shape.Z},
        { "wave", Shape.Wave},
        { "star", Shape.Star },
        { "alpha", Shape.Alpha },
        { "beta", Shape.Beta },
        { "delta", Shape.Delta },
        { "phi", Shape.Phi },
        { "cronos", Shape.Cronos },
        { "omega", Shape.Omega },
        { "zeta", Shape.Zeta },
        { "sigma", Shape.Sigma },
    };
    //List<Vector2> GetNormalizedPoints(LineRenderer line)
    //{
    //    int count = line.positionCount;
    //    List<Vector2> points = new();

    //    for (int i = 0; i < count; i++)
    //    {
    //        Vector3 p = line.GetPosition(i);
    //        points.Add(new Vector2(p.x, p.y));
    //    }

    //    // Tìm bounding box
    //    float minX = float.MaxValue, maxX = float.MinValue;
    //    float minY = float.MaxValue, maxY = float.MinValue;

    //    foreach (var p in points)
    //    {
    //        minX = Mathf.Min(minX, p.x);
    //        maxX = Mathf.Max(maxX, p.x);
    //        minY = Mathf.Min(minY, p.y);
    //        maxY = Mathf.Max(maxY, p.y);
    //    }

    //    float width = maxX - minX;
    //    float height = maxY - minY;

    //    // Normalize về [0,1]
    //    for (int i = 0; i < points.Count; i++)
    //    {
    //        points[i] = new Vector2(
    //            (points[i].x - minX) / width,
    //            (points[i].y - minY) / height
    //        );
    //    }

    //    return points;
    //}


    //List<Vector2> Simplify(List<Vector2> points, float epsilon)
    //{
    //    if (points.Count < 3) return points;

    //    int index = -1;
    //    float maxDist = 0;

    //    for (int i = 1; i < points.Count - 1; i++)
    //    {
    //        float dist = PerpendicularDistance(points[i], points[0], points[^1]);
    //        if (dist > maxDist)
    //        {
    //            index = i;
    //            maxDist = dist;
    //        }
    //    }

    //    if (maxDist > epsilon)
    //    {
    //        var left = Simplify(points.GetRange(0, index + 1), epsilon);
    //        var right = Simplify(points.GetRange(index, points.Count - index), epsilon);

    //        left.RemoveAt(left.Count - 1);
    //        left.AddRange(right);
    //        return left;
    //    }

    //    return new List<Vector2> { points[0], points[^1] };
    //}

    //float PerpendicularDistance(Vector2 p, Vector2 a, Vector2 b)
    //{
    //    float area = Mathf.Abs(
    //        a.x * b.y + b.x * p.y + p.x * a.y
    //        - b.x * a.y - p.x * b.y - a.x * p.y
    //    );

    //    float bottom = Vector2.Distance(a, b);
    //    return area / bottom;
    //}

    //public void DetectShape(LineRenderer line)
    //{
    //    var points = GetNormalizedPoints(line);

    //    // simplify
    //    points = Simplify(points, 0.02f);

    //    int cornerCount = points.Count - 1; // bỏ điểm cuối trùng đầu

    //    Debug.Log("Corner count: " + cornerCount);

    //    if (IsCircle(points))
    //    {
    //        Debug.Log("Circle");
    //    }
    //    else if (cornerCount == 3)
    //    {
    //        Debug.Log("Triangle");
    //    }
    //    else if (cornerCount == 4 && IsSquare(points))
    //    {
    //        Debug.Log("Square");
    //    }
    //    else
    //    {
    //        Debug.Log("Unknown shape");
    //    }
    //}
    //bool IsCircle(List<Vector2> points)
    //{
    //    Vector2 center = Vector2.zero;

    //    foreach (var p in points)
    //        center += p;

    //    center /= points.Count;

    //    float avgRadius = 0;
    //    foreach (var p in points)
    //        avgRadius += Vector2.Distance(center, p);

    //    avgRadius /= points.Count;

    //    float variance = 0;
    //    foreach (var p in points)
    //    {
    //        float dist = Vector2.Distance(center, p);
    //        variance += Mathf.Abs(dist - avgRadius);
    //    }

    //    variance /= points.Count;

    //    return variance < 0.05f; // threshold
    //}
    //bool IsSquare(List<Vector2> pts)
    //{
    //    if (pts.Count < 4) return false;

    //    for (int i = 0; i < 4; i++)
    //    {
    //        Vector2 a = pts[i];
    //        Vector2 b = pts[(i + 1) % 4];
    //        Vector2 c = pts[(i + 2) % 4];

    //        Vector2 ab = (b - a).normalized;
    //        Vector2 bc = (c - b).normalized;

    //        float dot = Vector2.Dot(ab, bc);

    //        if (Mathf.Abs(dot) > 0.3f) // ~90 độ
    //            return false;
    //    }

    //    return true;
    //}
}
