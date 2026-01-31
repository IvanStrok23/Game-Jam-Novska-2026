using System.Collections;
using UnityEngine;

public class SljepaUlicaScene : MonoBehaviour
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
        }
    }

    IEnumerator ChangeSceneDelayed(float delay)
    {
        _isTransitioning = true;
        yield return new WaitForSeconds(delay);
        SceneController.Instance.LoadSceneByIndex(3);
    }

}
