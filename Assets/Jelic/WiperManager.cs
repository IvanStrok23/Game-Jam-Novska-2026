using System.Collections;
using UnityEngine;

public class WiperManager : MonoBehaviour
{
    [SerializeField] private Renderer glass;
    [SerializeField] private float cleanDuration;
    [SerializeField] private AnimationCurve curve;
    private float clenliness;
    private Coroutine animationCorutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (animationCorutine != null)
            {
                StopCoroutine(animationCorutine);
            }
            animationCorutine = StartCoroutine(AnimateCleaning());
        }
        glass.material.SetFloat("_WiperMaskStrength", clenliness);
    }

    private IEnumerator AnimateCleaning()
    {
        for (float i = 0; i < cleanDuration; i += Time.deltaTime)
        {
            float t = i / cleanDuration;
            t = curve.Evaluate(t);
            float lerp = Mathf.Lerp(1, 0, t);
            clenliness = lerp;
            yield return null;
        }
        clenliness = 0;
    }


}
