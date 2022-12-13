using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleModeManager : MonoBehaviour
{
    public static SingleModeManager Instance { get; private set; }
    public bool isPlaying { get; private set; } = false;

    public Transform playerPrefab;
    public Transform bossPrefab;
    public Transform playerSpawner;
    public Transform bossSpawner;
    public Transform player { get; private set; }
    public Transform boss { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        gameObject.SetActive(true);
        isPlaying = true;
        CreatePlayer();
        CreateBoss();
    }

    private void CreatePlayer()
    {
        var playerSpawnerPosition = playerSpawner.position;
        var spawnPosition = new Vector3(playerSpawnerPosition.x, playerPrefab.position.y, playerSpawnerPosition.z);
        player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }

    private void CreateBoss()
    {
        var bossSpawnerPosition = bossSpawner.position;
        var spawnPosition = new Vector3(bossSpawnerPosition.x, bossPrefab.position.y, bossSpawnerPosition.z);
        boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
    }
}