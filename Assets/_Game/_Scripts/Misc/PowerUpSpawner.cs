using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public static PowerUpSpawner Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private PowerUp powerUp;

    [SerializeField] private float spawnTime = 1;
    [SerializeField] private float spawnDelay = PowerUp.lifeSpan + 2;
    public bool shouldSpawn = false;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPowerUp), spawnTime, spawnDelay);
    }

    private void SpawnPowerUp()
    {
        if (!shouldSpawn)
        {
            return;
        }

        var position = new Vector3(transform.position.x, powerUp.transform.position.y, transform.position.z);
        var newPowerUp = Instantiate(powerUp, position, Quaternion.identity);

        newPowerUp.Init();
    }
}