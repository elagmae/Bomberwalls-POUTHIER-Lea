using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<InputAction.CallbackContext, Vector2> OnMove;
    public event Action<InputAction.CallbackContext> OnBombCollection;
    public event Action<InputAction.CallbackContext> OnBombActivation;

    private PlayerInput _player;

    private void Awake()
    {
        _player = GetComponent<PlayerInput>();
        _player.onActionTriggered += OnInput;
    }

    public void OnInput(InputAction.CallbackContext ctx)
    {
        switch (ctx.action.name)
        {
            case "Move":
                OnMove?.Invoke(ctx, ctx.ReadValue<Vector2>());
                break;

            case "BombCollection":
                OnBombCollection?.Invoke(ctx);
                break;

            case "BombActivation":
                OnBombActivation?.Invoke(ctx);
                break;
        }
    }
}
