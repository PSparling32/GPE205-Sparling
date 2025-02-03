using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    string name = "Player";

    [SerializeField]
    private KeyCode keyMoveForward;
    [SerializeField] 
    private KeyCode keyMoveBackward;
    [SerializeField]
    private KeyCode keyRotateLeft;
    [SerializeField]
    private KeyCode keyRotateRight;
    [SerializeField]
    private KeyCode keyFire;

    TankPawn pawn;

    // Start is called before the first frame update
    void Start()
    {
        pawn = GetComponent<TankPawn>();
        GameManager.GetInstance().AddPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {

        //Movement Inputs
        if (Input.GetKey(keyMoveForward))
            pawn.Move(true);
        else if (Input.GetKey(keyMoveBackward))
            pawn.Move(false);

        //Rotational Inputs
        if (Input.GetKey(keyRotateLeft))
            pawn.Rotate(true);
        else if (Input.GetKey(keyRotateRight))
            pawn.Rotate(false);

        //Fire Controls
        if (Input.GetKeyDown(keyFire))
            pawn.Fire(name);
    }
}
