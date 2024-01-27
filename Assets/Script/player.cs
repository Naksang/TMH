using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    
    Rigidbody2D rigid;
    SpriteRenderer sprend;
    Animator anim;

    Vector2 initialPosition;

    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float fallDeath= -6f;


    [SerializeField]
    private int maxJumpCount = 1;
    private float _jumpCount = 0;

    public float JumpCount
    {
        get { return _jumpCount; }
        set {  _jumpCount  = value;
            if (_jumpCount > maxJumpCount) _jumpCount = maxJumpCount;
            else if (_jumpCount <0 ) _jumpCount = 0;
            Debug.Log(_jumpCount);
        }
    }

    [SerializeField]
    private float fallDelay;


    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        initialPosition = transform.position; // save Position

        transform.position = new Vector2(-12, -22);
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(h * speed, rigid.velocity.y);

        if (gameObject.transform.position.y < fallDeath)
            transform.position = initialPosition;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Horizontal")&& !anim.GetBool("isJump"))
        {
            sprend.flipX = Input.GetAxisRaw("Horizontal") > 0;
        }
        else
        {
            sprend.flipX = Input.GetAxisRaw("Horizontal") < 0;
        }

        anim.SetBool("isWalk", Mathf.Abs(rigid.velocity.x) >= 0.1);
        anim.SetBool("isJump", Mathf.Abs(rigid.velocity.y) >= 0.1);
        if (Input.GetButtonDown("Jump") && JumpCount > 0)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            JumpCount -= 1;

            Invoke("ApplyDownwardForce", fallDelay);
        }


    }
    private void ApplyDownwardForce()
    {
        rigid.velocity += Vector2.down * jumpPower;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    jumpCount = 0;
        //}
        if (collision.gameObject.CompareTag("Spike"))
        transform.position = initialPosition;   
    }
     }