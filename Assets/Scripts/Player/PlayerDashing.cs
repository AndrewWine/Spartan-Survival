using System.Collections;
using UnityEngine;
public class PlayerDashing : MonoBehaviour
{
       // Các thuộc tính
    [SerializeField] public float moveSpeed = 1f;
    bool isDashing;
    bool CanDash;
    // Các thuộc tính Dash
    [SerializeField] protected float dashSpeed = 0.002f; // Tốc độ khi dash
    [SerializeField] protected  float dashDuration = 0.2f; // Thời gian dash
    [SerializeField] protected  float dashCooldown = 1f; // Thời gian hồi chiêu giữa các lần dash
     [SerializeField] protected Vector2 dashDirection;

    public Vector2 DashDirection {get { return dashDirection; }}
    Vector2 moveDirection;
    
    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        CheckBool();
        AddComponent();
    }

    void Update()
    {
        CheckDashing();
    }

    void AddComponent()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }
    void CheckBool()
    {
        CanDash = true;
    }

    void CheckDashing()
    {
        if (isDashing) return;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        // Kiểm tra điều kiện để dash
        if (Input.GetKeyDown(KeyCode.Space) && CanDash) StartCoroutine(Dash());
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        isDashing = true;
        dashDirection = moveDirection; // Sử dụng hướng di chuyển hiện tại để dash
        animator.SetTrigger("Dashing");
        float dashStartTime = Time.time;

        while (Time.time < dashStartTime + dashDuration)
        {
            Vector2 dashPosition = rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime;
            if (HandleCollision(dashPosition))
            {
                yield return new WaitForSeconds(0.1f); // Thời gian bật lại
                break;
            }
            
            rb.MovePosition(dashPosition);
            yield return null; // Chờ đến khung hình tiếp theo
        }

        rb.velocity = Vector2.zero; // Dừng lại sau khi dash hoàn thành
        isDashing = false; // Đặt lại cờ khi dash hoàn thành
        yield return new WaitForSeconds(dashCooldown); // Chờ thời gian hồi chiêu
        CanDash = true; // Cho phép dash lại
    }

    private bool HandleCollision(Vector2 dashPosition)
    {
        // Kiểm tra va chạm với đối tượng có tag là "Canvas"
        Collider2D[] hits = Physics2D.OverlapCircleAll(dashPosition, 0.1f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Canvas"))
            {
                HandleBounce(dashPosition);
                return true;
            }
        }
        return false;
    }

    private void HandleBounce(Vector2 dashPosition)
    {
        Vector2 bounceDirection = -dashDirection;
        rb.MovePosition(dashPosition + bounceDirection * dashSpeed * Time.fixedDeltaTime);
    }


}
    

