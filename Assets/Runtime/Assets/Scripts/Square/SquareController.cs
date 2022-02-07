using System.Collections;
using UnityEngine;

public class SquareController : MonoBehaviour
{
    [SerializeField] float speed = 1.5f;
    [SerializeField] AudioClip collisionPlayerSound;
    [SerializeField] AudioClip collisionUpDownSound;
    [SerializeField] AudioClip scoreSound;
    Vector2 direction;
    ScoreController score;

    void Start()
    {
        score = FindObjectOfType<ScoreController>();
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
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
    }

    void OnBecameInvisible()
    {
        AudioController.Instance.PlayAudioCue(scoreSound);
        score.SetScore((transform.position.x < 0) ? 2 : 1);
        StartCoroutine(InitialMove());
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
