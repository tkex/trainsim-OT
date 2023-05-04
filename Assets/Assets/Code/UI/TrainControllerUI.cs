
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;

public class TrainControllerUI : MonoBehaviour
{
    public TrainController trainController;

    [Header("References")]
    public Slider numWagonsSlider;
    public Toggle useRandomStatesToggle;

    // Reference to the UI button for spawning the train
    public Button spawnTrainButton;

    // Delay before the train moves into the platform
    public float trainMoveInDelay = 2f;

    // References to the UI elements for assigning tasks to wagons
    public GameObject tasksPanel;

    // Reference to the wagon task panel prefab (containing structure=
    public GameObject wagonTaskPanelPrefab;
    public Dropdown dropdownPrefab;

    private List<GameObject> wagonTaskPanels = new List<GameObject>();

    // Define the horizontal distance between the wagon task panels
    private const float panelSpacing = 20f;

    private int numWagons;
    private bool useRandomStates;

    void Start()
    {
        // Initialize the UI elements
        numWagonsSlider.onValueChanged.AddListener(OnNumWagonsChanged);
        useRandomStatesToggle.onValueChanged.AddListener(OnUseRandomStatesChanged);
        spawnTrainButton.onClick.AddListener(OnSpawnTrainClicked);

        // Set the initial values
        numWagons = (int)numWagonsSlider.value;
        useRandomStates = useRandomStatesToggle.isOn;

        // Hide the tasks panel initially
        tasksPanel.SetActive(false);

        // Initialize the wagon task panels
        InitializeWagonTaskPanels();
    }

    void OnNumWagonsChanged(float value)
    {
        numWagons = (int)value;

        // Update the wagon task panels based on the new number of wagons
        UpdateWagonTaskPanels();
    }

    void OnUseRandomStatesChanged(bool value)
    {
        useRandomStates = value;

        // Show or hide the tasks panel depending on whether useRandomStates is selected or not
        tasksPanel.SetActive(!useRandomStates);

        // Initialize the wagon task panels if useRandomStates is false
        if (!useRandomStates)
        {
            UpdateWagonTaskPanels();
        }
    }

    void InitializeWagonTaskPanels()
    {
        // Instantiate wagonTaskPanelPrefab for each wagon
        for (int i = 0; i < numWagons; i++)
        {
            GameObject wagonTaskPanel = Instantiate(wagonTaskPanelPrefab, tasksPanel.transform);

            // Set the position of the wagonTaskPanel
            if (i > 0)
            {
                RectTransform rectTransform = wagonTaskPanel.GetComponent<RectTransform>();
                RectTransform prevRectTransform = wagonTaskPanels[i - 1].GetComponent<RectTransform>();
                Vector2 position = prevRectTransform.anchoredPosition + new Vector2(prevRectTransform.sizeDelta.x / 2.5f, 0f);
                rectTransform.anchoredPosition = position;
            }

            wagonTaskPanels.Add(wagonTaskPanel);

            // Set the wagon number in the panel's title
            wagonTaskPanel.GetComponentInChildren<TextMeshProUGUI>().text = $"Wagon {i + 1}";

            // Add listener to the Add Task button on this panel
            Button addTaskButton = wagonTaskPanel.GetComponentInChildren<Button>();
            addTaskButton.onClick.AddListener(() => OnAddTaskButtonClicked(wagonTaskPanel));
        }
    }
    void UpdateWagonTaskPanels()
    {
        // Destroy all existing wagon task panels
        foreach (GameObject panel in wagonTaskPanels)
        {
            Destroy(panel);
        }

        wagonTaskPanels.Clear();

        // Create new wagon task panels based on the updated number of wagons
        InitializeWagonTaskPanels();
    }
    public void OnAddTaskButtonClicked(GameObject panel)
    {
        // Find the "Dropdown" child element of the panel
        Transform dropdownTransform = panel.transform.Find("Dropdown");

        if (dropdownTransform != null)
        {
            // Instantiate a new dropdown by copying the found dropdown
            GameObject newDropdown = Instantiate(dropdownTransform.gameObject, panel.transform);

            // Calculate the new position based on the number of dropdown menus
            int dropdownCount = panel.transform.childCount - 3; // Exclude the "Dropdown" template
            float yOffset = -0.15f * dropdownCount;
            Vector3 newPosition = new Vector3(dropdownTransform.position.x, dropdownTransform.position.y + yOffset, dropdownTransform.position.z);
            newDropdown.transform.position = newPosition;

            // Add the new dropdown to the panel
            newDropdown.transform.SetParent(panel.transform);
        }
    }

    void OnSpawnTrainClicked()
    {
        // Destroy the old train if it exists
        if (trainController.emptyTrainGameObject != null)
        {
            Destroy(trainController.emptyTrainGameObject);
        }

        // Set the number of wagons and the random states flag based on the UI elements
        int numWagons = (int)numWagonsSlider.value;
        bool useRandomStates = useRandomStatesToggle.isOn;

        if (useRandomStates)
        {
            // Call the SpawnTrain method of the TrainController and pass in the configurator variables
            trainController.SpawnTrain(numWagons, useRandomStates);
        }
        // If not random, assign tasks manually
        else if (!useRandomStates)
        {
            // Call the SpawnTrain method of the TrainController and pass in the configurator variables
            trainController.SpawnTrain(numWagons, useRandomStates);

            // Then assign the UI tasks to the spawned wagons
            //AssignTasksToWagons();
        }

        // Move the train into the platform after a delay
        //Invoke("MoveTrainInHall", trainMoveInDelay);
    }


}

   