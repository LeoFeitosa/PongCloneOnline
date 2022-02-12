using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public static SpawnController spawn;

    [SerializeField] public Transform positionPlayer1;
    [SerializeField] public Transform positionPlayer2;
    [SerializeField] public Transform positionSquare;

    private void OnEnable()
    {
        if (SpawnController.spawn == null)
        {
            SpawnController.spawn = this;
        }
    }
}
