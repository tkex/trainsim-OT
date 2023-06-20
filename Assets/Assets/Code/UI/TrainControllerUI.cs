
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;
using System.Reflection;
using System.Collections;

public class TrainControllerUI : MonoBehaviour
{
    public TrainController trainController;
    public PlayerNavigation playerNavigation;

    [Header("References")]
    public Slider numWagonsSlider;
    public Toggle useRandomStatesToggle;

    // Reference to the UI button for spawning the train
    public Button spawnTrainButton;

    // Reference to the UI button for spawning the train
    public Button moveTrainInsideButton;
    public Button moveTrainOutsideButton;
    public Button decoupleTrainButton;
    public Button encoupleTrainButton;

    // Symbiose of the four buttons on top
    public Button startMaintenanceButton;
    public Button completeMaintenanceButton;



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

    // Create a dictionary to store the wagon index and the corresponding hashset of unique tasks
    private Dictionary<int, HashSet<string>> wagonTasks = new Dictionary<int, HashSet<string>>();


    private int numWagons;
    private bool useRandomStates;

    void Start()
    {
        // Initialize the UI elements
        numWagonsSlider.onValueChanged.AddListener(OnNumWagonsChanged);
        useRandomStatesToggle.onValueChanged.AddListener(OnUseRandomStatesChanged);
        spawnTrainButton.onClick.AddListener(OnSpawnTrainClicked);

        // Additional button

        moveTrainInsideButton.onClick.AddListener(OnMoveTrainInsideClicked);
        decoupleTrainButton.onClick.AddListener(OnTrainDecoupleClicked);
        moveTrainOutsideButton.onClick.AddListener(OnMoveTrainOutsideClicked);
        encoupleTrainButton.onClick.AddListener(OnTrainEncoupleClicked);

        startMaintenanceButton.onClick.AddListener(OnStartMaintenanceButtonClicked);
        completeMaintenanceButton.onClick.AddListener(OnCompleteMaintenanceButtonClicked);

        // Set buttons to non-interactable at start
        startMaintenanceButton.interactable = false;
        completeMaintenanceButton.interactable = false;

        // Set the initial values
        numWagons = (int)numWagonsSlider.value;
        useRandomStates = useRandomStatesToggle.isOn;

        // Hide the tasks panel initially
        tasksPanel.SetActive(false);

        // Initialize the wagon task panels
        InitializeWagonTaskPanels();
    }

    void Update()
    {
        // Get the TrainStateMachine from the trainController
        var trainStateMachine = trainController.GetComponent<TrainStateMachine>();

        // If there is a TrainStateMachine, check the state
        if (trainStateMachine != null)
        {
            // Check maintenance status for maintained and actvicate the interactble
            if (trainStateMachine.trainState == TrainState.Maintained)
            {
                completeMaintenanceButton.interactable = true;
            }
            else
            {
                completeMaintenanceButton.interactable = false;
            }
        }
    }


    // Button StartMaintenance
    private void OnStartMaintenanceButtonClicked()
    {
        // Move train inside the hall
        OnMoveTrainInsideClicked();

        // Wait for the duration specified in trainMoveInDuration
        StartCoroutine(WaitForTrainMoveInDuration());

        // Deactivate startMaintenanceButton after starting maintenance
        startMaintenanceButton.interactable = false;

        // Show navigation to taskdashboard
        playerNavigation.ButtonIsClicked();
    }

    private IEnumerator WaitForTrainMoveInDuration()
    {
        // Get the trainMoveInDuration value
        float duration = 20f;

        // Wait for the duration
        yield return new WaitForSeconds(duration);
        
        // Open doors
        StartCoroutine(trainController.OpenAllDoors());

        // Decouple train
        OnTrainDecoupleClicked();
    }

    // Button CompleteMaintenance

    private void OnCompleteMaintenanceButtonClicked()
    {
        // Start coroutine to close doors and then encouple train
        StartCoroutine(CloseDoorsAndEncoupleTrain());

        // Deactivate completeMaintenanceButton after completing maintenance
        completeMaintenanceButton.interactable = false;
    }

