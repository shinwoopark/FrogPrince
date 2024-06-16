using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Yellow : MonoBehaviour
{
    private BulletSystem _bulletSystem;
    private GameObject _player_gb;
    private Transform _player_tr;

    private void Awake()
    {
        _bulletSystem = GetComponent<BulletSystem>();
        _player_gb = GameObject.Find("Player");
        _player_tr = _player_gb.GetComponent<Transform>();
    }

    private void Start()
    {
        _bulletSystem.Dir = _player_tr.position - transform.position + new Vector3(Random.Range(-2.0f, 3.0f), 0, 0);
        _bulletSystem.Dir.Normalize();
        _bulletSystem.SetDir();
    }

    private void Update()
    {
        HitSystem();
    }

    private void HitSystem()
    {
        if (_bulletSystem.bHitPlayer || _bulletSystem.bHitPlatform)
        {
            Destroy(gameObject);
        }
    }
}
