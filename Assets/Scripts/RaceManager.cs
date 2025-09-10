using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RaceManager : MonoBehaviour
{
    [Header("Carros")]
    public DragCar playerCar;
    public List<DragCar> opponents = new();

    [Header("UI / Cronômetro")]
    public RaceTimer raceTimer; // arraste o RaceTimer do Canvas aqui

    private bool raceFinished = false;
    private bool countdownRunning = false;

    private float goTime = 0f;
    public float startTolerance = 0.2f; // tolerância de largada

    void Awake()
    {
        TryAutoWire();
    }

    void Start()
    {
        if (!ValidateRefs()) return;
        StartCoroutine(StartCountdown());
    }

    void Update()
    {
        if (!raceFinished && playerCar != null)
        {
            // 🚨 Queima de largada (com tolerância)
            if (countdownRunning && !playerCar.raceStarted && Input.GetKey(KeyCode.Space))
            {
                if (Time.time < goTime - startTolerance)
                {
                    BurnoutStart();
                }
            }
        }
    }

    IEnumerator StartCountdown()
    {
        foreach (var car in AllCars())
            car.raceStarted = false;

        countdownRunning = true;

        Debug.Log("3");
        yield return new WaitForSeconds(1f);
        Debug.Log("2");
        yield return new WaitForSeconds(1f);
        Debug.Log("1");
        yield return new WaitForSeconds(1f);

        // GO!
        Debug.Log("GO!");
        goTime = Time.time; // momento exato do GO
        countdownRunning = false;

        foreach (var car in AllCars())
        {
            if (car.isPlayer)
            {
                car.raceStarted = true;

                // 🔹 inicia o cronômetro do jogador
                if (raceTimer != null)
                    raceTimer.StartTimer();
            }
            else
            {
                float delay = Random.Range(0f, 0.3f); // atraso da IA
                StartCoroutine(StartAIRaceWithDelay(car, delay));
            }
        }
    }

    private IEnumerator StartAIRaceWithDelay(DragCar car, float delay)
    {
        car.raceStarted = false;
        yield return new WaitForSeconds(delay);
        car.raceStarted = true;
    }

    public void FinishRace(DragCar winner)
    {
        if (raceFinished) return;

        raceFinished = true;

        foreach (var car in AllCars())
            car.raceStarted = false;

        // Pausa o cronômetro
        if (raceTimer != null)
            raceTimer.StopTimer();

        if (winner != null && winner == playerCar)
            Debug.Log("🏆 Você venceu!");
        else
            Debug.Log($"❌ Oponente venceu! ({(winner ? winner.name : "desconhecido")})");

        StartCoroutine(RestartRaceAfterDelay(2f));
    }

    private void BurnoutStart()
    {
        raceFinished = true;
        countdownRunning = false;

        foreach (var car in AllCars())
            car.raceStarted = false;

        // Pausa o cronômetro
        if (raceTimer != null)
            raceTimer.StopTimer();

        Debug.Log("🚨 Queimou a largada!");
        StartCoroutine(RestartRaceAfterDelay(2f));
    }

    IEnumerator RestartRaceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var car in AllCars())
        {
            if (car != null)
            {
                car.ResetCar();

                // se não for o player, aplica variação pequena
                if (!car.isPlayer)
                {
                    car.acceleration *= Random.Range(0.95f, 1.05f); // +/-5%
                    car.maxSpeed *= Random.Range(0.98f, 1.02f);    // +/-2%
                }
            }
        }

        // 🔄 Reseta o cronômetro antes do novo countdown
        if (raceTimer != null)
            raceTimer.ResetTimer();

        raceFinished = false;
        StartCoroutine(StartCountdown());
    }

    // ===== Helpers =====

    IEnumerable<DragCar> AllCars()
    {
        if (playerCar != null) yield return playerCar;
        if (opponents != null)
        {
            foreach (var c in opponents)
                if (c != null) yield return c;
        }
    }

    void TryAutoWire()
    {
        var cars = FindObjectsOfType<DragCar>();

        // Player
        if (playerCar == null)
        {
            playerCar = cars.FirstOrDefault(c => c.isPlayer);
            if (playerCar == null)
                playerCar = cars.FirstOrDefault(c => c.name == "Supra");
        }

        // Oponentes
        if (opponents == null || opponents.Count == 0)
        {
            opponents = cars.Where(c => c != playerCar).ToList();
        }
    }

    bool ValidateRefs()
    {
        if (playerCar == null)
        {
            Debug.LogError("RaceManager: playerCar não definido e não foi possível auto-detectar.");
            enabled = false;
            return false;
        }

        if (opponents == null)
            opponents = new List<DragCar>();

        if (opponents.Count == 0)
            Debug.LogWarning("RaceManager: nenhum oponente encontrado.");

        return true;
    }
}