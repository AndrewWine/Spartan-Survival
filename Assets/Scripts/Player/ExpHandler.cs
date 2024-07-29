using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ExpHandle : MonoBehaviour
{
    [SerializeField] private GameObject Card;

    public int MaxExp = 100;  // Biến public
    public int currentExp = 0;  // Biến public
    public int surplus;
    public Image expBar;
    public GameObject ExpBar;

    private void Awake()
    {   

        if (PlayerPrefs.HasKey("PlayerExp"))
        {
            currentExp = PlayerPrefs.GetInt("PlayerExp", currentExp); // Nếu không có dữ liệu lưu, sử dụng giá trị tối đa làm mặc định
        }

         if (PlayerPrefs.HasKey("PlayerSurplusExp"))
        {
            surplus = PlayerPrefs.GetInt("PlayerSurplusExp", surplus); // Nếu không có dữ liệu lưu, sử dụng giá trị tối đa làm mặc định
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
        while (currentExp >= MaxExp)
        {
            surplus = currentExp - MaxExp;
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
