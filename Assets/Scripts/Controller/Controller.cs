using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected TankPawn pawn;
    protected string name = "Controller";

    protected void SetUp()
    {
        pawn = GetComponent<TankPawn>();
        if (pawn == null)
        {
            Debug.LogError("Controller: No TankPawn found on this GameObject.");
        }
        GameManager.GetInstance().AddController(this);
    }
}
