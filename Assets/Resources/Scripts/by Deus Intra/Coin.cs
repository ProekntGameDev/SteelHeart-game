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
            Debug.LogWarning("����� ����������� ��� ��������� 100 �����, �� ���� �������� ����� ����������. " +
                             "��������, ����� ������� ��� �� ������� �����");
            player.coins %= 100;
            player.additional_lives += 1;
        }

        gameObject.SetActive(false);
    }
}
