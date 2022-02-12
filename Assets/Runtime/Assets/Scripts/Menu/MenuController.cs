using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class MenuController : MonoBehaviourPunCallbacks
{
    [Header("Canvas")]
    [SerializeField]
    GameObject _panel;

    [SerializeField]
    GameObject _nickNameCanvas;

    [SerializeField]
    GameObject _roomsCanvas;

    [SerializeField]
    GameObject _gameCanvas;

    [Header("Input Texts")]
    [SerializeField]
    TMP_InputField _inputNickname;

    [SerializeField]
    TMP_InputField _inputRoomName;

    [Header("ButtonToRoom")]
    [SerializeField]
    GameObject _buttomRoom;

    [Header("ListRoms")]
    [SerializeField]
    Transform _content;


    public void ActiveNickName()
    {
        _nickNameCanvas.SetActive(true);
        _roomsCanvas.SetActive(false);
        _gameCanvas.SetActive(false);
    }
    public void ActiveGame()
    {
        _panel.SetActive(false);
        _gameCanvas.SetActive(true);
    }

    public void ActiveMenuRooms()
    {
        ListRooms();
        _nickNameCanvas.SetActive(false);
        _roomsCanvas.SetActive(true);
        _gameCanvas.SetActive(false);
    }

    public void SetNickname()
    {
        NetworkManager.Instance.SetNickName(_inputNickname.text.Trim());
        ActiveMenuRooms();
    }

    void ClearListRooms()
    {
        foreach (Transform room in _content)
        {
            Destroy(room.gameObject);
        }
    }

    void ListRooms()
    {
        ClearListRooms();

        foreach (Photon.Realtime.RoomInfo room in NetworkManager.Instance.RoomList)
        {
            GameObject newButtomRoom = Instantiate(_buttomRoom, _content) as GameObject;
            newButtomRoom.transform.Find("TextRoomName").GetComponent<TextMeshProUGUI>().text = room.Name.Trim();
            newButtomRoom.transform.Find("TextPlayerCount").GetComponent<TextMeshProUGUI>().text = $"{room.PlayerCount}/{room.MaxPlayers}";
            newButtomRoom.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(room.Name.Trim()); });
        }
    }

    public void CreateRoom()
    {
        NetworkManager.Instance.CreateRoom(_inputRoomName.text);
        ListRooms();
    }

    public void JoinRoom(string roomName)
    {
        NetworkManager.Instance.JoinRoom(roomName);
    }
}
