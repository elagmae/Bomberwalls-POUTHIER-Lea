using System;
using System.Collections;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public event Action<string> OnWallTouched;
    private BombCollection _collection;
    private BombApparition _bombApparition;

    private void Start()
    {
        _collection = GetComponent<BombCollection>();
        _bombApparition = ObjectPool.Instance.gameObject.GetComponent<BombApparition>();
    }
    public IEnumerator Explosion(GameObject bomb)
    {
        bomb.tag = "ActivatedBomb";
        bomb.transform.position = this.gameObject.transform.position;
        bomb.SetActive(true);
        _collection.RemoveObject();

        yield return new WaitForSeconds(3f);

        Collider2D[] ray = Physics2D.OverlapCircleAll(bomb.transform.position, 1.28f);

        bomb.SetActive(false);
        ObjectPool.Instance.AddPoolObjectBack(bomb);
        bomb = ObjectPool.Instance.GetPooledObject();
        _bombApparition.PlacementReset(bomb);
        bomb.tag = "Bomb";

        foreach (Collider2D collider in ray)
        {
            if (collider.CompareTag("BreakableWall"))
            {
                OnWallTouched?.Invoke(collider.gameObject.name);
            }
        }
    }
}
