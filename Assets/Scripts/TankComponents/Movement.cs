using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed = 10; //forwards and backwards movement speed
    [SerializeField]
    private float rotationSpeed = 10; //rotation speed
    [SerializeField]
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>(); //cache rigid body
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveForward() //moves it forward
    {
        //calculate forward on a 2d surface
        Vector3 direction = rb.transform.forward;
        direction.y = 0;
        direction.Normalize();
        //add to current position
        rb.MovePosition(rb.transform.position + (direction *  movementSpeed * Time.deltaTime));
    }

    public void MoveBackwards()
    {
        //calculate forward on a 2d surface
        Vector3 direction = rb.transform.forward;
        direction.y = 0;
        direction.Normalize();
        //add to current position, reverse the direction
        rb.MovePosition(rb.transform.position + (-direction * movementSpeed * Time.deltaTime));
    }

    public void RotateLeft()
    {
        //rotate counter clockwise
        rb.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
    }

    public void RotateRight()
    {
        //rotate counter clockwise
        rb.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnDrawGizmos()
    {
        if (rb == null) //if the rb is not null, to surpress errors
            return;

        //show what direction forward is
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(rb.transform.position, rb.transform.position + rb.transform.forward * 4);
    }
}
