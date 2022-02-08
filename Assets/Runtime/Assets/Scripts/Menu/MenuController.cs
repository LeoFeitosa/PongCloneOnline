using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MenuController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject _nickName;

    [SerializeField]
    GameObject _rooms;

    [SerializeField]
    TMP_InputField _inputNickname;

    public override void OnConnectedToMaster()
    {
        _nickName.SetActive(true);
        _rooms.SetActive(false);
    }

    public void ActiveMenuRooms()
    {
        _rooms.SetActive(true);
        _nickName.SetActive(false);
    }

    public void SetNickname()
    {
        NetworkManager.Instance.SetNickName(_inputNickname.text);
        ActiveMenuRooms();
    }
}
