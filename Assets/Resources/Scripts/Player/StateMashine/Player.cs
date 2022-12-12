using GroundCheck;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [SerializeField] private GroundChecker _groundChecker;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private CapsuleCollider _collider;

    public bool OnGround => _groundChecker.IsGrounded;

    public Rigidbody Rigidbody => _rigidbody;
    public Animator Animator => _animator;
    public CapsuleCollider Collider => _collider;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();
    }
}
