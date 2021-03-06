using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SquareController : MonoBehaviour
{
    PhotonView _photonView;

    [Header("Config Square")]
    [SerializeField] float speed = 1.5f;

    [Header("Config Audio")]
    [SerializeField] AudioClip collisionPlayerSound;
    [SerializeField] AudioClip collisionUpDownSound;
    [SerializeField] AudioClip scoreSound;

    Rigidbody2D _rb2D;
    Vector2 direction;
    ScoreController _score;

    Vector2 networkPosition;

    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _rb2D = GetComponent<Rigidbody2D>();
        _score = FindObjectOfType<ScoreController>();
    }

    void Start()
    {
        StartMove();
    }

    public void FixedUpdate()
    {
        if (!_photonView.IsMine)
        {
            _rb2D.position = Vector3.MoveTowards(_rb2D.position, networkPosition, Time.fixedDeltaTime);
        }
    }

    void StartMove()
    {
        StartCoroutine(InitialMove());
    }

    IEnumerator InitialMove()
    {
        yield return new WaitForSeconds(2);
        RandomDirection();
    }

    void RandomDirection()
    {
        transform.position = Vector2.zero;

        int dirX = Random.Range(-1, 2);
        int dirY = Random.Range(-1, 2);

        if (dirX == 0 || dirY == 0)
        {
            RandomDirection();
        }
        else
        {
            direction = new Vector2(dirX, dirY);

            Move();
        }
    }

    void Move()
    {
        _rb2D.velocity = direction.normalized * speed * Time.fixedDeltaTime;
    }

    void OnBecameInvisible()
    {
        AudioController.Instance.PlayAudioCue(scoreSound);
        _score.SetScore((transform.position.x < 0) ? 2 : 1);

        if (enabled)
        {
            StartCoroutine(InitialMove());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioController.Instance.PlayAudioCue(collisionPlayerSound);
        }

        else if (collision.gameObject.CompareTag("CollisionUpDown"))
        {
            AudioController.Instance.PlayAudioCue(collisionUpDownSound);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this._rb2D.position);
            stream.SendNext(this._rb2D.velocity);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            _rb2D.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTimestamp));
            networkPosition += _rb2D.velocity * lag;
        }
    }
}