    private IEnumerator CloseDoorsAndEncoupleTrain()
    {
        // Start coroutine to close doors and wait for it to finish
        yield return StartCoroutine(trainController.CloseAllDoors());

        // Encoupling train
        OnTrainEncoupleClicked();

        // Wait for  duration to move train outside
        StartCoroutine(WaitBeforeTrainMoveOut());
    }

    private IEnumerator WaitBeforeTrainMoveOut()
    {
        // Delay duration in seconds
        float delayDuration = 8f;

         yield return new WaitForSeconds(delayDuration);

        // Move train outside
        OnMoveTrainOutsideClicked();
    }

    // To get the wagon door handling of a wagon
    private WagonDoorHandling GetWagonDoorHandling(GameObject wagon)
    {
        WagonDoorHandling wagonDoorHandling = wagon.GetComponent<WagonDoorHandling>();

        if (wagonDoorHandling == null)
        {
            Debug.LogError("WagonDoorHandling not found: " + wagon.name);
        }

        return wagonDoorHandling;
    }

    // Button n's

    private void OnMoveTrainInsideClicked()
    {
        // Move train inside when button is pressed
        //StartCoroutine(trainController.DelayedStartOfTrainMovementInsideHall(1.0f));

        trainController.MoveTrainInsideHall();
    }

    private void OnMoveTrainOutsideClicked()
    {
        // Move train inside when button is pressed
        //StartCoroutine(trainController.DelayedStartOfTrainMovementInsideHall(1.0f));

        trainController.MoveTrainOutOfHall();
    }

    private void OnTrainDecoupleClicked()
    {
        // Decouple train
        StartCoroutine(trainController.ExecuteDecoupleAfterTime(1.0f));
    }

    private void OnTrainEncoupleClicked()
    {
        //Encouple train
        StartCoroutine(trainController.ExecuteEncoupleAfterTime(1.0f));
    }

