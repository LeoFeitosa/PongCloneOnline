using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class MenuController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject _nickName;

    [SerializeField]
    GameObject _rooms;

    [SerializeField]
    TMP_InputField _inputNickname;

    [SerializeField]
    TMP_InputField _inputRoomName;

    [SerializeField]
    GameObject _buttomRoom;

    [SerializeField]
    Transform _content;

    public override void OnConnectedToMaster()
    {
        _nickName.SetActive(true);
        _rooms.SetActive(false);
    }

    public void ActiveMenuRooms()
    {
        _rooms.SetActive(true);
        _nickName.SetActive(false);
        ListRooms();
    }

    public void SetNickname()
    {
        NetworkManager.Instance.SetNickName(_inputNickname.text);
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
        Debug.Log("ok");

        /* if (NetworkManager.Instance.RoomList.Count <= 0)
         {
             GameObject newButtomRoom = Instantiate(_buttomRoom, _content) as GameObject;
             newButtomRoom.transform.Find("TextNameRom").GetComponent<TextMeshProUGUI>().text = "Sem salas criadas";
             newButtomRoom.transform.Find("TextPlayersInRom").GetComponent<TextMeshProUGUI>().text = "";
         }*/

        foreach (Photon.Realtime.RoomInfo room in NetworkManager.Instance.RoomList)
        {
            if (room.PlayerCount == 0)
            {
                continue;
            }

            GameObject newButtomRoom = Instantiate(_buttomRoom, _content) as GameObject;
            newButtomRoom.transform.Find("TextNameRom").GetComponent<TextMeshProUGUI>().text = room.Name;
            newButtomRoom.transform.Find("TextPlayersInRom").GetComponent<TextMeshProUGUI>().text = $"{room.PlayerCount}/{room.MaxPlayers}";
            newButtomRoom.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(room.Name); });
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
