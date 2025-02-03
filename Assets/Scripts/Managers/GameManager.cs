using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    List<PlayerController> playerControllers = new List<PlayerController>();
    List<TankPawn> tankPawns = new List<TankPawn>();
    List<Pawn> pawns = new List<Pawn>();

    private static GameManager instance;
    public static GameManager GetInstance()//get instance
    {
        if (instance == null) //if one does not exist
            instance = new GameManager();//set a new instance
        return instance;//return instance
    }
    public void AddTank(TankPawn pawn)
    {
        tankPawns.Add(pawn);
        pawns.Add(pawn);
    }

    public void RemoveTank(TankPawn pawn)
    {
        tankPawns.Remove(pawn);
        pawns.Remove(pawn);
    }

    public void AddPlayer(PlayerController player)
    {
        playerControllers.Add(player);
    }

    public void RemovePlayer(PlayerController player)
    {
        playerControllers.Remove(player);
    }

    public void AddPawn(Pawn pawn)
    {
        pawns.Add(pawn);
    }

    public void RemovePawn(Pawn pawn)
    {
        pawns.Remove(pawn);
    }
}
