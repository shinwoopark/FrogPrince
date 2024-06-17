using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSystem : MonoBehaviour
{
    public Animator Animator;

    public GameObject Collider;

    [HideInInspector]
    public int Color;

    public void ChooseFlower()
    {
        Color = Random.Range(0, 3);

        switch (Color)
        {
            case 0:
                Animator.SetBool("bGrowRed", true);
                break;
            case 1:
                Animator.SetBool("bGrowYellow", true);
                break;
            case 2:
                Animator.SetBool("bGrowWhite", true);
                break;
        }
    }

    public IEnumerator GrowUpFlower()
    {
        ChooseFlower();
        yield return new WaitForSeconds(1);
        Collider.SetActive(true);
    }

    public void WitherFlower()
    {
        switch (Color)
        {
            case 0:
                Animator.SetBool("bGrowRed", false);
                break;
            case 1:
                Animator.SetBool("bGrowYellow", false);
                break;
            case 2:
                Animator.SetBool("bGrowWhite", false);
                break;
        }

        Collider.SetActive(false);
    }
}
