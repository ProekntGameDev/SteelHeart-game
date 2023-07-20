using UnityEngine;
using Interfaces;

public class Coin : MonoBehaviour, ITriggerableMonoBehaviour
{
    [SerializeField] private int worth = 1;

    public void Trigger(Transform obj)
    {
        var _coinHolder = obj.GetComponent<GearsHolder>();
        if (_coinHolder == null) return;

        _coinHolder.Increase(worth);
        gameObject.SetActive(false);
    }
}
