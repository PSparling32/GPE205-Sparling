using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShooterData
{
    public Transform projectileSpawnPoint;
    public int damage;
    public int fireDelay;
    public int projectileForce;
    public float projectileLifespan;
    public GameObject projectilePrefab;
}

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private ShooterData shooterData;

    private float projectileCooldown; //control variable, time remaining until another projectile can be fired

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        projectileCooldown = Mathf.Max(0, projectileCooldown - Time.deltaTime);
    }

    public void Fire(string owner = "None")
    {
        if (CanFire())
        {
            //make a projectile, caches it, sets transform to the projectile spawn
            Rigidbody rb = Instantiate(
                    shooterData.projectilePrefab,
                    shooterData.projectileSpawnPoint.position,
                    shooterData.projectileSpawnPoint.rotation
                )
                .GetComponent<Rigidbody>();

            //sets data
            rb.GetComponent<Projectile>()
                .SetData(owner, shooterData.damage, shooterData.projectileLifespan);

            //launches data
            rb.AddForce(rb.transform.forward * shooterData.projectileForce, ForceMode.Impulse);

            //reset cooldown
            projectileCooldown = shooterData.fireDelay;
        }
    }

    public bool CanFire() //checks if the shooter can shoot
    {
        return projectileCooldown <= 0;
    }

    public Vector3 GetBarrelForward()
    {
        return shooterData.projectileSpawnPoint.forward;
    }

    /// Returns the direction the projectile will travel in
    public Vector3 GetBarrelPosition()
    {
        return shooterData.projectileSpawnPoint.position;
    }

    /// Returns the maximum distance the projectile can travel based on its speed and lifespan
    public float GetFireDistance()
    {
        // Calculate the maximum distance the projectile can travel based on its speed and lifespan
        float projectileSpeed = shooterData.projectileForce;
        float projectileLifespan = shooterData.projectileLifespan;
        return projectileSpeed * projectileLifespan;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(GetBarrelPosition(), GetBarrelPosition() + GetBarrelForward() * 10);

        // Calculate the maximum distance the projectile can travel based on its speed and lifespan
        float maxDistance = GetFireDistance();

        // Draw a line from the barrel position to the maximum distance
        Gizmos.DrawLine(
            GetBarrelPosition(),
            GetBarrelPosition() + GetBarrelForward() * maxDistance
        );
    }
}
