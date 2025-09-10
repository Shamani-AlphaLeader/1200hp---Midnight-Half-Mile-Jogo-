using UnityEngine;

public class Wheel : MonoBehaviour
{
    [Header("Referência ao Carro")]
    public DragCar car;                     // referência ao carro

    [Header("Configurações de Rotação")]
    public float maxRotationSpeed = 1000f;  // rotação máxima das rodas
    public float rotationAcceleration = 200f; // aceleração da rotação
    public float rotationDeceleration = 150f; // desaceleração quando carro reduz velocidade
    public float minRotationThreshold = 0.1f; // velocidade mínima para começar a girar

    private float currentRotationSpeed = 0f;

    void Update()
    {
        if (car == null || !car.raceStarted) return; // não gira se a corrida não começou

        // Velocidade máxima da marcha atual
        float maxSpeedForGear = car.maxSpeed * (car.gearRatios[car.currentGear] / car.gearRatios[car.gearRatios.Length - 1]);

        // Velocidade alvo da roda proporcional à velocidade do carro na marcha atual
        float targetRotationSpeed = car.speed / maxSpeedForGear * maxRotationSpeed;

        // Se o carro estiver praticamente parado, não gira
        if (car.speed < minRotationThreshold)
            targetRotationSpeed = 0f;

        // Suaviza aceleração ou desaceleração
        if (currentRotationSpeed < targetRotationSpeed)
            currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, targetRotationSpeed, rotationAcceleration * Time.deltaTime);
        else
            currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, targetRotationSpeed, rotationDeceleration * Time.deltaTime);

        // Aplica rotação
        transform.Rotate(0f, 0f, currentRotationSpeed * Time.deltaTime);
    }

    // 🔄 Reseta a roda ao reiniciar a corrida
    public void ResetWheel()
    {
        currentRotationSpeed = 0f;
        transform.localRotation = Quaternion.identity; // zera a rotação visual
    }
}