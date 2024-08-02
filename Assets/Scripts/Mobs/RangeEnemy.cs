using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using TMPro;
using System;
using System.Runtime.CompilerServices;

public class RangeEnemy : MonoBehaviour
{
    // Fields for enemy properties and components
    public PlayerHealth checkIsDead;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 rightAttackOffset;
    public Collider2D swordCollider;
    public Transform target;
    public GameObject healthText;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    private GameObject player;
    private float timer;
    [SerializeField] private int expValue = 10;
    public ExpHandle expHandle;

    // Distance
    private float separation;
    public float initialHealth = 5;
    // Attributes
   [SerializeField] protected float health = 5;
    public float Gethealth { get => health; set => health = value; }
    [SerializeField] protected float speed = 1;
    public float GetSpeed { get => speed; }
    [SerializeField] protected float damage = 2;
    public float GetDamage { get => damage; }
    [SerializeField] protected float attackCooldown = 5.5f; // Cooldown time between attacks
    public float GetAttackCooldown { get => attackCooldown; }
    [SerializeField] protected float lastAttackTime;
    public float GetLastAttackTime { get => lastAttackTime; set => lastAttackTime = value; }
    public event Action OnDisabled;

    private IKnockBack knockBackHandler;
    private ObjectPool<RangeEnemy> pool;

    private void Start()
    {
        AddComponent();
        CheckPosition();
        ChecComponent();
        FindTarget();
    }

    private void Update()
    {
        MobMovement();
        MobAttack();     
    }

     public int GetExpValue()
    {
        return expValue;
    }
    
    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    public void Initialize(ObjectPool<RangeEnemy> pool, Transform target)
    {
        this.pool = pool;
        this.target = target;
        ResetHealth();

    }

    private void ResetHealth()
    {
        health = initialHealth;
    }

    private void OnDisable()
    {
        OnDisabled?.Invoke();
    }
    void AddComponent()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
         knockBackHandler = GetComponent<EnemyKnockBack>();
    }

    void ChecComponent()
    {
         // Kiểm tra xem các thành phần đã được gán chưa
        if (animator == null) Debug.LogError("Animator is not assigned.");
      
        if (spriteRenderer == null) Debug.LogError("SpriteRenderer is not assigned.");
        
        if (knockBackHandler == null) Debug.LogError("KnockBackHandler is not assigned.");
    }

    void FindTarget()
    {
        if (target == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }

        if (expHandle == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                expHandle = player.GetComponent<ExpHandle>();
                if (expHandle == null) Debug.LogError("ExpHandle component not found on the Player.");
            }
        }
    }
    void CheckPosition()
    {
        rightAttackOffset = transform.localPosition;
    }
    
     public void OnHit(float damage)
    {
        Health -= damage;
        animator.SetTrigger("beingHit");

        // Instantiate health text và hiển thị
        TMP_Text textTransform = Instantiate(healthText).GetComponent<TMP_Text>();
        textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        textTransform.transform.SetParent(canvas.transform);
        textTransform.text = damage.ToString();

        knockBackHandler.KnockBack(transform, target, 1f);
    }

    // Other methods omitted for brevity
    void MobMovement()
    {
        #region Movement
        if (target != null)
        {
            // Calculate the distance between enemy and player
            separation = Vector2.Distance(transform.position, target.position);

            if (separation > 3 && Gethealth > 0)
            {
                Vector2 direction = target.position - transform.position;

                // Flip the sprite based on the movement direction
                if (direction.x < 0)
                {
                    spriteRenderer.flipX = true;
                    rightAttackOffset = new Vector2(-rightAttackOffset.x, rightAttackOffset.y);
                }
                else if (direction.x > 0)
                {
                    spriteRenderer.flipX = false;
                    rightAttackOffset = new Vector2(Mathf.Abs(rightAttackOffset.x), rightAttackOffset.y);
                }

                // Move the enemy towards the target
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
            #endregion
        }
    }
  
 

    void MobAttack()
    {
         #region Attack
            // Check if the enemy is close enough to attack
            if ( health > 0 && Time.time >= GetLastAttackTime + GetAttackCooldown )
            {
                Attack();
                GetLastAttackTime = Time.time;
            }
            #endregion
    }
    

    private void Attack()
    {
        animator.SetTrigger("Attack");
        if (target != null)
        {   
            Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
          
        }
    }

    public void OnBeingHitAnimationStart()
    {
        animator.SetBool("isMoving", false);
    }

    public void OnBeingHitAnimationEnd()
    {
        animator.SetBool("isMoving", true);
    }

    public void Defeated()
    {
        
        animator.SetTrigger("Defeated");

        if (expHandle != null)
        {
            expHandle.PoolExp(expValue);
        }
        else
        {
            Debug.LogError("ExpHandle is not assigned.");
        }

        StartCoroutine(ReturnToPoolAfterDelay(2f));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (pool != null)
        {
            gameObject.SetActive(false);
            pool.ReturnToPool(this);
            Debug.Log("Enemy returned to pool.");
        }
        else
        {
            Debug.LogError("Pool is not assigned to Enemy.");
        }
    }
}
