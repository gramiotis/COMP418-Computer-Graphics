using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseLogic : MonoBehaviour
{
    public GameObject panel;
    public GameObject villager;

    // Start is called before the first frame update
    void Start()
    {
        GameMaster.population += GameMaster.populationInterval;

        Instantiate(villager, new Vector3(gameObject.transform.position.x + 5f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler(0, 0, 0));
        Instantiate(villager, new Vector3(gameObject.transform.position.x + 5f, gameObject.transform.position.y, gameObject.transform.position.z - 5f), Quaternion.Euler(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
