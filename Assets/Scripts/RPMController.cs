using UnityEngine;

public class RPMController : MonoBehaviour
{
    public DragCar playerCar; // Referência pro carro do jogador
    public float minAngle = -45f;  // Ângulo no 0 RPM
    public float maxAngle = 225f;  // Ângulo no redline

    void Update()
    {
        if (playerCar == null) return;

        // Normaliza RPM (0 → 1)
        float normalizedRPM = playerCar.currentRPM / playerCar.redlineRPM;

        // Calcula ângulo
        float angle = Mathf.Lerp(minAngle, maxAngle, normalizedRPM);

        // Aplica rotação no ponteiro
        transform.localEulerAngles = new Vector3(0, 0, -angle);
    }
}