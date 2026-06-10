using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] float cadencia = 1.5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gunPoint;

    Transform jugador;
    bool disparando = false;

    void Start()
    {
        var obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null) jugador = obj.transform;

        // Si no hay gunPoint asignado usar este mismo transform
        if (gunPoint == null) gunPoint = transform;

        StartCoroutine(LoopDisparo());
    }

    IEnumerator LoopDisparo()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            Disparar();
            yield return new WaitForSeconds(cadencia);
        }
    }

    void Disparar()
    {
        if (jugador == null || bulletPrefab == null) return;

        Vector3 origen = gunPoint != null ? gunPoint.position : transform.position;

        // Forzar mismo Y para que la bala vaya recta sin caer
        Vector3 targetPos = new Vector3(jugador.position.x, origen.y, jugador.position.z);
        Vector3 dir = (targetPos - origen).normalized;

        GameObject balaObj = Instantiate(bulletPrefab, origen, Quaternion.identity);
        Bullet balaScript = balaObj.GetComponent<Bullet>();

        if (balaScript == null)
        {
            Destroy(balaObj);
            return;
        }

        balaScript.Init(dir, isEnemy: true);
    }
}