using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    NetworkLog _networkLog;

    [SerializeField]
    byte _maxPlayers = 2;

    [SerializeField]
    string _roomName;

    public List<RoomInfo> RoomList { get; private set; }

    public static NetworkManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _networkLog = FindObjectOfType<NetworkLog>();
        PhotonNetwork.GameVersion = "1.0.0";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreateRoom(string roomName)
    {
        _roomName = roomName;
        _networkLog.SetLog($"Criando a sala {_roomName}...", NetworkLog.Color.yellow);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayers;
        PhotonNetwork.CreateRoom(_roomName, roomOptions, null);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomList = roomList;
    }

    public override void OnConnectedToMaster()
    {
        _networkLog.SetLog("Conectado com sucesso!", NetworkLog.Color.green);
        JoinLobby();
    }

    public void SetNickName(string nickName)
    {
        PhotonNetwork.LocalPlayer.NickName = nickName;
    }

    public void JoinLobby()
    {
        if (PhotonNetwork.InLobby == false)
        {
            _networkLog.SetLog("Entrando no lobby...", NetworkLog.Color.yellow);
            PhotonNetwork.JoinLobby();
        }
    }

    //Entrou no lobbby e chama funcao para entrar na sala
    public override void OnJoinedLobby()
    {
        _networkLog.SetLog("Entrou no lobby com sucesso!", NetworkLog.Color.green);
    }

    public void JoinRoom(string room)
    {
        PhotonNetwork.JoinRoom(room);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _networkLog.SetLog($"ERRO AO ENTRAR NA SALA: {message}. CODIGO {returnCode}", NetworkLog.Color.red);
    }

    public override void OnJoinedRoom()
    {
        _networkLog.SetLog($"O jogador {PhotonNetwork.NickName} entrou na sala {_roomName}", NetworkLog.Color.green);

        //aqui deve instanciar o player na tela
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _networkLog.SetLog($"O jogador {newPlayer.NickName} entrou na sala", NetworkLog.Color.green);
    }

    public override void OnLeftRoom()
    {
        _networkLog.SetLog("Voce saiu da sala", NetworkLog.Color.yellow);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _networkLog.SetLog($"O jogador {otherPlayer.NickName} saiu da sala", NetworkLog.Color.yellow);
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        _networkLog.SetLog($"OCORREU UM ERRO: {errorInfo.Info}", NetworkLog.Color.red);
    }
}
