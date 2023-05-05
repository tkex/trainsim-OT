
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;
using System.Reflection;

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

    // Define the horizontal distance between the wagon task panels
    private const float panelSpacing = 20f;

    private GameObject firstDropdownPanel;
    public Dictionary<string, Type> taskTypeMapping = new Dictionary<string, Type>();

    public List<Dropdown> taskDropdowns;

    // Wagon index and their tasks
    private Dictionary<int, List<WagonTask>> wagonTaskMapping;


    private int numWagons;
    private bool useRandomStates;

    void Start()
    {
        // Find the first dropdown menu in the tasks panel and disable it
        DisableFirstDropdownMenu();

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

        // Init dictionary
        wagonTaskMapping = new Dictionary<int, List<WagonTask>>();

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

        // Disable the first dropdown menu in the first panel
        DisableFirstDropdownMenu();
    }

    void DisableFirstDropdownMenu()
    {
        // Get the first dropdown menu in the tasks panel
        TMP_Dropdown firstDropdown = tasksPanel.GetComponentInChildren<TMP_Dropdown>();

        // If a dropdown menu exists and it has options, disable it
        if (firstDropdown != null && firstDropdown.options.Count > 0)
        {
            firstDropdown.interactable = false;
            firstDropdown.captionText.color = Color.gray;
            firstDropdown.value = 0;

            // Add a listener to the first dropdown to handle value changes
            firstDropdown.onValueChanged.AddListener((value) => OnTaskDropdownValueChanged(wagonTaskPanels[0], firstDropdown));
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

            // Get all available WagonTask types
            List<Type> taskTypes = GetAllWagonTaskTypes(panel);

            // Get the TMP_Dropdown component from the newDropdown game object
            TMP_Dropdown dropdown = newDropdown.GetComponent<TMP_Dropdown>();

            // Clear the default options from the dropdown
            dropdown.ClearOptions();

            // Add the available task types as new options to the dropdown
            List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
            foreach (Type taskType in taskTypes)
            {
                string taskTypeName = taskType.Name;

                // Check if the task type is already selected in another dropdown in this panel
                bool taskTypeSelected = false;

                // Get all dropdown menus in the same panel
                TMP_Dropdown[] dropdownsInPanel = panel.GetComponentsInChildren<TMP_Dropdown>();

                // Loop through each dropdown and check if the task type is selected
                foreach (TMP_Dropdown d in dropdownsInPanel)
                {
                    if (d.value > 0 && d.options[d.value].text == taskTypeName)
                    {
                        taskTypeSelected = true;
                        break;
                    }
                }

                // Add the task type to the dropdown options if it's not already selected
                if (!taskTypeSelected)
                {
                    dropdownOptions.Add(new TMP_Dropdown.OptionData(taskTypeName));
                    taskTypeMapping[taskTypeName] = taskType;
                }
            }

            dropdown.AddOptions(dropdownOptions);

            // Add a listener to the dropdown to handle value changes
            dropdown.onValueChanged.AddListener((value) => OnTaskDropdownValueChanged(panel, dropdown));

            // Call the OnTaskDropdownValueChanged method for the newly added dropdown
            OnTaskDropdownValueChanged(panel, dropdown);


            // Set the value of the new dropdown to the first option if available
            if (dropdown.options.Count > 0)
            {
                dropdown.value = 0;
            }
            else
            {
                // Disable the dropdown if there are no options
                dropdown.interactable = false;
            }

            // Disable the "+" button if all task types are already selected in the other dropdown menus
            bool allTaskTypesSelected = true;
            foreach (Type taskType in taskTypes)
            {
                if (!IsTaskTypeSelected(taskType.Name, panel))
                {
                    allTaskTypesSelected = false;
                    break;
                }
            }

            Button addTaskButton = panel.GetComponentInChildren<Button>();
            addTaskButton.interactable = !allTaskTypesSelected;
        }
    }

    private void OnTaskDropdownValueChanged(GameObject panel, TMP_Dropdown dropdown)
    {
        int wagonIndex = wagonTaskPanels.IndexOf(panel);

        // Get the selected task type name from the dropdown
        string taskTypeName = dropdown.options[dropdown.value].text;

        // Get the task type from the taskTypeMapping dictionary
        Type taskType;
        if (taskTypeMapping.TryGetValue(taskTypeName, out taskType))
        {
            // Create a new instance of the task type and add it to the wagon
            WagonTask task = (WagonTask)Activator.CreateInstance(taskType);
            AddTaskToWagon(wagonIndex, task);
        }
        else
        {
            // Remove the task from the wagon if the dropdown value is changed to none
            RemoveTaskFromWagon(wagonIndex);
        }
    }

    private bool IsTaskTypeSelected(string taskTypeName, GameObject panel)
    {
        // Check all dropdowns in the panel to see if they have the task type selected
        foreach (TMP_Dropdown dropdown in panel.GetComponentsInChildren<TMP_Dropdown>())
        {
            if (dropdown != null)
            {
                TMP_Dropdown.OptionData selectedOption = dropdown.options[dropdown.value];
                if (selectedOption != null && selectedOption.text == taskTypeName)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private List<Type> GetAllWagonTaskTypes(GameObject panel)
    {
        firstDropdownPanel = tasksPanel.transform.GetChild(0).gameObject;

        List<Type> taskTypes = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(WagonTask)) && !type.IsAbstract)
                {
                    // Check if the wagon task type is already selected in another dropdown in this panel or the first dropdown
                    bool taskTypeSelected = IsTaskTypeSelected(type.Name, panel) || IsTaskTypeSelected(type.Name, firstDropdownPanel);

                    // Add the task type to the list if it's not already selected
                    if (!taskTypeSelected)
                    {
                        taskTypes.Add(type);
                    }
                }
            }
        }

        return taskTypes;
    }

    public void AddTaskToWagon(int wagonIndex, WagonTask task)
    {
        if (!wagonTaskMapping.ContainsKey(wagonIndex))
        {
            wagonTaskMapping[wagonIndex] = new List<WagonTask>();
        }

        wagonTaskMapping[wagonIndex].Add(task);
        Debug.Log($"Added task {task.GetType().Name} to wagon {wagonIndex + 1}");
    }

    public void RemoveTaskFromWagon(int wagonIndex)
    {
        if (wagonTaskMapping.ContainsKey(wagonIndex))
        {
            wagonTaskMapping[wagonIndex].Clear();
            Debug.Log($"Removed task from wagon {wagonIndex + 1}");
        }
    }


    public void AssignTasksToWagons()
    {
        foreach (var entry in wagonTaskMapping)
        {
            int wagonIndex = entry.Key;
            List<WagonTask> tasks = entry.Value;

            foreach (WagonTask task in tasks)
            {
                trainController.AssignTaskToWagon(wagonIndex, task);
                Debug.Log($"Assigned task {task.GetType().Name} to wagon {wagonIndex + 1}");
            }
        }
    }

    public void ShowWagonTaskMapping()
    {
        Debug.Log("WagonTaskMapping contents:");

        foreach (var entry in wagonTaskMapping)
        {
            int wagonIndex = entry.Key;
            List<WagonTask> tasks = entry.Value;

            Debug.Log($"Wagon {wagonIndex + 1}:");

            foreach (WagonTask task in tasks)
            {
                Debug.Log($"- Task: {task.GetType().Name}");
            }
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

        trainController.SpawnTrain(numWagons, useRandomStates);

        if (!useRandomStates)
        {
            AssignTasksToWagons();
        }
        else
        {
            // ....
        }

        ShowWagonTaskMapping();
    }
}

   