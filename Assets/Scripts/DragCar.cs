using UnityEngine;

public class DragCar : MonoBehaviour
{
    [Header("Configuração do Carro")]
    public bool isPlayer = false;
    public float acceleration = 10f;   // força base da aceleração
    public float maxSpeed = 50f;       // velocidade máxima final
    public float[] gearRatios = { 3.5f, 2.5f, 1.8f, 1.3f, 1.0f };
    public float shiftDelay = 0.5f;    // tempo mínimo entre trocas
    public float redlineRPM = 7000f;   // limite de RPM

    [Header("Status Atual (debug)")]
    public int currentGear = 0;
    public float currentRPM = 0f;
    public float speed = 0f;
    public bool raceStarted = false;

    private float lastShiftTime = -10f;
    private Vector3 startPosition; // posição inicial para reset

    // IA extra
    private float aiShiftThreshold = 0.9f; // troca quando passar de 90% do redline
    private float aiErrorMargin = 0.15f;   // margem de erro (chance de trocar cedo ou tarde)

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!raceStarted) return;

        if (isPlayer)
            HandlePlayerInput();
        else
            HandleAI();

        // aplica movimento no eixo X
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void HandlePlayerInput()
    {
        // 🚀 Espaço acelera
        if (Input.GetKey(KeyCode.Space))
            Accelerate();

        // 🔀 Shift direito troca marcha
        if (Input.GetKeyDown(KeyCode.RightShift) && Time.time - lastShiftTime > shiftDelay)
        {
            ShiftUp();
            lastShiftTime = Time.time;
        }
    }

    void HandleAI()
    {
        Accelerate();

        // IA troca quando chega perto do redline
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

        // aumenta velocidade gradualmente até maxSpeed
        speed += acceleration * ratio * Time.deltaTime;
        if (speed > maxSpeed)
            speed = maxSpeed;

        // RPM proporcional à velocidade
        currentRPM = (speed / maxSpeed) * redlineRPM * ratio;

        // 🚨 Corte de giro (rev limiter)
        if (currentRPM >= redlineRPM)
        {
            currentRPM = redlineRPM;

            // treme o RPM simulando corte
            currentRPM -= Mathf.PingPong(Time.time * 500f, 300f);

            // impede de acelerar mais
            speed -= acceleration * 0.5f * Time.deltaTime;
        }
    }

    void ShiftUp()
    {
        if (currentGear < gearRatios.Length - 1)
        {
            currentGear++;
            currentRPM *= 0.5f; // cai o giro ao trocar
        }
    }

    // 🔄 Reset para reiniciar corrida
    public void ResetCar()
    {
        transform.position = startPosition;
        speed = 0f;
        currentGear = 0;
        currentRPM = 0f;
        raceStarted = false;
    }
}