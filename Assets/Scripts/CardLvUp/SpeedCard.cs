using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedCard : MonoBehaviour
{
    private Button button;
    public PlayerActions playerActions;
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
        playerActions = GameObject.Find("Player").GetComponent<PlayerActions>();
        button = GetComponent<Button>();
    }
    public void OnButtonClicked()
    {
        IncreaseSpeed();
        ExitEvent();
    }

    private void IncreaseSpeed()
    {
        if (playerActions != null)
        {
            playerActions.IncreaseSpeed(0.5f);
        }
    }

    public void ExitEvent()
    {
        Time.timeScale = 1f;
        Card.SetActive(false);

    }
}
