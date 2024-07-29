using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class healthText : MonoBehaviour
{   
    
    public float timeToLive = 0.5f;
    public float floatSpeed = 300f;
    public Vector3 floatDirection = new Vector3(0, 1, 0);
    public TMP_Text textMesh;
    private RectTransform rTransform;
    private Color initialColor;
    private float timeElapsed = 0.0f;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        rTransform = GetComponent<RectTransform>();
     

        if (textMesh != null)
        {
            initialColor = textMesh.color;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on object!");
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        rTransform.position += floatDirection * (floatSpeed * Time.deltaTime);

        Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1 - (timeElapsed / timeToLive));

        if (textMesh != null)
        {
            textMesh.color = newColor;
        }

        if (timeElapsed >= timeToLive)
        {
            Destroy(gameObject);
        }
    }

  
    
    public void SetDamageText(int damage)
    {
        if (textMesh != null)
        {
            textMesh.text = damage.ToString(); // Hiển thị số lượng damage lên TextMeshPro
        }
    }
}
