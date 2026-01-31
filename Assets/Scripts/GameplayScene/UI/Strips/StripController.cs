using System;
using System.Collections;
using UnityEngine;

public class StripController : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] _menus;
    [SerializeField] private float _fadeDuration = 2f;

    private int _currentIndex = 0;
    private bool _isTransitioning;
    private Action _onFinish;

    public void StartStrip(Action onFinish)
    {
        _onFinish = onFinish;
        gameObject.SetActive(true);

        // Init all menus hidden
        for (int i = 0; i < _menus.Length; i++)
            SetVisible(_menus[i], i == 0);

        Advance();
    }

    void Update()
    {
        if (_isTransitioning) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            Advance();
        }
    }

    private void Advance()
    {
        _isTransitioning = true;

        CanvasGroup current = _menus[_currentIndex];

        StartCoroutine(FadeMenu(current, false, () =>
        {
            _currentIndex++;

            if (_currentIndex >= _menus.Length)
            {
                _onFinish?.Invoke();
                return;
            }

            CanvasGroup next = _menus[_currentIndex];
            StartCoroutine(FadeMenu(next, true, () =>
            {
                _isTransitioning = false;
            }));
        }));
    }

    private IEnumerator FadeMenu(CanvasGroup menu, bool fadeIn, Action onComplete)
    {
        float time = 0f;
        float start = fadeIn ? 0f : 1f;
        float end = fadeIn ? 1f : 0f;

        menu.alpha = start;
        SetVisible(menu, fadeIn);

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            menu.alpha = Mathf.Lerp(start, end, time / _fadeDuration);
            yield return null;
        }

        menu.alpha = end;
        SetVisible(menu, fadeIn);

        onComplete?.Invoke();
    }

    private void SetVisible(CanvasGroup group, bool visible)
    {
        group.blocksRaycasts = visible;
        group.interactable = visible;
    }
}
