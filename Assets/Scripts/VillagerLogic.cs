using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerLogic : MonoBehaviour
{
    public int resourceDamage = 40;
    public NavMeshAgent villagerAgent;
    public Animator animator;
    public PlayMusic mainAudio;
    GameObject[] doodadsInRange;

    // Start is called before the first frame update
    void Start()
    {
        villagerAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainAudio = GameObject.Find("MainAudio").GetComponent<PlayMusic>();
    }

    // Update is called once per frame
    void Update()
    {
        // based on units selected decide stopping distance
        if (UnitSelections.Instance.unitsSelected.Count < 4)
        {
            if (villagerAgent.remainingDistance > 1f)
                animator.SetTrigger("Walk");
            else
            {
                animator.SetTrigger("Stop");
                villagerAgent.ResetPath();
            }
        }
        else if(UnitSelections.Instance.unitsSelected.Count >= 4 && UnitSelections.Instance.unitsSelected.Count < 6)
        {
            if (villagerAgent.remainingDistance > 2f)
                animator.SetTrigger("Walk");
            else
            {
                animator.SetTrigger("Stop");
                villagerAgent.ResetPath();
            }
        }
        else
        {
            if (villagerAgent.remainingDistance > 3f)
                animator.SetTrigger("Walk");
            else
            {
                animator.SetTrigger("Stop");
                villagerAgent.ResetPath();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 8 && other.gameObject.GetComponent<DoodadBehav>().clickedFromThis.Contains(gameObject))
        {
            villagerAgent.isStopped = true;

            animator.SetTrigger("Stop");

            StartCoroutine(Attack(other));

            villagerAgent.isStopped = false;
        }
    }

    IEnumerator Attack(Collider doodad)
    {
        if (doodad.tag == "Wood")
            mainAudio.Cutting();
        else
            mainAudio.Mining();

        doodad.gameObject.GetComponent<DoodadBehav>().TakeDamage(resourceDamage);
        yield return new WaitForSeconds(1f);
        IsEnemyDead(doodad);

        GameObject[] temp1 = GameObject.FindGameObjectsWithTag("Stone");
        GameObject[] temp2 = GameObject.FindGameObjectsWithTag("Wood");

        doodadsInRange = new GameObject[temp1.Length + temp2.Length];
        temp1.CopyTo(doodadsInRange, 0);
        temp2.CopyTo(doodadsInRange, temp1.Length);

        GameObject temp = null;

        foreach (var tempDoodad in doodadsInRange)
        {
            if ((transform.position - tempDoodad.transform.position).magnitude < 5f)
            {
                temp = tempDoodad;
            }
        }

        if (temp != null)
        {
            villagerAgent.SetDestination(temp.transform.position);
            StartCoroutine(Attack(temp.GetComponent<Collider>()));
        }
    }

    void IsEnemyDead(Collider doodad)
    {
        if (doodad == null)
            return;
        else
            StartCoroutine(Attack(doodad));

    }
}
