using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PoliceMeter : MonoBehaviour
{
    [Header("References")]
    public Slider slider;

    [Header("Smooth Settings")]
    public float moveSpeed = 1.5f;

    [Header("Low Value Tilt")]
    public float tiltThreshold = 0.05f;
    public float tiltAmount = 0.05f;
    public float tiltSpeed = 8f;

    private Coroutine moveCoroutine;
    private float targetValue;

    void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        targetValue = slider.value;
    }

    /// <summary>
    /// Delta-based change (+0.05 / -0.05)
    /// </summary>
    public void SetValue(float delta)
    {
        targetValue = Mathf.Clamp01(targetValue + delta);

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        if (targetValue <= tiltThreshold)
            moveCoroutine = StartCoroutine(MoveToZeroThenTilt());
        else
            moveCoroutine = StartCoroutine(SmoothMoveToTarget());
    }

    IEnumerator SmoothMoveToTarget()
    {
        while (!Mathf.Approximately(slider.value, targetValue))
        {
            slider.value = Mathf.MoveTowards(
                slider.value,
                targetValue,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
    }

    IEnumerator MoveToZeroThenTilt()
    {
        // Step 1: smoothly move to zero
        while (!Mathf.Approximately(slider.value, 0f))
        {
            slider.value = Mathf.MoveTowards(
                slider.value,
                0f,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Step 2: tilt while target stays near zero
        float t = 0f;
        while (targetValue <= tiltThreshold)
        {
            t += Time.deltaTime * tiltSpeed;
            slider.value = Mathf.Lerp(
                0f,
                tiltAmount,
                Mathf.PingPong(t, 1f)
            );
            yield return null;
        }

        // If target increases again, resume normal movement
        moveCoroutine = StartCoroutine(SmoothMoveToTarget());
    }

}
