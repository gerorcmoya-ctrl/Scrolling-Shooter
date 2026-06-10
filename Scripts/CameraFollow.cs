using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("Posicion")]
    [SerializeField] float height = 20f;
    [SerializeField] float zOffset = -4f;

    [Header("Angulo")]
    [SerializeField] float tiltAngle = 75f;

    [Header("Suavidad")]
    [SerializeField] float smoothing = 8f;

    // Shake
    Vector3 shakeOffset;

    public static CameraFollow Instance;

    void Awake()
    {
        Instance = this;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = new Vector3(
            target.position.x,
            target.position.y + height,
            target.position.z + zOffset
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos + shakeOffset,
            smoothing * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }

    public void Shake(float intensidad, float duracion)
    {
        StartCoroutine(ShakeRoutine(intensidad, duracion));
    }

    IEnumerator ShakeRoutine(float intensidad, float duracion)
    {
        Debug.Log($"Shake iniciado — intensidad: {intensidad} duracion: {duracion}");
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            shakeOffset = new Vector3(
                Random.Range(-intensidad, intensidad),
                0f,
                Random.Range(-intensidad, intensidad)
            );

            tiempo += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
        Debug.Log("Shake terminado");
    }
}