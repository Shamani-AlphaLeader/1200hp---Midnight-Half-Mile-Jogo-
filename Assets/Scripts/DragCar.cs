using UnityEngine;
using System.Collections;

public class DragCar : MonoBehaviour
{
    [Header("Configuração do Carro")]
    public bool isPlayer = false;
    public float acceleration = 10f;
    public float maxSpeed = 50f;
    public float[] gearRatios = { 3.5f, 2.5f, 1.8f, 1.3f, 1.0f };
    public float shiftDelay = 0.5f;
    public float redlineRPM = 7000f;

    [Header("Status Atual")]
    public int currentGear = 0;
    public float currentRPM = 0f;
    public float speed = 0f;
    public bool raceStarted = false;

    private float lastShiftTime = -10f;
    private Vector3 startPosition;

    private float aiShiftThreshold = 0.9f;
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
        speed += acceleration * ratio * Time.deltaTime;
        if (speed > maxSpeed) speed = maxSpeed;

        currentRPM = (speed / maxSpeed) * redlineRPM * ratio;

        if (currentRPM >= redlineRPM)
        {
            currentRPM = redlineRPM;
            currentRPM -= Mathf.PingPong(Time.time * 500f, 300f);
            speed -= acceleration * 0.5f * Time.deltaTime;
        }
    }

    void ShiftUp()
    {
        if (currentGear < gearRatios.Length - 1)
        {
            currentGear++;
            currentRPM *= 0.5f;
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
