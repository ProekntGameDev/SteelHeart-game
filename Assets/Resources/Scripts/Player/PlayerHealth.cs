using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHealth : Health
{
    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();

        OnDeath.AddListener(() => _player.enabled = false);
    }

    protected override DamageBlockResult TryBlockDamage(Damage damage, Health from) => _player.Combat.TryBlock(damage, from);
}
