using System.Collections;
using UnityEngine;

public class NocVjesticaSceneController : MonoBehaviour
{
    private float fadeDuration = 2;
    private bool _isTransitioning = false;

    private void Start()
    {
        SoundManager.MonoInstance.PlayNocVjesticaBackground();
    }

    void Update()
    {
        if (_isTransitioning)
            return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ChangeSceneDelayed(fadeDuration));
            StartCoroutine(PlayBirdDelayed(fadeDuration / 2));
        }
    }

    IEnumerator ChangeSceneDelayed(float delay)
    {
        _isTransitioning = true;
        yield return new WaitForSeconds(delay);
        SceneController.Instance.LoadSceneByIndex(2);
    }

    IEnumerator PlayBirdDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.MonoInstance.PlayBirdSound();
    }

}
