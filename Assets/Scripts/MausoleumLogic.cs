using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MausoleumLogic : MonoBehaviour
{
    public float interval = 1f;
    float nextPayoutTime = 0f;
    public GameObject panel;

    GameObject[] soldiers;
    GameObject[] plague;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextPayoutTime)
        {
            GameMaster.gold += GameMaster.goldInterval;
            nextPayoutTime = Time.time + interval;
        }
    }

    public void PowerUpSoldiers()
    {
        if (GameMaster.gold < 60)
        {
            GameMaster.Instance.SendMessage("GameMessage", "Not Enough Resources");
            return;
        }
        else
        {
            GameMaster.gold -= 60;
        }

        soldiers = GameObject.FindGameObjectsWithTag("Soldier");

        foreach (GameObject soldier in soldiers)
        {
            soldier.GetComponent<SoldierLogic>().soldierDamage += 20;
        }

        plague = GameObject.FindGameObjectsWithTag("PlagueDoctor");

        foreach (GameObject plag in plague)
        {
            plag.GetComponent<PlagueDoctorLogic>().doctorDamage += 20;
        }

        GameMaster.Instance.SendMessage("GameMessage", "Soldiers Powered Up");
    }

    public void Click()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
