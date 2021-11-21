using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehavior : MonoBehaviour
{
    //how long the frog should wait between jumps.
    public float jumpCooldownTime;

    //Box walls to help with boundaries for frog.
    public Transform leftWall;
    public Transform rightWall;
    public Transform ceiling;
    public Transform floor;
    public Transform backWall;
    public Transform frontWall;
    public Transform frogSpawn;

    public bool jumpCooldownElapsed = true;

    Frog frog;

    // Start is called before the first frame update
    void Start()
    {
        frog = new Frog(frogSpawn.position, leftWall.position.x, rightWall.position.x, floor.position.y, ceiling.position.y, frontWall.position.z, backWall.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //make sure the frog is still in the box
        frog.CheckBoundaries();

        //only jump every 2 seconds
        if (jumpCooldownElapsed)
        {
            jumpCooldownElapsed = false;
            StartCoroutine(JumpCooldown());
        }
    }

    //wait for 2 seconds, then call the jump function
    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldownTime);
        frog.Jump();
        jumpCooldownElapsed = true;
    }
}

public class Frog
{
    public Vector3 jumpForce;

    public Rigidbody body;
    public GameObject frogObject;
    public float radius;

    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float zMin;
    float zMax;

    //frog constructor
    public Frog(Vector3 position, float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;
        this.zMin = zMin;
        this.zMax = zMax;

        frogObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = frogObject.AddComponent<Rigidbody>();

        radius = 1f;

        frogObject.transform.position = position + Vector3.up * radius;

        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;
    }

    public void CheckBoundaries()
    {
        //No need for restrained velocity, since it should stop after every jump. These fields below essentially makes sure it stays in the box.
        //If the frog attempts to go outside of the box, its position will simply reset to be inside the box. The use of positive
        //and negative in the field names, is simply to distinguish whether it is adding or subtracting the radius to get the position it should be sent
        //back to.
        Vector3 originXPositive = new Vector3(xMin + radius, frogObject.transform.position.y, frogObject.transform.position.z);
        Vector3 originXNegative = new Vector3(xMax - radius, frogObject.transform.position.y, frogObject.transform.position.z);
        Vector3 originY = new Vector3(frogObject.transform.position.x, yMin + radius, frogObject.transform.position.z);
        Vector3 originZPositive = new Vector3(frogObject.transform.position.x, frogObject.transform.position.y, zMin + radius);
        Vector3 originZNegative = new Vector3(frogObject.transform.position.x, frogObject.transform.position.y, zMax - radius);

        if (body.position.y - radius < yMin)
        {
            body.velocity = Vector3.zero;
            frogObject.transform.position = originY;
        }
        //the frog should never get high enough to touch the ceiling, so no worries there.

        if (body.position.x - radius < xMin)
        {
            frogObject.transform.position = originXPositive;
        }
        else if (body.position.x + radius > xMax)
        {
            frogObject.transform.position = originXNegative;
        }

        if (body.position.z - radius < zMin)
        {
            frogObject.transform.position = originZPositive;
        }
        else if (body.position.z + radius > zMax)
        {
            frogObject.transform.position = originZNegative;
        }
    }

    public void Jump()
    {
        //make sure the frog is on the ground before it jumps
        if(frogObject.transform.position.y <= yMin + radius)
        {
            //make sure that the frog is adding to its velocity when it jumps
            body.velocity = Vector3.zero;

            //change the direction it jumps. It will always jump the same height.
            jumpForce = new Vector3(Random.Range(-10f, 10f), 25f, Random.Range(-10f, 10f));
            body.AddForce(jumpForce, ForceMode.Impulse);
        }
    }
}
