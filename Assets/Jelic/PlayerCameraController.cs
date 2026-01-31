using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float sensitvity;

    [SerializeField] private Vector2 pitchClamp = new Vector2(-90f, 90f);
    [SerializeField] private Vector2 yawClamp = new Vector2(-90f, 90f);


    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        yaw += sensitvity * Input.GetAxis("Mouse X");
        pitch -= sensitvity * Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, pitchClamp.x, pitchClamp.y);
        yaw = Mathf.Clamp(yaw, yawClamp.x, yawClamp.y);

        transform.localEulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
