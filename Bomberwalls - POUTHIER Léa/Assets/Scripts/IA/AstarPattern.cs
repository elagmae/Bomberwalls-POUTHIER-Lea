using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AstarPattern : MonoBehaviour
{
    public bool PathFinished { get; set; } = true;
    public bool FirstNodeGoten { get; set; } = false;
    public InventoryUI Inventory { get; private set; }

    [SerializeField]
    private GetNodeInfos _firstNode;
    [SerializeField]
    private GetNodeInfos _wallNode;

    private readonly List<List<GetNodeInfos>> _closedPaths = new();
    private readonly List<List<GetNodeInfos>> _tempClosedPaths = new();
    private readonly List<GetNodeInfos> _activePath = new();
    private GetNodeInfos _finalNode;
    private GetNodeInfos _currentNode;
    private GetUsedNode _usedNode;
    private ChooseBomb _chooseBomb;
    private ChooseShortestPath _choosePath;
    private MoveIA _moveIA;
    private NodeDistance _nodeDistance;

    private void Awake()
    {
        _currentNode = _firstNode;
        _usedNode = GetComponent<GetUsedNode>();
        Inventory = GetComponent<InventoryUI>();
        _chooseBomb = GetComponent<ChooseBomb>();
        _nodeDistance = GetComponent<NodeDistance>();
        _choosePath = GetComponent<ChooseShortestPath>();
        _moveIA = GetComponent<MoveIA>();
    }

    private void Start()
    {
        _currentNode = _firstNode;
    }

    private void Update()
    {
        if (PathFinished)
        {
            _currentNode = _usedNode.UsedNode;

            if (Inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count == 0 && !FirstNodeGoten)
            {
                if (!_chooseBomb.Bombs.Any((b) => b.activeInHierarchy == true && b.CompareTag("Bomb"))) return;

                FirstNodeGoten = true;
                _finalNode = _chooseBomb.GetClosestBomb();
                StartAstar();
            }

            else if(Inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count > 0 && !FirstNodeGoten)
            {
                FirstNodeGoten = true;
                _finalNode = _wallNode;
                StartAstar();
            }

            PathFinished = false;
        }
    }

    public void StartAstar()
    {
        _activePath.Clear();
        _closedPaths.Clear();
        _tempClosedPaths.Clear();

        _activePath.Add(_currentNode);

        while (_currentNode != null && _currentNode != _finalNode)
        {
            _choosePath.ChooseShortest(_activePath,  _nodeDistance, _closedPaths, _finalNode);
            GetClosestLink();
        }

        if (_currentNode == _finalNode)
        {
            _finalNode = null;
            StartCoroutine(_moveIA.MoveToNode(_activePath));
        }
    }


    public List<GetNodeInfos> GetClosestLink()
    {
        List<GetNodeInfos> minLinkList = new() { _activePath[^1].Links[0] };

        foreach (GetNodeInfos link in _activePath[^1].Links)
        {
            if (_activePath.Contains(link)) continue;

            else if (_nodeDistance.GetF(link, _finalNode.gameObject, _activePath) < _nodeDistance.GetF(minLinkList[0], _finalNode.gameObject, _activePath))
            {
                minLinkList.Clear();
                minLinkList.Add(link);
            }

            else if(_nodeDistance.GetF(link, _finalNode.gameObject, _activePath) == _nodeDistance.GetF(minLinkList[0], _finalNode.gameObject, _activePath) && !minLinkList.Contains(link))
            {
                minLinkList.Add(link);
            }
        }

        for(int i = 1; i < minLinkList.Count; i++)
        {
            List<GetNodeInfos> tempActive = new(_activePath)
            {
                minLinkList[i]
            };
            _closedPaths.Add(tempActive);
        }

        _activePath.Add(minLinkList[0]);
        _currentNode = _activePath[^1];

        return minLinkList;
    }
}
