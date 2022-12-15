using GroundCheck;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [SerializeField] private GroundChecker _groundChecker;

    public bool OnGround => _groundChecker.IsGrounded;

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public CapsuleCollider Collider { get; private set; }
    public Health Health { get; private set; }

    public void Awake()
    {
        Health = new Health();
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<CapsuleCollider>();
    }
}