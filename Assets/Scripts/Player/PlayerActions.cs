using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Collider2D swordCollider;
    public GameObject swordHitBox;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    // Các thuộc tính
    [SerializeField] public float moveSpeed = 1f;
    bool canMove = true;
    bool isDashing;
    bool CanDash;
    // Các thuộc tính Dash
    [SerializeField] private float dashSpeed = 0.002f; // Tốc độ khi dash
    [SerializeField] private float dashDuration = 0.2f; // Thời gian dash
    [SerializeField] private float dashCooldown = 1f; // Thời gian hồi chiêu giữa các lần dash
    private float dashTime;
    private float lastDashTime;
    private Vector2 dashDirection;
    Vector2 moveDirection;
    Vector2 mousePosition;

    void Start()
    {
        CanDash = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (PlayerPrefs.HasKey("PlayerAction"))
        {
            moveSpeed = PlayerPrefs.GetFloat("PlayerAction", moveSpeed); // Nếu không có dữ liệu lưu, sử dụng giá trị mặc định
        }

        if (swordHitBox != null)
        {
            swordCollider = swordHitBox.GetComponent<Collider2D>();
        }
        if (swordAttack == null)
        {
            Debug.LogError("Component SwordAttack chưa được gán trong Inspector");
        }
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Kiểm tra điều kiện để dash
        if (Input.GetKeyDown(KeyCode.Space) && CanDash)
        {
            StartCoroutine(Dash());
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (canMove)
        {
            // Di chuyển bình thường
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }

                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            // Xoay hướng của sprite theo hướng di chuyển
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Kiểm tra va chạm tiềm năng
            int count = rb.Cast(
                direction, // Giá trị X và Y từ -1 đến 1 đại diện cho hướng kiểm tra va chạm
                movementFilter, // Các thiết lập xác định nơi va chạm có thể xảy ra như lớp va chạm
                castCollisions, // Danh sách lưu các va chạm tìm thấy sau khi Cast hoàn tất
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // Khoảng cách kiểm tra bằng chuyển động cộng thêm một khoảng bù

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // Không thể di chuyển nếu không có hướng di chuyển
            return false;
        }
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        isDashing = true;
        dashDirection = moveDirection; // Sử dụng hướng di chuyển hiện tại để dash
        animator.SetTrigger("Dashing");
        // Dash logic
        float dashStartTime = Time.time;
        while (Time.time < dashStartTime + dashDuration)
        {
            Vector2 dashPosition = rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime;
            rb.MovePosition(dashPosition);

            // Kiểm tra va chạm với đối tượng có tag là "Canvas"
            Collider2D[] hits = Physics2D.OverlapCircleAll(dashPosition, 0.1f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Canvas"))
                {
                    // Nếu va chạm với đối tượng có tag "Canvas", bật lại một khoảng cách
                    Vector2 bounceDirection = -dashDirection;
                    rb.MovePosition(dashPosition + bounceDirection * dashSpeed * Time.fixedDeltaTime);
                    yield return new WaitForSeconds(0.1f); // Thời gian bật lại
                    break;
                }
            }
            yield return null; // Chờ đến khung hình tiếp theo
        }

        rb.velocity = Vector2.zero; // Dừng lại sau khi dash hoàn thành
        isDashing = false; // Đặt lại cờ khi dash hoàn thành
        yield return new WaitForSeconds(dashCooldown); // Chờ thời gian hồi chiêu
        CanDash = true; // Cho phép dash lại
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack()
    {
        LockMovement();

        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        if (swordAttack != null)
        {
            swordAttack.StopAttack();
        }
        else
        {
            Debug.LogError("swordAttack là null trong phương thức EndSwordAttack");
        }
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void be_Attacked()
    {
        animator.SetTrigger("be_Attacked");
    }

    public void Defeated()
    {
        animator.SetTrigger("Defeated");
        LockMovement();
    }

    private void StopafterDeath()
    {
        animator.SetBool("isMoving", false);
    }

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log("Tốc độ mới: " + moveSpeed);
    }

    public void ResetSpeed()
    {
        moveSpeed = 2;
        PlayerPrefs.DeleteKey("PlayerAction");
    }
}
