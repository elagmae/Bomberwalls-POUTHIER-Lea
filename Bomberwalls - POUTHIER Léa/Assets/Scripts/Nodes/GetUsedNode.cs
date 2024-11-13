using System.Collections.Generic;
using UnityEngine;

public class GetUsedNode : MonoBehaviour
{
    [SerializeField]
    private GameObject _beginningNode;
    public GameObject UsedNode {get;private set;}

    private void Awake()
    {
        transform.position = _beginningNode.transform.position;
        UsedNode = _beginningNode;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            UsedNode = collision.gameObject;
        }
    }
}
