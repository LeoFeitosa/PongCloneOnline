using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    NetworkLog _networkLog;
    MenuController _menuController;
    SpawnController _spawnController;

    [SerializeField] byte _maxPlayers = 2;

    string _roomName;

    public List<RoomInfo> RoomList { get; private set; }

    public static NetworkManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    void Start()
    {
        _networkLog = FindObjectOfType<NetworkLog>();
        _menuController = FindObjectOfType<MenuController>();
        _spawnController = FindObjectOfType<SpawnController>();

        if (!PhotonNetwork.IsConnected)
        {
            _networkLog.SetLog($"Conectando...", NetworkLog.Color.yellow);
            PhotonNetwork.GameVersion = "1.0.0";
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = true;
        }
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

    public void CreateRoom(string roomName)
    {
        _roomName = roomName;
        _networkLog.SetLog($"Criando a sala {_roomName}...", NetworkLog.Color.yellow);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayers;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        _networkLog.SetLog($"Sala {_roomName} criada com sucesso!", NetworkLog.Color.green);
    }

    public void JoinLobby()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            _networkLog.SetLog("Entrando no lobby...", NetworkLog.Color.yellow);
            _menuController.ActiveNickName();
        }
    }

    //Entrou no lobbby e chama funcao para entrar na sala
    public override void OnJoinedLobby()
    {
        _networkLog.SetLog("Entrou no lobby com sucesso!", NetworkLog.Color.green);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomList = roomList;
    }

    public void JoinRoom(string room)
    {
        PhotonNetwork.JoinRoom(room);
    }

    public override void OnJoinedRoom()
    {
        _menuController.ActiveGame();

        if (PhotonNetwork.PlayerList.Length == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("PlayerPrefabs", "PlayerBar"), _spawnController.positionPlayer1.position, Quaternion.identity, 0);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("PlayerPrefabs", "PlayerBar"), _spawnController.positionPlayer2.position, Quaternion.identity, 0);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _menuController.ActiveGame();
        _networkLog.SetLog($"O jogador {newPlayer.NickName} entrou na sala", NetworkLog.Color.green);
        PhotonNetwork.Instantiate(Path.Combine("SquarePrefabs", "Square"), _spawnController.positionSquare.position, Quaternion.identity, 0);
    }

    public override void OnLeftRoom()
    {
        _networkLog.SetLog("Voce saiu da sala", NetworkLog.Color.yellow);
    }

    #region Errors      
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _networkLog.SetLog($"O jogador {otherPlayer.NickName} saiu da sala", NetworkLog.Color.yellow);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _networkLog.SetLog($"ERRO AO CRIAR A SALA: {message}. CODIGO {returnCode}", NetworkLog.Color.red);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _networkLog.SetLog($"ERRO AO ENTRAR NA SALA: {message}. CODIGO {returnCode}", NetworkLog.Color.red);
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        _networkLog.SetLog($"OCORREU UM ERRO: {errorInfo.Info}", NetworkLog.Color.red);
    }
    #endregion Errors
}
