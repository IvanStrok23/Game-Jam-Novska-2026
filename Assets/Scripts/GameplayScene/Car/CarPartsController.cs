using UnityEngine;

public class CarPartsController : MonoBehaviour
{

    public void PlayHorns()
    {
        Debug.Log("Play Horns");
    }

    public void PlayWipers()
    {
        Debug.Log("Play Wipers");
    }

    public void PlayLights()
    {
        Debug.Log("Play Lights");
    }

    public void OnGetDirty(DirtType type)
    {
        Debug.Log("Dirty: " + type);
    }
}

public enum DirtType
{
    None,
    Dirt
}