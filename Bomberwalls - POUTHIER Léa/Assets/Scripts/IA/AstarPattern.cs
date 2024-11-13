using System.Collections.Generic;
using UnityEngine;

public class AstarPattern : MonoBehaviour
{
    [SerializeField]
    private GetNodeInfos _wallNode;
    private GameObject[] _bombs;
    private GetNodeInfos _finalNode;
    private GetNodeInfos _currentNode;

    [SerializeField]
    private GetNodeInfos _firstNode;
    private BombApparition _bombApparition;
    private InventoryUI _inventory;
    private MoveIA _moveIA;

    private List<GetNodeInfos> _activePath = new List<GetNodeInfos>();
    private List<List<GetNodeInfos>> _closedPaths = new List<List<GetNodeInfos>>();
    private List<List<GetNodeInfos>> _tempClosedPaths = new List<List<GetNodeInfos>>();

    private bool _pathFinished = true;
    private bool _firstNodeGotten = false;

    private GameObject _minObject;

    private void Awake()
    {
        _currentNode = _firstNode;
        _bombApparition = ObjectPool.Instance.gameObject.GetComponent<BombApparition>();
        _inventory = GetComponent<InventoryUI>();;
        _moveIA = GetComponent<MoveIA>();
    }

    private void Start()
    {
        _currentNode = null;
        _bombs = GameObject.FindGameObjectsWithTag("Bomb");

        if (_pathFinished)
        {
            _currentNode = null;
            if (_inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count == 0 && _finalNode == null && !_firstNodeGotten)
            {
                _firstNodeGotten = true;
                _finalNode = GetClosestBomb();
                StartAstar();
            }

            else if(_finalNode == null && _inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count > 0 && !_firstNodeGotten)
            {
                _firstNodeGotten = true;
                _finalNode = _wallNode;
                StartAstar();
            }
        }
    }

    public void StartAstar()
    {
        _pathFinished = false;

        //_activePath.Clear();
        //_closedPaths.Clear();
        //_tempClosedPaths.Clear();

        _activePath.Add(_currentNode);

        while (_currentNode != null && _currentNode != _finalNode)
        {
            print(_currentNode.gameObject.name);
            ChooseShortest();
            GetClosestLink();
        }

        if (_currentNode == _finalNode)
        {
            _finalNode = null;
            StartCoroutine(_moveIA.MoveToNode(_activePath));
        }
    }

    public GetNodeInfos GetClosestBomb()
    {
        if (_bombs.Length != 0)
        {
            _minObject = _bombs[0];
            foreach (var bomb in _bombs)
            {
                if (((bomb.transform.position) - this.transform.position).magnitude <= ((_minObject.transform.position) - this.transform.position).magnitude)
                {
                    _minObject = bomb;
                }
            }

            foreach (var node in _bombApparition.AllNodes)
            {
                if (_minObject.transform.position == node.gameObject.transform.position)
                {
                    return node;
                }
            }

            return GetClosestBomb();
        }

        return GetClosestBomb();

    }

    public List<GetNodeInfos> ChooseShortest()
    {
        List<GetNodeInfos> minList = new(_activePath);

        List<List<GetNodeInfos>> closedPath = new(_closedPaths);
        foreach (List<GetNodeInfos> path in closedPath)
        {
            if (GetF(path[^1]) < GetF(minList[^1]))
            {
                minList = new(path);
            }
        }

        _activePath = new(minList);

        if (_closedPaths.Contains(_activePath))
        {
            _closedPaths.Remove(_activePath);
        }

        return _activePath;
    }

    public List<GetNodeInfos> GetClosestLink()
    {
        List<GetNodeInfos> minLinkList = new() { _activePath[^1].Links[0] };

        foreach (GetNodeInfos link in _activePath[^1].Links)
        {
            if (_activePath.Contains(link)) continue;

            else if (GetF(link) < GetF(minLinkList[0]))
            {
                minLinkList.Clear();
                minLinkList.Add(link);
            }

            else if(GetF(link) == GetF(minLinkList[0]) && !minLinkList.Contains(link))
            {
                minLinkList.Add(link);
            }
        }

        for(int i = 1; i < minLinkList.Count; i++)
        {
            List<GetNodeInfos> tempActive = new(_activePath);
            tempActive.Add(minLinkList[i]);
            _closedPaths.Add(tempActive);
        }

        _activePath.Add(minLinkList[0]);
        _currentNode = _activePath[^1];

        return minLinkList;
    }

    // Valeurs calculées de chaque node.
    public float GetH(GetNodeInfos node)
    {
        node.H = (_finalNode.transform.position - node.transform.position).magnitude;
        return node.H;
    }

    public float GetG(GetNodeInfos node)
    {
        node.G = (_activePath.Count + 1);
        return node.G;
    }

    public float GetF(GetNodeInfos node)
    {
        node.F = GetH(node) + GetG(node);
        return node.F;
    }
}
