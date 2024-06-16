using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSystem : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public Sprite Red, Yellow, White;

    public GameObject Collider;

    [HideInInspector]
    public int Color;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChooseFlower()
    {
        Color = Random.Range(0, 3);

        //switch(Color)
        //{
        //    case 0:
        //        _spriteRenderer.sprite = Red;
        //        break;
        //    case 1:
        //        _spriteRenderer.sprite = Yellow;
        //        break;
        //    case 2:
        //        _spriteRenderer.sprite = White;
        //        break;
        //}
    }

    public IEnumerator GrowUpFlower()
    {
        ChooseFlower();
        yield return new WaitForSeconds(1);
        Collider.SetActive(true);
    }

    public void WitherFlower()
    {
        Collider.SetActive(false);
    }
}
