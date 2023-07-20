using UnityEngine;

public class Card : Pickupable
{
    public int Level => _level;

    [SerializeField, Min(0)] private int _level;
}
