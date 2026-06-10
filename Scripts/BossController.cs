using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float velocidadFase1 = 4f;
    [SerializeField] float velocidadFase2 = 8f;
    [SerializeField] float limiteX = 6f;
    [SerializeField] float posicionZ = 15f;

    [Header("Disparo")]
    [SerializeField] float cadenciaFase1 = 1.5f;
    [SerializeField] float cadenciaFase2 = 0.6f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gunCenter;
    [SerializeField] Transform gunLeft;
    [SerializeField] Transform gunRight;

    float velocidadActual;
    float cadenciaActual;
    bool moviendoDerecha = true;
    bool fase2 = false;
    bool entradaCompleta = false;  // ← nuevo
    Transform jugador;

    void Start()
    {
        var obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null) jugador = obj.transform;

        velocidadActual = velocidadFase1;
        cadenciaActual = cadenciaFase1;

        StartCoroutine(EntradaBoss());
    }

    IEnumerator EntradaBoss()
    {
        Debug.Log("Boss entrando...");

        // Posición objetivo — adelante del jugador
        Vector3 posObjetivo = new Vector3(
            0f,
            jugador.position.y,
            jugador.position.z + posicionZ
        );

        // Moverse hacia la posición de combate
        while (Vector3.Distance(transform.position, posObjetivo) > 0.5f)
        {
            // Actualizar objetivo mientras el jugador avanza
            posObjetivo = new Vector3(
                0f,
                jugador.position.y,
                jugador.position.z + posicionZ
            );

            transform.position = Vector3.MoveTowards(
                transform.position,
                posObjetivo,
                velocidadActual * 3f * Time.deltaTime
            );
            yield return null;
        }

        Debug.Log("Boss en posición — iniciando combate");
        entradaCompleta = true;

        StartCoroutine(LoopMovimiento());
        StartCoroutine(LoopDisparo());
    }

    void Update()
    {
        // Solo seguir al jugador en Z DESPUÉS de la entrada
        if (!entradaCompleta || jugador == null) return;

        Vector3 pos = transform.position;
        pos.z = Mathf.Lerp(pos.z, jugador.position.z + posicionZ, 3f * Time.deltaTime);
        pos.y = jugador.position.y;
        transform.position = pos;
    }

    IEnumerator LoopMovimiento()
    {
        while (true)
        {
            float targetX = moviendoDerecha ? limiteX : -limiteX;

            while (Mathf.Abs(transform.position.x - targetX) > 0.1f)
            {
                Vector3 pos = transform.position;
                pos.x = Mathf.MoveTowards(pos.x, targetX, velocidadActual * Time.deltaTime);
                transform.position = pos;
                yield return null;
            }

            moviendoDerecha = !moviendoDerecha;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator LoopDisparo()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Boss iniciando disparo");

        while (true)
        {
            if (!fase2)
                DisparoTriple();
            else
                DisparoFase2();

            yield return new WaitForSeconds(cadenciaActual);
        }
    }

    void DisparoTriple()
    {
        if (jugador == null) return;
        DispararBala(gunCenter);
        DispararBalaConOffset(gunLeft, -0.3f);
        DispararBalaConOffset(gunRight, 0.3f);
    }

    void DisparoFase2()
    {
        if (jugador == null) return;
        for (int i = -2; i <= 2; i++)
            DispararBalaConOffset(gunCenter, i * 0.25f);
    }

    void DispararBala(Transform origen)
    {
        if (origen == null || bulletPrefab == null) return;
        Vector3 targetPos = new Vector3(jugador.position.x, origen.position.y, jugador.position.z);
        Vector3 dir = (targetPos - origen.position).normalized;
        var bala = Instantiate(bulletPrefab, origen.position, Quaternion.identity);
        bala.GetComponent<Bullet>().Init(dir, isEnemy: true);
    }

    void DispararBalaConOffset(Transform origen, float offsetX)
    {
        if (origen == null || bulletPrefab == null) return;
        Vector3 targetPos = new Vector3(jugador.position.x + offsetX * 5f, origen.position.y, jugador.position.z);
        Vector3 dir = (targetPos - origen.position).normalized;
        var bala = Instantiate(bulletPrefab, origen.position, Quaternion.identity);
        bala.GetComponent<Bullet>().Init(dir, isEnemy: true);
    }

    public void ActivarFase2()
    {
        fase2 = true;
        velocidadActual = velocidadFase2;
        cadenciaActual = cadenciaFase2;
        Debug.Log("Boss fase 2 — más rápido y más balas");
    }
}