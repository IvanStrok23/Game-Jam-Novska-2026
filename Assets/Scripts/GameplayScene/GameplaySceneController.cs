using UnityEngine;

public class GameplaySceneController : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private UIManager _uiManager;

    private float _playerHealth = 100;
    private bool _isFasterFromPolice;


    private void Awake()
    {
        _carMovement.Init(_playerInput, OnCarSpeedChange, OnCarDamageReceived);
        _uiManager.Init(_playerInput, () => StartGame());
    }

    private void OnCarSpeedChange(float speed)
    {
        bool newIsFaster = speed > 100f;

        // Only react if state changed
        if (newIsFaster != _isFasterFromPolice)
        {
            _isFasterFromPolice = newIsFaster;

            float delta = _isFasterFromPolice ? -0.05f : 0.05f;
            _uiManager.StartPoliceMeter(delta);
        }
    }

    private void OnCarDamageReceived(float damage)
    {
        _playerHealth = _playerHealth - damage;
        _playerHealth = _playerHealth < 0 ? 0 : _playerHealth;

        if (_playerHealth == 0)
        {
            Debug.Log("Brken !!");
        }
    }


    private void StartGame()
    {
        _carMovement.TurnOn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _carMovement.ResetCar();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            _carMovement.TurnOn();
        }


        // Increase value
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnCarSpeedChange(1.1f);
        }

        // Decrease value
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnCarSpeedChange(0.1f);

        }
    }
}
