using System.Collections;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float motorForce = 50f;
    public float maxSteerAngle = 30f;
    public WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;
    public Transform frontLeftWheelTransform, frontRightWheelTransform, rearLeftWheelTransform, rearRightWheelTransform;

    public GameObject centerOfMassObject;
    public bool enable4x4 = false;

    public WheelCollider[] wheelCollidersBrake;
    public float brakeForce = 500f;

    public float[] gearRatios;
    public float shiftThreshold = 5000f;


    private int _currentGear = 1;
    private Rigidbody _rb;
    private float _stopSpeedThreshold = 1f;
    private Quaternion prevRotation;
    private JrsInputController _mobileInputController;
    private bool _hasStartedMoving = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        prevRotation = frontLeftWheelTransform.rotation;
        _mobileInputController = FindFirstObjectByType<JrsInputController>();
        StartCoroutine(DelayedEngineSound());
    }

    IEnumerator DelayedEngineSound()
    {
        while (!_hasStartedMoving)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
    }


    void Update()
    {
        if (centerOfMassObject)
        {
            _rb.centerOfMass = transform.InverseTransformPoint(centerOfMassObject.transform.position);
        }

        float v = _mobileInputController != null ? _mobileInputController.GetVerticalInput() : Input.GetAxis("Vertical") * motorForce;
        float h = _mobileInputController != null ? _mobileInputController.GetHorizontalInput() : Input.GetAxis("Horizontal") * maxSteerAngle;

        // Apply motor torque to the wheels
        frontLeftWheel.motorTorque = v;
        frontRightWheel.motorTorque = v;

        // Apply steering angle to the front wheels
        frontLeftWheel.steerAngle = h;
        frontRightWheel.steerAngle = h;

        // Update wheel poses
        UpdateWheelPoses();

        if (Input.GetKey(KeyCode.Space) || _mobileInputController.brakeButton.IsButtonPressed())
        {
            foreach (WheelCollider wheelCollider in wheelCollidersBrake)
            {
                wheelCollider.brakeTorque = brakeForce;
            }
        }
        else
        {
            foreach (WheelCollider wheelCollider in wheelCollidersBrake)
            {
                wheelCollider.brakeTorque = 0;
            }
        }
    }

    void FixedUpdate()
    {
        float v = _mobileInputController != null ? _mobileInputController.GetVerticalInput() * motorForce : 0f;
        float h = _mobileInputController != null ? _mobileInputController.GetHorizontalInput() * maxSteerAngle : 0f;

        // Calculate the current wheel speed in km/h
        float currentSpeedKmph = frontLeftWheel.radius * Mathf.PI * frontLeftWheel.rpm * 60f / 1000f;
        Debug.Log("Current Speed: " + currentSpeedKmph + " Kmph");

        // Calculate the current engine RPM based on the wheel speed and gear ratio
        float currentRPM = frontLeftWheel.rpm * gearRatios[Mathf.Clamp(_currentGear - 1, 0, gearRatios.Length - 1)];

        // Check if it's time to shift to a higher gear
        if (currentRPM > shiftThreshold && _currentGear < gearRatios.Length)
        {
            _currentGear++; // Shift to the next gear
        }
        else if (currentSpeedKmph < _stopSpeedThreshold && _currentGear > 1)
        {
            _currentGear--; // Shift to the previous gear when slowing down
        }

        // Adjust the motor torque based on the current gear ratio
        float adjustedTorque = v * gearRatios[Mathf.Clamp(_currentGear - 1, 0, gearRatios.Length - 1)];

        // Apply motor torque to the wheels
        if (enable4x4)
        {
            frontLeftWheel.motorTorque = adjustedTorque;
            frontRightWheel.motorTorque = adjustedTorque;
            rearLeftWheel.motorTorque = adjustedTorque;
            rearRightWheel.motorTorque = adjustedTorque;
        }
        else
        {
            frontLeftWheel.motorTorque = adjustedTorque;
            frontRightWheel.motorTorque = adjustedTorque;
            rearLeftWheel.motorTorque = 0f; // No torque applied to rear wheels
            rearRightWheel.motorTorque = 0f; // No torque applied to rear wheels
        }

        frontLeftWheel.steerAngle = h;
        frontRightWheel.steerAngle = h;

        UpdateWheelPoses(); //todo: remove if we dont need wheel rotation

        // Calculate the wheel's angular velocity
        Quaternion currentRotation = frontLeftWheelTransform.rotation;
        float angularVelocity = Quaternion.Angle(prevRotation, currentRotation) / Time.fixedDeltaTime;
        prevRotation = currentRotation;


        if (!_hasStartedMoving && currentSpeedKmph > 0.1f)
        {
            _hasStartedMoving = true;
        }
    }

    void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftWheelTransform);
        UpdateWheelPose(frontRightWheel, frontRightWheelTransform, true);
        UpdateWheelPose(rearLeftWheel, rearLeftWheelTransform);
        UpdateWheelPose(rearRightWheel, rearRightWheelTransform, true);
    }

    void UpdateWheelPose(WheelCollider collider, Transform transform, bool flip = false)
    {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;

        collider.GetWorldPose(out pos, out quat);

        if (flip)
        {
            quat *= Quaternion.Euler(0, 180, 0);
        }

        transform.position = pos;
        transform.rotation = quat;
    }
}
