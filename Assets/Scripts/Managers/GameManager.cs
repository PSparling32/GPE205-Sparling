using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public delegate void OnSoundDelegate(NoiseData data);
    public event OnSoundDelegate OnSound;

    List<Controller> controller = new List<Controller>();
    List<TankPawn> tankPawns = new List<TankPawn>();
    List<Pawn> pawns = new List<Pawn>();

    private static GameManager instance;

    public static GameManager GetInstance() //get instance
    {
        if (instance == null) //if one does not exist
            instance = new GameManager(); //set a new instance
        return instance; //return instance
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

    public void AddController(Controller c)
    {
        controller.Add(c);
    }

    public void RemoveController(Controller c)
    {
        controller.Remove(c);
    }

    public void AddPawn(Pawn pawn)
    {
        pawns.Add(pawn);
    }

    public void RemovePawn(Pawn pawn)
    {
        pawns.Remove(pawn);
    }

    public void TriggerSound(NoiseData data)
    {
        if (OnSound != null)
        {
            OnSound(data);
        }
    }
}
