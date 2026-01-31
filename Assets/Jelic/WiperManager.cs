using System.Collections;
using UnityEngine;

public class WiperManager : MonoBehaviour
{
    [SerializeField] private Renderer glass;
    [SerializeField] private float startDuration;
    [SerializeField] private float endDuration;
    [SerializeField] private AnimationCurve startCurve;
    [SerializeField] private AnimationCurve stopCurve;

    [SerializeField] private WiperAnimator[] wiperAnimators;
    [SerializeField] private ParticleSystem[] wiperParticles;
    [SerializeField] private DirtyGlassShaderData[] dirtyGlassData;

    [SerializeField] private int test;
    private Coroutine animationCorutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartDirtyGlass(test);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {

            for (int i = 0; i < wiperAnimators.Length; i++)
            {

                wiperAnimators[i].AnimateWiper();
            }
            EndDirtyGlass();

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            EndDirtyGlass();
        }
    }

    private void SetParticleSimulationState(bool shouldSimulate)
    {
        for (int i = 0; i < wiperParticles.Length; i++)
        {
            var main = wiperParticles[i].main;

            if (shouldSimulate)
                main.simulationSpeed = 1;
            else
                main.simulationSpeed = 0;
        }
    }

    public void StartDirtyGlass(int dirtyGlassDataIndex)
    {
        var data = dirtyGlassData[dirtyGlassDataIndex];

        glass.material.SetColor("_DirtColor", data.color);
        glass.material.SetFloat("_NoiseScale", data.noiseScale);

        if (animationCorutine != null)
        {
            StopCoroutine(animationCorutine);
        }
        animationCorutine = StartCoroutine(AnimateCleaning(1, startDuration, startCurve));
    }

    public void EndDirtyGlass()
    {
        if (animationCorutine != null)
        {
            StopCoroutine(animationCorutine);
        }
        animationCorutine = StartCoroutine(AnimateCleaning(0, endDuration, stopCurve));
    }


    private IEnumerator AnimateCleaning(float target, float cleanDuration, AnimationCurve curve)
    {
        float current = glass.material.GetFloat("_Opacity");

        for (float i = 0; i < cleanDuration; i += Time.deltaTime)
        {
            float t = i / cleanDuration;
            t = curve.Evaluate(t);
            float lerp = Mathf.Lerp(current, target, t);
            glass.material.SetFloat("_Opacity", lerp);
            yield return null;
        }
        glass.material.SetFloat("_Opacity", target);
    }


}

[System.Serializable]
public struct DirtyGlassShaderData
{
    public Color color;
    public float drainSpeedMultiplier;
    public float noiseScale;
}