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

    public PlayerInput playerInput;
    public CarPartsController partsController;


    private int _currentGear = 1;
    private Rigidbody _rb;
    private float _stopSpeedThreshold = 1f;
    private Quaternion prevRotation;
    private bool _hasStartedMoving = false;

    void Start()
    {
        playerInput.RegisterOnHornsKeyDown(partsController.PlayHorns);
        playerInput.RegisterOnWipersKeyDown(partsController.PlayWipers);
        playerInput.RegisterOnLightKeyDown(partsController.PlayLights);

        _rb = GetComponent<Rigidbody>();
        prevRotation = frontLeftWheelTransform.rotation;
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

        float v = playerInput.VerticalInput * motorForce;
        float h = playerInput.HorizontalInput * maxSteerAngle;

        frontLeftWheel.motorTorque = v;
        frontRightWheel.motorTorque = v;

        frontLeftWheel.steerAngle = h;
        frontRightWheel.steerAngle = h;

        UpdateWheelPoses();

        if (playerInput.IsBreaking)
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
        float v = playerInput.VerticalInput * motorForce;
        float h = playerInput.HorizontalInput * maxSteerAngle;

        float currentSpeedKmph = frontLeftWheel.radius * Mathf.PI * frontLeftWheel.rpm * 60f / 1000f;
        float currentRPM = frontLeftWheel.rpm * gearRatios[Mathf.Clamp(_currentGear - 1, 0, gearRatios.Length - 1)];

        if (currentRPM > shiftThreshold && _currentGear < gearRatios.Length)
        {
            _currentGear++;
        }
        else if (currentSpeedKmph < _stopSpeedThreshold && _currentGear > 1)
        {
            _currentGear--;
        }

        float adjustedTorque = v * gearRatios[Mathf.Clamp(_currentGear - 1, 0, gearRatios.Length - 1)];

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
