using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] Transform positionPlayer1;
    [SerializeField] Transform positionPlayer2;
    [SerializeField] Transform positionSquare;

    SquareController _squareController;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("PlayerBar", positionPlayer1.position, Quaternion.identity, 0);
            Debug.Log("1");
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("PlayerBar", positionPlayer2.position, Quaternion.identity, 0);
            Debug.Log("2");
            StartSquare();
        }
    }

    void StartSquare()
    {
        PhotonNetwork.Instantiate("Square", positionSquare.position, Quaternion.identity, 0);
        _squareController = FindObjectOfType<SquareController>();
        _squareController.StartGame();
    }
}
