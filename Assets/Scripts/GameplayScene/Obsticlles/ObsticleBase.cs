using System;
using UnityEngine;
using UnityEngine.AI;

public class ObstacleBase : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform[] _walkPoints;
    [SerializeField] private DirtType _dirtType;
    [SerializeField] private ObsticleType _type;
    [SerializeField] private float _damage = 5;
    [SerializeField] private float _lifetimeAfterHit = 3;
    [SerializeField] private AudioClip _hitSound;

    private float _hornSpeedMultiplier = 2f;
    private float _defaultSpeed;
    private bool _isHornAffected = true;

    private int _currentPointIndex = 0;
    public bool IsHit = false;
    private Action _onHit;

    public float Damage => _damage;
    public DirtType DirtType => _dirtType;
    public ObsticleType Type => _type;
    public float LifetimeAfterHit => _lifetimeAfterHit;

    void Start()
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        _defaultSpeed = _agent.speed;

        GoToNextPoint();
    }

    void Update()
    {
        if (IsHit) return;

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            GoToNextPoint();
        }
    }

    public void RegisterOnHit(Action onHit)
    {
        _onHit = onHit;
    }
    void GoToNextPoint()
    {
        if (_walkPoints.Length == 0) return;

        _agent.speed = _defaultSpeed;


        _agent.destination = _walkPoints[_currentPointIndex].position;
        _currentPointIndex = (_currentPointIndex + 1) % _walkPoints.Length;
    }

    public void Hit(Vector3 force)
    {
        if (IsHit) return;
        IsHit = true;

        _agent.enabled = false;

        if (_hitSound != null)
        {
            SoundManager.MonoInstance.PlayOnce(_hitSound);
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(force, ForceMode.Impulse);
        }


        _onHit?.Invoke();
        Invoke(nameof(DestroyObject), _lifetimeAfterHit);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    internal void OnHornAffect(Vector3 carPosition)
    {
        if (!_isHornAffected || IsHit) return;
        if (_walkPoints == null || _walkPoints.Length == 0) return;

        _agent.speed = _defaultSpeed * _hornSpeedMultiplier;

        Vector3 awayFromCar = (transform.position - carPosition).normalized;

        float bestDot = -Mathf.Infinity;
        int bestIndex = 0;

        for (int i = 0; i < _walkPoints.Length; i++)
        {
            Vector3 dirToPoint =
                (_walkPoints[i].position - transform.position).normalized;

            float dot = Vector3.Dot(awayFromCar, dirToPoint);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestIndex = i;
            }
        }

        _currentPointIndex = bestIndex;
        _agent.destination = _walkPoints[_currentPointIndex].position;

        // Continue patrol after flee point
        _currentPointIndex = (_currentPointIndex + 1) % _walkPoints.Length;
    }
}


public enum ObsticleType
{
    Buildings,
    People
}
