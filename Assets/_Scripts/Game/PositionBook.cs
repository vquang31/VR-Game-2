using UnityEngine;

public class PositionBook : NewMonoBehaviour
{

    [SerializeField]
    private float positionY_faceUp= -0.7f;
    [SerializeField]
    private float positionY_faceDown = 0.3f;


    [SerializeField] private float damping = 2.5f;

    private Transform cam;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (Camera.main != null)
            cam = Camera.main.transform;
    }

    private void Update()
    {
        if (cam == null) return;

        // Hướng nhìn của camera
        Vector3 forward = cam.forward;

        // Dot với up để biết đang nhìn lên hay xuống
        float dot = Vector3.Dot(forward, Vector3.up); // [-1, 1]

        // Convert từ [-1,1] -> [0,1]
        float t = (dot + 1f) * 0.5f;

        // Lerp sang khoảng mong muốn
        float targetY = Mathf.Lerp(positionY_faceDown, positionY_faceUp, t);

        // Smooth
        Vector3 localPos = transform.localPosition;
        localPos.y = Mathf.Lerp(localPos.y, targetY, Time.deltaTime * damping);
        transform.localPosition = localPos;
    }
}
 //float cameraRotationX = mainCamera.transform.eulerAngles.x;
        //float newY = transform.position.y;

        //// Normalize angle to [0, 360)
        //cameraRotationX = (cameraRotationX + 360f) % 360f;

        //// Face up: looking down (camera X between 270 and 360 or 0 and 45)
        //if ((cameraRotationX >= 270f && cameraRotationX <= 360f) || (cameraRotationX >= 0f && cameraRotationX <= 45f))
        //{
        //    newY = positionY_faceUp;
        //}
        //// Face down: looking up (camera X between 135 and 225)
        //else if (cameraRotationX >= 135f && cameraRotationX <= 225f)
        //{
        //    newY = positionY_faceDown;
        //}
        //// Normal: otherwise
        //else
        //{
        //    newY = positionY_Normal;
        //}

        //Vector3 pos = transform.localPosition;
        //pos.y = newY;
        //transform.localPosition = pos;