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

    [SerializeField] private GameObject GameVictoryScreen;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float duration = 10f; // Đặt thời gian mặc định là 10 giây
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
        Time.timeScale = 1f; // Đặt lại Time.timeScale
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Nếu đã tồn tại instance khác, hủy đối tượng hiện tại
        }
    }

    private void Start()
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

    void Update()
    {
        if (playerHealth != null && !isGameOver)
        {
            if (playerHealth.health <= 0)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        Debug.Log("Game Over!");
        isGameOver = true; // Đánh dấu rằng game đã over
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Đặt lại Time.timeScale trước khi tải lại cảnh
        SceneManager.LoadSceneAsync("Map1LV1");
    }

    public void DataSave()
    {
        PlayerPrefs.SetFloat("PlayerHealth", playerHealth.health);
        PlayerPrefs.SetFloat("PlayerMaxHealth", playerHealth.maxHealth);
        PlayerPrefs.SetFloat("PlayerAction", playerActions.moveSpeed);
        PlayerPrefs.SetFloat("SwordAttack", SwordDamage.damage);
        PlayerPrefs.SetFloat("PlayerExp", expHandle.currentExp);
        PlayerPrefs.SetFloat("PlayerSurplusExp", expHandle.surplus);
        PlayerPrefs.Save(); // Đảm bảo lưu dữ liệu ngay lập tức
        Debug.Log("Player health saved: " + playerHealth.health);
        Debug.Log("Player speed saved: " + playerActions.moveSpeed);
        Debug.Log("Player damage saved: " + SwordDamage.damage);
        Debug.Log("Player exp saved: " + expHandle.currentExp);
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
