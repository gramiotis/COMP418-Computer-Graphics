using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class SoldierLogic : MonoBehaviour
{
    public NavMeshAgent soldierAgent;
    public Animator animator;
    public PlayMusic mainAudio;

    public Image health;

    public int soldierDamage = 10;

    public int maxHealth = 100;
    int currentHealth;

    public Vector3 enemyDestination;

    // Start is called before the first frame update
    void Start()
    {
        soldierAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        mainAudio = GameObject.Find("MainAudio").GetComponent<PlayMusic>();
    }

    // Update is called once per frame
    void Update()
    {
        health.fillAmount = (float)currentHealth / (float)maxHealth;

        // based on units selected decide stopping distance
        if (UnitSelections.Instance.unitsSelected.Count < 4)
        {
            if (soldierAgent.remainingDistance <= 1f)
            {
                soldierAgent.ResetPath();
            }
        }
        else if (UnitSelections.Instance.unitsSelected.Count >= 4 && UnitSelections.Instance.unitsSelected.Count < 6)
        {
            if (soldierAgent.remainingDistance <= 2f)
            {
                soldierAgent.ResetPath();
            }
        }
        else
        {
            if (soldierAgent.remainingDistance <= 3f)
            { 
                soldierAgent.ResetPath();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Boss")
        {
            other.gameObject.SendMessage("SoldierAttacked", gameObject);

            soldierAgent.ResetPath();

            StartCoroutine(Attack(other));
        }
    }

    IEnumerator Attack(Collider enemy)
    {
        /* if soldier leaves battle before killing the enemy */
        if (Vector3.Distance(transform.position, enemy.transform.position) > 5f)
        {
            yield return null;
        }

        animator.SetTrigger("Attack");
        mainAudio.SoldierAttack();

        if(enemy.tag == "Enemy")
            enemy.gameObject.GetComponent<EnemyLogic>().HealthDamage(soldierDamage);
        else
            enemy.gameObject.GetComponent<BossLogic>().HealthDamage(soldierDamage);

        yield return new WaitForSeconds(1f);
        IsEnemyDead(enemy);
    }

    void IsEnemyDead(Collider enemy)
    {
        if (enemy == null)
        {
            return;
        }
        else
            StartCoroutine(Attack(enemy));

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

    public void Heal(int amountToHeal)
    { 
        if (currentHealth + amountToHeal <= maxHealth)
        {
            currentHealth += amountToHeal;
        }
        else
        {
            currentHealth = maxHealth;
        }
    }
}
