using UnityEngine;

public class Apple : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _collider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private Vector3 _pendingForce;
    private bool _applyForce;

    public void Hit(Vector3 direction, float force)
    {
        _collider.isTrigger = false;
        _rb.isKinematic = false;

        _pendingForce = direction * force;
        _applyForce = true;
    }

    private void FixedUpdate()
    {
        if (_applyForce)
        {
            _rb.AddForce(_pendingForce, ForceMode.Impulse);
            _applyForce = false;
        }
    }
}
