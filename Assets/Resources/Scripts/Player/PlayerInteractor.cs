using UnityEngine;

[RequireComponent(typeof(OverlapSphere))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Player _player;

    private OverlapSphere _overlapSphere;

    private void Start()
    {
        _overlapSphere = GetComponent<OverlapSphere>();

        _player.Input.Player.Interact.performed += (context) => Interact();
    }

    private void Interact()
    {
        foreach (var collider in _overlapSphere.GetColliders())
            if(collider.TryGetComponent(out IInteractable interactable))
                interactable.Interact();
    }
}
