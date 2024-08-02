using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy enemyPrefab; // Prefab của quái vật
    private ObjectPool<Enemy> enemyPool; // Pool để quản lý quái vật
    [SerializeField] protected float spawnInterval = 2.0f; // Thời gian giữa các lần spawn quái vật
    public float SpawnInterval { get => spawnInterval; set => spawnInterval = value; } // Thuộc tính để truy cập spawnInterval từ bên ngoài
    [SerializeField] protected int maxEnemies = 5; // Số lượng quái vật tối đa có thể xuất hiện
    public int MaxEnemies { get => maxEnemies; set => maxEnemies = value; } // Thuộc tính để truy cập maxEnemies từ bên ngoài

    private List<Enemy> activeEnemies = new List<Enemy>(); // Danh sách các quái vật đang hoạt động

    private void Awake()
    {
        CheckenemyPool();
    }


    private void Start()
    {
        StartCoroutine(SpawnEnemiesContinuously()); // Bắt đầu quá trình spawn quái vật liên tục
    }

     private void CheckenemyPool()
    {
        enemyPool = new ObjectPool<Enemy>(enemyPrefab, MaxEnemies); // Khởi tạo pool quái vật
    }
    private IEnumerator SpawnEnemiesContinuously()
    {
        while (true) // Vòng lặp liên tục
        {
            if (activeEnemies.Count < maxEnemies) // Nếu số lượng quái vật đang hoạt động ít hơn số tối đa cho phép
            {
                SpawnEnemy(); // Gọi hàm spawn quái vật
            }
            yield return new WaitForSeconds(SpawnInterval); // Đợi trong khoảng thời gian spawnInterval
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPool != null) // Kiểm tra nếu pool quái vật đã được khởi tạo
        {
            Enemy enemy = enemyPool.Get(); // Lấy một quái vật từ pool
            if (enemy != null) // Nếu quái vật hợp lệ
            {
                // Tạo vị trí ngẫu nhiên trong phạm vi chỉ định
                float randomX = Random.Range(-10f, 10f); // Điều chỉnh phạm vi theo nhu cầu
                float randomY = Random.Range(-5f, 5f);   // Điều chỉnh phạm vi theo nhu cầu
                Vector3 randomPosition = new Vector3(randomX, randomY, 0); // Vị trí ngẫu nhiên

                // Đặt vị trí của quái vật
                enemy.transform.position = randomPosition;

                // Khởi tạo quái vật
                enemy.Initialize(enemyPool, GameObject.FindGameObjectWithTag("Player").transform);

                // Thêm quái vật vào danh sách quái vật đang hoạt động
                activeEnemies.Add(enemy);

                // Kích hoạt quái vật
                enemy.gameObject.SetActive(true);

                // Thêm sự kiện để xóa quái vật khỏi danh sách khi quái vật bị vô hiệu hóa
                enemy.OnDisabled += () => activeEnemies.Remove(enemy);
            }
        }
        else
        {
            Debug.LogError("Pool quái vật chưa được khởi tạo."); // Hiển thị lỗi nếu pool chưa được khởi tạo
        }
    }
}
