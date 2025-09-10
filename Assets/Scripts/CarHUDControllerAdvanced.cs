using UnityEngine;
using UnityEngine.UI;

public class CarHUDControllerAdvanced : MonoBehaviour
{
    [Header("Carro")]
    public DragCar playerCar;

    [Header("HUD Elements")]
    public RectTransform rpmPointer;   // ponteiro do conta-giros
    public Slider speedSlider;         // barra de velocidade
    public Text gearText;              // texto de marcha
    public Image rpmAlert;             // cor que acende no redline

    [Header("Configuração")]
    public float rpmMaxAngle = 270f;
    public float rpmMinAngle = -45f;
    public Color normalRPMColor = Color.white;
    public Color redlineColor = Color.red;
    public float redlineWarningPercent = 0.95f; // alerta quando chega a 95% do redline
    public float pointerShakeAmount = 5f;       // trepidação do ponteiro no corte de giro

    void Update()
    {
        if (playerCar == null) return;

        UpdateSpeed();
        UpdateRPM();
        UpdateGear();
    }

    void UpdateSpeed()
    {
        if (speedSlider != null)
            speedSlider.value = playerCar.speed / playerCar.maxSpeed;
    }

    void UpdateRPM()
    {
        if (rpmPointer == null) return;

        float rpmNormalized = Mathf.Clamp01(playerCar.currentRPM / playerCar.redlineRPM);
        float angle = Mathf.Lerp(rpmMinAngle, rpmMaxAngle, rpmNormalized);

        // 🔥 Trepidação se estiver no corte de giro
        if (playerCar.currentRPM >= playerCar.redlineRPM)
        {
            angle += Mathf.Sin(Time.time * 50f) * pointerShakeAmount;
        }

        rpmPointer.localEulerAngles = new Vector3(0f, 0f, -angle);

        // ⚠️ Cor de alerta
        if (rpmAlert != null)
        {
            rpmAlert.color = (rpmNormalized >= redlineWarningPercent) ? redlineColor : normalRPMColor;
        }
    }

    void UpdateGear()
    {
        if (gearText != null)
            gearText.text = "Marcha: " + (playerCar.currentGear + 1);
    }
}