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

    private event Action _onWipersKeyDown;
    private event Action _onHornKeyDown;
    private event Action _onLightKeyDown;

    public void RegisterOnWipersKeyDown(Action onWipersKeyDown) => _onWipersKeyDown += onWipersKeyDown;
    public void RegisterOnHornsKeyDown(Action onWipersKeyDown) => _onHornKeyDown += onWipersKeyDown;
    public void RegisterOnLightKeyDown(Action onWipersKeyDown) => _onLightKeyDown += onWipersKeyDown;

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
            VerticalInput = 1f;
        }
        else if (Input.GetKey(reverseKey))
        {
            VerticalInput = -1f;
        }

        float targetHorizontalInput = 0f;

        if (Input.GetKey(turnLeftKey))
        {
            targetHorizontalInput = -1f;
        }
        else if (Input.GetKey(turnRightKey))
        {
            targetHorizontalInput = 1f;
        }

        HorizontalInput = Mathf.MoveTowards(HorizontalInput, targetHorizontalInput, steerSpeed * Time.deltaTime);
        IsBreaking = Input.GetKey(breaksKey);
    }

    private void HandleTriggerButtons()
    {
        if (Input.GetKeyDown(playWipersKey))
        {
            _onWipersKeyDown?.Invoke();
        }

        if (Input.GetKeyDown(playHornKey))
        {
            _onHornKeyDown?.Invoke();
        }

        if (Input.GetKeyDown(playLightKey))
        {
            _onLightKeyDown?.Invoke();
        }
    }
}
