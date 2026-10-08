using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public GameManager gameManager;

    private Rigidbody rb;
    private float movementX;
    private float movementY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        if (!gameManager.gameStarted)
            return;

        Vector3 movement = new Vector3(
            movementX,
            0.0f,
            movementY
        );

        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameManager.LoseGame();
        }
    }
}