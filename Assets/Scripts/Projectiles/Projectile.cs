using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private string owner; //who owns it
    [SerializeField]
    private int damage; //damage dealt

    [SerializeField]
    private float lifeTime; //how long the projectile is alive for before being destroyed automatically
    public void SetData(string owner, int damage) //sets projectile data, called by instantiator
    {
        this.owner = owner;
        this.damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime-= Time.deltaTime; //reduce how long it has left to live

        if (lifeTime < 0) //if liftime is 0
        {
            Destroy(gameObject); //destroy object
        }
    }

    private void OnTriggerEnter(Collider other) //handles trigger collisision
    {
        Health health = other.GetComponent<Health>();
        if (health != null) //if it has a health component
        {
            health.TakeDamage(damage); //deal damage
        }

        Destroy(gameObject);
    }
}
