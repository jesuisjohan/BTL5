using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using DG.Tweening;

public class PowerUp : MonoBehaviour
{
    public static readonly float lifeSpan = 5;

    public void Init()
    {
        Debug.Log("Init PowerUp");
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(1, 0.2f).SetEase(Ease.InOutSine).OnComplete(
            () => { Invoke(nameof(DestroyPowerUp), lifeSpan); }
        );
    }

    private void DestroyPowerUp()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>())
        {
            var projectile = collision.gameObject.GetComponent<Projectile>();

            var player = projectile.playerShooting;

            player.ReduceCoolDown(2);
            PlayDisappearingAnimation();
        }
        else if (collision.gameObject.GetComponent<PlayerShooting>())
        {
            var player = collision.gameObject.GetComponent<PlayerShooting>();
            player.ReduceCoolDown(1.5f);
            PlayDisappearingAnimation();
        }
    }

    private void PlayDisappearingAnimation()
    {
        transform.DOScale(0, 0.5f).SetEase(Ease.OutBack).OnComplete(() => { DestroyPowerUp(); });
    }
}