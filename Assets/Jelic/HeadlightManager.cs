using UnityEngine;

public class HeadlightManager : MonoBehaviour
{
    [SerializeField] private GameObject[] headlights;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < headlights.Length; i++)
            {
                headlights[i].SetActive(!headlights[i].activeSelf);
            }
        }
    }
}
