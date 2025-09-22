using UnityEngine;

public class DragCar : MonoBehaviour
{
    [Header("Configuração do Carro")]
    public bool isPlayer = false;
    public float acceleration = 15f;
    public float maxSpeed = 320f; // velocidade máxima total do carro
    public float[] gearRatios = { 3.5f, 2.5f, 1.8f, 1.3f, 1.0f, 0.85f }; // 6 marchas
    public float shiftDelay = 0.5f;
    public float redlineRPM = 7000f;

    [Header("Status Atual")]
    public int currentGear = 0;
    public float currentRPM = 0f;
    public float speed = 0f;
    public bool raceStarted = false;

    [Header("Efeitos Visuais")]
    public CameraShake cameraShake; // arraste a Main Camera aqui
    public float shakeMagnitude = 0.08f; // intensidade do shake
    public float shakeDuration = 0.1f;   // duração do shake

    private float lastShiftTime = -10f;
    private Vector3 startPosition;

    private float aiShiftThreshold = 0.85f;
    private float aiErrorMargin = 0.15f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!raceStarted)
            return;

        if (isPlayer)
            HandlePlayerInput();
        else
            HandleAI();

        // Movimento do carro
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Camera Shake baseado na velocidade
        if (cameraShake != null && speed > maxSpeed * 0.5f)
        {
            float speedFactor = speed / maxSpeed; // aumenta o shake conforme a velocidade
            cameraShake.Shake(shakeDuration, shakeMagnitude * speedFactor);
        }
    }

    void HandlePlayerInput()
    {
        if (Input.GetKey(KeyCode.Space))
            Accelerate();

        if (Input.GetKeyDown(KeyCode.RightShift) && Time.time - lastShiftTime > shiftDelay)
        {
            ShiftUp();
            lastShiftTime = Time.time;
        }
    }

    void HandleAI()
    {
        Accelerate();

        if (currentRPM >= redlineRPM * (aiShiftThreshold + Random.Range(-aiErrorMargin, aiErrorMargin)))
        {
            if (currentGear < gearRatios.Length - 1 && Time.time - lastShiftTime > shiftDelay)
            {
                ShiftUp();
                lastShiftTime = Time.time;
            }
        }
    }

    void Accelerate()
    {
        float ratio = gearRatios[currentGear];

        // Calcula aumento de velocidade baseado na marcha atual
        float gearTopSpeed = maxSpeed * ((currentGear + 1f) / gearRatios.Length);
        speed += acceleration * ratio * Time.deltaTime;
        if (speed > gearTopSpeed) speed = gearTopSpeed;

        // Atualiza RPM
        currentRPM = (speed / gearTopSpeed) * redlineRPM * ratio;

        // Limite do redline
        if (currentRPM >= redlineRPM)
        {
            currentRPM = redlineRPM;
            speed -= acceleration * 0.5f * Time.deltaTime; // desaceleração leve se atingir redline
        }
    }

    void ShiftUp()
    {
        if (currentGear < gearRatios.Length - 1)
        {
            currentGear++;
            currentRPM *= 0.5f; // simula desaceleração momentânea ao trocar marcha
        }
    }

    public void ResetCar()
    {
        transform.position = startPosition;
        speed = 0f;
        currentGear = 0;
        currentRPM = 0f;
        raceStarted = false;
    }
}
