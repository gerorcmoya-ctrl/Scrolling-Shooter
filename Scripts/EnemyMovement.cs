using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Distancia")]
    [SerializeField] float distanciaIdeal = 8f;
    [SerializeField] float rangoToleancia = 1.5f;

    [Header("Velocidad")]
    [SerializeField] float velocidadSeguir = 5f;
    [SerializeField] float velocidadAlejar = 4f;

    [Header("Movimiento lateral")]
    [SerializeField] float amplitudLateral = 2f;
    [SerializeField] float frecuenciaLateral = 1f;

    Transform jugador;
    float tiempo;

    void Start()
    {
        var obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null) jugador = obj.transform;
    }

    void Update()
    {
        if (jugador == null) return;

        tiempo += Time.deltaTime;

        float distancia = Vector3.Distance(transform.position, jugador.position);
        Vector3 pos = transform.position;
        Vector3 dirAlJugador = (jugador.position - transform.position);
        dirAlJugador.y = 0f;
        dirAlJugador.Normalize();

        if (distancia > distanciaIdeal + rangoToleancia)
        {
            pos += dirAlJugador * velocidadSeguir * Time.deltaTime;
        }
        else if (distancia < distanciaIdeal - rangoToleancia)
        {
            pos -= dirAlJugador * velocidadAlejar * Time.deltaTime;
        }
        else
        {
            pos.x += Mathf.Sin(tiempo * frecuenciaLateral) * amplitudLateral * Time.deltaTime;
        }

        pos.y = jugador.position.y;
        transform.position = pos;

        // Se destruye cuando queda muy atrás del jugador
        if (transform.position.z < jugador.position.z - 20f)
            Destroy(gameObject);
    }
}