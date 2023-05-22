using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public string inputSteeringAxis = "Horizontal";
    public string inputThrottleAxis = "Vertical";
    public float SteerInput { get; private set; }
    public float ThrottleInput { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState == GameManager.State.PLAY)
        {
            SteerInput = Input.GetAxis(inputSteeringAxis);
            ThrottleInput = Input.GetAxis(inputThrottleAxis);
        }
        else
        {
            SteerInput = 0f;
            ThrottleInput = 0f;
        }
    }
}
