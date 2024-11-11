using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> _inventoryUI;
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
                _inventoryUI[i].gameObject.SetActive(true);
            }

            else
            {
                _inventoryUI[i].gameObject.SetActive(false);
            }
        }
    }

    public void InventoryRemoveUI(List<GameObject> inventory)
    {
        foreach (Image item in _inventoryUI)
        {
            if (item.gameObject.activeInHierarchy)
            {
                item.gameObject.SetActive(false);
                break;
            }
        }
    }
}
