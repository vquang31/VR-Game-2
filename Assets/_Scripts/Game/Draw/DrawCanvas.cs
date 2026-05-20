using UnityEngine;

public class DrawCanvas : Singleton<DrawCanvas>
{

    public bool isDrawing = false;
    [SerializeField]
    private GameObject cameraGO;

    [SerializeField]
    Vector3 positionCanvas;
    [SerializeField]
    Vector3 rotationCanvas;

    [Header("DrawPointParticleSystem")]
    [SerializeField]
    private ParticleSystem drawPointParticleSystem;

    protected override void Start()
    {
        
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        positionCanvas = transform.localPosition;
        rotationCanvas = transform.localEulerAngles;
        
    }



    public void Drawing(Vector3 point)
    {

        isDrawing = true;
        gameObject.transform.parent = null;
        drawPointParticleSystem.Play();
        drawPointParticleSystem.transform.position = point;
    }

    public void ResetCanvas()
    {
        isDrawing = false;
        drawPointParticleSystem.Stop();
        gameObject.transform.parent = cameraGO.transform;
        transform.localPosition = positionCanvas;
        transform.localEulerAngles = rotationCanvas;

    }



}
