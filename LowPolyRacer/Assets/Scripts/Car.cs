using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public string driveMode;
    public Transform centerOfMass;
    public WheelCollider wheelColliderFrontLeft;
    public WheelCollider wheelColliderFrontRight;
    public WheelCollider wheelColliderBackLeft;
    public WheelCollider wheelColliderBackRight;
    public Transform wheelFrontLeft;
    public Transform wheelFrontRight;
    public Transform wheelBackLeft;
    public Transform wheelBackRight;

    public float motorTorque = 300f;
    public float maxSteer = 20f;

    private Rigidbody _rigidbody;
    float frontTorque = 0f;
    float backTorque = 0f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
        SetCarDriveMode();
    }

    void Update()
    {
        MoveWheels();
    }

    void FixedUpdate()
    {
        wheelColliderFrontLeft.motorTorque = wheelColliderFrontRight.motorTorque = Input.GetAxis("Vertical") * frontTorque;
        wheelColliderBackLeft.motorTorque = wheelColliderBackRight.motorTorque = Input.GetAxis("Vertical") * backTorque;

        wheelColliderFrontLeft.steerAngle = wheelColliderFrontRight.steerAngle = Input.GetAxis("Horizontal") * maxSteer;
    }
    void MoveWheels()
    {
        var pos = Vector3.zero;
        var rot = Quaternion.identity;

        wheelColliderFrontLeft.GetWorldPose(out pos, out rot);
        wheelFrontLeft.position = pos;
        wheelFrontLeft.rotation = rot;
        wheelColliderFrontRight.GetWorldPose(out pos, out rot);
        wheelFrontRight.position = pos;
        wheelFrontRight.rotation = rot * Quaternion.Euler(0, 180, 0);
        wheelColliderBackLeft.GetWorldPose(out pos, out rot);
        wheelBackLeft.position = pos;
        wheelBackLeft.rotation = rot;
        wheelColliderBackRight.GetWorldPose(out pos, out rot);
        wheelBackRight.position = pos;
        wheelBackRight.rotation = rot * Quaternion.Euler(0, 180, 0);
    }
    void Steering()
    {

    }
    void SetCarDriveMode()
    {
        switch (driveMode)
        {
            case "4WD":
                frontTorque = backTorque = motorTorque / 1.75f;
                break;
            case "Front":
                frontTorque = motorTorque;
                break;
            case "Back":
                backTorque = motorTorque;
                break;
        }
    }
}
