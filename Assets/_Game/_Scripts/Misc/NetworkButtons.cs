using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkButtons : MonoBehaviour
{
    private bool gameStarted = false;
    private void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
            if (GUILayout.Button("Host")) gameStarted = NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Server")) gameStarted = NetworkManager.Singleton.StartServer();
            if (GUILayout.Button("Client")) gameStarted = NetworkManager.Singleton.StartClient();

            if (gameStarted)
            {
                PowerUpSpawner.Instance.shouldSpawn = true;
            }
        }

        GUILayout.EndArea();
    }

    // private void Awake() {
    //     GetComponent<UnityTransport>().SetDebugSimulatorParameters(
    //         packetDelay: 120,
    //         packetJitter: 5,
    //         dropRate: 3);
    // }
}