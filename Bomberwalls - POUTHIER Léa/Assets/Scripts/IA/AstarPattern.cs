using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AstarPattern : MonoBehaviour
{
    public InventoryUI Inventory { get; private set; }

    [field: SerializeField, FormerlySerializedAs("_wallNode")]
    public GetNodeInfos WallNode { get; private set; }

    private readonly List<List<GetNodeInfos>> _closedPaths = new();
    private readonly List<GetNodeInfos> _activePath = new();
    private ChooseShortestPath _choosePath;
    private MoveIA _moveIA;
    private NodeDistance _nodeDistance;

    private Coroutine _coroutine;

    private void Awake()
    {
        Inventory = GetComponent<InventoryUI>();
        _nodeDistance = GetComponent<NodeDistance>();
        _choosePath = GetComponent<ChooseShortestPath>();
        _moveIA = GetComponent<MoveIA>();
    }

    public void StartAstar(GetNodeInfos current, GetNodeInfos final)
    {
        if (_coroutine != null) StopCoroutine(_coroutine); 

        _activePath.Clear();
        _closedPaths.Clear();

        _activePath.Add(current);

        while (current != null && current != final)
        {
            _choosePath.ChooseShortest(_activePath, _nodeDistance, _closedPaths, final);
            current = GetClosestLink(final);
        }

        if (current == final)
        {
            _coroutine = StartCoroutine(_moveIA.MoveToNode(_activePath));
        }
    }

    public GetNodeInfos GetClosestLink(GetNodeInfos final)
    {
        List<GetNodeInfos> minLinkList = new() { _activePath[^1].Links[0] };

        foreach (GetNodeInfos link in _activePath[^1].Links)
        {
            if (_activePath.Contains(link)) continue;

            else if (_nodeDistance.GetF(link, final.gameObject, _activePath) < _nodeDistance.GetF(minLinkList[0], final.gameObject, _activePath))
            {
                minLinkList.Clear();
                minLinkList.Add(link);
            }

            else if(_nodeDistance.GetF(link, final.gameObject, _activePath) == _nodeDistance.GetF(minLinkList[0], final.gameObject, _activePath) && !minLinkList.Contains(link))
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

        return _activePath[^1];
    }
}
