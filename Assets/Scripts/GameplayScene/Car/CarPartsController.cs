using System.Collections.Generic;
using UnityEngine;

public class CarPartsController : MonoBehaviour
{
    private readonly float hornRadius = 10f;
    [SerializeField] private LayerMask obsticleLayer;

    public void PlayHorns()
    {
        Debug.Log("Play Horns");

        Collider[] hits = Physics.OverlapSphere(transform.position, hornRadius, obsticleLayer);

        HashSet<ObstacleBase> affected = new HashSet<ObstacleBase>();

        foreach (var hit in hits)
        {
            ObstacleBase person = hit.GetComponent<ObstacleBase>();

            person ??= hit.GetComponentInParent<ObstacleBase>();

            if (person != null && affected.Add(person))
            {
                person.OnHornAffect(transform.position);
            }
        }
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hornRadius);
    }
}

public enum DirtType
{
    None,
    Dirt,
    Apples
}