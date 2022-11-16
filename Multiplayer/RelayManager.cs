using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

[RequireComponent(typeof(UnityTransport))]
public class RelayManager : MonoBehaviour {


    public class PlayerInfo
    {
        public ulong clientId;
        public string playerName;

        public PlayerInfo()
        {

        }

        public PlayerInfo(ulong _clientId, string _playerName)
        {
            clientId = _clientId;
            playerName = _playerName;
        }
    }

    public Dictionary<ulong, PlayerInfo> playerInfos = new Dictionary<ulong, PlayerInfo>();

    public void AddPlayerInfo(ulong _clientId, string _playername)
    {
        playerInfos.Add(_clientId, new PlayerInfo(_clientId, _playername));
    }

    public string GetPlayerName(ulong _clientId)
    {
        PlayerInfo info;
        if (playerInfos.TryGetValue(_clientId, out info))
        {
            return info.playerName;
        }
        return "PlayerNotFound";
    }
    //
    public static RelayManager singleton;

    public int maxPlayerCount = 12;

    private UnityTransport transport;

    private void Awake() {
        RelayManager.singleton = this;
        transport = GetComponent<UnityTransport>();
        if (transport == null) {
            Debug.LogError("Unity transport missing");
        }
    }

    public async Task<string> CreateGame()
    {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(maxPlayerCount);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        Debug.Log("Join Code: " + joinCode);
        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);
        NetworkManager.Singleton.StartHost();
       
        return joinCode;
        
    }
    public void AddPlayerInfos()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        string playerName = PlayerPrefs.GetString("BikerName");
        RelayManager ii = NetworkManager.Singleton.GetComponent<RelayManager>();
        ii.AddPlayerInfo(clientId, playerName);
    }

    public async Task<string> CreateDedicatedServer()
    {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(maxPlayerCount);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        Debug.Log("Join Code: " + joinCode);
        transport.SetRelayServerData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);
        NetworkManager.Singleton.StartServer();
        return joinCode;
    }

    public async Task JoinGame(string joinCode) {
        if (joinCode == "") {
            Debug.LogError("Join code rempty, cannot join game");
            return;
        }
        JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(joinCode);
        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
        NetworkManager.Singleton.StartClient();
    }
}
