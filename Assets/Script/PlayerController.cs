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

    public bool IsDragging => dragging;

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
        Distance = Mathf.Max(0, Mathf.FloorToInt(transform.position.y / 2f));
    }

    private void HandleInput()
    {
        if (!isGrounded)
            return;

        if (Camera.main == null)
            return;

        Vector3 screenPos = Input.mousePosition;
        if (float.IsInfinity(screenPos.x) || float.IsInfinity(screenPos.y) ||
            float.IsNaN(screenPos.x) || float.IsNaN(screenPos.y))
        {
            return;
        }

        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector2 pointerPos =
            Camera.main.ScreenToWorldPoint(screenPos);

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

        float dragDistance = Mathf.Clamp(dragVector.magnitude, 0f, maxDragDistance);

        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, dragDistance / maxDragDistance);

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(
            dragVector.normalized * jumpForce,
            ForceMode2D.Impulse
        );
    }

    public Vector2 GetPredictedVelocity()
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

        return dragVector.normalized * jumpForce;
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