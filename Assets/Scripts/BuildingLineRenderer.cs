using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLineRenderer : MonoBehaviour
{
    private LineRenderer line_renderer;

    // Start is called before the first frame update
    void Start()
    {
        line_renderer = GetComponent<LineRenderer>();

        line_renderer.material.color = Color.black;
        line_renderer.startWidth = 0.1f;
        line_renderer.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Draw(Vector3 linePos, string building)
    {
        if (building.Equals("House"))
            DrawHouse(linePos);

        if (building.Equals("Barracks"))
            DrawBarrack(linePos);

        if (building.Equals("Mausoleum"))
            DrawMausoleum(linePos);
    }

    void DrawHouse(Vector3 linePos)
    {
 
        line_renderer.positionCount = 4;

        Vector3[] positions = new Vector3[4] {
        new Vector3(linePos.x-3, 0, linePos.z+5),
        new Vector3(linePos.x+3, 0, linePos.z+5),
        new Vector3(linePos.x+3, 0, linePos.z-5),
        new Vector3(linePos.x-3, 0, linePos.z-5) };

        line_renderer.SetPositions(positions);
    }
    void DrawBarrack(Vector3 linePos)
    {
        line_renderer.positionCount = 4;

        Vector3[] positions = new Vector3[4] {
        new Vector3(linePos.x-3, 0, linePos.z+3),
        new Vector3(linePos.x+3, 0, linePos.z+3),
        new Vector3(linePos.x+3, 0, linePos.z-3),
        new Vector3(linePos.x-3, 0, linePos.z-3) };

        line_renderer.SetPositions(positions);
    }
    void DrawMausoleum(Vector3 linePos)
    {
        line_renderer.positionCount = 4;

        Vector3[] positions = new Vector3[4] {
        new Vector3(linePos.x-3, 0, linePos.z+3),
        new Vector3(linePos.x+3, 0, linePos.z+3),
        new Vector3(linePos.x+3, 0, linePos.z-3),
        new Vector3(linePos.x-3, 0, linePos.z-3) };

        line_renderer.SetPositions(positions);
    }

    public void ClearLines()
    {
        line_renderer.positionCount = 0;
    }
}
