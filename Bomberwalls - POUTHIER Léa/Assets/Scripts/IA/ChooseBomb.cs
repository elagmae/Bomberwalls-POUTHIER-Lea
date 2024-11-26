using System.Collections.Generic;
using UnityEngine;

public class ChooseBomb : MonoBehaviour
{
    public List<GameObject> Bombs { get; private set; } = new();
    public GameObject MinObject { get; set; } = null;

    private BombPlacement _placement;

    private void Start()
    {
        _placement = ObjectPool.Instance.GetComponent<BombPlacement>();
        foreach (GameObject bomb in ObjectPool.Instance.ActivatedObjects)
        {
            Bombs.Add(bomb);
        }
    }

    public GetNodeInfos GetClosestBomb()
    {
        List<GameObject> bombs = new();

        foreach (GameObject bomb in Bombs)
        {
            if (bomb.activeInHierarchy && bomb.CompareTag("Bomb"))
            {
                bombs.Add(bomb);
            }
        }

        if (bombs.Count != 0)
        {
            MinObject = bombs[0];
            foreach (var bomb in bombs)
            {
                if (((bomb.transform.position) - this.transform.position).magnitude <= ((MinObject.transform.position) - this.transform.position).magnitude)
                {
                    MinObject = bomb;
                }
            }

            foreach (var node in _placement.AllNodes)
            {
                if (MinObject.transform.position == node.gameObject.transform.position)
                {
                    return node;
                }
            }

            return GetClosestBomb();
        }

        return null;
    }
}
