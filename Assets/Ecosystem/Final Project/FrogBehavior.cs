using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehavior : MonoBehaviour
{
    /// <summary>
    /// All of the blocks of commented code are going to be modified eventuall for the frog tongue, which is why they haven't
    /// been deleted.
    /// </summary>

    //Box walls to help with boundaries for frog.
    public Transform leftWall;
    public Transform rightWall;
    public Transform ceiling;
    public Transform floor;
    public Transform backWall;
    public Transform frontWall;
    public Transform frogSpawn;

    public GameObject fly;

    //public bool jumpCooldownElapsed = true;

    Frog frog;
    FrogTongue frogTongue;

    // Start is called before the first frame update
    void Start()
    {
        frog = new Frog(frogSpawn.position, leftWall.position.x, rightWall.position.x, floor.position.y, ceiling.position.y, frontWall.position.z, backWall.position.z);
        frogTongue = new FrogTongue(frogSpawn.position);
        frogTongue.oGameObject.transform.SetParent(frog.frogObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        fly = GameObject.Find("fly");

        //make sure the frog is still in the box
        frog.CheckBoundaries();

        //frogTongue.velocity = fly.transform.position;
        //frogTongue.amplitude = fly.transform.position;

        ////Each oscillator object oscillating on the x-axis
        //float x = Mathf.Sin(frogTongue.angle.x) * frogTongue.amplitude.x;
        ////Each oscillator object oscillating on the y-axis
        //float y = Mathf.Sin(frogTongue.angle.y) * frogTongue.amplitude.y;
        //float z = Mathf.Sin(frogTongue.angle.z) * frogTongue.amplitude.z;

        ////Add the oscillator's velocity to its angle
        //frogTongue.angle += frogTongue.velocity;

        //frogTongue.oGameObject.transform.Translate(new Vector3(x, y, z) * Time.deltaTime);

        ////frogTongue.oGameObject.transform.LookAt(fly.transform.position);
        ////frogTongue.oGameObject.transform.Translate(fly.transform.position * Time.deltaTime);


        //// Draw the line for each oscillator
        //frogTongue.lineRender.SetPosition(0, frog.frogObject.transform.position);
        ////Debug.Log(sMover.mover.transform.position);
        //frogTongue.lineRender.SetPosition(1, frogTongue.oGameObject.transform.position);
        ////Destroy(frog.frogObject);
        ////Destroy(frogTongue.oGameObject);
        ////Destroy(frogTongue.lineRender);

        //only jump every 2 seconds
        if (frog.jumpCooldownElapsed)
        {
            frog.jumpCooldownElapsed = false;
            StartCoroutine(frog.JumpCooldown());
        }
    }

    ////wait for 2 seconds, then call the jump function
    //IEnumerator JumpCooldown()
    //{
    //    yield return new WaitForSeconds(jumpCooldownTime);

    //    frog.Jump();

    //    frogTongue.velocity = fly.transform.position;
    //    frogTongue.amplitude = fly.transform.position;

    //    //Each oscillator object oscillating on the x-axis
    //    float x = Mathf.Sin(frogTongue.angle.x) * frogTongue.amplitude.x;
    //    //Each oscillator object oscillating on the y-axis
    //    float y = Mathf.Sin(frogTongue.angle.y) * frogTongue.amplitude.y;
    //    float z = Mathf.Sin(frogTongue.angle.z) * frogTongue.amplitude.z;

    //    //Add the oscillator's velocity to its angle
    //    frogTongue.angle += frogTongue.velocity;

    //   // frogTongue.oGameObject.transform.Translate(new Vector3(x, y, z) * Time.deltaTime);

    //    frogTongue.oGameObject.transform.LookAt(fly.transform.position);
    //    //frogTongue.oGameObject.transform.Translate(fly.transform.position * Time.deltaTime);


    //    // Draw the line for each oscillator
    //    frogTongue.lineRender.SetPosition(0, frog.frogObject.transform.position);
    //    //Debug.Log(sMover.mover.transform.position);
    //    frogTongue.lineRender.SetPosition(1, frogTongue.oGameObject.transform.position);
    //    //Destroy(frog.frogObject);
    //    //Destroy(frogTongue.oGameObject);
    //    //Destroy(frogTongue.lineRender);

    //    jumpCooldownElapsed = true;
    //}
}

public class Frog
{
    //how long the frog should wait between jumps.
    public float jumpCooldownTime = Random.Range(1f, 2f);

    public Vector3 jumpForce;

    public List<Transform> flyPrey = new List<Transform>();
    public Rigidbody body;
    public GameObject frogObject;
    public SphereCollider frogCollider;
    public float radius;

    public bool jumpCooldownElapsed = true;

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
        frogObject.name = "Frog";
        frogObject.tag = "FlyPredator";
        body = frogObject.AddComponent<Rigidbody>();

        ///<summary>
        ///The PredatorAttack Script is having weird force errors, and I'm not quite sure how to fix them
        /// </summary>
        //PredatorAttack frogPredator = frogObject.AddComponent<PredatorAttack>();
        //frogPredator.predatorTag = "FlyPredator";

        frogCollider = frogObject.GetComponent<SphereCollider>();
        Renderer frogRenderer = frogObject.GetComponent<Renderer>();

        frogRenderer.material.SetColor("_Color", Color.green);

        frogCollider.enabled = false;

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

        ///POSSIBLE FIX FOR FROG BOUNDARIES IF NECESSARY
        ////Get the current position of each leg every fixed update. If one of the legs has gone beyond the box, then reset it to be at the box border.
        //float xPos = frogObject.transform.position.x;
        //float yPos = frogObject.transform.position.y;
        //float zPos = frogObject.transform.position.z;

        ////x position
        //if (xPos > xMax - radius)
        //{
        //    frogObject.transform.position = new Vector3(xMax - radius, yPos, zPos);
        //}
        //else if (xPos < xMin + radius)
        //{
        //    frogObject.transform.position = new Vector3(xMin + radius, yPos, zPos);
        //}

        ////y position
        //if (yPos > yMax - radius)
        //{
        //    frogObject.transform.position = new Vector3(xPos, yMax - radius, zPos);
        //}
        //else if (yPos < yMin + radius)
        //{
        //    frogObject.transform.position = new Vector3(xPos, yMin + radius, zPos);
        //}

        ////z position
        //if (zPos > zMax - radius)
        //{
        //    frogObject.transform.position = new Vector3(xPos, yPos, zMax - radius);
        //}
        //else if (zPos < zMin + radius)
        //{
        //    frogObject.transform.position = new Vector3(xPos, yPos, zMin + radius);
        //}
    }

    //wait for 2 seconds, then call the jump function
    public IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldownTime);

        Jump();

        //frogTongue.velocity = fly.transform.position;
        //frogTongue.amplitude = fly.transform.position;

        ////Each oscillator object oscillating on the x-axis
        //float x = Mathf.Sin(frogTongue.angle.x) * frogTongue.amplitude.x;
        ////Each oscillator object oscillating on the y-axis
        //float y = Mathf.Sin(frogTongue.angle.y) * frogTongue.amplitude.y;
        //float z = Mathf.Sin(frogTongue.angle.z) * frogTongue.amplitude.z;

        ////Add the oscillator's velocity to its angle
        //frogTongue.angle += frogTongue.velocity;

        //// frogTongue.oGameObject.transform.Translate(new Vector3(x, y, z) * Time.deltaTime);

        //frogTongue.oGameObject.transform.LookAt(fly.transform.position);
        ////frogTongue.oGameObject.transform.Translate(fly.transform.position * Time.deltaTime);


        //// Draw the line for each oscillator
        //frogTongue.lineRender.SetPosition(0, frog.frogObject.transform.position);
        ////Debug.Log(sMover.mover.transform.position);
        //frogTongue.lineRender.SetPosition(1, frogTongue.oGameObject.transform.position);
        ////Destroy(frog.frogObject);
        ////Destroy(frogTongue.oGameObject);
        ////Destroy(frogTongue.lineRender);

        jumpCooldownElapsed = true;
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

    public Transform GetClosestPrey(List<Transform> prey)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = frogObject.transform.position;
        foreach (Transform potentialTarget in prey)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    //TODO add way for frog to kill fly
    public void HuntClosestPrey(GameObject predator, Transform preyPos)
    {
        Vector3 relativePos = preyPos.position - predator.transform.position;
        predator.GetComponent<Rigidbody>().AddForce(100 * relativePos);
    }
}

