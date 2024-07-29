using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealCard : MonoBehaviour
{
    private Button button;
    public PlayerHealth playerHealth;
    [SerializeField] private GameObject Card;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();

        if (Card == null)
        {
            Debug.LogWarning("Card is not assigned in the Inspector.");
        }
    }

    public void OnButtonClicked()
    {
        HealPlayer();
        ExitEvent();
    }

    private void HealPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.Heal(5);
            playerHealth.maxHealth += 5;
            Debug.Log("Max health increased to: " + playerHealth.maxHealth);
        }
        else
        {
            Debug.LogWarning("PlayerHealth component not found.");
        }
    }

    public void ExitEvent()
    {
        Time.timeScale = 1f;
        if (Card != null)
        {
            Card.SetActive(false);
        }
    }
}
