using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private PlayerInputHandler _input;
    private Vector2 _direction;
    private Rigidbody2D _rb;

    private bool _verticalMoving;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _input = GetComponent<PlayerInputHandler>();
        _input.OnMove += Move;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _direction * _speed;
    }

    public void Move(InputAction.CallbackContext ctx, Vector2 dir)
    {
        _verticalMoving = dir.y != 0;

        if (_verticalMoving)
        {
            dir.x = 0;
        }

        else
        {
            dir.y = 0;
        }

        _direction = dir;
    }
}
