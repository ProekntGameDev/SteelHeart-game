using UnityEngine;

[RequireComponent(typeof(Stamina))]
public class PlayerBlockAbility : MonoBehaviour
{
    public KeyCode blockKey = KeyCode.Mouse1;


    public bool IsBlocking { get; private set; }

    [SerializeField] private float _blockStaminaSpend = 2f;
    private Stamina _stamina;


    private void Awake()
    {
        _stamina = GetComponent<Stamina>();
    }

    private void Update()
    {
        IsBlocking = Input.GetKey(blockKey) && _stamina.IsSufficient;
    }

    private void FixedUpdate()
    {
        if (IsBlocking == false) return;

        _stamina.DecayFixedTime(_blockStaminaSpend * Time.fixedDeltaTime);
    }
}
