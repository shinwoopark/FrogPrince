using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTongueAction : MonoBehaviour
{
    public PlayerMove PlayerMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == 7 ||
                collision.gameObject.layer == 8 ||
                collision.gameObject.layer == 9)
            {
                PlayerMove.HitTongue("platform");
            }
            else if (collision.gameObject.layer == 6)
            {
                //PlayerMove.HitTongue(collision.gameObject.tag);
            }
        }
    }
}