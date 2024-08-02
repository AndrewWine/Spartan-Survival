using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public PlayerHealth checkIsDead;
    Animator animator;
    [SerializeField] private int expValue = 10;
    public ExpHandle expHandle;

    SpriteRenderer spriteRenderer;
    Vector2 rightAttackOffset;
    public Collider2D swordCollider;
    public Transform target;
    public GameObject ExpDrop;  // Prefab của đối tượng muốn drop
    public GameObject healthText; // Prefab hiển thị điểm số
    public float initialHealth = 5;

    // Distance
    private float separation;

    // Attributes
    [SerializeField] protected float health = 5;
    public float Gethealth { get => health; }
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
    private ObjectPool<Enemy> pool;

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

    void AddComponent()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockBackHandler = GetComponent<EnemyKnockBack>();
    }

    void CheckPosition()
    {
        rightAttackOffset = transform.localPosition;
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

    public void Initialize(ObjectPool<Enemy> pool, Transform target)
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

    public void OnHit(float damage)
    {
        Health -= damage;
        animator.SetTrigger("beingHit");

        // Instantiate health text and display
        if (healthText != null)
        {
            TMP_Text textTransform = Instantiate(healthText).GetComponent<TMP_Text>();
            textTransform.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            textTransform.transform.SetParent(canvas.transform);
            textTransform.text = damage.ToString();
        }
        else
        {
            Debug.LogWarning("healthText prefab is not assigned.");
        }

        if (knockBackHandler != null)
        {
            knockBackHandler.KnockBack(transform, target, 1f);
        }
        else
        {
            Debug.LogWarning("KnockBackHandler is not assigned.");
        }
    }

    void MobMovement()
    {
        if (target != null)
        {
            separation = Vector2.Distance(transform.position, target.position);

            if (separation > 0.2f && health > 0)
            {
                Vector2 direction = target.position - transform.position;

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

                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    void MobAttack()
    {
        if (separation < 0.2f && health > 0 && Time.time >= GetLastAttackTime + GetAttackCooldown)
        {
            Attack();
            GetLastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        if (target != null)
        {
            PlayerHealth player = target.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
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
