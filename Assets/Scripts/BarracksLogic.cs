using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarracksLogic : MonoBehaviour
{
    public GameObject soldier;
    public GameObject plagueDoctor;

    public GameObject panel; //ui panel

    public int buildingCounter;
    public int buildingCounterPlague;

    public TextMeshProUGUI counter; // soldiers and plague doctors counter text


    public int stoneReq = 5;
    public int popReq = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        counter.text = "Building Soldiers: x" + buildingCounter + "\nBuilding Plague: x" + buildingCounterPlague + "\nMax building queue: 5 for each type"; //Update Soldier And Plague Counters Text
    }

    //Button for spawning soldiers
    public void Button_Spawner_Soldier()
    {
        if (!checkRequirements() || buildingCounter > 4)
        {
            GameMaster.Instance.SendMessage("GameMessage", "Not Enough Resources Or Reached The Build Limit");
            return;
        }   
        else
        {
            GameMaster.stone -= stoneReq;
            GameMaster.population -= popReq;
            buildingCounter += 1;
            BuildingSoldiersLogic.Instance.AddToBuildingQueueSoldier(gameObject); //add to queue of building unit
        }
    }

    //Button for spawning plague doctors
    public void Button_Spawner_Plague()
    {
        if (!checkRequirements() || buildingCounterPlague > 4)
        {
            GameMaster.Instance.SendMessage("GameMessage", "Not Enough Resources Or Reached The Build Limit");
            return;
        }
        else
        {
            GameMaster.stone -= stoneReq;
            GameMaster.population -= popReq;
            buildingCounterPlague += 1;
            BuildingSoldiersLogic.Instance.AddToBuildingQueuePlague(gameObject); //add to queue of building unit
        }
    }

    // enable/disable barracks ui
    public void Click()
    {
        panel.SetActive(!panel.activeSelf);           
    }

    //check requirements for building units
    public bool checkRequirements()
    {
        return (GameMaster.stone >= stoneReq && GameMaster.population >= popReq);
    }

    //notification from BuildingBoldiersLogic that one unit was built from this barrack so decrease counter
    public void OneUnitBuilt(GameObject unit)
    {
        if(unit == soldier)
        {
            buildingCounter -= 1;
        }
        else
        {
            buildingCounterPlague -= 1;
        }
    }
}
