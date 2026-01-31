using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIKeysPanel _keysPanel;

    private PlayerInput _playerInput;
    public void Init(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        _playerInput.RegisterOnSteeringKeyDown((steeringType) => HandleInputIndicators(steeringType));
        _playerInput.RegisterOnHornsKeyDown((keyCode) => _keysPanel.ShowHornsIndicator(keyCode));
        _playerInput.RegisterOnWipersKeyDown((keyCode) => _keysPanel.ShowWipersIndicator(keyCode));
        _playerInput.RegisterOnLightKeyDown((keyCode) => _keysPanel.ShowLightIndicator(keyCode));
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
}