    // --- 

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
                // 2.5 before
                Vector2 position = prevRectTransform.anchoredPosition + new Vector2(prevRectTransform.sizeDelta.x / 4.2f, 0f);
                rectTransform.anchoredPosition = position;
            }

            wagonTaskPanels.Add(wagonTaskPanel);

            // Set the wagon number in the panel's title
            wagonTaskPanel.GetComponentInChildren<TextMeshProUGUI>().text = $"Wagon {i + 1}";

            // Add listener to the Add Task button on this panel
            Button addTaskButton = wagonTaskPanel.GetComponentInChildren<Button>();
            addTaskButton.onClick.AddListener(() => OnAddTaskButtonClicked(wagonTaskPanel));

            // Initialize the hashset of tasks for this wagon
            wagonTasks[i] = new HashSet<string>();

            // Get the first task from the dropdown menu
            TMP_Dropdown firstDropdown = wagonTaskPanel.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();

            // Clear the default options from the dropdown
            firstDropdown.ClearOptions();

            // Add the available task types as new options to the dropdown
            List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
            List<Type> taskTypes = GetAllWagonTaskTypes();
            foreach (Type taskType in taskTypes)
            {
                dropdownOptions.Add(new TMP_Dropdown.OptionData(taskType.Name));
                taskTypeMapping[taskType.Name] = taskType;
            }
            firstDropdown.AddOptions(dropdownOptions);

            // Add to hashset
            string firstTask = firstDropdown.options[firstDropdown.value].text;
            wagonTasks[i].Add(firstTask);

            // Add listener to update the dictionary when the dropdown value changes
            int wagonIndex = i;
            firstDropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(wagonIndex, firstDropdown); });

            // # # # # # # # # # # # # # # 

            /*
            // Print the contents of the dictionary
            foreach (var entry in wagonTasks)
            {
                wagonIndex = entry.Key;
                HashSet<string> tasks = entry.Value;

                Debug.Log($"Wagon {wagonIndex + 1} has tasks:");

                foreach (string task in tasks)
                {
                    Debug.Log($" - {task}");
                }
            }
            */
        }
    }


    private List<Type> GetAllWagonTaskTypes()
    {
        List<Type> taskTypes = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(WagonTask)) && !type.IsAbstract)
                {
                    taskTypes.Add(type);
                }
            }
        }

        return taskTypes;
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

    private void OnDropdownValueChanged(int wagonIndex, TMP_Dropdown changedDropdown)
    {
        // Clear the HashSet for the wagonIndex before adding the new tasks
        wagonTasks[wagonIndex].Clear();

        // Get all dropdowns in the wagon task panel
        TMP_Dropdown[] dropdowns = wagonTaskPanels[wagonIndex].GetComponentsInChildren<TMP_Dropdown>();

        // Iterate through all dropdowns and add their values to the HashSet
        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            string task = dropdown.options[dropdown.value].text;
            wagonTasks[wagonIndex].Add(task);
        }

        // Print the updated wagon tasks for debugging
        //Debug.Log($"Wagon {wagonIndex + 1} tasks updated:");
        //foreach (string task in wagonTasks[wagonIndex])
        //{
        //    Debug.Log($" - {task}");
        //}
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

            // -0.15f before
            float yOffset = -0.10f * dropdownCount;
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

            // Get the wagonIndex using the panel index
            int wagonIndex = wagonTaskPanels.IndexOf(panel);

            // Update the wagonTasks dictionary with the new dropdown value
            OnDropdownValueChanged(wagonIndex, dropdown);

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
            StartCoroutine(DelayedAssignTasksToWagon(1.0f));
        }
        else
        {
            // ....
        }

        // Activate startMaintenanceButton after spawning the train thus the maintenance can start
        startMaintenanceButton.interactable = true;
    }

    private IEnumerator DelayedAssignTasksToWagon(float delay)
    {
        yield return new WaitForSeconds(delay);
        AssignTasksToWagon();
    }

    // Important: Must be refined and customized the moment the structure of the train (TrainController) changes.
    // Otherwise the UI wagon task mapping is not working properly. Might be overworked in the future.
    public void AssignTasksToWagon()
    {
        if (trainController.emptyTrainGameObject != null)
        {
            Debug.Log("Empty train game object found");

            int wagonDictTaskIndex = 0;

            for (int i = 1; i < trainController.emptyTrainGameObject.transform.childCount; i++)
            {
                Transform wagon = trainController.emptyTrainGameObject.transform.GetChild(i);
                WagonTaskAssigner wagonTaskAssigner = wagon.GetComponent<WagonTaskAssigner>();

                if (wagonTaskAssigner != null)
                {
                    Debug.Log("WagonTaskAssigner found " + wagon.name);

                    if (wagonTasks.TryGetValue(wagonDictTaskIndex, out HashSet<string> assignedTaskNames))
                    {
                        foreach (string taskName in assignedTaskNames)
                        {
                            if (taskTypeMapping.TryGetValue(taskName, out Type taskType))
                            {
                                WagonTask taskToAssign = (WagonTask)Activator.CreateInstance(taskType);
                                wagonTaskAssigner.AssignSpecificTaskToWagon(taskToAssign);
                            }
                        }

                        // Get reference to  WagonTaskHandling component
                        WagonTaskHandling wagonTaskHandling = wagon.GetComponent<WagonTaskHandling>();
                        if (wagonTaskHandling != null)
                        {
                            // Spawn the tasks of the wagon
                            wagonTaskHandling.SpawnTasks();
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No wagon tasks found " + wagon.name);
                    }

                    wagonDictTaskIndex++;
                }
                else
                {
                    Debug.LogWarning("WagonTaskAssigner not found " + wagon.name);
                }
            }
        }
        else
        {
            Debug.LogWarning("Empty train game object not found");
        }
    }
}

   