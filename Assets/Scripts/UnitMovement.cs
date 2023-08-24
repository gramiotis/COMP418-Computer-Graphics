using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;

    public LayerMask Ground;
    public LayerMask Doodad;
    public LayerMask Unit;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Collider>().enabled = true;

        // move to destination based on the object we clicked
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            agent.isStopped = false;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Ground))
            {
                agent.SetDestination(hit.point); 
            }

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, Doodad))
            {
                if (gameObject.CompareTag("Villager")) 
                {
                    hit.collider.gameObject.SendMessage("ClickedFrom", gameObject);
                    GetComponent<Collider>().enabled = true;
                    agent.SetDestination(hit.point);
                }
                else
                {
                    agent.ResetPath();
                    StartCoroutine(GameMaster.Instance.GameMessage("This object is not interactable with doodads"));
                }
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Unit))
            {
                if (((gameObject.CompareTag("Soldier") || gameObject.CompareTag("PlagueDoctor"))) && (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Boss")))
                {
                    GetComponent<Collider>().enabled = true;
                    agent.SetDestination(hit.point);                       
                }
                else if(gameObject.tag == "Villager" && (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Boss")))
                {
                    agent.ResetPath();
                    StartCoroutine(GameMaster.Instance.GameMessage("Villagers cannot attack enemies"));
                }
            }
        }
    }
}
