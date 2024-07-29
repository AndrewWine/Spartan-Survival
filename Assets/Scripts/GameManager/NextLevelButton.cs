using UnityEngine;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnNextLevelButtonClicked);
    }

    private void OnNextLevelButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NextLevel();
        }
    }
}
