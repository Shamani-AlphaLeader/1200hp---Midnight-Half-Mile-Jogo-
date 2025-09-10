using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // carro do jogador
    public float smoothSpeed = 0.125f; // suavidade do movimento
    public float offsetX = 5f; // dist�ncia extra � frente do carro

    void LateUpdate()
    {
        if (target == null) return;

        // posi��o desejada s� no eixo X
        float desiredX = target.position.x + offsetX;

        // suaviza a transi��o
        float smoothedX = Mathf.Lerp(transform.position.x, desiredX, smoothSpeed);

        // aplica na c�mera (Y e Z ficam fixos)
        transform.position = new Vector3(smoothedX, transform.position.y, transform.position.z);
    }
}