public class FrogTongue
{
    public float maxDistance = 1f;
    public float minDistance = 0f;
    float radius;

    // The basic properties of an oscillator class
    public Vector3 velocity, angle, amplitude;

    // The window limits
    private Vector3 maximumPos;

    // Gives the class a GameObject to draw on the screen
    public GameObject oGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    //Create variables for rendering the line between two vectors
    public LineRenderer lineRender;

    public FrogTongue(Vector3 position)
    {

        radius = 1f;

        oGameObject.transform.position = position + Vector3.up * radius;

        //findWindowLimits();
        angle = Vector3.zero;
        velocity = new Vector3(0.05f, 0.05f, 0.05f);//Random.Range(-0.05f, 0.05f), 0f, Random.Range(-0.05f, 0.05f));
        amplitude = new Vector3(5f, 5f, 5f);//Random.Range(-maximumPos.x / 2, maximumPos.x / 2), Random.Range(-maximumPos.y / 2, maximumPos.y / 2), Random.Range(-maximumPos.z / 2, maximumPos.z / 2));

        //We need to create a new material for WebGL
        Renderer r = oGameObject.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));

        // Create a GameObject that will be the line
        GameObject lineDrawing = new GameObject();
        lineDrawing.transform.SetParent(oGameObject.transform);

        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Diffuse"));

        //Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        //Make sure the end of the line (1) appears at the new Vector3
        Vector3 center = new Vector2(oGameObject.transform.position.x, oGameObject.transform.position.y);
        lineRender.SetPosition(0, center);
        lineRender.SetPosition(1, center);
    }
}
