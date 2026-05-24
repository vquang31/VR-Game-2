using PDollarGestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DrawWithRay : NewMonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public LineRenderer linePrefab;

    [Header("Input")]
    public InputActionProperty triggerAction;

    //[SerializeField]
    private LineRenderer currentLine;
    //[SerializeField]
    private List<Vector3> points = new();

    public float minDistance = 0.01f;


    /// <summary>
    /// 
    /// </summary>

    void OnEnable()
    {
        triggerAction.action.Enable();
    }

    void OnDisable()
    {
        triggerAction.action.Disable();
    }

    void Update()
    {
        if(Player.Instance.canDraw == false)
        {
            if (currentLine != null)
                Destroy(currentLine.gameObject);
            currentLine = null;
            return;
        }
        float triggerValue = triggerAction.action.ReadValue<float>();

        if (triggerValue > 0.1f)
        {
            if (currentLine == null)
                StartLine();

            UpdateLine();
        }
        else
        {
            if (currentLine != null)
                EndLine();
        }
    }

    void StartLine()
    {
        //Debug.Log("Start Line");
        if(currentLine != null)
            Destroy(currentLine.gameObject);
        currentLine = Instantiate(linePrefab);
        points.Clear();        
        LineManager.Instance.line = currentLine.gameObject;
        currentLine.gameObject.transform.parent = LineManager.Instance.transform;
    }

    void UpdateLine()
    {
        //Debug.Log("Update Line");
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (!hit.collider.CompareTag("DrawCanvas"))
                return;

            Vector3 point = hit.point;

            if (points.Count == 0 || Vector3.Distance(points[^1], point) > minDistance)
            {
                DrawCanvas.Instance.Drawing(point);
                points.Add(point);
                currentLine.positionCount = points.Count;
                currentLine.SetPositions(points.ToArray());
            }
        }
    }

    void EndLine()
    {
        DrawCanvas.Instance.ResetCanvas();
        //Debug.Log("End Line");

        Result gestureResult = DetectorShape.Instance.DetectShape(currentLine);
        CastStatus ct = Player.Instance.Cast(gestureResult);
        if(ct == CastStatus.Failed)
        {
            SoundFXManager.Instance.PlaySoundFX("reduce", transform);
            UIGameManager.Instance.ShowAnnouncement(ConstUI.announcementFailCast, ConstUI.durationAnnouncement);
        }
        else if(ct == CastStatus.Cooldown)
        {
            SoundFXManager.Instance.PlaySoundFX("buff", transform);
            UIGameManager.Instance.ShowAnnouncement(ConstUI.announcementOnCooldown, ConstUI.durationAnnouncement);
        }
        else if(ct == CastStatus.Success)
        {
            SoundFXManager.Instance.PlaySoundFX("buff", transform);
        }
        StartCoroutine(SpawnVFX(currentLine, ct));
        //Destroy(LineManager.Instance.line);
        if (currentLine != null)
            Destroy(currentLine.gameObject);
        currentLine = null;
    }

    private IEnumerator SpawnVFX(LineRenderer line, CastStatus castStatus)
    {
        if (line == null)
        {
            //Debug.LogWarning("LineRenderer is null. Cannot spawn VFX.");
            yield break;
        }
        List<GameObject> vfxList = new();
        float duration = 0f;

        int step = Mathf.Max(1, line.positionCount / MaxVFXPoints);
        
        for (int i = 0; i < line.positionCount; i += step)
        {
            Dictionary<GameObject,float> vfx = SpawnVFXAtPoint(line.GetPosition(i), castStatus);
            vfxList.Add(vfx.Keys.First());
            duration = vfx.Values.First();
        }
        WaitForSeconds wait = new(duration);
        yield return wait;
        // Despawn VFX
        foreach (GameObject vfx in vfxList)
        {
            ObjectPoolManager.Instance.Despawn(vfx);
        }

    }


    

    private int MaxVFXPoints => 25;


    private Dictionary<CastStatus, Color> castStatusToColor = new()
    {
        { CastStatus.Success, Color.cyan },
        { CastStatus.Failed, Color.red },
        { CastStatus.Cooldown, Color.yellow },
    };


    private Dictionary<GameObject, float> SpawnVFXAtPoint(Vector3 point, CastStatus castStatus)
    {
        // Tạo hiệu ứng tại điểm đã cho
        // Ví dụ: Instantiate(vfxPrefab, point, Quaternion.identity);
        GameObject vfx = ObjectPoolManager.Instance.Spawn("DrawLineVFX");
        ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = castStatusToColor.TryGetValue(castStatus, out var color) ? color : Color.white;
        vfx.transform.position = point;
        float duration = main.duration;
        return new Dictionary<GameObject, float> { { vfx, duration } };
    }
}




