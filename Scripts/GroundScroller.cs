using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 10f;
    [SerializeField] float tileLength = 80f;
    [SerializeField] Transform otherTile;

    void Update()
    {
        transform.Translate(Vector3.back * scrollSpeed * Time.deltaTime);

        // Resetea cuando el CENTRO del tile pasa el origen
        // en vez de esperar que salga completamente
        if (transform.position.z < -(tileLength * 0.5f))
        {
            float nuevaZ = otherTile.position.z + tileLength;
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                nuevaZ
            );
        }
    }
}
