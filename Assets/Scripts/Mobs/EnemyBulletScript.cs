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

    private BulletManager bulletManager; 

    void Start()
    {
        AddComponent(); 
        CheckPlayer();
    }

    void Update()
    {
        CheckRealTime();
        CheckBulletDistance();
        CheckBullet();
    }

    void CheckPlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
        else Debug.LogError("Player not found");
        
    }
    
    void CheckBullet()
    {
        if (bulletManager == null) bulletManager = FindObjectOfType<BulletManager>();
    }
    void AddComponent()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void CheckBulletDistance()
    {
       if (timer >= 7)
        {
            ReturnToPool();
        }
    }
    void CheckRealTime()
    {
        timer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (bulletManager != null)
        {
            bulletManager.ReturnBullet(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
