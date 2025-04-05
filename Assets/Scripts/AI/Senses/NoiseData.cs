using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NoiseData
{
    public float Distance { get; private set; }
    public Vector3 Location { get; private set; }

    public NoiseData(float distance, Vector3 location)
    {
        Distance = distance;
        Location = location;
    }
}
