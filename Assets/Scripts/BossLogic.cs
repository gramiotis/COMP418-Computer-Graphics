using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class BossLogic : MonoBehaviour
{
    public NavMeshAgent enemyAgent;
    public Animator animator;
    public PlayMusic mainAudio;

    public Image health;

    public float timeChangeMessage;
    public float nextTime = 5f;

    public int enemyDamage = 40;
    public int maxHealth = 20000;
    int currentHealth;
    bool lookForSoldiers;

    public GameObject enemySoldier; // holds the enemy gameobject this unit is engaging
    public GameObject blackBars;
    public GameObject healingStatue;
    public GameObject resource;
    public GameObject plague;
    public GameObject villagers;
    public TextMeshProUGUI dialogue;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        lookForSoldiers = false;
        enemyAgent = GetComponent<NavMeshAgent>();
        mainAudio = GameObject.Find("MainAudio").GetComponent<PlayMusic>();
        animator = GetComponent<Animator>();

        dialogue = blackBars.GetComponentInChildren<TextMeshProUGUI>();
        timeChangeMessage = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySoldier != null)
            enemyAgent.SetDestination(enemySoldier.transform.position);

        health.fillAmount = (float)currentHealth / (float)maxHealth;

        CheckForSoldiers();

        //Check if soldiers in range, if true charge at them
        Collider[] colliders = Physics.OverlapSphere(transform.position, 8f);

        foreach (var col in colliders)
        {
            if (col.gameObject.tag == "Soldier" || col.gameObject.tag == "PlagueDoctor")
            {
                if (Time.time >= timeChangeMessage)
                {
                    timeChangeMessage += nextTime;
                    BattleMessage();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7 && !other.CompareTag("Enemy"))
            SoldierAttacked(other.gameObject);
    }

    // notification from units that someone is attacking this gameobject
    private void SoldierAttacked(GameObject soldier)
    {
        mainAudio.PlayBossMusic();

        lookForSoldiers = true;

        if (enemySoldier == null)
            enemySoldier = soldier;

        transform.LookAt(soldier.transform);

        plague.SetActive(false);
        villagers.SetActive(false);
        resource.SetActive(false);
        blackBars.SetActive(true);

        StartCoroutine(Attack(soldier));
    }

    // apply health damage to enemy unit
    IEnumerator Attack(GameObject soldier)
    {
        animator.SetTrigger("Attack");

        if (soldier.CompareTag("Soldier"))
            soldier.GetComponent<SoldierLogic>().HealthDamage(enemyDamage);
        else
            soldier.GetComponent<PlagueDoctorLogic>().HealthDamage(enemyDamage);


        mainAudio.SoldierAttack();
        yield return new WaitForSeconds(1f);
        IsEnemyDead(soldier);
    }

    void IsEnemyDead(GameObject soldier)
    {
        if (soldier == null)
            return;
        else
            StartCoroutine(Attack(soldier));

    }

    public void HealthDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        mainAudio.SoldierDeath();
        Destroy(gameObject);
    }

    // looks for enemy soldiers in range after this gameobject was attacked if none found then start the epilogue
    void CheckForSoldiers()
    {
        bool foundEnemy = false;

        if (lookForSoldiers)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 30f);

            foreach (var col in colliders)
            {
                if (col.gameObject.tag == "Soldier" || col.gameObject.tag == "PlagueDoctor")
                {
                    foundEnemy = true;
                }
            }

            if (!foundEnemy)
            {
                lookForSoldiers = false;
                StartCoroutine("FinalMessage");
            }
        }
    }

    // messages when boss engages in battle
    void BattleMessage()
    {
        int counter = Random.Range(1, 4);

        if(counter == 1)
            dialogue.text = "Aaaaahhhhhhhhhhh";
        if (counter == 2)
            dialogue.text = "Die you Scum!!!!!";
        else
            dialogue.text = "Grrrgrggrgrgr";
    }

    // game epilogue
    IEnumerator FinalMessage()
    {
        Camera.main.GetComponent<CameraLogic>().enabled = false;

        yield return new WaitForSeconds(3);

        Camera.main.transform.position = new Vector3(transform.position.x, 30f, transform.position.z - 20f);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,180,0), Time.deltaTime * 2f);

        dialogue.text = "Finally....";
        yield return new WaitForSeconds(5);
        dialogue.text = "....humanity will fall and after a long time....";
        yield return new WaitForSeconds(5);
        dialogue.text = "....i win....";
        yield return new WaitForSeconds(5);
        dialogue.text = "....................";
        yield return new WaitForSeconds(5);

        Camera.main.transform.position = new Vector3(healingStatue.transform.position.x, 20f, healingStatue.transform.position.z - 10f);
        dialogue.text = "Poor soul consumed by revenge....";
        yield return new WaitForSeconds(5);
        dialogue.text = "....for those who betrayed thyself the only thing that awaits them is....";
        yield return new WaitForSeconds(5);
        dialogue.text = "....a sad ending....";
        yield return new WaitForSeconds(5);

        Camera.main.transform.position = new Vector3(transform.position.x, 30f, transform.position.z - 20f);
        dialogue.text = "NOOOOOOOOOOOOOOOOO";
        yield return new WaitForSeconds(5);
        blackBars.SetActive(false);
        Die();
    }
}
