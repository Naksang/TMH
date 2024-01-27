using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Foot_Triger"))
        {
            anim.SetTrigger("doTrigger");
        }
        else
            anim.SetTrigger("doTrigger");

    }
}
