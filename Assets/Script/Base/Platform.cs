using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpCoeff = 1f;
    public AudioSource jump;
    public AudioSource powerup1;
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.rigidbody;
            if (rb != null)
            {
                if(Player.jumpForce > 10)
                    SoundManager.instance.Play(powerup1);
                else
                    SoundManager.instance.Play(jump);
                Vector2 velocity = rb.velocity;
                velocity.y = jumpCoeff * Player.jumpForce;
                rb.velocity = velocity;
            }
        }
    }

    private void Update()
    {
        //Destroy if under the camera
        if (transform.position.y < cam.position.y - 10)
        {
            float duration = 0f;
            AudioSource[] sounds = GetComponentsInChildren<AudioSource>();
            foreach (AudioSource sound in sounds)
            {
                duration = Mathf.Max(duration, sound.clip.length);
            }
            
            Destroy(gameObject, duration);
        }
    }
}
