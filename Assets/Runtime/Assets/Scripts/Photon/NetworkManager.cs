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
    string roomName;

    public static NetworkManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Connect();
    }

    void Connect()
    {
        _networkLog = FindObjectOfType<NetworkLog>();
        SetNickName("nome_jogador_"+Random.Range(1, 999));
        PhotonNetwork.ConnectUsingSettings();
    }

    private void CreateRoom()
    {
        _networkLog.SetLog($"Criando a sala {roomName}...", NetworkLog.Color.yellow);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayers;
        PhotonNetwork.CreateRoom(roomName, roomOptions, null);
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
        _networkLog.SetLog("Entrou no lobby", NetworkLog.Color.green);
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _networkLog.SetLog($"ERRO AO ENTRAR NA SALA: {message}. CODIGO {returnCode}", NetworkLog.Color.red);

        if (returnCode == ErrorCode.GameDoesNotExist)
        {
            CreateRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        _networkLog.SetLog($"O jogador {PhotonNetwork.NickName} entrou na sala {roomName}", NetworkLog.Color.green);

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
