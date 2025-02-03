using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    int currentHealth = 2; //current health
    [SerializeField]
    int maxHealth = 4; //maximum health
    [SerializeField]
    bool spawnWithMaxHealth = true; //should the component start with max health

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void OnDie()
    {
        Destroy(gameObject); //Destroys object
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) { //clamps health
            currentHealth = 0;
        }

        if (currentHealth == 0) //is health 0
            OnDie(); //triggers OnDie
    }
}
