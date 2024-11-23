using UnityEngine;

public class GetUsedNode : MonoBehaviour
{
    [SerializeField]
    private GetNodeInfos _beginningNode;
    private BombPlacement _placement;
    public GetNodeInfos UsedNode {get;private set;}

    private void Start()
    {
        _placement = ObjectPool.Instance.GetComponent<BombPlacement>();

        UsedNode = _beginningNode;
        this.gameObject.transform.position = UsedNode.gameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(var node in _placement.AllNodes)
        {
            if (node.gameObject == collision.gameObject)
            {
                UsedNode = node;
            }
        }
    }
}
