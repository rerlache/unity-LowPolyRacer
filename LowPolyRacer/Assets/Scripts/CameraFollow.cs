using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Public Variables
    public Transform Target;

    public Vector3 offset;
    public Vector3 eulerRotation;
    public float damper;
    #endregion

    #region Public Properties
    public static CameraFollow Instance { get; private set; }
    #endregion

    #region Private Variables
    Quaternion rotation;
    bool _mainCameraEnabled;
    #endregion

    #region Unity Methods/Functions
    void Awake(){
    }
    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        //_mainCameraEnabled = Camera.main.enabled ? true : false;
        if (Target == null)
        {
            return;
        }
        if (GameManager.Instance.GameState != GameManager.State.PLAY)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, Target.position + offset, damper * Time.time);
        /*if (!_mainCameraEnabled)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Target.transform.rotation * rotation, 0.8f);
        }*/
    }
    #endregion

    #region Custom Methods
    public void SwitchCameraPosition(bool toDefaultPosition)
    {
        if(toDefaultPosition){
            return;
        }
        transform.eulerAngles = eulerRotation;
        rotation = transform.rotation * Quaternion.Inverse(Target.transform.rotation);
    }
    #endregion
}
