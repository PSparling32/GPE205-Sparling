using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    Transform projectileSpawnPoint; //where the projectile spawns and the forward

    [SerializeField]
    private int damage = 1; //damage fired projectiles deal

    [SerializeField]
    private int fireDelay = 1;//seconds between shots

    [SerializeField]
    private int projectileForce; //how much force the projectile is fired with

    [SerializeField]
    private GameObject projectilePrefab; //the prefab to use for the projectile

    private float projectileCooldown; //control variable, time remaining untill another projectile can be fired
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        projectileCooldown = projectileCooldown > 0 ? projectileCooldown - Time.deltaTime : 0;
    }

    public void Fire(string owner = "None")
    {
        //make a projectile, caches it, sets transform to the projectile spawn
        Rigidbody rb = Instantiate(projectilePrefab, projectileSpawnPoint).GetComponent<Rigidbody>();
        rb.transform.parent = null;
        rb.transform.position = projectileSpawnPoint.position;
        rb.transform.rotation = projectileSpawnPoint.rotation;

        //sets data
        rb.GetComponent<Projectile>().SetData(owner, damage);

        //launches data
        rb.AddForce(rb.transform.forward*projectileForce, ForceMode.Impulse);

        //reset cooldown
        projectileCooldown = fireDelay;
    }

    public bool CanFire() //checks if the shooter can shoot
    {
        return projectileCooldown <= 0;
    }
}
