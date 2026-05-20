using UnityEngine;

public class ControllerManager : Singleton<ControllerManager>
{

    public GameObject rightController;
    public GameObject leftController;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        rightController = transform.Find("RightController").gameObject;
        leftController = transform.Find("LeftController").gameObject;

    }

    //public void SwitchHand()
    //{
    //    // Switch left and right hand
    //    // DrawWithRay
    //    // OpenBook, searchBook
    //    DrawWithRay drawWithRayRight = rightController.GetComponent<DrawWithRay>();
    //    DrawWithRay drawWithRayLeft = leftController.GetComponent<DrawWithRay>();
        
    //    // Swap the DrawWithRay components
    //    rightController.GetComponent<DrawWithRay>().enabled = !rightController.GetComponent<DrawWithRay>().enabled;
    //    leftController.GetComponent<DrawWithRay>().enabled = !leftController.GetComponent<DrawWithRay>().enabled;

    public void SwitchHand()
    {
        // Toggle DrawWithRay component enabled state on both controllers
        ToggleDrawWithRay(rightController);
        ToggleDrawWithRay(leftController);
    }

    private void ToggleDrawWithRay(GameObject controller)
    {
        if (controller.TryGetComponent<DrawWithRay>(out var drawWithRay))
        {
            drawWithRay.enabled = !drawWithRay.enabled;
        }
    }
    
}
