using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    Camera _mainCamera;
    public Camera HoodCamera;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _mainCamera.enabled = true;
        HoodCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Camera check");
            if (_mainCamera.enabled)
            {
                Debug.Log("Main Camera enabled");
                SwitchToHoodCamera();
            }
            else if(!_mainCamera.enabled)
            {
                Debug.Log("Main Camera not enabled");
                SwitchToMainCamera();
            }
        }
    }

    void SwitchToHoodCamera()
    {
        HoodCamera.enabled = true;
        _mainCamera.enabled = false;
    }
    void SwitchToMainCamera()
    {
        _mainCamera.enabled = true;
        HoodCamera.enabled = false;
    }
}
