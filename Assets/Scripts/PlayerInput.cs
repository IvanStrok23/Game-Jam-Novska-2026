using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode accelerateKey = KeyCode.W;
    [SerializeField] private KeyCode reverseKey = KeyCode.S;
    [SerializeField] private KeyCode turnLeftKey = KeyCode.A;
    [SerializeField] private KeyCode turnRightKey = KeyCode.D;
    [SerializeField] private KeyCode breaksKey = KeyCode.Space;


    [SerializeField] private KeyCode playWipersKey = KeyCode.Y;
    [SerializeField] private KeyCode playHornKey = KeyCode.U;
    [SerializeField] private KeyCode playLightKey = KeyCode.I;



    public float steerSpeed = 2f;

    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }
    public bool IsBreaking { get; private set; }

    private event Action<SteeringType> _onSteeringKeyDown;
    private event Action<KeyCode> _onWipersKeyDown;
    private event Action<KeyCode> _onHornKeyDown;
    private event Action<KeyCode> _onLightKeyDown;

    public void RegisterOnSteeringKeyDown(Action<SteeringType> onSteeringKeyDown) => _onSteeringKeyDown += onSteeringKeyDown;
    public void RegisterOnWipersKeyDown(Action<KeyCode> onWipersKeyDown) => _onWipersKeyDown += onWipersKeyDown;
    public void RegisterOnHornsKeyDown(Action<KeyCode> onWipersKeyDown) => _onHornKeyDown += onWipersKeyDown;
    public void RegisterOnLightKeyDown(Action<KeyCode> onWipersKeyDown) => _onLightKeyDown += onWipersKeyDown;

    private void Update()
    {
        HandleSteeringInputs();
        HandleTriggerButtons();
    }

    private void HandleSteeringInputs()
    {
        VerticalInput = 0f;

        if (Input.GetKey(accelerateKey))
        {
            _onSteeringKeyDown?.Invoke(SteeringType.Acceleration);
            VerticalInput = 1f;
        }
        else if (Input.GetKey(reverseKey))
        {
            _onSteeringKeyDown?.Invoke(SteeringType.Reverse);
            VerticalInput = -1f;
        }

        float targetHorizontalInput = 0f;

        if (Input.GetKey(turnLeftKey))
        {
            _onSteeringKeyDown?.Invoke(SteeringType.MoveLeft);

            targetHorizontalInput = -1f;
        }
        else if (Input.GetKey(turnRightKey))
        {
            _onSteeringKeyDown?.Invoke(SteeringType.MoveRight);

            targetHorizontalInput = 1f;
        }

        HorizontalInput = Mathf.MoveTowards(HorizontalInput, targetHorizontalInput, steerSpeed * Time.deltaTime);

        if (Input.GetKey(breaksKey))
        {
            _onSteeringKeyDown?.Invoke(SteeringType.Break);
            IsBreaking = true;
        }
        else
        {
            IsBreaking = false;
        }
    }

    private void HandleTriggerButtons()
    {
        if (Input.GetKeyDown(playWipersKey))
        {
            _onWipersKeyDown?.Invoke(playWipersKey);
        }

        if (Input.GetKeyDown(playHornKey))
        {
            _onHornKeyDown?.Invoke(playHornKey);
        }

        if (Input.GetKeyDown(playLightKey))
        {
            _onLightKeyDown?.Invoke(playLightKey);
        }
    }
}

public enum SteeringType
{
    Acceleration,
    Reverse,
    MoveLeft,
    MoveRight,
    Break
}
