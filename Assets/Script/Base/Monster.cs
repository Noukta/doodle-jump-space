using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public float jumpForce = 12f;
    public int health = 20;
    private int damage = 0;
    private Transform cam;
    private Vector3 originPosition;

    public AudioSource monsterDiedSound;

    private Collider2D collider2d;
    private ParticleSystem destroyEffect;
    private SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        originPosition = transform.position;
        collider2d = GetComponent<Collider2D>();
        destroyEffect = GetComponent<ParticleSystem>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
    
    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        //Destroy if under the camera
        if (transform.position.y < cam.position.y - 10)
        {
            Destroy(gameObject, monsterDiedSound.clip.length);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.rigidbody;
            if (rb != null)
            {
                Vector2 velocity = rb.velocity;
                velocity.y = jumpForce;
                rb.velocity = velocity;
                Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fire")
        {
            damage += 10;
            if (health == damage)
            {
                Die();
            }
        }
        else if (Player.invincible && collision.tag == "Player") {
            Die();
        }
    }
    void LateUpdate()
    {
        transform.position += originPosition;
    }

    void Die()
    {
        collider2d.enabled = false;
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = false;
        }

        destroyEffect.Play();
        SoundManager.instance.Play(monsterDiedSound);
        float duration = Mathf.Max(destroyEffect.main.duration, monsterDiedSound.clip.length);
        Destroy(gameObject, duration);
        LevelManager.instance.score += health;
    }
}
