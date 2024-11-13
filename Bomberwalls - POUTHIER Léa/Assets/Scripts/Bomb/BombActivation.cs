using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BombActivation : MonoBehaviour
{
    private PlayerInputHandler _input;
    private BombCollection _bombCollection;
    private BombApparition _bombApparition;
    private BombExplosion _bombExplosion;

    private void Start()
    {
        _input = GetComponent<PlayerInputHandler>();
        _input.OnBombActivation += Activate;

        _bombCollection = GetComponent<BombCollection>();
        _bombExplosion = GetComponent<BombExplosion>();
        _bombApparition = ObjectPool.Instance.gameObject.GetComponent<BombApparition>();
    }

    public void Activate(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && _bombCollection.Inventory.Count > 0)
        {
            var bomb = _bombCollection.Inventory[0];
            StartCoroutine(_bombExplosion.Explosion(bomb));
        }
    }
}
