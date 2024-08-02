using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] protected  GameObject gameVictoryScreen;
    [SerializeField] protected  TextMeshProUGUI timeText;
    [SerializeField] protected int duration = 60; // Đặt thời gian mặc định là 60 giây
    public int GetDuration {get { return duration; }}

    private GameManager gameManager;
    private float currentTime;

    void Start()
    {
        SetUpTime();
        UpdateTimeText();
        SetInstance();
        if (gameManager != null)
        {
            StartCoroutine(TimeCountdown());
        }
        else
        {
            Debug.LogError("GameManager instance is not assigned. Coroutine not started.");
        }
    }

    void SetUpTime()
    {
        currentTime = GetDuration;
    }

    void SetInstance()
    {
        gameManager = GameManager.Instance; // Lấy instance của GameManager trước khi bắt đầu coroutine

        if (gameManager == null)
        {
            Debug.LogError("GameManager instance is null. Ensure GameManager is correctly set up.");
        }
    }

    void UpdateTimeText()
    {
        if (timeText != null)
        {
            timeText.text = currentTime.ToString("F2"); // Hiển thị số thập phân 2 chữ số
        }
        else
        {
            Debug.LogWarning("timeText is not assigned in the Inspector.");
        }
    }

    IEnumerator TimeCountdown()
    {
        while (currentTime > 0 && !gameManager.isGameOver)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            UpdateTimeText();
        }

        if (currentTime <= 0)
        {
            OpenVictoryScreen();
        }
    }

    void OpenVictoryScreen()
    {
        if (gameVictoryScreen != null)
        {
            gameVictoryScreen.SetActive(true);
            Time.timeScale = 0f; // Dừng toàn bộ hành động trong trò chơi
        }
        else
        {
            Debug.LogWarning("gameVictoryScreen is not assigned in the Inspector.");
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Đặt lại Time.timeScale trước khi tải lại cảnh
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
