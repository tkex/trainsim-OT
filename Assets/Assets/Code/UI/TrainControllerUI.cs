
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
    private List<GameObject> wagonTaskPanels = new List<GameObject>();


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
        // Destroy any existing wagon task panels
        foreach (var panel in wagonTaskPanels)
        {
            Destroy(panel);
        }

        // Instantiate new wagon task panels based on the number of wagons
        for (int i = 0; i < numWagons; i++)
        {
            GameObject panel = Instantiate(wagonTaskPanelPrefab, tasksPanel.transform);
            wagonTaskPanels.Add(panel);

            // Set the wagon number in the panel's title
            panel.GetComponentInChildren<TextMeshProUGUI>().text = $"Wagon {i + 1}";

            // Add a listener to the panel's "Add Task" button
            Button addTaskButton = panel.transform.Find("AddTaskButton").GetComponent<Button>();
            addTaskButton.onClick.AddListener(() => OnAddTaskButtonClicked(panel));
        }
    }

    void UpdateWagonTaskPanels()
    {
        // If the number of wagons has increased, instantiate new panels
        if (numWagons > wagonTaskPanels.Count)
        {
            for (int i = wagonTaskPanels.Count; i < numWagons; i++)
            {
                GameObject panel = Instantiate(wagonTaskPanelPrefab, tasksPanel.transform);
                wagonTaskPanels.Add(panel);

                // Set the wagon number in the panel's title
                panel.GetComponentInChildren<TextMeshProUGUI>().text = $"Wagon {i + 1}";

                // Add a listener to the panel's "Add Task" button
                Button addTaskButton = panel.transform.Find("AddTaskButton").GetComponent<Button>();
                addTaskButton.onClick.AddListener(() => OnAddTaskButtonClicked(panel));
            }
        }
        // If the number of wagons has decreased, destroy extra panels
        else if (numWagons < wagonTaskPanels.Count)
        {
            for (int i = wagonTaskPanels.Count - 1; i >= numWagons; i--)
            {
                GameObject panel = wagonTaskPanels[i];
                wagonTaskPanels.RemoveAt(i);
                Destroy(panel);
            }
        }
    }

    void OnAddTaskButtonClicked(GameObject panel)
    {
        Debug.Log("Task Dropdown MenuButton was pressed");
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

   