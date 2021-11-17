using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporadicFlyBehavior : MonoBehaviour
{
    //fly location and speed
    Vector3 location = new Vector3(0, 0, 0);
    public Vector3 velocity = new Vector3(1f, 1f, 1f);
    Vector3 acceleration = new Vector3(0, 0, 0);
    float topSpeed = 4f;

    //Box walls to help with boundaries for fly.
    public Transform leftWall;
    public Transform rightWall;
    public Transform ceiling;
    public Transform floor;
    public Transform backWall;
    public Transform frontWall;

    GameObject fly;


    // Start is called before the first frame update
    void Start()
    {
        //Create our fly
        fly = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    // Update is called once per frame
    void Update()
    {
        CheckBoundaries();
        MoveFly();
    }

    void MoveFly()
    {
        // Random acceleration but it's not normalized!
        acceleration = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range (-1f, 1f));
        // Normilize the acceletation
        acceleration.Normalize();
        // Now we can scale the magnitude as we wish!
        acceleration *= Random.Range(5f, 20f);

        // Speeds up the mover
        velocity += acceleration * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

        velocity = Vector3.ClampMagnitude(velocity, topSpeed);

        location += velocity * Time.deltaTime;

        // Now we apply the positions to the fly to put it in it's place
        fly.transform.position = new Vector3(location.x, location.y, location.z);
    }

    //check to see when the fly hits a wall, then change its direction when it does
    public void CheckBoundaries()
    {
        if (fly.transform.position.x <= leftWall.position.x || fly.transform.position.x >= rightWall.position.x)
        {
            velocity.x = -velocity.x;
        }

        if (fly.transform.position.y <= floor.position.y || fly.transform.position.y >= ceiling.position.y)
        {
            velocity.y = -velocity.y;
        }

        if (fly.transform.position.z <= frontWall.position.z || fly.transform.position.z >= backWall.position.z)
        {
            velocity.z = -velocity.z;
        }
    }
}
