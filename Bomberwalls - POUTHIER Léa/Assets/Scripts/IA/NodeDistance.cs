using System.Collections.Generic;
using UnityEngine;

public class NodeDistance : MonoBehaviour
{
    // Valeurs calculées de chaque node.
    public float GetH(GetNodeInfos node, GameObject finalNode)
    {
        node.H = (finalNode.transform.position - node.transform.position).magnitude;
        return node.H;
    }

    public float GetG(GetNodeInfos node, List<GetNodeInfos> activePath)
    {
        node.G = (activePath.Count + 1);
        return node.G;
    }

    public float GetF(GetNodeInfos node, GameObject finalNode, List<GetNodeInfos> activePath)
    {
        node.F = GetH(node, finalNode) + GetG(node, activePath);
        return node.F;
    }
}
