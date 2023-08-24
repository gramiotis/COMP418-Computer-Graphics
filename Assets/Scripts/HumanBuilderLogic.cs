using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class HumanBuilderLogic : MonoBehaviour
{
    public NavMeshAgent builderAgent;
    public Animator animator;


    public TextMeshProUGUI cooldown;
    public TextMeshProUGUI counter;

    List<Vector3> buildingPos = new List<Vector3>(); // stores the position of the building we want to build
    List<GameObject> buildingObject = new List<GameObject>(); // stores the building to build

    bool isBuilding;

    // Start is called before the first frame update
    void Start()
    {
        isBuilding = false;
        builderAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        counter.text = "x" + buildingObject.Count.ToString();
        Build();

        if (builderAgent.remainingDistance > 0.1 /*&& !animator.GetCurrentAnimatorStateInfo(0).IsName("human_walk_forward")*/)
            animator.SetTrigger("Walk");
        else
            animator.SetTrigger("Stop");
    }

    public void AddToQueue(Vector3 objectPos, GameObject objectToBuild)
    {
        buildingPos.Add(objectPos);
        buildingObject.Add(objectToBuild);
    }

    void Build()
    {
        if (!isBuilding && buildingObject.Count !=0)
        {

            Vector3 destination = buildingPos[0];
            destination.x -= 5f;

            builderAgent.SetDestination(destination);

            if (Mathf.Abs(builderAgent.transform.position.x - builderAgent.destination.x) < 0.5)
            {
                builderAgent.isStopped = true;
                StartCoroutine("CalculateTimeToBuild");
            }
        }

        builderAgent.isStopped = false;
    }

    public IEnumerator CalculateTimeToBuild()
    {
        transform.LookAt(buildingPos[0]);

        isBuilding = true;
        cooldown.text = "5s";
        yield return new WaitForSeconds(1);
        cooldown.text = "4s";
        yield return new WaitForSeconds(1);
        cooldown.text = "3s";
        yield return new WaitForSeconds(1);
        cooldown.text = "2s";
        yield return new WaitForSeconds(1);
        cooldown.text = "1s";
        yield return new WaitForSeconds(1);
        cooldown.text = "";

        GameObject objectToBuild = buildingObject[0];
        Vector3 objectPos = buildingPos[0];


        Instantiate(objectToBuild, objectPos, Quaternion.Euler(0, 180, 0));
        GameMaster.Instance.UpdateSurface();

        buildingObject.RemoveAt(0);
        buildingPos.RemoveAt(0);
        isBuilding = false;
    }


}
