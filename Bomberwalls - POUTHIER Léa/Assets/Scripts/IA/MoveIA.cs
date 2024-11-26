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
    private ChooseBomb _chooseBomb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _astar = GetComponent<AstarPattern>();
        _chooseBomb = GetComponent<ChooseBomb>();
    }

    public IEnumerator MoveToNode(List<GetNodeInfos> finalPath)
    {
        foreach (GetNodeInfos node in finalPath)
        {
            var direction = node.transform.position - this.transform.position;
            var velocity = direction.normalized * Time.fixedDeltaTime;
            var distance = direction.magnitude;

            while (Mathf.Abs(distance) >= 0.1f)
            {
                _rb.velocity = velocity * _speed;
                distance = (node.transform.position - this.transform.position).magnitude;
                yield return new WaitForEndOfFrame();

                if ((!(_chooseBomb.Bombs.Any((b) => b.activeInHierarchy == true && b.CompareTag("Bomb")))) && _astar.Inventory._inventoryUI.FindAll((g) => g.activeInHierarchy).Count == 0) break;
            }
        }

        _rb.velocity = Vector2.zero;
    }
}
