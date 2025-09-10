using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private RaceManager raceManager;

    void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();
        if (raceManager == null)
        {
            Debug.LogError("FinishLine: Nenhum RaceManager encontrado na cena!");
        }
    }

    private void OnTriggerEnter(Collider other) // se for 3D
    {
        DragCar car = other.GetComponent<DragCar>();
        if (car != null && raceManager != null)
        {
            raceManager.FinishRace(car);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // se for 2D
    {
        DragCar car = other.GetComponent<DragCar>();
        if (car != null && raceManager != null)
        {
            raceManager.FinishRace(car);
        }
    }
}