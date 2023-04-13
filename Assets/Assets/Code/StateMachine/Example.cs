using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    private StateMachine stateMachine = new StateMachine();
    private ExampleObserver exampleObserver = new ExampleObserver();

    void Start()
    {
        stateMachine.ChangeState(new StateA());
        exampleObserver.OnNotify();

    }

    void Update()
    {
        stateMachine.Update();

        // Check if 'Enter' has been pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Change state to state B
            stateMachine.ChangeState(new StateB());
        }
    }

    private class StateA : State
    {
        public override void Enter()
        {
            Debug.Log("State A entered");
        }

        public override void Exit()
        {
            Debug.Log("State A exited");
        }

        public override void Update()
        {
            Debug.Log("State A updated");
        }
    }

    private class StateB : State
    {
        public override void Enter()
        {
            Debug.Log("State B entered");
        }

        public override void Exit()
        {
            Debug.Log("State B exited");
        }

        public override void Update()
        {
            Debug.Log("State B updated");
        }
    }

    private class ExampleObserver : Observer
    {
        public override void OnNotify()
        {
            Debug.Log("Observer notified");
        }
    }
}