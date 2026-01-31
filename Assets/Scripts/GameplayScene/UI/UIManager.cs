using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIKeysPanel _keysPanel;
    [SerializeField] private PoliceMeter _policeMeter;
    [SerializeField] private StripController _startStrip;
    [SerializeField] private StripController _endStrip;

    private PlayerInput _playerInput;
    private Coroutine _policeMeterRoutine;
    private float _currentDelta;

    public void Init(PlayerInput playerInput, System.Action onStartGame)
    {
        _playerInput = playerInput;
        _playerInput.RegisterOnSteeringKeyDown((steeringType) => HandleInputIndicators(steeringType));
        _playerInput.RegisterOnHornsKeyDown((keyCode) => _keysPanel.ShowHornsIndicator(keyCode));
        _playerInput.RegisterOnWipersKeyDown((keyCode) => _keysPanel.ShowWipersIndicator(keyCode));
        _playerInput.RegisterOnLightKeyDown((keyCode) => _keysPanel.ShowLightIndicator(keyCode));
        _startStrip?.StartStrip(() => onStartGame());
    }


    public void StartPoliceMeter(float delta)
    {
        _currentDelta = delta;

        if (_policeMeterRoutine != null)
            StopCoroutine(_policeMeterRoutine);

        _policeMeterRoutine = StartCoroutine(PoliceMeterRoutine());
    }

    private IEnumerator PoliceMeterRoutine()
    {
        while (true)
        {
            _policeMeter.SetValue(_currentDelta);
            yield return new WaitForSeconds(0.5f); // adjust tick speed
        }
    }
    private void HandleInputIndicators(SteeringType steeringType)
    {
        switch (steeringType)
        {
            case SteeringType.Acceleration:
                _keysPanel.ShowAccelerateIndicator();
                break;
            case SteeringType.Reverse:
                _keysPanel.ShowReverseIndicator();
                break;
            case SteeringType.MoveLeft:
                _keysPanel.ShowTurnLeftIndicator();
                break;
            case SteeringType.MoveRight:
                _keysPanel.ShowTurnRightIndicator();
                break;
            case SteeringType.Break:
                _keysPanel.ShowTurnBreakIndicator();
                break;
        }
    }

    internal void OnGameFinish()
    {
        _endStrip.StartStrip(null);
    }
}
