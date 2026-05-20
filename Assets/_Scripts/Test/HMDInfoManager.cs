using UnityEngine;

public class HMDInfoManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Is Active: " + UnityEngine.XR.XRSettings.isDeviceActive);
        Debug.Log("Device Name: " + UnityEngine.XR.XRSettings.loadedDeviceName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
