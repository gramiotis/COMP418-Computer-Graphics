using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarLogic : MonoBehaviour
{
    public GameObject fogPlane;
    public Transform player;
    public Material fogPlaneMat;
     
    public LayerMask fogLayer;
    public float radius = 10f;
    private float radiusCircle { get { return radius * radius; } }

    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;

    public bool stop = true;

    private void Awake()
    {
        fogPlane.transform.localScale = new Vector3(500f, 500f, 500f);
        fogPlane.GetComponent<MeshRenderer>().material = fogPlaneMat;
        fogPlane.layer = 10;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initiliaze();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            Ray ray = new Ray(transform.position, player.position - transform.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, fogLayer, QueryTriggerInteraction.Collide))
            {
                Debug.Log("Hit Fog");
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 v = fogPlane.transform.TransformPoint(vertices[i]);
                    float dist = Vector3.SqrMagnitude(v - hit.point);
                    if (dist < radiusCircle)
                    {
                        float alpha = Mathf.Min(colors[i].a, dist / radiusCircle);
                        colors[i].a = alpha;
                    }
                }
                UpdateColors();
            }
        }
    }

    void Initiliaze()
    {
        mesh = fogPlane.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];

        for(int i =0; i<colors.Length; i++)
        {
            colors[i] = Color.black;
        }
        UpdateColors();
    }

    void UpdateColors()
    {
        mesh.colors = colors;
    }
}
