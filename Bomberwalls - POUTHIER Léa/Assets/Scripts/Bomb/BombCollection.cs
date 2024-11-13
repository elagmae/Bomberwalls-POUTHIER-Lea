using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BombCollection : MonoBehaviour
{
    public event Action<List<GameObject>> OnBombCollect;
    public event Action<List<GameObject>> OnBombPlacement;

    [SerializeField]
    private int _maxinventorySlots = 2;
    public List<GameObject> Inventory { get; private set; } = new List<GameObject>();
    private PlayerInputHandler _input;
    private bool _collect;

    private void Awake()
    {
        _input = GetComponent<PlayerInputHandler>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomb") && Inventory.Count < _maxinventorySlots)
        {
            collision.gameObject.SetActive(false);
            Inventory.Add(collision.gameObject);
            OnBombCollect?.Invoke(Inventory);
        }
    }

    public void RemoveObject()
    {
        Inventory.Remove(Inventory[0]);
        OnBombPlacement?.Invoke(Inventory);
    }
}
