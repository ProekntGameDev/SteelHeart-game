using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class RobotVision
{
    [SerializeField] private Transform _eyeTransform;
    [SerializeField] private float _maxDistance = 10f;

    public bool IsVisible() => IsVisible(out Player player);

    public bool IsVisible(out Player player)
    {
        Collider[] colliders = Physics.OverlapSphere(_eyeTransform.position, _maxDistance);

        foreach(var collider in colliders)
            if (collider.TryGetComponent(out player))
                return true;

        player = null;
        return false;
    }

    public void OnGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_eyeTransform.position, _maxDistance);
    }
}
