using UnityEngine;
public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    public GameManager gameManager;

    bool gameOver = false;

    void Update()
    {
        if (!gameManager.gameStarted)
            return;

        if (gameOver)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, player.position) < 0.7f)
        {
            gameOver = true;
            gameManager.LoseGame();
        }
    }
}