using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;

    private void Awake()
    {
        ChangeScreenCondition();
    }

    void ChangeScreenCondition()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Đảm bảo ScenesManager không bị hủy khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Nếu đã tồn tại instance khác, hủy đối tượng hiện tại
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
