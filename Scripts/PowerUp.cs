using UnityEngine;
using System.Collections;

public abstract class PowerUp : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float velocidad = 5f;
    [SerializeField] float rotacion = 90f;
    [SerializeField] float lifetime = 8f;

    Transform jugador;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (jugador == null) return;

        // Moverse exactamente a la posición del jugador incluyendo Y
        Vector3 targetPos = jugador.position;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,        // ← incluye la Y del jugador
            velocidad * Time.deltaTime
        );

        // Rotar
        transform.Rotate(Vector3.up, rotacion * Time.deltaTime);

        // Destruir si se aleja mucho
        if (Vector3.Distance(transform.position, jugador.position) > 30f)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Aplicar(other.gameObject);
            Destroy(gameObject);
        }
    }

    // Cada PowerUp implementa su propio efecto
    protected abstract void Aplicar(GameObject player);
}