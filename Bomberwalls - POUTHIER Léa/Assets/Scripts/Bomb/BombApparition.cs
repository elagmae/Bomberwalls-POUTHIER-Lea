using System.Collections.Generic;
using UnityEngine;

public class BombApparition : MonoBehaviour
{
    public List<GetNodeInfos> AllNodes { get; set; } = new List<GetNodeInfos>();

    [SerializeField]
    private GetUsedNode _playerPlacement;
    private GameObject[] _allNodes;

    private List<Vector2> _placements = new List<Vector2>();
    [SerializeField]
    private GetUsedNode _iaPlacement;

    private void Awake()
    {
        _allNodes = GameObject.FindGameObjectsWithTag("Ground");
    }

    private void Start()
    {
        for (int i = 0; i < ObjectPool.Instance.AmountToPool; i++)
        {
            var bomb = ObjectPool.Instance.GetPooledObject();
            PlacementReset(bomb);
        }
    }

    public List<Vector2> RandomPlacement()
    {
        var choice = Random.Range(0, _allNodes.Length);

        if (_allNodes[choice] != null && _allNodes[choice] != _iaPlacement.UsedNode && _allNodes[choice] != _playerPlacement.UsedNode)
        {
            if (!_placements.Contains(_allNodes[choice].transform.position))
            {
                _placements.Add(_allNodes[choice].transform.position);
                return _placements;
            }
        }

        return RandomPlacement();
        
    }

    public void PlacementReset(GameObject bomb)
    {
        bomb.SetActive(true);
        bomb.transform.position = RandomPlacement()[0];
        if (_placements.Count > 0)
        {
            _placements.Remove(_placements[0]);
        }
    }

    private void OnDisable()
    {
        _placements.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            print(collision.gameObject.name);
        }
    }
}
