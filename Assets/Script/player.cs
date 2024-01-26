using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    Rigidbody2D rigid;

    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float speed;

    [SerializeField]
    private float fallDelay;

    float jumpCount = 0;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(h * speed, rigid.velocity.y);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount == 0)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
            Invoke("ApplyDownwardForce", fallDelay);
        }
    }
    private void ApplyDownwardForce()
    {
        rigid.velocity += Vector2.down * jumpPower;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}
