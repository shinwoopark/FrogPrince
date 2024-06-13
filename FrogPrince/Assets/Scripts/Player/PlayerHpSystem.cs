using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpSystem : MonoBehaviour
{
    private EnemyStateSystem _enemyStateSystem;

    private SpriteRenderer _spriteRenderer;

    public float InvincibilityTime;

    private bool _bNuckBack;
    private float _nuckBackTime;
    private float _nuckBackDir;
    private float _nuckBackPower;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        CoolTimeSystem();
    }

    private void FixedUpdate()
    {
        UpdateNuckBack();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 6:
                
                HpDown();

                ContactPoint2D contact = collision.contacts[0];
                Vector3 hitPos = contact.point;

                if (transform.position.x - hitPos.x > 0)
                {
                    _nuckBackDir = 1;
                }
                if (transform.position.x - hitPos.x < 0)
                {
                    _nuckBackDir = -1;
                }
               
                _enemyStateSystem = collision.collider.GetComponent<EnemyStateSystem>();
                _nuckBackPower = _enemyStateSystem.NuckBackPower;
                _nuckBackTime = 0.1f;
                _bNuckBack = true;
                break;
        }
    }

    private void HpDown()
    {
        GameInstance.instance.CurrentHp -= 1;
        InvincibilityTime = 1;
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        if(GameInstance.instance.CurrentHp > 0)
        {
            _spriteRenderer.color -= new Color(0, 0, 0, 0.75f);
            yield return new WaitForSeconds(0.25f);
            _spriteRenderer.color += new Color(0, 0, 0, 0.4f);
            yield return new WaitForSeconds(0.25f);
            _spriteRenderer.color -= new Color(0, 0, 0, 0.4f);
            yield return new WaitForSeconds(0.25f);
            _spriteRenderer.color += new Color(0, 0, 0, 0.75f);
            yield return new WaitForSeconds(0.25f);
        }     
    }

    private void UpdateNuckBack()
    {
        if (_bNuckBack)
        {
            transform.position += new Vector3(_nuckBackDir * _nuckBackPower * Time.deltaTime, 0, 0);
        }       
    }

    private void CoolTimeSystem()
    {
        if (InvincibilityTime > 0)
        {
            gameObject.layer = 10;
            InvincibilityTime -= Time.deltaTime;
        }
        else if( InvincibilityTime <= 0)
        {
            gameObject.layer = 3;
        }

        if (_bNuckBack)
        {
            _nuckBackTime -= Time.deltaTime;

            if (_nuckBackTime <= 0)
                _bNuckBack = false;
        }
    }
}
