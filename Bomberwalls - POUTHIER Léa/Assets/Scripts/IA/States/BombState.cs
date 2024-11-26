using System.Linq;
using UnityEngine;

public class BombState : StatesBase
{
    private ChooseBomb _chooseBomb;
    private GetUsedNode _usedNode;
    private StateMachine _machine;

    private GetNodeInfos _lastNodes;

    public BombState(StateMachine machine)
    {
        _machine = machine;
        _chooseBomb = machine.GetComponent<ChooseBomb>();
        _usedNode = machine.GetComponent<GetUsedNode>();
    }

    public override void OnStateEnter()
    {
        Debug.Log("OnStateEnter -> BombState");
        _lastNodes = null;
    }

    public override void OnStateExit()
    {   
    }

    public override void OnUpdate()
    {
        Debug.Log("OnUpdate -> BombState");
        if (_machine.Astar.Inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count > 0)
        {
            _machine.TransitionTo(StateMachine.States.GoToWall);
            return;
        }

        if (!_chooseBomb.Bombs.Any((b) => b.activeInHierarchy == true && b.CompareTag("Bomb"))) return;

        var final = _chooseBomb.GetClosestBomb();

        if(_lastNodes != final) {
            _lastNodes = final;
            _machine.Astar.StartAstar(_usedNode.UsedNode, final);
        }
    }
}