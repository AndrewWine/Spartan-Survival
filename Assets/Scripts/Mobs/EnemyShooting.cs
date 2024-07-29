using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    private GameObject player;
    private float timer;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);   
        Debug.Log(distance);
        if (distance < 10)
        {
            timer += Time.deltaTime;
              if (timer >= 2)
                {
                    Shoot();
                    timer = 0;
                }
        }
      
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

    }
}
