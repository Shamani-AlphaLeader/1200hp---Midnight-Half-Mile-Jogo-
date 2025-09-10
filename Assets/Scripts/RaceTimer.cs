using UnityEngine;
using TMPro;

public class RaceTimer : MonoBehaviour
{
    public DragCar playerCar;               // refer�ncia ao carro do jogador
    public TextMeshProUGUI timerText;       // UI TextMeshPro para mostrar o cron�metro

    private float raceTime = 0f;
    private bool raceActive = false;

    void Update()
    {
        if (playerCar == null) return;

        if (raceActive && playerCar.raceStarted)
            raceTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(raceTime / 60f);
        int seconds = Mathf.FloorToInt(raceTime % 60f);
        int centiseconds = Mathf.FloorToInt((raceTime * 100f) % 100f);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);
    }

    // Zera o cron�metro, chamado quando a corrida reinicia
    public void ResetTimer()
    {
        raceTime = 0f;
        raceActive = false;

        if (timerText != null)
            timerText.text = "00:00:00";
    }

    // Deve ser chamado pelo RaceManager quando o GO! aparecer
    public void StartTimer()
    {
        raceActive = true;
    }

    // Opcional: pausar o cron�metro quando a corrida termina
    public void StopTimer()
    {
        raceActive = false;
    }
}