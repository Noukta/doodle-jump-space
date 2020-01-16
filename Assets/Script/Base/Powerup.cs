using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    public float duration = 5f;
    public float jumpCoeff = 2f;
    public int type;
    public GameObject Fire;
    public AudioSource powerupSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type==0 && collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.rigidbody;
            if (rb != null)
            {
                SoundManager.instance.Play(powerupSound);
                Vector2 velocity = rb.velocity;
                velocity.y = jumpCoeff * Player.jumpForce;
                rb.velocity = velocity;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type==1 && !Player.powered && collision.tag == "Player")
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    public void ActiveFire()
    {
        Fire.SetActive(true);
    }
}
