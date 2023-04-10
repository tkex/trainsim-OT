using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WagonStateController : MonoBehaviour
{

    public Dictionary<GameObject, HashSet<WagonState>> wagonStatesDict = new Dictionary<GameObject, HashSet<WagonState>>();

    public void SetRandomWagonStates(GameObject wagonGO, int maxNumStates, HashSet<WagonState> existingStates)
    {
        int numStates = UnityEngine.Random.Range(1, maxNumStates + 1); // Get a random number of states for the wagon between 1 and maxNumStates    
        HashSet<WagonState> wagonStates = new HashSet<WagonState>(); // Create a HashSet to store the wagon states

        // Loop through and randomly select unique states for the wagon
        for (int i = 0; i < numStates; i++)
        {
            // Get a random WagonState that is not already in the wagonStates HashSet
            WagonState randomWagonState = GetRandomUniqueWagonState(wagonStates);

            // Add the WagonState to the wagonStates HashSet
            wagonStates.Add(randomWagonState);

            //Debug.Log("Wagon " + wagonGO.name + " State " + i + ": " + randomWagonState);
        }

        wagonStatesDict.Add(wagonGO, wagonStates); // Add the wagon and its states to the dictionary
    }

    // Helper method to get a random WagonState that is not already in the given list
    WagonState GetRandomUniqueWagonState(HashSet<WagonState> existingStates)
    {
        HashSet<WagonState> existingStateSet = new HashSet<WagonState>(existingStates); // Create a HashSet from the existing states list

        WagonState randomState = (WagonState)UnityEngine.Random.Range(0, Enum.GetValues(typeof(WagonState)).Length); // Get a random WagonState

        // Loop until the randomState is not already in the existingStates list
        while (existingStateSet.Contains(randomState))
        {
            randomState = (WagonState)UnityEngine.Random.Range(0, Enum.GetValues(typeof(WagonState)).Length); // Get a new random WagonState
        }

        return randomState;
    }


    // Add a state to a wagon's HashSet of states in the dictionary
    public void AddState(GameObject wagonGO, WagonState state)
    {
        // Check if the wagon exists in the dictionary
        if (wagonStatesDict.ContainsKey(wagonGO))
        {
            // Add the state to the wagon's HashSet of states
            wagonStatesDict[wagonGO].Add(state);
        }
        else
        {
            Debug.LogError("Wagon " + wagonGO.name + " does not exist in the dictionary!");
        }
    }

    // Remove a state from a wagon's HashSet of states in the dictionary
    public void RemoveState(GameObject wagonGO, WagonState state)
    {
        // Check if the wagon exists in the dictionary
        if (wagonStatesDict.ContainsKey(wagonGO))
        {
            // Remove the state from the wagon's HashSet of states
            wagonStatesDict[wagonGO].Remove(state);
        }
        else
        {
            Debug.LogError("Wagon " + wagonGO.name + " does not exist in the dictionary!");
        }
    }

    // Show all wagon states in the dictionary
    public void ShowAllWagonStates()
    {
        foreach (var kvp in wagonStatesDict)
        {
            GameObject wagonGO = kvp.Key;
            HashSet<WagonState> wagonStates = kvp.Value;

            Debug.Log("Wagon " + wagonGO.name + " states:");

            foreach (WagonState state in wagonStates)
            {
                Debug.Log(state);
            }
        }
    }

    // Remove all states from all wagons in the dictionary
    public void CleanStates()
    {
        // Loop through all wagons in the dictionary
        foreach (GameObject wagonGO in wagonStatesDict.Keys)
        {
            // Remove all states from the wagon's HashSet of states
            wagonStatesDict[wagonGO].Clear();
        }
    }
}