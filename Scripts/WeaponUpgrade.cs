using UnityEngine;

public class WeaponUpgrade : PowerUp
{
    protected override void Aplicar(GameObject player)
    {
        player.GetComponent<PlayerShooter>()?.UpgradeGun();
        Debug.Log("PowerUp: arma mejorada");
    }
}