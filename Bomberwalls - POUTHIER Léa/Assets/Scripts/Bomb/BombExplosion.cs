using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class BombExplosion : MonoBehaviour
{
    public event Action<string> OnWallTouched;
    [SerializeField]
    private CinemachineImpulseSource _source;
    private BombCollection _collection;
    private BombPlacement _bombPlacement;

    private void Start()
    {
        _collection = GetComponent<BombCollection>();
        _bombPlacement = ObjectPool.Instance.gameObject.GetComponent<BombPlacement>();
    }
    public IEnumerator Detonation(GameObject bomb)
    {
        // Permet d'activer la bombe posée (on ne peut alors plus la déplacer, elle ne nous appartient plus).
        bomb.tag = "ActivatedBomb";
        bomb.transform.position = this.gameObject.transform.position;
        bomb.SetActive(true);
        _collection.RemoveObject();

        // On déclenche l'explosion 3 secondes après l'activation.
        yield return new WaitForSeconds(3f);
        Explosion(bomb);
    }

    // Permet à la bombe d'exploser de manière circulaire (avec un radius d'une case).
    public void Explosion(GameObject bomb)
    {
        Collider2D[] ray = Physics2D.OverlapCircleAll(bomb.transform.position, 1.28f);

        ObjectPool.Instance.AddPoolObjectBack(bomb);

        _bombPlacement.PlacementReset(bomb);
        bomb = ObjectPool.Instance.GetPooledObject();

        bomb.tag = "Bomb";
        _source.GenerateImpulse();

        foreach (Collider2D collider in ray)
        {
            if (collider.CompareTag("BreakableWall"))
            {
                OnWallTouched?.Invoke(collider.gameObject.name);
            }
        }
    }
}
