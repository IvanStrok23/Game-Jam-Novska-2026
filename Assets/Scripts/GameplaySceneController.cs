using UnityEngine;

public class GameplaySceneController : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private UIManager _uiManager;


    private void Awake()
    {
        _carMovement.Init(_playerInput);
        _uiManager.Init(_playerInput);
    }
}
