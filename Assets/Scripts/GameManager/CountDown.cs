using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject GameVictoryScreen;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] float duration = 10f; // Đặt thời gian mặc định là 10 giây
    private GameManager gameManager;
    private float currentTime;

    void Start()
    {
        
        currentTime = duration;
        UpdateTimeText();
        gameManager = GameManager.Instance; // Lấy instance của GameManager trước khi bắt đầu coroutine
        StartCoroutine(TimeCountdown());

    }

    void UpdateTimeText()
    {
        if (timeText != null)
        {
            timeText.text = currentTime.ToString();
        }
      
    }

    IEnumerator TimeCountdown()
    {
    
        while (currentTime > 0 && gameManager.isGameOver == false) // Sử dụng IsGameOver từ GameManager
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            UpdateTimeText();
        }
        if(currentTime <= 0)
        {
    
        OpenVictoryScreen();
        }
    }

    void OpenVictoryScreen()
    {
        if (timeText != null)
        {
            timeText.text = "";
        }
        else
        {
            Debug.LogWarning("timeText is not assigned in the Inspector.");
        }

        if (GameVictoryScreen != null)
        {
            GameVictoryScreen.SetActive(true);
            Time.timeScale = 0f; // Dừng toàn bộ hành động trong trò chơi
        }
        else
        {
            Debug.LogWarning("GameVictoryScreen is not assigned in the Inspector.");
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Đặt lại Time.timeScale trước khi tải lại cảnh
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
