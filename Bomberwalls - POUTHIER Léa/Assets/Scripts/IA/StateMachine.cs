using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine : MonoBehaviour
{
    public enum States
    {
        GoToWall,
        GoToBomb,
    }

    [SerializeField]
    private States _defaultState;

    public StatesBase CurrentState { get; private set; }
    public List<StatesBase> statesBases;

    public AstarPattern Astar { get; private set; }

    private void Awake()
    {
        Astar = GetComponent<AstarPattern>();

        statesBases = new List<StatesBase>() { new WallState(this), new BombState(this) };

        CurrentState = statesBases[(int)_defaultState];
        CurrentState.OnStateEnter();

    }

    public void TransitionTo(States state)
    {
        CurrentState.OnStateExit();
        CurrentState = statesBases[(int)state];
        CurrentState.OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.OnUpdate();
    }
}
