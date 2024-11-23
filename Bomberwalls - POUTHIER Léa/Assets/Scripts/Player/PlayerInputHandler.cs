using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //Event permettant d'appeler une méthode via une action donnée (ici les inputs du joueur).
    public event Action<InputAction.CallbackContext, Vector2> OnMove;
    public event Action<InputAction.CallbackContext> OnBombActivation;

    private PlayerInput _player;

    private void Awake()
    {
        _player = GetComponent<PlayerInput>();
        _player.onActionTriggered += OnInput;
    }

    // Fonction reliant les events aux méthodes directement par les inputs.
    public void OnInput(InputAction.CallbackContext ctx)
    {
        switch (ctx.action.name)
        {
            case "Move":
                OnMove?.Invoke(ctx, ctx.ReadValue<Vector2>());
                break;

            case "BombActivation":
                OnBombActivation?.Invoke(ctx);
                break;
        }
    }
}
