using System.Collections;
using System.Linq;
using UnityEngine;

public class WallDestructIA : MonoBehaviour
{
    [SerializeField]
    private GameObject _wallNode;
    private BombExplosion _explosion;
    private ChooseBomb _chooseBomb;
    private AstarPattern _astar;

    private void Awake()
    {
        _chooseBomb = GetComponent<ChooseBomb>();
        _astar = GetComponent<AstarPattern>();
        _explosion = GetComponent<BombExplosion>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (collision.gameObject == _wallNode && _astar.Inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count > 0)
        {
            StartCoroutine(PlaceBomb());
        }
    }

    public IEnumerator PlaceBomb()
    {
        yield return new WaitForSeconds(0.25f);
        _astar.PathFinished = true;
        if(_astar.Inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count == 2) {
            StartCoroutine(_explosion.Detonation(_chooseBomb.Bombs.ToList().Find((b) => b != _chooseBomb.CurrentBomb)));
        }

        yield return _explosion.Detonation(_chooseBomb.CurrentBomb);
    }
}
