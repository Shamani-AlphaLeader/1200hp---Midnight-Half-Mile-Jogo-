using UnityEngine;

public class PointerController : MonoBehaviour
{
    public DragCar playerCar; // Referência pro carro do jogador
    public float minAngle = -45f; // Ângulo do ponteiro no 0 km/h
    public float maxAngle = 225f; // Ângulo no máximo da escala

    void Update()
    {
        if (playerCar == null) return;

        // Normaliza a velocidade (0 → 1)
        float normalizedSpeed = playerCar.speed / playerCar.maxSpeed;

        // Calcula ângulo
        float angle = Mathf.Lerp(minAngle, maxAngle, normalizedSpeed);

        // Aplica rotação no ponteiro
        transform.localEulerAngles = new Vector3(0, 0, -angle);
    }
}