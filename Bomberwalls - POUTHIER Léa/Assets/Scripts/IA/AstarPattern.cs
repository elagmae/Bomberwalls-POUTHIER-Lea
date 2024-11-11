using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AstarPattern : MonoBehaviour
{
    private GetNodeInfos _firstNode;
    [SerializeField]
    private GetNodeInfos _finalNode;
    [SerializeField]
    private GetNodeInfos _currentNode;

    private List<GetNodeInfos> _activePath = new List<GetNodeInfos>();
    private List<List<GetNodeInfos>> _closedPaths = new List<List<GetNodeInfos>>();
    private List<List<GetNodeInfos>> _tempClosedPaths = new List<List<GetNodeInfos>>();

    private void Awake()
    {
        _firstNode = _currentNode;

        _activePath.Add(_currentNode);
        _closedPaths.Add(new List<GetNodeInfos>() { _currentNode });
    }

    private void Update()
    {
        if(_finalNode != null && _currentNode != _finalNode)
        {
            print(GetPath().Count);
        }

        else
        {
            foreach (GetNodeInfos info in _activePath)
            {
                print("fin");
                Debug.Log(info.gameObject.name);
            }
        }
    }

    public List<GetNodeInfos> ChooseShortest(List<List<GetNodeInfos>> closedPath)
    {
        List<GetNodeInfos> minList = _activePath.ToList();

        foreach (List<GetNodeInfos> path in closedPath)
        {
            if (GetF(path[^1]) < GetF(minList[^1]))
            {
                minList = path.ToList();
            }
        }

        _activePath = minList.ToList();
        _currentNode = _activePath.ToList()[^1];
        print(_currentNode.gameObject.name);

        return _activePath;
    }

    public List<GetNodeInfos> GetPath()
    {
        _tempClosedPaths = _closedPaths.ToList();
        foreach (List<GetNodeInfos> path in _closedPaths.ToList())
        {
            if (path[^1] == _currentNode)
            {
                foreach (GetNodeInfos link in _currentNode.Links)
                {
                    List<GetNodeInfos> potentialPath = path.ToList();
                    _tempClosedPaths.Remove(path);

                    if (!potentialPath.Contains(link))
                    {
                        potentialPath.Add(link);
                        _activePath = potentialPath.ToList();
                    }

                    else
                    {
                        _activePath.Remove(link);
                    }

                    _tempClosedPaths.Add(potentialPath.ToList());
                    potentialPath.Clear();
                }

                break;
            }
        }

        _closedPaths = _tempClosedPaths;

        return ChooseShortest(_closedPaths);
    }

    // Valeurs calculées de chaque node.
    public float GetH(GetNodeInfos node)
    {
        node.H = (_finalNode.transform.position - node.transform.position).magnitude;
        return node.H;
    }

    public float GetG(GetNodeInfos node)
    {
        node.G = (_firstNode.transform.position - node.transform.position).magnitude;
        return node.G;
    }

    public float GetF(GetNodeInfos node)
    {
        node.F = GetH(node) + GetG(node);
        return node.F;
    }
}
