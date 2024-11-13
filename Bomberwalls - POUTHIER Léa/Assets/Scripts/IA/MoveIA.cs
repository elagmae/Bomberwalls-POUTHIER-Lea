using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveIA : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private Rigidbody2D _rb;
    private AstarPattern _astar;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _astar = GetComponent<AstarPattern>();
    }

    public IEnumerator MoveToNode(List<GetNodeInfos> finalPath)
    {
        foreach (GetNodeInfos node in finalPath)
        {
            var direction = node.transform.position - this.transform.position;
            var velocity = direction.normalized * Time.fixedDeltaTime;
            var distance = direction.magnitude;

            while (distance >= 0.1f)
            {
                _rb.velocity = velocity * _speed;
                distance = (node.transform.position - this.transform.position).magnitude;
                yield return new WaitForEndOfFrame();

                if ((!(_astar.Bombs.Any((b) => b.activeInHierarchy == true && b.tag == "Bomb"))) && _astar.Inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count == 0) break;
            }
        }

        _astar.PathFinished = true;
        _astar.FirstNodeGoten = false;
        _rb.velocity = Vector2.zero;
    }
}
