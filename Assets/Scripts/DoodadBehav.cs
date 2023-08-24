using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodadBehav : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    public List<GameObject> clickedFromThis; // list that stores all villagers that clicked this doodad

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        clickedFromThis = new List<GameObject>(); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        if (gameObject.CompareTag("Stone"))
        {
            GameMaster.stone += GameMaster.stoneInterval;
        }

        if (gameObject.CompareTag("Wood"))
        {
            GameMaster.wood += GameMaster.woodInterval;
        }

        Destroy(gameObject);
    }

    void ClickedFrom(GameObject villager)
    {
        clickedFromThis.Add(villager);
    }
}
