using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public GameObject fire;
    public GameObject stick;
    public AudioSource playerDiedSound;
    public AudioSource catchPowerupSound;

    public static float speed = 10f;
    public static float stableForce = 5f;
    public static float jumpForce;
    public static bool Active = false;
    public static bool powered = false;
    public static bool invincible = false;

    private float movement = 0f;
    private Rigidbody2D rb;
    private ParticleSystem destroyEffect;
    private bool died = false;
    private Transform cam;
    private float edge;
    private float buffer;

    void Awake()
    {
        died = false;
        powered = false;
        invincible = false;
        jumpForce = stableForce;
        edge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0,0)).x;
        buffer = GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        rb = GetComponent<Rigidbody2D>();
        destroyEffect = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        cam = Camera.main.transform;
    }
    
    void Update()
    {
        if (!died && Active)
        {
            movement = Input.acceleration.x * speed * 2;
            //movement = Input.GetAxis("Horizontal") * speed;
            //if (!invincible && Input.GetTouch(0).phase == TouchPhase.Began)
            if (!invincible && Input.GetButtonDown("Fire1"))
            {
                ShootFire();
            }
            if (transform.position.y < cam.position.y - Camera.main.orthographicSize)
            {
                Die();
            }
        }
    }

    private void ShootFire()
    {
        Fire fireClone = Instantiate(fire, fire.transform.position, Quaternion.identity).GetComponent<Fire>();
        fireClone.Shoot();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = movement;
        
        if(!invincible && rb.velocity.y > 15)
        {
            invincible = true;
            velocity.y = jumpForce;
        }

        if (invincible && rb.velocity.y <= 0)
        {
            invincible = false;
        }
        rb.velocity = velocity;
    }

    private void LateUpdate()
    {
        
        if (transform.position.x - buffer > edge)
        {
            Vector3 position = transform.position;
            position.x = -position.x + buffer;
            transform.position = position;
        }
        else if (transform.position.x + buffer < - edge)
        {
            Vector3 position = transform.position;
            position.x = -position.x - buffer;
            transform.position = position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!invincible && collider.tag == "Monster")
        {
            Die();
        }
        else if(!powered && collider.tag == "Powerup")
        {
            Powerup powerup = collider.GetComponent<Powerup>();
            StartCoroutine(Powerup(powerup));
        }
    }

    private IEnumerator Powerup(Powerup powerup) {
        if (powerup.type == 0)
        {
            yield return null;
        }
        else
        {
            SoundManager.instance.Play(catchPowerupSound);
            powered = true;
            stick.SetActive(false);
            powerup.transform.parent = transform;
            powerup.transform.position = stick.transform.position;
            if (powerup.type == 1)
            {
                jumpForce *= powerup.jumpCoeff;
                yield return new WaitForSeconds(powerup.duration);
                jumpForce /= powerup.jumpCoeff;

            }
            else
            {
                SoundManager.instance.Play(powerup.powerupSound);
                rb.isKinematic = true;
                Vector2 velocity = rb.velocity;
                velocity.y = powerup.jumpCoeff * jumpForce;
                rb.velocity = velocity;
                invincible = true;
                powerup.ActiveFire();
                yield return new WaitForSeconds(powerup.duration);
                rb.isKinematic = false;
                invincible = false;
            }
            stick.SetActive(true);
            Destroy(powerup.gameObject);
            powered = false;
        }
    }

    private void Die()
    {
        died = true;
        SetActive(false);
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = false;
        }
        rb.simulated = false;
        destroyEffect.Play();
        SoundManager.instance.Play(playerDiedSound);
        float duration = Mathf.Max(destroyEffect.main.duration, playerDiedSound.clip.length);
        Destroy(gameObject, duration);
        LevelManager.instance.GameOver();
    }

    public static void SetActive(bool active)
    {
        Active = active;
        jumpForce = Active ? speed : stableForce;
    }
}
