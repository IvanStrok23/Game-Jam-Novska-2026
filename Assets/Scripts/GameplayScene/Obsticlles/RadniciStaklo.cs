using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadniciStaklo : MonoBehaviour
{
    [SerializeField] private ObstacleBase obstacle;
    [SerializeField] private List<Apple> glassParts;

    [SerializeField] private List<GameObject> workers;
    [SerializeField] private List<Rigidbody> workerRigids;
    [SerializeField] private List<Collider> workerColliders;
    [SerializeField] private List<Animator> _animators;

    private float forceStrength = 5f;

    private void Awake()
    {
        obstacle.RegisterOnHit(OnHit);
    }

    private void OnHit()
    {
        workers.ForEach(worker => worker.transform.SetParent(null));
        workerRigids.ForEach(rigid => rigid.isKinematic = false);
        workerColliders.ForEach(collider => collider.isTrigger = false);


        if (_animators.Any())
        {
            _animators.ForEach(a => a.enabled = false);
        }


        foreach (Apple apple in glassParts)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            randomDirection.y = Mathf.Abs(randomDirection.y);
            apple.Hit(randomDirection, forceStrength);
        }
        Invoke(nameof(DestroyObject), obstacle.LifetimeAfterHit);
    }

    public void DestroyObject()
    {
        workers.ForEach(worker => Destroy(worker));
    }
}
