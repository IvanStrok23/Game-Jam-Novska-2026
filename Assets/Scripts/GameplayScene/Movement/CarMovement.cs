using System;
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

    public CarPartsController partsController;
    private bool _isOn = false;

    private int _currentGear = 1;
    private Rigidbody _rb;
    private float _stopSpeedThreshold = 1f;
    private Quaternion prevRotation;
    private bool _hasStartedMoving = false;
    private PlayerInput _playerInput;


    private Action<float> _onSpeedChange;
    private Action<float> _onDamageReceived;
    private Action _onGameFinish;
    internal void Init(PlayerInput playerInput, Action<float> onSpeedChange, Action<float> onDamageReceived, Action onGameFinish)
    {
        _playerInput = playerInput;
        _onSpeedChange = onSpeedChange;
        _onDamageReceived = onDamageReceived;
        _onGameFinish = onGameFinish;

        _playerInput.RegisterOnHornsKeyDown((keyCode) => partsController.PlayHorns());
        _playerInput.RegisterOnWipersKeyDown((keyCode) => partsController.PlayWipers());
        _playerInput.RegisterOnLightKeyDown((keyCode) => partsController.PlayLights());

        _startPosition = transform.position;
        _startRotation = transform.rotation;

        if (_rb == null)
            _rb = GetComponent<Rigidbody>();

        _startVelocity = Vector3.zero;
        _startAngularVelocity = Vector3.zero;

        _currentGear = 1;
        _hasStartedMoving = false;
        _isOn = false;
        prevRotation = frontLeftWheelTransform.rotation;
        StartCoroutine(DelayedEngineSound());
    }

    public void TurnOn(bool isOn = true) => _isOn = isOn;

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
        if (!_isOn) { return; }

        if (centerOfMassObject)
        {
            _rb.centerOfMass = transform.InverseTransformPoint(centerOfMassObject.transform.position);
        }

        float v = _playerInput.VerticalInput;// * motorForce;
        float h = _playerInput.HorizontalInput;// * maxSteerAngle;

        frontLeftWheel.motorTorque = v;
        frontRightWheel.motorTorque = v;

        frontLeftWheel.steerAngle = h;
        frontRightWheel.steerAngle = h;

        UpdateWheelPoses();

        if (_playerInput.IsBreaking)
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
        if (!_isOn) { return; }

        float v = _playerInput.VerticalInput * motorForce;
        float h = _playerInput.HorizontalInput * maxSteerAngle;

        float currentSpeedKmph = frontLeftWheel.radius * Mathf.PI * frontLeftWheel.rpm * 60f / 1000f;
        float currentRPM = frontLeftWheel.rpm * gearRatios[Mathf.Clamp(_currentGear - 1, 0, gearRatios.Length - 1)];
        _onSpeedChange?.Invoke(currentSpeedKmph);

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obsticle"))
        {
            var obsticle = collision.gameObject.GetComponent<ObstacleBase>();
            if (obsticle != null && !obsticle.IsHit)
            {
                float damageReceived = obsticle.Damage;
                DirtType dirtType = obsticle.DirtType;
                ObsticleType type = obsticle.Type;
                obsticle.Hit(collision.relativeVelocity);
                AbsorbDamage(type, damageReceived);
                partsController.OnGetDirty(dirtType);

                //  if (obsticle)

            }
        }
        //if (collision.gameObject.CompareTag("FinishGame"))
        //{
        //    _onGameFinish?.Invoke();
        //    ResetCar();
        //}
    }


    private bool _isFirstBuildingHit = false;
    private bool _isFirstPeopleHit = false;

    private void AbsorbDamage(ObsticleType type, float damageReceived)
    {
        _onDamageReceived(damageReceived);
        if (type == ObsticleType.Buildings && !_isFirstBuildingHit)
        {
            _isFirstBuildingHit = true;
            SoundManager.MonoInstance.PlayOnFirstBuildingHit();
        }
        else if (!_isFirstPeopleHit)
        {

            _isFirstPeopleHit = true;
            SoundManager.MonoInstance.PlayOnFirstPeopleHit();
        }
    }



    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Vector3 _startVelocity;
    private Vector3 _startAngularVelocity;

    public void ResetCar()
    {
        // Turn off engine & input
        _isOn = false;

        // Reset transform
        transform.position = _startPosition;
        transform.rotation = _startRotation;

        // Reset Rigidbody
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.Sleep();

        // Reset wheels
        ResetWheel(frontLeftWheel);
        ResetWheel(frontRightWheel);
        ResetWheel(rearLeftWheel);
        ResetWheel(rearRightWheel);

        // Reset internal state
        _currentGear = 1;
        _hasStartedMoving = false;

        // Reset brakes
        foreach (WheelCollider wheel in wheelCollidersBrake)
        {
            wheel.brakeTorque = 0f;
        }

        // Reset visuals
        UpdateWheelPoses();
    }

    void ResetWheel(WheelCollider wheel)
    {
        wheel.motorTorque = 0f;
        wheel.steerAngle = 0f;
        wheel.brakeTorque = 0f;
    }
}
