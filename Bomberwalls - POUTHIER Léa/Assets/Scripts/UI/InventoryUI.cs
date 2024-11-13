using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [field : SerializeField]
    public List<GameObject> _inventoryUI {get;private set;}
    private BombCollection _bombCollection;

    private void Awake()
    {
        _bombCollection = GetComponent<BombCollection>();
        _bombCollection.OnBombCollect += InventoryAddUI;
        _bombCollection.OnBombPlacement += InventoryRemoveUI;
    }

    public void InventoryAddUI(List<GameObject> inventory)
    {
        for (int i = 0; i < _inventoryUI.Count; i++)
        {
            if (inventory.Count > i)
            {
                _inventoryUI[i].SetActive(true);
            }

            else
            {
                _inventoryUI[i].SetActive(false);
            }
        }
    }

    public void InventoryRemoveUI(List<GameObject> inventory)
    {
        foreach (GameObject item in _inventoryUI)
        {
            if (item.activeInHierarchy)
            {
                item.SetActive(false);
                break;
            }
        }
    }
}
