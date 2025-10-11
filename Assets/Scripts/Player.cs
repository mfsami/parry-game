using UnityEngine;

public class Player : MonoBehaviour
{
    // References
    
    public Rigidbody2D rb;

    // Variables
    public float moveSpeed = 5f;

    private Vector2 moveDirection;

    void Update()
    {
        ProcessInputs();
    }

    // Physics updates should be done in fixed update
    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
    }
}
