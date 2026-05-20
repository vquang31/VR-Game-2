using UnityEngine;

public class VFX2D : NewMonoBehaviour
{

    [SerializeField]
    private Camera _mainCamera;

    protected override void LoadComponents()
    {
        _mainCamera = Camera.main;

    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = _mainCamera.transform.position;
        cameraPosition.y = transform.position.y;
        transform.LookAt(cameraPosition);
        transform.Rotate(0, 180f, 0);
    }
}
