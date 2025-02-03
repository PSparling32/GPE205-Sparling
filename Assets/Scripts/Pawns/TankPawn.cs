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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(bool forward)
    {
        if (forward)
            movement.MoveForward();
        else
            movement.MoveBackwards();
    }

    public void Rotate(bool left)
    {
        if(left)
            movement.RotateLeft();
        else
            movement.RotateRight();
    }

    public void Fire(string name = "None")
    {
        if(shooter.CanFire())
            shooter.Fire(name);
    }
}
