using System;
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
        AddComponent();
        CheckButton();
    }

    void CheckButton()
    {
                button.onClick.AddListener(OnButtonClicked);

    }

    
    void AddComponent()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        button = GetComponent<Button>();
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
            playerHealth.GetMaxHealth += 5;
            Debug.Log("Max health increased to: " + playerHealth.GetMaxHealth);
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
