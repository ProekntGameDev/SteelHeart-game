using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using System.Collections;

[RequireComponent(typeof(OverlapSphere))]
public class PlayerInteractor : MonoBehaviour
{
    public Interactable SelectedInteractable { get; private set; }

    [SerializeField] private float _changeSelectionAngle = 25f;
    [SerializeField] private float _changeSelectionMagnitude = 0.005f;
    [SerializeField] private float _changeSelectionDelay = 0.5f;

    [Inject] private Player _player;
    [Inject] private Camera _mainCamera;

    private Coroutine _selectionDelay;
    private OverlapSphere _overlapSphere;

    private void Start()
    {
        _overlapSphere = GetComponent<OverlapSphere>();

        _player.Input.Player.Interact.performed += (context) => Interact();
    }

    private void FixedUpdate()
    {
        List<Interactable> interactables = new List<Interactable>();
        foreach (var collider in _overlapSphere.GetColliders())
            if (collider.TryGetComponent(out Interactable interactable) && interactable.CanInteract)
                interactables.Add(interactable);

        UpdateSelection(interactables);
    }

    private void Interact()
    {
        if (SelectedInteractable == null)
            return;

        SelectedInteractable.Interact();
    }

    private void UpdateSelection(IReadOnlyList<Interactable> interactables)
    {
        if (SelectedInteractable != null && (interactables.Contains(SelectedInteractable) == false || SelectedInteractable.CanInteract == false))
        {
            SetSelected(null);
            return;
        }

        if (SelectedInteractable == null && interactables.Count != 0)
            SetSelected(interactables.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First());

        Vector2 mouseDelta = _player.Input.Player.Mouse.ReadValue<Vector2>();

        if (mouseDelta.magnitude > new Vector2(_changeSelectionMagnitude * Screen.width, _changeSelectionMagnitude * Screen.height).magnitude)
            ChangeSelection(interactables, mouseDelta.normalized);
    }

    private void ChangeSelection(IReadOnlyList<Interactable> interactables, Vector2 mouseDirection)
    {
        if (_selectionDelay != null || interactables.Count == 0)
            return;

        Interactable bestItem = interactables.OrderBy(x => GetAngleToItem(mouseDirection, x)).First();

        if (GetAngleToItem(mouseDirection, bestItem) <= _changeSelectionAngle)
        {
            SetSelected(bestItem);
            _selectionDelay = StartCoroutine(SelectionDelay());
        }
    }

    private void SetSelected(Interactable interactable)
    {
        if (SelectedInteractable != null)
            SelectedInteractable.OnUnselect();

        SelectedInteractable = interactable;

        if (SelectedInteractable != null)
            SelectedInteractable.OnSelect();
    }

    private float GetAngleToItem(Vector2 screenDirection, Interactable item)
    {
        if (SelectedInteractable == item)
            return float.MaxValue;

        Vector2 itemScreenPosition = _mainCamera.WorldToScreenPoint(item.transform.position);
        itemScreenPosition -= new Vector2(_mainCamera.pixelWidth, _mainCamera.pixelHeight) / 2;
        itemScreenPosition.Normalize();

        return Vector2.Angle(screenDirection, itemScreenPosition);
    }

    private IEnumerator SelectionDelay()
    {
        yield return new WaitForSeconds(_changeSelectionDelay);
        _selectionDelay = null;
    }
}
