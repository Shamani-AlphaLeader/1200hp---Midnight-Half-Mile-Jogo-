using UnityEngine;
using UnityEngine.UI;

public class HUDSpeedController : MonoBehaviour
{
    public DragCar playerCar;  // seu carro
    public Slider speedSlider; // velocímetro

    void Update()
    {
        if (playerCar == null || speedSlider == null) return;

        // Atualiza a barra de velocidade proporcional à velocidade do carro
        speedSlider.value = playerCar.speed / playerCar.maxSpeed;
    }
}