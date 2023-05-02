using UnityEngine;
using UnityEngine.UI;

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
    }

    void OnNumWagonsChanged(float value)
    {
        numWagons = (int)value;
    }

    void OnUseRandomStatesChanged(bool value)
    {
        useRandomStates = value;
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

        // Call the SpawnTrain method of the TrainController and pass in the configurator variables
        trainController.SpawnTrain(numWagons, useRandomStates);

        // Call the MoveTrainInHall method of the TrainController after the specified delay
        // Invoke("MoveTrainInHall", trainMoveInDelay);
    }
}