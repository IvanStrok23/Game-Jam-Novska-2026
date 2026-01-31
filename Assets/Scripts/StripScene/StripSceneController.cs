using System;
using System.Collections;
using UnityEngine;

public class StripSceneController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _firstMenu;
    [SerializeField] private CanvasGroup _secondMenu;

    private float fadeDuration = 2;
    private bool _isTransitioning = false;
    private StripSceneState _state = StripSceneState.FirstMenu;

    void Update()
    {
        if (_isTransitioning)
            return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            _isTransitioning = true;

            if (_state == StripSceneState.FirstMenu)
            {
                StartCoroutine(PlaySoundDelayed(fadeDuration * 0.5f));
                StartCoroutine(FadeMenu(_firstMenu, false, () =>
                {
                    StartCoroutine(FadeMenu(_secondMenu, true, () =>
                    {
                        _isTransitioning = false;
                        _state = StripSceneState.SecondMenu;
                    }));
                }));
            }
            else if (_state == StripSceneState.SecondMenu)
            {
                StartCoroutine(PlaySoundDelayed(fadeDuration * 0.5f));
                StartCoroutine(FadeMenu(_secondMenu, false, () =>
                {
                    SceneController.Instance.LoadSceneByIndex(2);
                }));
            }

        }
    }

    IEnumerator PlaySoundDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.MonoInstance.PlayBirdSound();
    }

    IEnumerator FadeMenu(CanvasGroup menu, bool fadeIn, Action onComplete)
    {
        float time = 0f;

        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        menu.alpha = startAlpha;
        menu.blocksRaycasts = fadeIn;
        menu.interactable = fadeIn;

        while (time < fadeDuration)
        {
            menu.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        menu.alpha = endAlpha;
        menu.blocksRaycasts = fadeIn;
        menu.interactable = fadeIn;

        onComplete?.Invoke();
    }

    private enum StripSceneState
    {
        FirstMenu,
        SecondMenu,
    }
}
