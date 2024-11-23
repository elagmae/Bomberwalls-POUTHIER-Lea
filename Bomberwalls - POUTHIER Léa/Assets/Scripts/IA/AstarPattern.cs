using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AstarPattern : MonoBehaviour
{
    [SerializeField]
    private GetNodeInfos _wallNode;
    public GameObject[] Bombs {get; private set;}
    private GetNodeInfos _finalNode;
    private GetNodeInfos _currentNode;
    private GetUsedNode _usedNode;

    [SerializeField]
    private GetNodeInfos _firstNode;
    private BombApparition _bombApparition;
    public InventoryUI Inventory { get; private set; }
    private MoveIA _moveIA;

    private List<GetNodeInfos> _activePath = new List<GetNodeInfos>();
    private List<List<GetNodeInfos>> _closedPaths = new List<List<GetNodeInfos>>();
    private List<List<GetNodeInfos>> _tempClosedPaths = new List<List<GetNodeInfos>>();

    public bool PathFinished { get; set; } = true;
    public bool FirstNodeGoten { get; set; } = false;

    public GameObject MinObject { get; set; } = null;
    public GameObject CurrentBomb { get; set; } = null;

    private void Awake()
    {
        _currentNode = _firstNode;
        _usedNode = GetComponent<GetUsedNode>();
        _bombApparition = ObjectPool.Instance.gameObject.GetComponent<BombApparition>();
        Inventory = GetComponent<InventoryUI>();
        _moveIA = GetComponent<MoveIA>();
    }

    private void Start()
    {
        _currentNode = _firstNode;
        Bombs = GameObject.FindGameObjectsWithTag("Bomb");
    }

    private void Update()
    {
        if (PathFinished)
        {
            _currentNode = _usedNode.UsedNode.GetComponent<GetNodeInfos>();

            if (Inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count == 0 && !FirstNodeGoten)
            {
                if (!Bombs.Any((b) => b.activeInHierarchy == true && b.tag == "Bomb")) return;

                FirstNodeGoten = true;
                _finalNode = GetClosestBomb();
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
        List<GameObject> bombs = new List<GameObject>();

        foreach (GameObject bomb in Bombs)
        {
            if(bomb.activeInHierarchy && bomb.tag == "Bomb")
            {
                bombs.Add(bomb);
            }
        }

        if (bombs.Count != 0)
        {
            MinObject = bombs[0];
            foreach (var bomb in bombs)
            {
                if (((bomb.transform.position) - this.transform.position).magnitude <= ((MinObject.transform.position) - this.transform.position).magnitude)
                {
                    MinObject = bomb;
                }
            }

            foreach (var node in _bombApparition.AllNodes)
            {
                if (MinObject.transform.position == node.gameObject.transform.position)
                {
                    CurrentBomb = MinObject;
                    return node;
                }
            }

            return GetClosestBomb();
        }

        return null;
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
