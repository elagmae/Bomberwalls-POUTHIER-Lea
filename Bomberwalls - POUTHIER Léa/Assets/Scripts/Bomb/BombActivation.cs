using UnityEngine;
using UnityEngine.InputSystem;

public class BombActivation : MonoBehaviour
{
    private BombCollection _bombCollection;
    private BombExplosion _bombExplosion;

    private void Start()
    {
        var input = GetComponent<PlayerInputHandler>();
        input.OnBombActivation += Activate;

        _bombCollection = GetComponent<BombCollection>();
        _bombExplosion = GetComponent<BombExplosion>();
    }

    // Pose une bombe sur la case du joueur une fois l'input activé.
    public void Activate(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && _bombCollection.Inventory.Count > 0)
        {
            var bomb = _bombCollection.Inventory[0];
            StartCoroutine(_bombExplosion.Detonation(bomb));
        }
    }
}
