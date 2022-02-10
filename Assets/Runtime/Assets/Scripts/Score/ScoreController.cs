using Photon.Pun;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    PhotonView _photonView;
    [SerializeField] TextMeshProUGUI scorePlayer1;
    [SerializeField] TextMeshProUGUI scorePlayer2;
    bool _active;

    void Start()
    {
        _active = false;
        _photonView = GetComponent<PhotonView>();
    }

    public void SetScore(int player)
    {
        _active = true;
        _photonView.RPC("ShowScore", RpcTarget.All, player);
    }

    [PunRPC]
    public void ShowScore(int player)
    {
        if (_active)
        {
            _active = false;

            if (player == 1)
            {
                scorePlayer1.text = (ConvertStringToInt(scorePlayer1.text) + 1).ToString();
            }
            if (player == 2)
            {
                scorePlayer2.text = (ConvertStringToInt(scorePlayer2.text) + 1).ToString();
            }
        }
    }

    int ConvertStringToInt(string stringNumber)
    {
        int score;
        int.TryParse(stringNumber, out score);
        return score;
    }
}
