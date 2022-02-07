using UnityEngine;

public class PlayersInputController : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float limitMove = 5;
    Vector2 movement = Vector2.zero;
    Rigidbody2D rb2D;
    public enum Player
    {
        Player1, Player2
    };
    public Player playerType;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Player.Player1 == playerType)
        {
            InputPlayer1();
        }
        else
        {
            InputPlayer2();
        }
    }

    public void InputPlayer1()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Direction(1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Direction(-1);
        }
        else
        {
            Direction(0);
        }
    }

    void InputPlayer2()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Direction(1);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Direction(-1);
        }
        else
        {
            Direction(0);
        }
    }

    void Direction(int vertical)
    {
        movement += (Vector2.up * vertical) * speed * Time.deltaTime;
        movement.y = Mathf.Clamp(movement.y, -limitMove, limitMove);
        rb2D.MovePosition(movement);
    }
}
