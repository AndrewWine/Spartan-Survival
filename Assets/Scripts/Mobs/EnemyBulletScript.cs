using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float damage = 1;
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 7)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (other.CompareTag("Player"))
        {
           playerHealth.TakeDamage(damage);
           Destroy(gameObject);
            

            // Destroy the bullet after hitting the player
        }
    }
}
