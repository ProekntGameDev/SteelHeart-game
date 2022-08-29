using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerRespawnBehaviour : MonoBehaviour
{
    public int additionalLives = 0;
    public Vector2 checkpoint;

    private Health _health;


    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        checkpoint = transform.position;
    }

    private void OnEnable()
    {
        _health.OnDeath += Die;
    }

    private void OnDisable()
    {
        _health.OnDeath -= Die;
    }

    public void Die()
    {
        if (additionalLives > 0)
        {
            additionalLives -= 1;
            Respawn();
        }
        else Debug.Log("Death!");
    }

    public void Respawn()
    {
        transform.position = checkpoint;
        _health.FullHeal();
    }
}
