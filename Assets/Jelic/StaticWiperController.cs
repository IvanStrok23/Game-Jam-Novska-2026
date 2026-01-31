using UnityEngine;

public class StaticWiperController : MonoBehaviour
{

    [SerializeField] private Transform carTransform;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform rtCamera;

    [SerializeField] private Transform leftWiper;
    [SerializeField] private Transform rightWiper;

    [SerializeField] private Transform leftParticle;
    [SerializeField] private Transform rightParticle;


    private void Update()
    {
        Pose leftWiperPose = GetPose(leftWiper);
        leftParticle.SetPositionAndRotation(leftWiperPose.position, leftWiperPose.rotation);

        Pose rightWiperPose = GetPose(rightWiper);
        rightParticle.SetPositionAndRotation(rightWiperPose.position, rightWiperPose.rotation);
    }

    private void LateUpdate()
    {
        Pose rtPose = GetPose(playerCamera);
        rtCamera.SetPositionAndRotation(rtPose.position, rtPose.rotation);

    }

    private Pose GetPose(Transform relative)
    {
        Vector3 rtPos = carTransform.InverseTransformPoint(relative.position);
        Quaternion rtRot = Quaternion.Inverse(carTransform.rotation) * relative.rotation;


        Pose pose = new Pose()
        {
            position = rtPos,

            rotation = rtRot
        };
        return pose;
    }

}
