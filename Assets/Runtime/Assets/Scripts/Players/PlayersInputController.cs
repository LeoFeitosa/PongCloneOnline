using Photon.Pun;
using UnityEngine;

public class PlayersInputController : MonoBehaviour
{
    PhotonView _photonView;
    [SerializeField] float _speed = 5;
    [SerializeField] float _limitMove = 5;
    Vector2 _movement = Vector2.zero;
    Rigidbody2D _rb2D;

    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        InputPlayer();
    }

    public void InputPlayer()
    {
        if (!_photonView.IsMine) 
        {
            return;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Direction(1);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
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
        _movement += (Vector2.up * vertical) * _speed * Time.deltaTime;
        _movement.y = Mathf.Clamp(_movement.y, -_limitMove, _limitMove);
        _rb2D.MovePosition(_movement);
    }
}
