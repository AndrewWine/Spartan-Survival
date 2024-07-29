using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExpDrop : MonoBehaviour
{
    [SerializeField] private int expValue = 10;
    private GameObject player;
    public ExpHandle expHandle;

    private Rigidbody2D rb;

    // Tạo phương thức công khai để lấy giá trị của expValue
    public int GetExpValue()
    {
        return expValue;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && expHandle != null)
        {
            // Tăng điểm kinh nghiệm và phá hủy vật phẩm
            expHandle.PoolExp(expValue);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Null");
        }
        Debug.Log("Player has collected the item");
    }
}
