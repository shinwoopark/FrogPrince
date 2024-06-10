using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpSystem : MonoBehaviour
{
    private bool _invincibility;
    public float InvincibilityTime;

    private void Update()
    {
        UpdateInvincibilityTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!_invincibility)
        {
            if(collision.gameObject.layer == 6)
            {
                HpDown();
            }
            else if (collision.gameObject.layer == 10)
            {
                HpDown();
            }
        }
    }

    private void HpDown()
    {
        GameInstance.instance.CurrentHp -= 1;
        InvincibilityTime = 0.5f;
    }

    private void UpdateInvincibilityTime()
    {
        if (InvincibilityTime > 0)
        {
            _invincibility = true;
            InvincibilityTime -= Time.deltaTime;
        }
        else if( InvincibilityTime <= 0)
        {
            _invincibility = false;
        }
    }
}
