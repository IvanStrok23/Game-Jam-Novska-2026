using System.Collections;
using UnityEngine;

public class KarnevalSceneController : MonoBehaviour
{
    [SerializeField] private Animator bird;
    private float fadeDuration = 4;
    private bool _isTransitioning = false;

    private void Start()
    {
        SoundManager.MonoInstance.PlayKarnevalBackground();
    }

    void Update()
    {
        if (_isTransitioning)
            return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ChangeSceneDelayed(fadeDuration));
            SoundManager.MonoInstance.PlayBirdSound();
            StartCoroutine(PlayBirdDelayed(0.1f));
        }
    }

    IEnumerator ChangeSceneDelayed(float delay)
    {
        _isTransitioning = true;
        yield return new WaitForSeconds(delay);
        SceneController.Instance.LoadSceneByIndex(1);
    }

    IEnumerator PlayBirdDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        bird.enabled = true;
    }

}
