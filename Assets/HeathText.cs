using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IHealthText : MonoBehaviour
{
    public float floatSpeed = 300f;
    public Vector3 floatDirection = new Vector3(0, 1, 0);
    public float timeToLive = 0.5f;

    public TextMeshPro textMesh;

    private RectTransform rectTransform;
    private float timeElapsed = 0.0f;

    internal void SetDamageText(int damage)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeToLive)
        {
            Destroy(gameObject);
            return; // Thoát khỏi hàm Update sau khi hủy đối tượng
        }

        rectTransform.position += floatDirection * (floatSpeed * Time.deltaTime);

        // Thiết lập màu sắc của văn bản
        textMesh.color = new Color(textMesh.color.g, textMesh.color.r, 1 - (timeElapsed / timeToLive));
    }
}
