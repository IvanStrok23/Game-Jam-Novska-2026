using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _collider;

    public void Hit(Vector3 direction, float force)
    {
        _collider.isTrigger = false;
        _rb.isKinematic = false;
        _rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
