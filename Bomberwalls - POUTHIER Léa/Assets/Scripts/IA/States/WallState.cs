using UnityEngine;

public class WallState : StatesBase
{
    private GetUsedNode _usedNode;
    private StateMachine _machine;

    public WallState(StateMachine machine)
    {
        _machine = machine;
        _usedNode = machine.GetComponent<GetUsedNode>();
    }

    public override void OnStateEnter()
    {
        _machine.Astar.StartAstar(_usedNode.UsedNode, _machine.Astar.WallNode);
    }

    public override void OnStateExit()
    {
    }

    public override void OnUpdate()
    {
        if (_machine.Astar.Inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count == 0)
        {
            _machine.TransitionTo(StateMachine.States.GoToBomb);
            return;
        }
    }
}

