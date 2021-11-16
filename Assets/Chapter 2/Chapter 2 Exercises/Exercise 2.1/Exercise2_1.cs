using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise2_1 : MonoBehaviour
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
        b.windPush();
        b.gravityPush();
        Debug.Log(b.gravity);
    }
}

public class Balloon
{
    public Vector3 gravity;
    public Vector3 wind;
    
    public Rigidbody body;
    public GameObject gameObject;
    public float radius;

    float xMin;
    float xMax;
    float yMin;
    float yMax;


    public Balloon(Vector3 position, float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;

        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = gameObject.AddComponent<Rigidbody>();

        radius = 1f;

        gameObject.transform.position = position + Vector3.up * radius;

        gameObject.transform.localScale = 2 * radius * Vector3.one;

        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;

        body.useGravity = false;

        wind = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        gravity = new Vector3(0, 0.9f, 0);
    }

    public void CheckBoundaries()
    {
        Vector3 restrainedVelocity = body.velocity;
        if (body.position.y - radius < yMin)
        {
            // Using the absolute value here is an important safe
            // guard for the scenario that it takes multiple ticks
            // of FixedUpdate for the mover to return to its boundaries.
            // The intuitive solution of flipping the velocity may result
            // in the mover not returning to the boundaries and flipping
            // direction on every tick.
            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
        }
        else if (body.position.y + radius > yMax)
        {
            restrainedVelocity.y = -Mathf.Abs(restrainedVelocity.y);
        }

        if (body.position.x - radius < xMin)
        {
            restrainedVelocity.x = Mathf.Abs(restrainedVelocity.x);
        }
        else if (body.position.x + radius > xMax)
        {
            restrainedVelocity.x = -Mathf.Abs(restrainedVelocity.x);
        }
        body.velocity = restrainedVelocity;
    }

    public void gravityPush()
    {
        gravity = new Vector3(0, Random.Range(0f, 2f), 0);
        body.AddForce(gravity, ForceMode.Force);
    }

    public void windPush()
    {
        body.AddForce(wind, ForceMode.Impulse);
    }
}
