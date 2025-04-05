using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    Health health;
    Movement movement;
    Shooter shooter;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        movement = GetComponent<Movement>();
        shooter = GetComponent<Shooter>();

        GameManager.GetInstance().AddTank(this);
        GameManager.GetInstance().AddPawn(this);
    }

    public void Move(bool forward)
    {
        movement.Move(forward);
    }

    public void Rotate(bool left)
    {
        movement.Rotate(!left);
    }

    public void Fire(string name = "None")
    {
        if (shooter.CanFire())
            shooter.Fire(name);
    }

    public Shooter GetShooter()
    {
        return shooter;
    }

    public Movement GetMovement()
    {
        return movement;
    }

    public Health GetHealth()
    {
        return health;
    }
}
