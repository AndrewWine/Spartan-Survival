using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject healthText;
    public Image healthBar;
    private Animator animator;
    private PlayerActions playerActions;
    [SerializeField] protected float health = 20;
    public float Gethealth { get { return health; } set { health = value; } }
    [SerializeField] protected bool isDead;
    public bool GetIsDead { get { return isDead; }  }
    [SerializeField] protected float maxHealth = 20;
    public float GetMaxHealth { get { return maxHealth; } set { maxHealth = value; } }

   
    private void Start()
    {
        AddedComponent();
        CheckDataSave();
    }

    void Update()
    {
        CheckGameOverScreen();
    }

    private void CheckGameOverScreen()
    {
        if (gameManager != null && !isDead)
        {
            if (health <= 0)
            {
                playerActions.LockMovement();
                gameManager.GameOver();
                Debug.Log("Game Over!");
                isDead = true;
            }
        }
    }

    private void CheckDataSave()
    {
        // Tải dữ liệu máu khi bắt đầu

        if (PlayerPrefs.HasKey("PlayerMaxHealth"))
        {
            maxHealth = PlayerPrefs.GetFloat("PlayerMaxHealth", GetMaxHealth); // Nếu không có dữ liệu lưu, sử dụng giá trị mặc định
        }
        Gethealth = GetMaxHealth;
        Debug.Log("Player health: " + health);
        healthBar.fillAmount = Gethealth / GetMaxHealth;
    }
     private void AddedComponent()
    {
        animator = GetComponent<Animator>();
        playerActions = GetComponent<PlayerActions>();
    }
    private void StartBeing_Hit()
    {
        animator.SetBool("isMoving", false);
    }

    private void EndBeing_Hit()
    {
        animator.SetBool("isMoving", true);
    }

    public void TakeDamage(float damage)
    {
        animator.SetTrigger("be_Attacked");
        health -= damage;
        healthBar.fillAmount = health / maxHealth;

        RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
        textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        textTransform.SetParent(canvas.transform);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.fillAmount = health / maxHealth;
        Debug.Log("New health: " + health);
    }

    void Die()
    {
        if (playerActions != null)
        {
            playerActions.LockMovement();
        }
        animator.SetBool("isMoving", false);
        animator.SetTrigger("Defeated");
        Debug.Log("Player died.");
        isDead = true;
    }


    public void ResetHp()
    {
        health = 20;  
        maxHealth = 20;     
        PlayerPrefs.DeleteKey("PlayerHealth"); 
        PlayerPrefs.DeleteKey("PlayerMaxHealth"); 
        healthBar.fillAmount = health / maxHealth;
    }
}
