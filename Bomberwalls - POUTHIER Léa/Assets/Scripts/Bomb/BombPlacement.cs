using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BombPlacement : MonoBehaviour
{
    [field : SerializeField]
    public List<GetNodeInfos> AllNodes { get; private set; } = new();

    [SerializeField]
    private GetUsedNode _playerPlacement;
    [SerializeField]
    private GetUsedNode _iaPlacement;

    private readonly List<GameObject> _allNodesGo = new();

    private void Awake()
    {
        foreach(var node in AllNodes)
        {
            _allNodesGo.Add(node.gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < ObjectPool.Instance.AmountToPool; i++)
        {
            var bomb = ObjectPool.Instance.GetPooledObject();
            PlacementReset(bomb);
        }
    }

    // Permet aux bombes de se placer al�atoirement sur la grille, si la place est disponible (donc vide).
    public Vector3 RandomPlacement()
    {
        var choice = Random.Range(0, _allNodesGo.Count);

        Vector3 placement = Vector3.zero;

        if (_allNodesGo[choice] != null && _allNodesGo[choice] != _iaPlacement.UsedNode && _allNodesGo[choice] != _playerPlacement.UsedNode)
        {
            placement = _allNodesGo[choice].transform.position;

            foreach (var bnb in ObjectPool.Instance.ActivatedObjects) 
            {
                if(bnb.transform.position == placement) return RandomPlacement();
            }
        }

        return placement;
    }

    // Permet � une bombe de r�actualiser sa position, en utilisant la fonction de placement al�atoire.
    public void PlacementReset(GameObject bomb)
    {
        bomb.transform.position = RandomPlacement();
    }
}
