using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform groundCheck;
    bool isGrounded;
    Animator animation;
    int playerHealth = 5;
    int currentHealth;
    bool isHit = false;

    public Main main;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        currentHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);           
        }
        if(Input.GetAxis("Horizontal") == 0 && ( isGrounded))
        {
            animation.SetInteger("State", 1);
        }
        else
        {
            Flip();
            if (isGrounded)
            {
                animation.SetInteger("State", 2);
            }
        }
        if (isGrounded == false)
        {
            animation.SetInteger("State", 3);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }

    void Flip()
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if ((Input.GetAxis("Horizontal") < 0))
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
    }

    public void RecountHealth(int deltaHP)
    {
        currentHealth = currentHealth + deltaHP;
        if(deltaHP < 0)
        {
            StopCoroutine(OnHit());
            isHit = true;
            StartCoroutine(OnHit());
        }
        if(currentHealth <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.6f);
        }
    }

    IEnumerator OnHit()
    {
        if (isHit)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.05f, GetComponent<SpriteRenderer>().color.b - 0.05f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.05f, GetComponent<SpriteRenderer>().color.b + 0.05f);
        }
        if(GetComponent<SpriteRenderer>().color.g == 1)
        {
            StopCoroutine(OnHit());
        }
        if(GetComponent<SpriteRenderer>().color.g <= 0)
        {
            isHit = false;
        }
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }

    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }
}
