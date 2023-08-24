using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class PlagueDoctorLogic : MonoBehaviour
{
    public NavMeshAgent plagueDoctorAgent;
    public Animator animator;
    public PlayMusic mainAudio;

    public Image health;

    public int doctorDamage = 40;

    public int maxHealth = 100;
    int currentHealth;
    float radius = 10f;
    bool attack;

    Collider enemy;

    // Start is called before the first frame update
    void Start()
    {
        plagueDoctorAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        mainAudio = GameObject.Find("MainAudio").GetComponent<PlayMusic>();
        animator = GetComponent<Animator>();
        attack = true;
        enemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        health.fillAmount = (float)currentHealth / (float)maxHealth;

        // based on units selected decide stopping distance
        if (UnitSelections.Instance.unitsSelected.Count < 4)
        {
            if (plagueDoctorAgent.remainingDistance > 1f)
            {
                animator.SetTrigger("Walk");
            }
            else
            {
                animator.SetTrigger("Stop");
                plagueDoctorAgent.ResetPath();
            }
        }
        else if (UnitSelections.Instance.unitsSelected.Count >= 4 && UnitSelections.Instance.unitsSelected.Count < 6)
        {
            if (plagueDoctorAgent.remainingDistance > 2f)
            {
                animator.SetTrigger("Walk");
            }
            else
            {
                animator.SetTrigger("Stop");
                plagueDoctorAgent.ResetPath();
            }
        }
        else
        {
            if (plagueDoctorAgent.remainingDistance > 3f)
            {
                animator.SetTrigger("Walk");
            }
            else
            {
                animator.SetTrigger("Stop");
                plagueDoctorAgent.ResetPath();
            }
        }

        CheckIfEnemyInRadius();
    }

    // priority goes to enemies closest to this unit
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
            enemy = other;
    }

    // check for enemis that are in range
    private void CheckIfEnemyInRadius()
    {
        if (!attack)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(var col in colliders)
        {
            if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Boss")
            {
                enemy = col;
                attack = false;
            }
        }

        if (enemy != null)
        {
            enemy.gameObject.SendMessage("SoldierAttacked", gameObject);

            plagueDoctorAgent.isStopped = true;

            StartCoroutine(Attack(enemy));

            plagueDoctorAgent.isStopped = false;
        }
    }

    IEnumerator Attack(Collider enemy)
    {
        /* if soldier leaves battle before killing the enemy */
        if (Vector3.Distance(transform.position, enemy.transform.position) > 16f)
        {
            yield break;
        }

        mainAudio.SoldierAttack();

        if (enemy.tag == "Enemy")
            enemy.gameObject.GetComponent<EnemyLogic>().HealthDamage(doctorDamage);
        else
            enemy.gameObject.GetComponent<BossLogic>().HealthDamage(doctorDamage);

        yield return new WaitForSeconds(1f);
        IsEnemyDead(enemy);
    }

    void IsEnemyDead(Collider enemy)
    {
        if (enemy == null)
        {
            attack = true;
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
