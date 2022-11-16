using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;

//Handles character spawning for multiplayer
public class NetworkSpawner : NetworkBehaviour {

    private GameObject[] characterPrefabs;

    private void Awake() {
        characterPrefabs = LoadCharacter.singleton.characterPrefabs;
    }

    public override async void OnNetworkSpawn()
    {
        if (GlobalValues.GetGameMode() != GlobalValues.GameMode.Single)
        {


            if (!IsOwner)
            {
                return;
            }

            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
            SpawnPlayerServerRpc(selectedCharacter, OwnerClientId);
        }

    }

    private void ForPlayerName(ulong clientId)
    {
        if (IsServer)
        {
            NononLogger.Instance.LogDebug($"{NetworkManager.Singleton.GetComponent<RelayManager>().GetPlayerName(clientId)} connected...");

        }
    }
    [ServerRpc]
    private void SpawnPlayerServerRpc(int selectedChar, ulong clientId)
    {
        GameObject prefab = characterPrefabs[selectedChar];
        GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.Euler(180, 0, 0));
        clone.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    }


    //new
    private void NetworkOnClientDisconnected(ulong obj)
    {
        if (IsServer)
        {
            NononLogger.Instance.LogDebug($"{NetworkManager.Singleton.GetComponent<RelayManager>().GetPlayerName(obj)} disconnected...");

        }
    }

}
