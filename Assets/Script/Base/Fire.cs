using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public float speed = 10f;
    public AudioSource shootFireSound;

    Rigidbody2D rb;
    private Transform cam;

    void Awake () {
		rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        if (transform.position.y > cam.position.y + 12)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (name!= "Fire" && collision.tag == "Monster")
        {
            Destroy(gameObject);
        }
    }

    public void Shoot () {
        SoundManager.instance.Play(shootFireSound);
        rb.velocity = new Vector3(0, speed, 0);
	}
}