using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float minJumpForce = 4f;
    public float maxJumpForce = 15f;
    public float maxDragDistance = 3f;

    [Header("Distance")]
    public int Distance;

    private Rigidbody2D rb;

    private Vector2 dragStart;
    private Vector2 dragCurrent;

    private bool dragging;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CalculateDistance();
        HandleInput();
    }

    private void CalculateDistance()
    {
        Distance = Mathf.Max(
            0,
            Mathf.FloorToInt(transform.position.y / 2f)
        );
    }

    private void HandleInput()
    {
        if (!isGrounded)
            return;

        Vector2 pointerPos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            dragStart = pointerPos;
            dragCurrent = pointerPos;
            dragging = true;
        }

        if (Input.GetMouseButton(0) && dragging)
        {
            dragCurrent = pointerPos;
        }

        if (Input.GetMouseButtonUp(0) && dragging)
        {
            Jump();
            dragging = false;
        }
    }

    private void Jump()
    {
        Vector2 dragVector = dragStart - dragCurrent;

        float dragDistance =
            Mathf.Clamp(
                dragVector.magnitude,
                0f,
                maxDragDistance
            );

        float jumpForce =
            Mathf.Lerp(
                minJumpForce,
                maxJumpForce,
                dragDistance / maxDragDistance
            );

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(
            dragVector.normalized * jumpForce,
            ForceMode2D.Impulse
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!dragging)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(dragStart, dragCurrent);
    }
}