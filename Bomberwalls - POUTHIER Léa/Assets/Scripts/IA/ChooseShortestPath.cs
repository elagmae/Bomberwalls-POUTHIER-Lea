using System.Collections.Generic;
using UnityEngine;

public class ChooseShortestPath : MonoBehaviour
{
    public List<GetNodeInfos> ChooseShortest(List<GetNodeInfos> activePath, NodeDistance nodeDistance, List<List<GetNodeInfos>> closedPaths, GetNodeInfos finalNode)
    {
        List<GetNodeInfos> minList = new(activePath);

        List<List<GetNodeInfos>> closedPath = new(closedPaths);
        foreach (List<GetNodeInfos> path in closedPath)
        {
            if (nodeDistance.GetF(path[^1], finalNode.gameObject, activePath) < nodeDistance.GetF(minList[^1], finalNode.gameObject, activePath))
            {
                minList = new(path);
            }
        }

        activePath = new(minList);

        if (closedPaths.Contains(activePath))
        {
            closedPaths.Remove(activePath);
        }

        return activePath;
    }
}
