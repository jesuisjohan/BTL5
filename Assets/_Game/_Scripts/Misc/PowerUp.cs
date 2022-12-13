using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PowerUp : MonoBehaviour
{
    public static readonly float lifeSpan = 5;

    public void Init()
    {
        Debug.Log("Init PowerUp");
        Invoke(nameof(DestroyPowerUp), lifeSpan);
    }

    private void DestroyPowerUp()
    {
        Destroy(gameObject);
    }
}