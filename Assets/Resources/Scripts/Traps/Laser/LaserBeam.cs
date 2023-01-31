using System.Collections.Generic;
using UnityEngine;


public class LaserBeam
{
    private Vector3 _pos;
    private Vector3 _dir;
    private float _width;
    private GameObject _laserObj;
    public LineRenderer _laser;
    public List<Vector3> _laserIndices = new List<Vector3>();

    private int _baseDamage;

    /// <summary>
    /// Information about the laser and its parameters
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <param name="material"></param>
    /// <param name="width"></param>
    /// <param name="baseDamage"></param> 
    public LaserBeam(Vector3 pos, Vector3 dir, Material material, float width, int baseDamage)
    {
        _laser = new LineRenderer();
        _laserObj = new GameObject();
        _laserObj.name = "LaserBeam";
        _width = width;
        _pos = pos;
        _dir = dir;

        _laser = _laserObj.AddComponent(typeof(LineRenderer)) as LineRenderer;
        _laser.widthMultiplier = _width;
        _laser.material = material;
        _laser.numCapVertices = 10;

        _baseDamage = baseDamage;

        CastRay(_pos, _dir, _laser);
    }


    // We start the beam and read the information
    public void CastRay(Vector3 _pos, Vector3 _dir, LineRenderer _laser)
    {
        _laserIndices.Add(_pos);

        Ray _ray = new Ray(_pos, _dir);
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, 30))
        {
            CheckHit(_hit, _dir, _laser);
        }
        else
        {
            _laserIndices.Add(_ray.GetPoint(30));
            UpdateLaser();
        }

    }

    void UpdateLaser()
    {
        int _count = 0;
        _laser.positionCount = _laserIndices.Count;
        foreach (Vector3 idx in _laserIndices)
        {
            _laser.SetPosition(_count, idx);
            _count++;
        }

    }

    // We check what we have encountered and choose: the beam will be reflected, the beam will cause damage, the beam will find the end point
    void CheckHit(RaycastHit _hitInfo, Vector3 _direction, LineRenderer _laser)
    {
        if (_hitInfo.collider.gameObject.tag == "Mirror")
        {
            Vector3 _pos = _hitInfo.point;
            Vector3 _dir = Vector3.Reflect(_direction, _hitInfo.normal);
            CastRay(_pos, _dir, _laser);
        }
        else if (_hitInfo.collider.TryGetComponent(out IDamagable damagable))
        {
            _laserIndices.Add(_hitInfo.point);
            UpdateLaser();
            damagable.TakeDamage(_baseDamage);  
        }            
        else
        {
            _laserIndices.Add(_hitInfo.point);
            UpdateLaser();
        }
    }

    

}
