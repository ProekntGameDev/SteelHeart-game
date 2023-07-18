using System.Collections;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private Transform _itemHolder;
    [SerializeField] private Vector3 _dropForce;
    [SerializeField] private float _minPullDistance;
    [SerializeField] private float _pullSpeed;

    private PickupableItem _currentItem;

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) == false)
            return;

        if (other.TryGetComponent(out IPickupable pickupable))
            if(TryPickUp(pickupable))
                pickupable.Pickup();

        if (other.TryGetComponent(out ItemReceiver receiver) && _currentItem != null)
            if (receiver.IsReceived == false)
                receiver.ReceiveItem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TryDrop();
        }
    }

    private bool TryPickUp(IPickupable item)
    {
        if (item == null || item.IsPickedUp || _currentItem != null)
            return false;

        Transform itemTransform = item.Pickup();

        StartCoroutine(MoveItemToHolder(itemTransform));

        return true;
    }

    private void TryDrop()
    {
        if (_currentItem == null || _currentItem.IsPickedUp == false)
            return;

        _currentItem.Drop(_dropForce);
        _currentItem = null;
    }

    private IEnumerator MoveItemToHolder(Transform item)
    {
        float distanceToObject = Vector3.Distance(item.transform.position, _itemHolder.position);

        while(distanceToObject > _minPullDistance)
        {
            Vector3 direction = _itemHolder.position - item.transform.position;
            direction.Normalize();

            item.transform.Translate(direction * _pullSpeed * Time.deltaTime);

            distanceToObject = Vector3.Distance(item.transform.position, _itemHolder.position);

            yield return new WaitForEndOfFrame();
        }

        item.transform.parent = _itemHolder;
        item.transform.localPosition = Vector3.zero;
    }
}
