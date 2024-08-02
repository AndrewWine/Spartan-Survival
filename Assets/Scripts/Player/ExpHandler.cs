using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class ExpHandle : MonoBehaviour
{
    [SerializeField] protected GameObject Card;

    [SerializeField] protected float MaxExp = 100;  // Biến public
    public float GetMaxExp { get { return MaxExp; } set { MaxExp = value; } }
    [SerializeField] protected float currentExp = 0;  // Biến public
    public float GetCurrentExp { get { return currentExp; } set { currentExp = value; } }
    [SerializeField] protected float surplus;
    public float GetSurplus { get { return surplus; } set { surplus = value; } }
    public Image expBar;
    public GameObject ExpBar;

    private void Start()
    {   
        CheckDataLoad();
    }

    private void CheckDataLoad()
    {
        if (PlayerPrefs.HasKey("PlayerExp"))
        {
            currentExp = PlayerPrefs.GetFloat("PlayerExp", GetCurrentExp); // Nếu không có dữ liệu lưu, sử dụng giá trị tối đa làm mặc định
        }

         if (PlayerPrefs.HasKey("PlayerSurplusExp"))
        {
            surplus = PlayerPrefs.GetFloat("PlayerSurplusExp", GetSurplus); // Nếu không có dữ liệu lưu, sử dụng giá trị tối đa làm mặc định
        }

        if (Card == null)
        {
            Debug.LogWarning("Card is not assigned in the Inspector.");
        }

        UpdateExpBar();  // Cập nhật thanh exp khi bắt đầu
        if (Card != null)
        {
            Card.SetActive(false);
        }
    }

 


    public void PoolExp(int expValue)
    {
        currentExp += expValue;
        Debug.Log("Current EXP: " + currentExp);
        UpdateExpBar();  // Cập nhật thanh exp mỗi khi cộng exp
        CheckLevelUp();  // Kiểm tra xem có lên cấp hay không
    }

    public void CheckLevelUp()
    {
        while (GetCurrentExp >= GetMaxExp)
        {
            GetSurplus = GetCurrentExp - GetMaxExp;  // Giữ kiểu float
            expBar.fillAmount = 0;  // Reset thanh exp
            currentExp = surplus;
            Debug.Log("Leveled Up! Current EXP: " + currentExp);
            UpdateExpBar();  // Cập nhật thanh exp
            // Gọi hàm hoặc kích hoạt sự kiện lên cấp ở đây
            EventOpenCard();
        }
    }

    private void UpdateExpBar()
    {
        if (expBar != null)
        {
            expBar.fillAmount = (float)currentExp / MaxExp;
        }
    }

    void EventOpenCard()
    {
        if (Card != null)
        {   
            Card.SetActive(true);
            Time.timeScale = 0f; // Dừng toàn bộ hành động trong trò chơi
        }
        else
        {
            Debug.LogWarning("Card is not assigned in the Inspector.");
        }
    }

    public void ResetExp()
    {
        currentExp = 0;  // Reset current exp về 0
        surplus = 0;     // Reset surplus về 0
        PlayerPrefs.DeleteKey("PlayerExp"); // Xóa dữ liệu exp lưu trữ
        PlayerPrefs.DeleteKey("PlayerSurplusExp"); // Xóa dữ liệu surplus lưu trữ
        UpdateExpBar(); // Cập nhật thanh exp sau khi reset
      
    }

}
