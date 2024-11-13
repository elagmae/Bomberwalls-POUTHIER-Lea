using System.Collections.Generic;
using UnityEngine;

public class GetNodeInfos : MonoBehaviour
{
    public List<GetNodeInfos> Links { get; private set; } = new List<GetNodeInfos>();

    [SerializeField]
    private BombApparition _bombApparition;

    private List<Vector2> _directions;

    // Valeurs de chaque node qui contient ce script.
    public float H { get; set; }
    public float G { get; set; }
    public float F { get; set; }

    private void Awake()
    {
        _bombApparition.AllNodes.Add(this);

        // On prend chaque position voisine à l'objet.
        _directions = new List<Vector2>() {new Vector2(transform.position.x + 1.28f, transform.position.y), new Vector2(transform.position.x - 1.28f, transform.position.y), new Vector2(transform.position.x, transform.position.y + 1.28f), new Vector2(transform.position.x, transform.position.y - 1.28f) };

        //Et pour chaque position, on vérifie si un node de sol est présent.
        foreach (Vector2 dir in _directions)
        {
            RaycastHit2D ray = Physics2D.Raycast(dir, dir );
            if(ray && ray.transform.gameObject.CompareTag("Ground"))
            {
                Debug.DrawLine(dir, transform.position, Color.blue, 20f);

                // S'il est présent et qu'il possède bien le cript attendu, on l'ajoute à la liste des voisins.
                if(ray.transform.gameObject.GetComponent<GetNodeInfos>() != null)
                {
                    Links.Add(ray.transform.gameObject.GetComponent<GetNodeInfos>());
                }
            }
        }
    }
}
