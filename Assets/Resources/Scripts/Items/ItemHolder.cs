using System.Collections;
using UnityEngine;

[RequireComponent(typeof(OverlapSphere))]
public class ItemHolder : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _itemHolder;
    [SerializeField] private float _dropSpeed;
    [SerializeField] private float _minPullDistance;
    [SerializeField] private float _pullSpeed;

    private Pickupable _currentItem;
    private OverlapSphere _pickupRange;

    private void Start()
    {
        _pickupRange = GetComponent<OverlapSphere>();
    }

    private bool TryPickUp(Pickupable item)
    {
        if (item == null || item.IsPickedUp || _currentItem != null)
            return false;

        item.Pickup();

        StartCoroutine(MoveItemToHolder(item));

        return true;
    }

    private void TryDrop()
    {
        if (_currentItem == null || _currentItem.IsPickedUp == false)
            return;

        _currentItem.Drop(_dropSpeed);
        _currentItem.transform.parent = null;
        _currentItem = null;
    }

    private IEnumerator MoveItemToHolder(Pickupable item)
    {
        float distanceToObject = Vector3.Distance(item.transform.position, _itemHolder.position);

        while(distanceToObject > _minPullDistance)
        {
            Vector3 direction = _itemHolder.position - item.transform.position;
            direction.Normalize();

            item.transform.Translate(direction * _pullSpeed * Time.deltaTime, Space.World);

            distanceToObject = Vector3.Distance(item.transform.position, _itemHolder.position);

            yield return new WaitForEndOfFrame();
        }

        item.transform.parent = _itemHolder;
        item.transform.localPosition = Vector3.zero;

        _currentItem = item;
    }

    private void OverlapTrigger()
    {
        foreach (var other in _pickupRange.GetColliders())
        {
            if (other.TryGetComponent(out Pickupable pickupable))
                if (TryPickUp(pickupable))
                    pickupable.Pickup();

                else if (other.TryGetComponent(out ItemReceiver receiver) && _currentItem != null)
                    if (receiver.IsReceived == false)
                        receiver.TryReceive(_currentItem);
        }
    }

    private void OnEnable()
    {
        _player.Input.Player.Interact.performed += (c) => OverlapTrigger();
        _player.Input.Player.Drop.performed += (c) => TryDrop();
    }

    private void OnDisable()
    {
        _player.Input.Player.Interact.performed -= (c) => OverlapTrigger();
        _player.Input.Player.Drop.performed -= (c) => TryDrop();
    }
}
