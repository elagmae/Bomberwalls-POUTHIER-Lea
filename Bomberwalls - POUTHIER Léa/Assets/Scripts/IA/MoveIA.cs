using System.Collections;
using System.Collections.Generic;
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
            }
        }

        if (finalPath[^1].transform.position.magnitude >= this.transform.position.magnitude)
        {
            _rb.velocity = Vector3.zero;
        }
    }
}
