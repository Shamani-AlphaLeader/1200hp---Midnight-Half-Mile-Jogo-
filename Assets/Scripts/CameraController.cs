using UnityEngine;

public class CameraController : MonoBehaviour
{
    public DragCar playerCar;       // arraste seu carro jogador aqui
    public CameraShake camShake;    // arraste a câmera com CameraShake aqui

    [Header("Shake Settings")]
    public float minSpeedForShake = 50f;   // a partir de qual velocidade começa a tremer
    public float maxSpeedForShake = 320f;  // velocidade máxima para efeito
    public float maxShakeMagnitude = 0.15f; // quanto a câmera pode tremer no máximo
    public float shakeDuration = 0.05f;    // duração de cada tremida

    void Update()
    {
        if (playerCar == null || camShake == null || !playerCar.raceStarted)
            return;

        // calcula intensidade baseada na velocidade
        float speedRatio = Mathf.InverseLerp(minSpeedForShake, maxSpeedForShake, playerCar.speed);
        float magnitude = speedRatio * maxShakeMagnitude;

        if (magnitude > 0f)
        {
            camShake.Shake(shakeDuration, magnitude);
        }
    }
}
