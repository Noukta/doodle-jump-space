using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : Platform {

    public AudioSource breakPlatformSound;

    private ParticleSystem destroyEffect;
    private SpriteRenderer spriteRenderer;
    private EdgeCollider2D collider2d;

    private void Awake()
    {
        destroyEffect = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<EdgeCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.rigidbody;
            if (rb != null)
            {
                spriteRenderer.enabled = false;
                collider2d.enabled = false;
                destroyEffect.Play();
                SoundManager.instance.Play(breakPlatformSound);
                float duration = Mathf.Max(destroyEffect.main.duration, breakPlatformSound.clip.length);
                Destroy(gameObject, duration);
            }
        }
    }
}
