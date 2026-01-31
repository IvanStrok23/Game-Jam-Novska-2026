using System.Collections.Generic;
using UnityEngine;

public class VoceGuy : MonoBehaviour
{
    [SerializeField] private ObstacleBase obstacle;
    [SerializeField] private List<Apple> apples;

    private float forceStrength = 5f;

    private void Awake()
    {
        obstacle.RegisterOnHit(OnHit);
    }

    private void OnHit()
    {
        Debug.Log("HIIIIT");
        foreach (Apple apple in apples)
        {
            if (apple.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Vector3 randomDirection = Random.onUnitSphere;
                randomDirection.y = Mathf.Abs(randomDirection.y);
                apple.Hit(randomDirection, forceStrength);
            }
        }
    }
}
