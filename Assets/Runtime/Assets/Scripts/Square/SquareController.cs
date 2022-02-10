using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SquareController : MonoBehaviour
{
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float speed = 1.5f;
    [SerializeField] AudioClip collisionPlayerSound;
    [SerializeField] AudioClip collisionUpDownSound;
    [SerializeField] AudioClip scoreSound;
    Vector2 direction;
    ScoreController _score;

    void Start()
    {
        _score = FindObjectOfType<ScoreController>();
    }

    public void StartGame()
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
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;// * (float)(PhotonNetwork.Time - info.SentServerTime);
    }

    void OnBecameInvisible()
    {
        AudioController.Instance.PlayAudioCue(scoreSound);
        _score.SetScore((transform.position.x < 0) ? 2 : 1);
        StartGame();
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
}
