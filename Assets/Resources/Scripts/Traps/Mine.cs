using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Mine : MonoBehaviour, IDamagable
{
    [SerializeField] int _maxColliders = 20;
    [SerializeField] float _timeToExplosion = 2f;
    [SerializeField] float _timeToTriggerExplosion = 0.1f;
    [SerializeField] float _activationRadius = 1.5f;
    [SerializeField] float _damageRadius = 2.0f;
    [SerializeField] float _baseDamage = 20;
    [SerializeField] bool _onActivate = false;
    [SerializeField] LayerMask _damagedLayers = Physics.AllLayers;
    [SerializeField] LayerMask _ignoredLayers;

    private void Awake()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }
    private void Start()
    {
        StartCoroutine(ActivationCoroutine());
    }

    private IEnumerator ActivationCoroutine()
    {
        Collider[] colliders = new Collider[_maxColliders];
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
            int numColl = Physics.OverlapSphereNonAlloc(transform.position, _activationRadius, colliders);
            for (int i = 0; i < numColl; i++)
            {
                if (colliders[i].gameObject.TryGetComponent<Player>(out var unit)) _onActivate = true;
            }
            if (_onActivate)
            {
                yield return new WaitForSeconds(_timeToExplosion);
                MineAcctivate();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _timeToExplosion = _timeToTriggerExplosion;
        _onActivate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_ignoredLayers == (_ignoredLayers | 1 << other.gameObject.layer)) return;
        MineAcctivate();
    }

    void MineAcctivate()
    {
        MakeDamage();
        ShowEffects();
        Destroy();
    }

    private void MakeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _damageRadius, _damagedLayers);
        foreach (var item in colliders)
        {
            if (item.TryGetComponent(out IDamagable damagable)) damagable.TakeDamage(_baseDamage);
        }
    }

    private void ShowEffects()
    {

    }

    private void Destroy() => gameObject.SetActive(false);
}
