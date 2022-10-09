using UnityEngine;
using Interfaces;

public class Coin : MonoBehaviour, ITriggerableMonoBehaviour
{
    public int worth = 1;

    public void Trigger(Transform obj)
    {
        var _coinHolder = obj.GetComponent<PlayerCoinHolder>();
        if (_coinHolder == null) return;

        _coinHolder.AddCoin(worth);
        gameObject.SetActive(false);
    }
}
