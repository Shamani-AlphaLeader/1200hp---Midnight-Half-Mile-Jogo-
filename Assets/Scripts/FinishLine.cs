using UnityEngine;

public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        DragCar car = other.GetComponent<DragCar>();
        if (car != null && car.raceStarted)
        {
            RaceManager rm = FindObjectOfType<RaceManager>();
            if (rm != null)
                rm.FinishRace(car);
        }
    }
}
