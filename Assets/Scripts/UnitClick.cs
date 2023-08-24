using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitClick : MonoBehaviour
{
    private Camera cam;
    private GameObject building;

    public LayerMask Unit;
    public LayerMask Ground;
    public LayerMask Building;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // check if what we are clicking is a gameobject
        {   
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //if we hit clickable unit
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, Unit))
            {
                if (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Boss"))
                {
                    UnitSelections.Instance.DeselectAll();
                    return;
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelections.Instance.shiftSelect(hit.collider.gameObject);
                }
                else
                {
                    UnitSelections.Instance.clickSelect(hit.collider.gameObject);
                }
            }
            else if(Physics.Raycast(ray, out hit, Mathf.Infinity, Building))
            {
                building = hit.collider.gameObject;
                building.SendMessage("Click");
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelections.Instance.DeselectAll();
                }
            }
        }

    }
}
