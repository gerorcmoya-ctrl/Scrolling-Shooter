using UnityEngine;
using System.Collections;

public class PlayerShooter : MonoBehaviour
{
    public enum TipoArma { Single, Double, Triple, Spread, Laser }

    [Header("Arma actual")]
    [SerializeField] TipoArma armaActual = TipoArma.Single;
    [SerializeField] float cadencia = 0.15f;   // segundos entre disparos

    [Header("Puntos de disparo — arrastrá los GunPoints acá")]
    [SerializeField] Transform gunCenter;
    [SerializeField] Transform[] gunDouble;          // arrastrar GunPoint_Left y Right

    [Header("Prefabs de balas — los creamos en el siguiente paso")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject laserPrefab;

    Coroutine disparoCoroutine;

    // Llamado cuando se presiona el botón de disparo
    public void StartShooting()
    {
        if (disparoCoroutine != null) return;  // ya está disparando
        disparoCoroutine = StartCoroutine(LoopDisparo());
    }

    // Llamado cuando se suelta el botón
    public void StopShooting()
    {
        if (disparoCoroutine == null) return;
        StopCoroutine(disparoCoroutine);
        disparoCoroutine = null;
    }

    IEnumerator LoopDisparo()
    {
        while (true)
        {
            Disparar();
            yield return new WaitForSeconds(cadencia);
        }
    }

    void Disparar()
    {
        switch (armaActual)
        {
            case TipoArma.Single:
                SpawnBala(bulletPrefab, gunCenter.position, Vector3.forward);
                break;

            case TipoArma.Double:
                foreach (var gp in gunDouble)
                    SpawnBala(bulletPrefab, gp.position, Vector3.forward);
                break;

            case TipoArma.Triple:
                SpawnBala(bulletPrefab, gunCenter.position, Vector3.forward);
                SpawnBala(bulletPrefab, gunCenter.position, new Vector3(-0.3f, 0f, 1f).normalized);
                SpawnBala(bulletPrefab, gunCenter.position, new Vector3(0.3f, 0f, 1f).normalized);
                break;

            case TipoArma.Spread:
                for (int i = -2; i <= 2; i++)
                    SpawnBala(bulletPrefab, gunCenter.position,
                              new Vector3(i * 0.25f, 0f, 1f).normalized);
                break;

            case TipoArma.Laser:
                SpawnBala(laserPrefab ?? bulletPrefab, gunCenter.position, Vector3.forward);
                break;
        }
    }

    void SpawnBala(GameObject prefab, Vector3 pos, Vector3 dir)
    {
        var bala = Instantiate(prefab, pos, Quaternion.LookRotation(dir));
        bala.GetComponent<Bullet>().Init(dir, isEnemy: false, owner: gameObject);
    }

    // Llamado por PowerUps
    public void UpgradeGun()
    {
        // Solo permite llegar hasta Double
        if (armaActual == TipoArma.Single)
            armaActual = TipoArma.Double;

        Debug.Log($"Arma: {armaActual}");
    }

    public void SetBulletPrefab(GameObject prefab) => bulletPrefab = prefab;
}