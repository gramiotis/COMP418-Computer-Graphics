using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingSoldiersLogic : MonoBehaviour
{
    public GameObject soldier;
    public GameObject plagueDoctor;

    public GameObject iconLoading;
    public GameObject iconLoadingPlague;

    public TextMeshProUGUI cooldown;
    public TextMeshProUGUI counter;

    public TextMeshProUGUI cooldownPlague;
    public TextMeshProUGUI counterPlague;

    public int buildingCounter;
    public int buildingCounterPlague;

    public bool isBuilding;
    public bool isBuildingPlague;

    public float offset = 1f;

    public List<GameObject> fromBuilding;

    public List<GameObject> fromBuildingPlague;

    public static BuildingSoldiersLogic _instance;
    public static BuildingSoldiersLogic Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isBuilding = false;
        isBuildingPlague = false;

        fromBuilding = new List<GameObject>();
        fromBuildingPlague = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        counter.text = "x" + buildingCounter.ToString();

        counterPlague.text = "x" + buildingCounterPlague.ToString();
        QueueIsEmpty();
    }

    public void AddToBuildingQueueSoldier(GameObject building)
    {
        fromBuilding.Add(building);

        buildingCounter += 1;
    }

    public void AddToBuildingQueuePlague(GameObject building)
    {
        fromBuildingPlague.Add(building);
        
        buildingCounterPlague += 1;
    }

    // check if the are soldiers to be built
    void QueueIsEmpty()
    {
        if (!isBuilding && buildingCounter > 0)
        {
            GameObject building = fromBuilding[0];

            fromBuilding.RemoveAt(0);

            iconLoading.SetActive(true);
            StartCoroutine(CalculateTimeSoldier(building));
            isBuilding = true;
        }

        if (!isBuildingPlague && buildingCounterPlague > 0)
        {
            GameObject building = fromBuildingPlague[0];

            fromBuildingPlague.RemoveAt(0);

            iconLoadingPlague.SetActive(true);
            StartCoroutine(CalculateTimePlague(building));
            isBuildingPlague = true;
        }

        if (buildingCounter == 0)
            iconLoading.SetActive(false);

        if (buildingCounterPlague == 0)
            iconLoadingPlague.SetActive(false);
    }

    IEnumerator CalculateTimeSoldier(GameObject building)
    {
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
        cooldown.text = "0s";

        Instantiate(soldier, new Vector3(building.transform.position.x + offset, building.transform.position.y, building.transform.position.z - 3f), Quaternion.Euler(0, 0, 0));

        building.SendMessage("OneUnitBuilt", soldier);

        if (offset == 1f)
            offset = 0f;
        else
            offset = 1f;

        buildingCounter--;
        isBuilding = false;
    }

    IEnumerator CalculateTimePlague(GameObject building)
    {
        cooldownPlague.text = "5s";
        yield return new WaitForSeconds(1);
        cooldownPlague.text = "4s";
        yield return new WaitForSeconds(1);
        cooldownPlague.text = "3s";
        yield return new WaitForSeconds(1);
        cooldownPlague.text = "2s";
        yield return new WaitForSeconds(1);
        cooldownPlague.text = "1s";
        yield return new WaitForSeconds(1);
        cooldownPlague.text = "0s";

        Instantiate(plagueDoctor, new Vector3(building.transform.position.x + offset, building.transform.position.y, building.transform.position.z - 3f), Quaternion.Euler(0, 0, 0));

        building.SendMessage("OneUnitBuilt", plagueDoctor);

        if (offset == 1f)
            offset = 0f;
        else
            offset = 1f;

        buildingCounterPlague--;
        isBuildingPlague = false;
    }
}
