using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Mine : MonoBehaviour, IDamagable
{
    [SerializeField] MineRadar _radar;
    [SerializeField] float _timeToTriggerExplosion = 0.1f;
    [SerializeField] float _damageRadius = 2.0f;
    [SerializeField] float _baseDamage = 20;
    [SerializeField] LayerMask _damagedLayers = Physics.AllLayers;
    [SerializeField] LayerMask _ignoredLayers = 1 << 8;

    bool _onActivate;
    float _explosionTime;

    private void Awake()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }
    private void OnEnable() => _radar.OnTargetEnter += MineAcctivate;
    private void OnDisable() => _radar.OnTargetEnter -= MineAcctivate;
    private void OnTriggerEnter(Collider other)
    {
        if (_ignoredLayers == (_ignoredLayers | 1 << other.gameObject.layer)) return;
        MineAcctivate(_timeToTriggerExplosion);
    }

    public void TakeDamage(float damage) => MineAcctivate(_timeToTriggerExplosion);
    void MineAcctivate(float activationTime)
    {
        _explosionTime = Time.time + activationTime;
        _onActivate = true;
    }

    void Update()
    {
        if (!_onActivate) return;

        if (Time.time >= _explosionTime) Explosion();
    }

    void Explosion()
    {
        MakeDamage();
        ShowEffects();
        Destroy();
    }
    void MakeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _damageRadius, _damagedLayers);
        foreach (var item in colliders)
        {
            if (item.TryGetComponent(out IDamagable damagable)) damagable.TakeDamage(_baseDamage);
        }
    }
    void ShowEffects()
    {

    }
    void Destroy() => gameObject.SetActive(false);
}
