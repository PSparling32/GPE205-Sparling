using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public struct MovementData
{
    public float movementSpeed;
    public float rotationSpeed;
}

public class Movement : MonoBehaviour
{
    [SerializeField]
    private MovementData movementData;

    [SerializeField]
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>(); //cache rigid body
    }

    // Update is called once per frame
    void Update() { }

    public void Move(bool forward)
    {
        Vector3 direction = rb.transform.forward;
        direction.y = 0;
        direction.Normalize();
        float speed = forward ? movementData.movementSpeed : -movementData.movementSpeed;
        rb.MovePosition(rb.transform.position + (direction * speed * Time.deltaTime));
    }

    public void Rotate(bool clockwise)
    {
        float rotationSpeed = clockwise ? movementData.rotationSpeed : -movementData.rotationSpeed;
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

    //returns speed
    public float Speed
    {
        get { return movementData.movementSpeed; }
    }
}
