using UnityEngine;
using UnityEngine.UI;

public class RaceHUD : MonoBehaviour
{
    public DragCar playerCar;

    [Header("Referências UI")]
    public Text speedText;
    public Text rpmText;
    public Text gearText;

    void Update()
    {
        if (playerCar == null) return;

        // Velocidade
        speedText.text = "Velocidade: " + Mathf.Round(playerCar.speed) + " km/h";

        // RPM (arredondado)
        rpmText.text = "RPM: " + Mathf.Round(playerCar.currentRPM);

        // Marcha (+1 porque começa do 0 no array)
        gearText.text = "Marcha: " + (playerCar.currentGear + 1);
    }
}