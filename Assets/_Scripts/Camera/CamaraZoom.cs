//using Unity.Cinemachine;
//using UnityEngine;

//public class CamaraZoom : NewMonoBehaviour
//{
//    public CinemachineFollowZoom cinemachineFollowZoom;

//    protected override void LoadComponents()
//    {
//        base.LoadComponents();
//        if (cinemachineFollowZoom == null)
//        {
//            cinemachineFollowZoom = GetComponent<CinemachineFollowZoom>();
//        }
//    }

//    private void Update()
//    {
//        if(Input.mouseScrollDelta.y != 0)
//        {
//            cinemachineFollowZoom.Width += Input.mouseScrollDelta.y * -1; // Zoom in/out
//            cinemachineFollowZoom.Width = Mathf.Clamp(cinemachineFollowZoom.Width, 2f, 10f); // Giới hạn zoom
//        }
//    }

//}
