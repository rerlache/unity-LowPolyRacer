using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 offset;
    public Vector3 eulerRotation;
    public float damper;
    Quaternion rotation;
    bool _mainCameraEnabled;
    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = eulerRotation;
        rotation = transform.rotation * Quaternion.Inverse(Target.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        //_mainCameraEnabled = Camera.main.enabled ? true : false;
        if (Target == null)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, Target.position + offset, damper * Time.time);
        /*if (!_mainCameraEnabled)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Target.transform.rotation * rotation, 0.8f);
        }*/
    }
}
