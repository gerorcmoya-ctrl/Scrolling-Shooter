using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float velocidad = 20f;
    [SerializeField] int daño = 1;

    Vector3 direccion;
    bool esEnemiga;
    GameObject dueno;

    public void Init(Vector3 dir, bool isEnemy, GameObject owner = null)
    {
        direccion = dir.normalized;
        esEnemiga = isEnemy;
        dueno = owner;
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += direccion * velocidad * Time.deltaTime;
        // Debug.Log($"Bala pos: {transform.position} | dir: {direccion} | esEnemiga: {esEnemiga}");
    }

    void OnTriggerEnter(Collider other)
    {

        if (dueno != null && other.gameObject == dueno) return;
        if (dueno != null && other.transform.IsChildOf(dueno.transform)) return;

        Debug.Log($"Bala tocó: {other.gameObject.name} | Tag: {other.tag} | EsEnemiga: {esEnemiga}");

        if (esEnemiga && (other.CompareTag("Player") || other.GetComponentInParent<PlayerHealth>() != null))
        {
            var playerHealth = other.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("DAÑO AL PLAYER");
                playerHealth.TakeDamage(daño);
                Destroy(gameObject);
            }
        }
        else if (!esEnemiga && (other.CompareTag("Enemy") || other.GetComponentInParent<EnemyHealth>() != null))
        {
            var enemyHealth = other.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("DAÑO AL ENEMIGO — vida restante debería bajar");
                enemyHealth.TakeDamage(daño);
                Destroy(gameObject);
            }

        }// Agregar después del bloque del enemigo
        else if (!esEnemiga && other.CompareTag("Boss"))
        {
            other.GetComponentInParent<BossHealth>()?.TakeDamage(daño);
            Destroy(gameObject);
        }
        else if (!esEnemiga && other.GetComponentInParent<BossHealth>() != null)
        {
            other.GetComponentInParent<BossHealth>()?.TakeDamage(daño);
            Destroy(gameObject);
        }
    }
}