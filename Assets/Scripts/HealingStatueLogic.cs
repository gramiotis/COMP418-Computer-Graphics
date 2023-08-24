using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingStatueLogic : MonoBehaviour
{
    private Collider[] unitColliders;
    public GameObject panel;

    private int amountToHeal = 10;
    private float healDistance = 10f;

    private float curHealTime = 0f;
    private float timeBetweenHeals = 5f;

    public int segments = 50;
    public float xradius;
    public float zradius;
    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        xradius = healDistance;
        zradius = healDistance;

        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        line.enabled = false;
        line.material.color = Color.white;
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // check for soldiers and plague doctors inside radius (healDistance)
        unitColliders = Physics.OverlapSphere(transform.position, healDistance);
        for (int i = 0; i < unitColliders.Length; i++)
        {
            if (unitColliders[i].tag == "Soldier" || unitColliders[i].tag == "PlagueDoctor")
            {
                HealUnit(unitColliders[i].gameObject);
            }
        }
    }

    // call corresponding unit's heal function
    public void HealUnit(GameObject UnitToHeal)
    {
        curHealTime += Time.deltaTime;
        if (curHealTime >= timeBetweenHeals)
        {
            if (UnitToHeal.tag == "Soldier")
                UnitToHeal.GetComponent<SoldierLogic>().Heal(amountToHeal);
            else
                UnitToHeal.GetComponent<PlagueDoctorLogic>().Heal(amountToHeal);
            curHealTime = 0; 
        }
    }

    // draw healing radius when building is clicked
    void CreatePoints()
    {
        float x;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * zradius;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }

    public void Click()
    {
        line.enabled = !line.enabled;

        if (line.enabled)
            CreatePoints();

        panel.SetActive(!panel.activeSelf);

    }
}
