using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CarSelectionManager : MonoBehaviour
{
    [Header("UI")]
    public Text carNameText;
    public Button leftButton;
    public Button rightButton;
    public Button selectButton;

    [Header("Carros")]
    public List<GameObject> availableCars = new List<GameObject>(); // todos os carros possíveis
    public Transform raceStartPoint; // ponto inicial da corrida
    public RaceManager raceManager;

    private int currentIndex = 0;
    private GameObject playerCar;

    void Start()
    {
        UpdateCarDisplay();

        leftButton.onClick.AddListener(PreviousCar);
        rightButton.onClick.AddListener(NextCar);
        selectButton.onClick.AddListener(SelectCar);
    }

    void PreviousCar()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = availableCars.Count - 1;
        UpdateCarDisplay();
    }

    void NextCar()
    {
        currentIndex++;
        if (currentIndex >= availableCars.Count)
            currentIndex = 0;
        UpdateCarDisplay();
    }

    void UpdateCarDisplay()
    {
        if (carNameText != null && availableCars.Count > 0)
            carNameText.text = availableCars[currentIndex].name;
    }

    void SelectCar()
    {
        if (availableCars.Count == 0) return;

        // Marca o carro do jogador
        playerCar = availableCars[currentIndex];
        foreach (var car in availableCars)
        {
            DragCar dc = car.GetComponent<DragCar>();
            if (dc != null)
                dc.isPlayer = (car == playerCar);
        }

        // Posiciona o player na pista
        playerCar.transform.position = raceStartPoint.position;

        // Seleciona automaticamente os oponentes
        List<DragCar> opponents = new List<DragCar>();
        foreach (var car in availableCars)
        {
            if (car != playerCar)
            {
                car.transform.position = raceStartPoint.position + new Vector3(Random.Range(3f, 6f), 0f, 0f);
                DragCar dc = car.GetComponent<DragCar>();
                if (dc != null) opponents.Add(dc);
            }
        }

        // Passa referências pro RaceManager
        if (raceManager != null)
        {
            raceManager.playerCar = playerCar.GetComponent<DragCar>();
            raceManager.opponents = opponents;
        }

        // Esconde a tela de seleção
        gameObject.SetActive(false);

        Debug.Log("Carro selecionado: " + playerCar.name + " | Oponentes: " + opponents.Count);
    }
}
