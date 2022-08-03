using UnityEngine;

public class Coin : MonoBehaviour, IInteractableMonoBehaviour
{
    public int worth = 1;

    public void Interact(Transform obj)
    {
        var player = obj.GetComponent<PlayerController>();
        if (player == null) return;

        player.coins += worth;
        if (player.coins >= 100)
        {
            Debug.LogWarning("∆изнь добавл€етс€ при собирании 100 монет, но этот параметр может изменитьс€. " +
                             "¬озможно, стоит вынести его из скрипта коина");
            player.coins %= 100;
            player.additional_lives += 1;
        }

        gameObject.SetActive(false);
    }
}
