using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float Steer { get; set; }
    public float Throttle { get; set; }
    public bool isSelected { get; set; }
    public static Car Instance { get; private set; }

    public Transform centerOfMass;
    public float motorTorque = 300f;
    public float maxSteer = 20f;

    private Rigidbody _rigidbody;
    private Wheel[] wheels;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        wheels = GetComponentsInChildren<Wheel>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
    }

    void Update()
    {
        if (isSelected)
        {
            Steer = GameManager.Instance.InputController.SteerInput;
            Throttle = GameManager.Instance.InputController.ThrottleInput;
            foreach (var wheel in wheels)
            {
                wheel.SteerAngle = Steer * maxSteer;
                wheel.Torque = Throttle * motorTorque;
            }
        }
    }

    void FixedUpdate()
    {
    }
}
