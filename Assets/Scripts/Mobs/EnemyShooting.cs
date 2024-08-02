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
        AddComponent();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance(); 
    }
    void CheckDistance()
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
    void AddComponent()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }   
    void Shoot()
    {
        Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

    }
}
