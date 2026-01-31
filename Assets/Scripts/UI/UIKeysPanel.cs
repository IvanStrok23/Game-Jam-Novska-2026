using UnityEngine;

public class UIKeysPanel : MonoBehaviour
{
    [SerializeField] private UIButtonIndicator _accelerationButton;
    [SerializeField] private UIButtonIndicator _reverseButton;
    [SerializeField] private UIButtonIndicator _turnRightButton;
    [SerializeField] private UIButtonIndicator _turnLeftButton;
    [SerializeField] private UIButtonIndicator _breakButton;

    [SerializeField] private UIButtonIndicator _hornsButton;
    [SerializeField] private UIButtonIndicator _wipersButton;
    [SerializeField] private UIButtonIndicator _lightButton;

    internal void ShowAccelerateIndicator() => _accelerationButton.ShowIndicator();
    internal void ShowReverseIndicator() => _reverseButton.ShowIndicator();
    internal void ShowTurnRightIndicator() => _turnRightButton.ShowIndicator();
    internal void ShowTurnLeftIndicator() => _turnLeftButton.ShowIndicator();
    internal void ShowTurnBreakIndicator() => _breakButton.ShowIndicator();

    internal void ShowHornsIndicator(KeyCode keyCode) => _hornsButton.ShowIndicator(keyCode);
    internal void ShowWipersIndicator(KeyCode keyCode) => _wipersButton.ShowIndicator(keyCode);
    internal void ShowLightIndicator(KeyCode keyCode) => _lightButton.ShowIndicator(keyCode);
}
