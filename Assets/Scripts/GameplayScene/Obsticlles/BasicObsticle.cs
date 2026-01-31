using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicObsticle : MonoBehaviour
{
    [SerializeField] private ObstacleBase obstacle;
    [SerializeField] private List<Animator> _animators;


    private void Awake()
    {
        obstacle.RegisterOnHit(OnHit);
    }


    private void OnHit()
    {
        if (_animators.Any())
        {
            _animators.ForEach(a => a.enabled = false);
        }
    }
}
