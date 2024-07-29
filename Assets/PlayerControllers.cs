using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{ 
    public float collisionOffset = 0.05f;
    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;

    Vector2 movementInput;
    Rigidbody2D rb;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Chỉnh sửa chữ 'd' thành 'D' trong Rigidbody2D
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero) // kiểm tra xem có đang di chuyển hay không?
        {
            int count = rb.Cast(movementInput,
                                movementFilter,
                                castCollisions,
                                moveSpeed * Time.fixedDeltaTime + collisionOffset);
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
