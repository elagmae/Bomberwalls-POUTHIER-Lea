using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WallDestructIA : MonoBehaviour
{
    [SerializeField]
    private GameObject _wallNode;
    private BombExplosion _explosion;
    private AstarPattern _astar;

    private void Awake()
    {
        _astar = GetComponent<AstarPattern>();
        _explosion = GetComponent<BombExplosion>();
    }

    private async void OnTriggerEnter2D(Collider2D collision)
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
            StartCoroutine(_explosion.Explosion(_astar.Bombs.ToList().Find((b) => b != _astar.CurrentBomb)));
        }

        yield return _explosion.Explosion(_astar.CurrentBomb);
    }
}
