using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHealth : Health
{
    private void Start()
    {
        Player player = GetComponent<Player>();

        OnDeath.AddListener(() => player.enabled = false);
    }
}
