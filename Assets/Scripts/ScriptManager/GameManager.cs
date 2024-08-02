using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;
    private float playerHealthValue;
    public PlayerHealth playerHealth;
    public PlayerActions playerActions;
    public SwordAttack SwordDamage;
    public ExpHandle expHandle; // Biến công khai để tham chiếu đến ExpHandle
    public GameObject gameOverUI;
    public bool isGameOver;
    private string currentSceneName;
    [SerializeField] private GameObject GameVictoryScreen;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private int duration = 60; // Đặt thời gian mặc định là 10 giây
    private float currentTime;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        ResetTimeCountdown();
        CheckInstance();
    }

    private void Start()
    {
        CheckGameVictoryCondition();
        CheckSence();

    }

    void Update()
    {
       CheckGameOverCondition();
    }

    void ResetTimeCountdown()
    {
        Time.timeScale = 1f; // Đặt lại Time.timeScale trước khi tải lại cảnh
    }
    void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Nếu đã tồn tại instance khác, hủy đối tượng hiện tại
        }
    }
    void CheckGameVictoryCondition()
    {
        if (GameVictoryScreen != null)
        {
            GameVictoryScreen.SetActive(false);
        }
        isGameOver = false;
        currentTime = duration;
        UpdateTimeText();
        StartCoroutine(TimeCountdown());
    }
    void CheckGameOverCondition()
    {
        if (playerHealth.Gethealth <= 0)
        {
            if (playerHealth != null && !isGameOver)
            {
                if (playerHealth.Gethealth <= 0) GameOver();
            }
        }
    }
    public void GameOver()
    {
        if (gameOverUI != null) gameOverUI.SetActive(true);
        Debug.Log("Game Over!");
        isGameOver = true; // Đánh dấu rằng game đã over
    }

     void CheckSence()
    {
        // Lấy tên của scene hiện tại
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void Restart()
    {
        // Đặt lại Time.timeScale trước khi tải lại cảnh
        Time.timeScale = 1f;
        
        // Cập nhật tên scene hiện tại
        CheckSence();
        
        // Tải lại cảnh hiện tại
        SceneManager.LoadSceneAsync(currentSceneName);
    }

    public void DataSave()
    {
        PlayerPrefs.SetFloat("PlayerMaxHealth", playerHealth.GetMaxHealth);
        PlayerPrefs.SetFloat("PlayerAction", playerActions.GetmoveSpeed);
        PlayerPrefs.SetFloat("SwordAttack", SwordDamage.GetDamage);
        PlayerPrefs.SetFloat("PlayerExp", expHandle.GetCurrentExp);
        PlayerPrefs.SetFloat("PlayerSurplusExp", expHandle.GetSurplus);
        PlayerPrefs.Save(); // Đảm bảo lưu dữ liệu ngay lập tức
        Debug.Log("Player speed saved: " + playerActions.GetmoveSpeed);
        Debug.Log("Player damage saved: " + SwordDamage.GetDamage);
        Debug.Log("Player exp saved: " + expHandle.GetCurrentExp);
    }

    public void NextLevel()
    {
        // Lưu dữ liệu máu của người chơi trước khi chuyển cảnh
        DataSave();
        // Tải cảnh kế tiếp
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayButton()
    {
        expHandle.ResetExp(); // Gọi phương thức ResetExp trên đối tượng expHandle
        playerActions.ResetSpeed(); // Gọi phương thức ResetSpeed trên đối tượng playerActions
        playerHealth.ResetHp(); // Gọi phương thức ResetHealth trên đối tượng playerHealth
        SwordDamage.ResetAttack(); // Gọi phương thức ResetDamage trên đối tượng swordDamage
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    private void UpdateTimeText()
    {
        if (timeText != null)
        {
            timeText.text = currentTime.ToString();
        }
    }

    private IEnumerator TimeCountdown()
    {
        // Đếm ngược thời gian
        while (currentTime > 0 && !isGameOver)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            UpdateTimeText();
        }

        // Kiểm tra lại điều kiện isGameOver trước khi mở màn hình chiến thắng
        if (!isGameOver && currentTime <= 0)
        {
            OpenVictoryScreen();
        }
    }

    private void OpenVictoryScreen()
    {
        if (timeText != null)
        {
            timeText.text = "";
        }
        if (GameVictoryScreen != null)
        {
            GameVictoryScreen.SetActive(true);
            Time.timeScale = 0f; // Dừng toàn bộ hành động trong trò chơi
        }
    }   

    public void InstructionScreenButton()
    {
        SceneManager.LoadSceneAsync("InstructionScreen");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadSceneAsync("Splash Screen");
    }

}


