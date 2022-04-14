using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().RecountHealth(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 7f, ForceMode2D.Impulse);
        }
    }
}
