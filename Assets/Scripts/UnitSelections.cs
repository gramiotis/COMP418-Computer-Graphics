using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    public List<GameObject> listOfUnits = new List<GameObject>(); // list of all units
    public List<GameObject> unitsSelected = new List<GameObject>(); // list of units that are selected

    public static UnitSelections _instance;
    public static UnitSelections Instance { get { return _instance; } }

    void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickSelect(GameObject unit)
    {
        DeselectAll();
        unitsSelected.Add(unit);
        unit.transform.Find("SelectLine").gameObject.SetActive(true);
        unit.GetComponent<UnitMovement>().enabled = true;
    }

    public void shiftSelect(GameObject unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            unit.transform.Find("SelectLine").gameObject.SetActive(true);
            unit.GetComponent<UnitMovement>().enabled = true;
        }
        else
        {
            unit.GetComponent<UnitMovement>().enabled = false;
            unit.transform.Find("SelectLine").gameObject.SetActive(false);
            unitsSelected.Remove(unit);
        }
    }

    public void DeselectAll()
    {
        foreach(var unit in unitsSelected)
        {
            if (unit != null)
            {
                unit.GetComponent<UnitMovement>().enabled = false;
                unit.transform.Find("SelectLine").gameObject.SetActive(false);
            }
        }
        unitsSelected.Clear();
    }

    public void SelectAllSoldiers()
    {
        DeselectAll();

        GameObject[] soldiersList = GameObject.FindGameObjectsWithTag("Soldier");

        foreach(var unit in soldiersList)
        {
            if (unit != null)
            {
                unitsSelected.Add(unit);
                unit.transform.Find("SelectLine").gameObject.SetActive(true);
                unit.GetComponent<UnitMovement>().enabled = true;
            }
        }
    }

    public void SelectAllPlagueDoctors()
    {
        DeselectAll();

        GameObject[] plagueDoctorsList = GameObject.FindGameObjectsWithTag("PlagueDoctor");

        foreach (var unit in plagueDoctorsList)
        {
            if (unit != null)
            {
                unitsSelected.Add(unit);
                unit.transform.Find("SelectLine").gameObject.SetActive(true);
                unit.GetComponent<UnitMovement>().enabled = true;
            }
        }
    }

    public void SelectAllCombatUnits()
    {
        DeselectAll();

        GameObject[] soldiersList = GameObject.FindGameObjectsWithTag("Soldier");
        GameObject[] plagueDoctorsList = GameObject.FindGameObjectsWithTag("PlagueDoctor");

        GameObject[] combatUnits = new GameObject[soldiersList.Length + plagueDoctorsList.Length];
        soldiersList.CopyTo(combatUnits, 0);
        plagueDoctorsList.CopyTo(combatUnits, soldiersList.Length);

        foreach (var unit in combatUnits)
        {
            if (unit != null)
            {
                unitsSelected.Add(unit);
                unit.transform.Find("SelectLine").gameObject.SetActive(true);
                unit.GetComponent<UnitMovement>().enabled = true;
            }
        }
    }

    public void SelectAllVillagers()
    {
        DeselectAll();

        GameObject[] villagersList = GameObject.FindGameObjectsWithTag("Villager");

        foreach (var unit in villagersList)
        {
            unitsSelected.Add(unit);
            unit.transform.Find("SelectLine").gameObject.SetActive(true);
            unit.GetComponent<UnitMovement>().enabled = true;
        }
    }

}
