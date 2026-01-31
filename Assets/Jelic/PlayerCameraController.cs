using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float sensitvity;
    [SerializeField] private Camera _camera;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float zoomedFOV;
    [SerializeField] private float zoomSpeed;


    [SerializeField] private Transform map;
    [SerializeField] private Transform lowerdPosition;
    [SerializeField] private Transform heightendPosition;
    [SerializeField] private float mapLoweredAngle;
    [SerializeField] private float mapRaisedAngle;


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


        float angle01 = Mathf.InverseLerp(mapLoweredAngle, mapRaisedAngle, pitch);

        Vector3 mapPos = Vector3.Lerp(lowerdPosition.position, heightendPosition.position, angle01);
        Quaternion mapRot = Quaternion.Lerp(lowerdPosition.rotation, heightendPosition.rotation, angle01);

        map.SetPositionAndRotation(mapPos, mapRot);


        if (Input.GetMouseButton(0))
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, zoomedFOV, Time.deltaTime * zoomSpeed);
        }
        else
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, defaultFOV, Time.deltaTime * zoomSpeed);
        }

    }
}
