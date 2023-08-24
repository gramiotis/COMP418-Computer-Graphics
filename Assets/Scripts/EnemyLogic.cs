using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyLogic : MonoBehaviour
{
    public NavMeshAgent enemyAgent;
    public Animator animator;
    public PlayMusic mainAudio;

    public Image health;

    public int enemyDamage = 5;
    public int maxHealth = 100;
    public int currentHealth;

    public GameObject enemySoldier; // holds the enemy gameobject currently engaging


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        enemyAgent = GetComponent<NavMeshAgent>();
        mainAudio = GameObject.Find("MainAudio").GetComponent<PlayMusic>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySoldier != null)
            enemyAgent.SetDestination(enemySoldier.transform.position);

        health.fillAmount = (float)currentHealth / (float)maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Soldier" || other.tag == "PlagueDoctor")
            SoldierAttacked(other.gameObject);
    }

    // notification that unit is engaging this gameobject
    private void SoldierAttacked(GameObject soldier)
    {
        if (enemySoldier == null)
            enemySoldier = soldier;

        transform.LookAt(soldier.transform);

        StartCoroutine(Attack(soldier));  
    }
     // apply damage to unit
    IEnumerator Attack(GameObject soldier)
    {
        /* if soldier leaves battle before killing the enemy */
        if (Vector3.Distance(transform.position, soldier.transform.position) > 5f)
        {
            yield break;
        }

        animator.SetTrigger("Attack");

        if(soldier.CompareTag("Soldier"))
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

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        mainAudio.SoldierDeath();
        Destroy(gameObject);
    }
}
