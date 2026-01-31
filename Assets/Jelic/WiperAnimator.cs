using System.Collections;
using UnityEngine;

public class WiperAnimator : MonoBehaviour
{
    [SerializeField] private Transform wiperTransform;
    [SerializeField] private float animationDuration;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float closedRotaion;
    [SerializeField] private float openRotation;
    [SerializeField] private int wipeAmount;

    private Coroutine animationCorutine;


    public void AnimateWiper()
    {
        if (animationCorutine != null)
        {
            StopCoroutine(animationCorutine);
        }
        animationCorutine = StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        for (int j = 0; j < wipeAmount; j++)
        {
            for (float i = 0; i < animationDuration; i += Time.deltaTime)
            {
                float t = i / animationDuration;
                t = curve.Evaluate(t);

                float lerp = Terp(closedRotaion, openRotation, closedRotaion, t);
                wiperTransform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, lerp));
                yield return null;
            }
        }
        wiperTransform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, closedRotaion));
    }
    public float Terp(float a, float b, float c, float t)
    {
        if (t <= 0.5f)
            return Mathf.Lerp(a, b, t * 2f);
        else
            return Mathf.Lerp(b, c, (t - 0.5f) * 2f);
    }
}
