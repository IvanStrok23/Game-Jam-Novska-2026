using System.Collections;
using TMPro;
using UnityEngine;

public class UIButtonIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _keyText;
    [SerializeField] private CanvasGroup canvasGroup;
    private float visibleDuration = 1f;

    private Coroutine _activeRoutine;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowIndicator(KeyCode key = KeyCode.None)
    {
        if (key != KeyCode.None)
            _keyText.text = key.ToString();

        if (_activeRoutine != null)
            StopCoroutine(_activeRoutine);

        _activeRoutine = StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        canvasGroup.alpha = 1f;

        float elapsed = 0f;

        while (elapsed < visibleDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / visibleDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        _activeRoutine = null;
    }

}
