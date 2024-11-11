using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BombActivation : MonoBehaviour
{
    public event Action<string> OnWallTouched;
    private PlayerInputHandler _input;
    private BombCollection _bombCollection;
    private BombApparition _bombApparition;

    private void Start()
    {
        _input = GetComponent<PlayerInputHandler>();
        _input.OnBombActivation += Activate;

        _bombCollection = GetComponent<BombCollection>();
        _bombApparition = ObjectPool.Instance.gameObject.GetComponent<BombApparition>();
    }

    public void Activate(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && _bombCollection.Inventory.Count > 0)
        {
            var bomb = _bombCollection.Inventory[0];
            bomb.tag = "ActivatedBomb";
            StartCoroutine(BombExplosion(bomb));
        }
    }

    public IEnumerator BombExplosion(GameObject bomb)
    {
        bomb.transform.position = this.gameObject.transform.position;
        bomb.SetActive(true);
        _bombCollection.RemoveObject();

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
