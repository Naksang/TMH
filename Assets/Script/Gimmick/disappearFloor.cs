using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappearFloor : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigid;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            rigid.gravityScale = 1.0f;
            Debug.Log("Ãæµ¹");
        }
    }
}
