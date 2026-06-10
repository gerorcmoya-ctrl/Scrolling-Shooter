using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject prefab;
        public Vector3 posicion;    // donde spawnea
        public float delay;       // segundos de espera antes de spawnear este enemigo
    }

    [System.Serializable]
    public class Wave
    {
        public string nombre;
        public List<EnemySpawnData> enemigos;
        public float delayDespues = 3f;  // espera entre waves
    }

    [Header("Waves")]
    [SerializeField] List<Wave> waves;
    [Header("Boss")]
    [SerializeField] GameObject bossPrefab;
    [SerializeField] float bossSpawnZ = 20f;

    void SpawnBoss()
    {
        if (bossPrefab == null) return;

        Vector3 spawnPos = new Vector3(
            0f,
            jugador.position.y,
            jugador.position.z + 50f
        );

        Debug.Log($"Jugador Z: {jugador.position.z} | Boss spawn Z: {spawnPos.z}");
        Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    }
    [Header("Spawn")]
    [SerializeField] float spawnZ = 25f;  // distancia adelante del jugador donde spawnean

    Transform jugador;
    int enemigosVivos = 0;

    void Start()
    {
        var obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null) jugador = obj.transform;

        StartCoroutine(RunLevel());
    }

    IEnumerator RunLevel()
    {
        // Pequeña espera al inicio
        yield return new WaitForSeconds(2f);

        foreach (var wave in waves)
        {
            Debug.Log($"Iniciando wave: {wave.nombre}");
            yield return StartCoroutine(SpawnWave(wave));

            // Esperar que mueran todos los enemigos de la wave
            yield return new WaitUntil(() => enemigosVivos <= 0);
            Debug.Log($"Wave {wave.nombre} completada");

            yield return new WaitForSeconds(wave.delayDespues);
        }
        Debug.Log("¡Todas las waves completadas!");
        yield return new WaitForSeconds(3f);
        SpawnBoss();
    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (var data in wave.enemigos)
        {
            yield return new WaitForSeconds(data.delay);

            // Spawnear adelante del jugador
            Vector3 spawnPos = new Vector3(
                data.posicion.x,
                jugador.position.y,   // misma altura que el jugador
                jugador.position.z + spawnZ + data.posicion.z
            );

            var enemigo = Instantiate(data.prefab, spawnPos, Quaternion.identity);

            // Contar enemigos vivos
            enemigosVivos++;
            enemigo.GetComponent<EnemyHealth>().OnDeath += () => enemigosVivos--;
        }
    }
}