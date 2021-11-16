using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise2_2 : MonoBehaviour
{
    public float floorY;
    public float ceilingY;
    public float leftWallX;
    public float rightWallX;
    public Transform balloonSpawn;

    Balloon b;

    // Start is called before the first frame update
    void Start()
    {
        b = new Balloon(balloonSpawn.position, leftWallX, rightWallX, floorY, ceilingY);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        b.CheckBoundaries();
        DynamicMass();
        b.windPush();
        b.gravityPush();
    }

    void DynamicMass()
    {
        b.body.mass = (Random.Range(0f, 4f) / Random.Range(0f, 3f)) * Mathf.PI * b.radius * b.radius * b.radius;
        Debug.Log(b.body.mass);
    }
}
