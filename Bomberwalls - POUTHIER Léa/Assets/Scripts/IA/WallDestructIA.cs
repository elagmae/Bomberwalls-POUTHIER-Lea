using System.Collections;
using System.Linq;
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

        print("AOEIUGHBAOIEUHG");
        for(int i = 1; i < _astar.Inventory._inventoryUI.Count((g) => g.activeInHierarchy); i++) {
            StartCoroutine(_explosion.Detonation(ObjectPool.Instance.GetPooledObject()));
        }

        yield return _explosion.Detonation(ObjectPool.Instance.GetPooledObject());
    }
}
