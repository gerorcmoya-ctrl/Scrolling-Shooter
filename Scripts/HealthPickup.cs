using UnityEngine;

public class HealthPickup : PowerUp
{
    [SerializeField] int cantidadVida = 1;

    protected override void Aplicar(GameObject player)
    {
        player.GetComponent<PlayerHealth>()?.Heal(cantidadVida);
        Debug.Log("PowerUp: +1 vida");
    }
}