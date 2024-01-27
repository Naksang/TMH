using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot_Trigger : MonoBehaviour
{
    [SerializeField]
    private float JumpPower;

    //bool JJump = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jump"))
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Rigidbody2D playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
                if (playerRigidbody != null)
                {
                    playerRigidbody.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
                    //JJump = true;
                }
                else;   
                    //JJump = false;
            }
        }
    }
}
