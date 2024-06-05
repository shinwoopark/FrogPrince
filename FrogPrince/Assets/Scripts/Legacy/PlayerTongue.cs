using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTongueAction : MonoBehaviour
{
    public PlayerMove PlayerMove;
    public LayerMask _mask;

    Vector2 _collisionPoint;

    private void Start()
    {
        _collisionPoint = transform.position;

    }

    private void Update()
    {



        if (Input.GetKeyDown(KeyCode.C))
        {
            test();
        }
    }
    public void test()
    {
        //_mask = LayerMask.GetMask("Ceiling");



        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 100f, _mask);
        if (hit.collider != null)
        {
            _collisionPoint = hit.point;
            //PlayerMove.HitTongue("platform", _collisionPoint);
            Debug.Log(hit.collider.gameObject.layer);

        }
        else
        {
            PlayerMove.HitTongue("none", _collisionPoint);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    //    if (collision != null)
    //    {
    //        if (collision.gameObject.layer == 7 ||
    //            collision.gameObject.layer == 8 ||
    //            collision.gameObject.layer == 9)
    //        {
    //            Vector3 collisionPoint = collision.ClosestPoint(transform.position);


    //        }
    //        else if (collision.gameObject.layer == 6)
    //        {
    //            //PlayerMove.HitTongue(collision.gameObject.tag);
    //        }
    //    }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_collisionPoint, 0.5f);
        Gizmos.DrawLine(transform.position, _collisionPoint);
    }
}