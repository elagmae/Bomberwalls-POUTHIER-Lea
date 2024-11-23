using System;
using System.Collections.Generic;
using UnityEngine;

public class BombCollection : MonoBehaviour
{
    // Event permettant au joueur d'actualiser son inventaire (UI) selon ses actions directement.
    public event Action<List<GameObject>> OnBombCollect;
    public event Action<List<GameObject>> OnBombPlacement;

    // Garde en mémoire les bombes que le joueur possède.
    public List<GameObject> Inventory { get; private set; } = new List<GameObject>();

    private int amountToPool;
    private int _maxinventorySlots;

    private void Start()
    {
        amountToPool = ObjectPool.Instance.AmountToPool;
        _maxinventorySlots = amountToPool;
    }

    // Permet au joueur de récupérer une bombe dans son inventaire lorsque ce dernier marche sur cette dernière.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomb") && Inventory.Count < _maxinventorySlots)
        {
            collision.gameObject.SetActive(false);
            Inventory.Add(collision.gameObject);
            OnBombCollect?.Invoke(Inventory);
        }
    }

    // Place la bombe et la sort de l'inventaire du joueur.
    public void RemoveObject()
    {
        Inventory.Remove(Inventory[0]);
        OnBombPlacement?.Invoke(Inventory);
    }
}
