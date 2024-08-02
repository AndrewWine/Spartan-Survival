using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordCard : MonoBehaviour
{
    private Button button;
    public SwordAttack damage;
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
        damage = GameObject.Find("SwordHitbox").GetComponent<SwordAttack>();
        button = GetComponent<Button>();
    }
    public void OnButtonClicked()
    {
        IncreaseDamage();
        ExitEvent();
    }

    private void IncreaseDamage()
    {
        if (damage != null)
        {
            damage.IncreaseDamage(1);
        }
    }

    public void ExitEvent()
    {
        Time.timeScale = 1f;
        Card.SetActive(false);

    }
}
