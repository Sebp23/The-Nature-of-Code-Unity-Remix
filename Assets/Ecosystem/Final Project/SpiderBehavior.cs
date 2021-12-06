using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehavior : MonoBehaviour
{
    //Box walls to help with boundaries for frog.
    public Transform leftWall;
    public Transform rightWall;
    public Transform ceiling;
    public Transform floor;
    public Transform backWall;
    public Transform frontWall;

    public Transform spiderSpawn;

    SpiderMover sMover;

    // Start is called before the first frame update
    void Start()
    {
        sMover = new SpiderMover(spiderSpawn.position, leftWall.position.x, rightWall.position.x, floor.position.y, ceiling.position.y, frontWall.position.z, backWall.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        sMover.CheckBoundaries();
        sMover.MoveLegs();
        sMover.Move();
    }
}

public class SpiderMover
{
    float radius = 1f;

    Vector3 location = new Vector3(0, 0, 0);
    Vector3 velocity = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)); //1f, 1f, 1f);

    List<SpiderLegOscillator> legs = new List<SpiderLegOscillator>();

    public Vector3 spiderPosition;

    // Variables to limit the mover within the screen space
    float xMin, yMin, zMin, xMax, yMax, zMax;

    // A Variable to represent our mover in the scene
    public GameObject mover;
    Rigidbody body;

    public SpiderMover(Vector3 position, float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;
        this.zMin = zMin;
        this.zMax = zMax;

        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

       //mover.transform.position = new Vector3(Random.Range(xMin))

        mover.AddComponent<Rigidbody>();
        body = mover.GetComponent<Rigidbody>();
        body.freezeRotation = true;
        body.useGravity = false;

        //body.freezeRotation = true;

        SphereCollider moverCollider = mover.GetComponent<SphereCollider>();
        moverCollider.enabled = false;

        location = position + Vector3.up * radius;
        Debug.Log(mover.transform.position + "|" + position);

        mover.name = "Spider";
        mover.tag = "FrogPredator";

        spiderPosition = mover.transform.position;

        while (legs.Count < 8)
        {
            Vector3 spiderCurrentPos = mover.transform.position;

            legs.Add(new SpiderLegOscillator(spiderCurrentPos, xMin, xMax, yMin, yMax, zMin, zMax));//oMover.mover.transform.position));
        }

        foreach (SpiderLegOscillator s in legs)
        {
            s.oGameObject.transform.SetParent(mover.transform);

            //make the leg color black to differentiate from the spider body
            Renderer sphereColor = s.oGameObject.GetComponent<Renderer>();
            sphereColor.material.color = new Color(0f, 0f, 0f);
        }
    }

    public void Move()
    {
        // Now we apply the positions to the mover to put it in it's place
        //mover.transform.position += new Vector3(x, y, z);

        location += velocity * Time.deltaTime;

        // Now we apply the positions to the fly to put it in it's place
        mover.transform.position = new Vector3(location.x, location.y, location.z);
    }

    public void CheckBoundaries()
    {
        // Each frame, we will check to see if the mover has touched a boarder
        // We check if the X/Y position is greater than the max position OR if it's less than the minimum position
        bool xHitBorder = location.x > xMax - radius || location.x < xMin + radius;
        bool yHitBorder = location.y > yMax - radius || location.y < yMin + radius;
        bool zHitBorder = location.z > zMax - radius || location.z < zMin + radius;

        // If the mover has hit at all, we will mirror it's speed with the corrisponding boarder

        if (xHitBorder)
        {
            velocity.x = -velocity.x;
        }

        if (yHitBorder)
        {
            velocity.y = 0;
        }

        if (zHitBorder)
        {
            velocity.z = -velocity.z;
        }
    }

    public void MoveLegs()
    {
        foreach (SpiderLegOscillator s in legs)
        {
            s.CheckLegBoundary();
            //Each oscillator object oscillating on the x-axis
            float x = Mathf.Sin(s.angle.x) * s.amplitude.x;
            //Each oscillator object oscillating on the y-axis
            float y = Mathf.Sin(s.angle.y) * s.amplitude.y;
            //Each oscillator object oscillating on the z-axis
            float z = Mathf.Sin(s.angle.z) * s.amplitude.z;

            //Add the oscillator's velocity to its angle
            s.angle += s.velocity;

            // Draw the line for each oscillator
            s.lineRender.SetPosition(0, mover.transform.position);
            Debug.Log(mover.transform.position);
            s.lineRender.SetPosition(1, s.oGameObject.transform.position);

            //Move the oscillator
            s.oGameObject.transform.Translate(new Vector3(x, y, z) * Time.deltaTime);
            s.CheckLegBoundary();
        }
    }
}

public class SpiderLegOscillator
{
    // Variables to limit the mover within the box
    float xMin, yMin, zMin, xMax, yMax, zMax;

    float radius = 1f;

    // The basic properties of an oscillator class
    public Vector3 velocity, angle, amplitude;

    // Gives the class a GameObject to draw on the screen
    public GameObject oGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    //Create variables for rendering the line between two vectors
    public LineRenderer lineRender;

    public SpiderLegOscillator(Vector3 startPosition, float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;
        this.zMin = zMin;
        this.zMax = zMax;

        oGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        oGameObject.transform.position = startPosition;
        
        angle = Vector3.zero;
        velocity = new Vector3(Random.Range(-0.05f, 0.05f), 0f, Random.Range(-0.05f, 0.05f));
        amplitude = new Vector3(0.5f, 0f, 0.5f);

        //We need to create a new material for WebGL
        Renderer r = oGameObject.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));

        // Create a GameObject that will be the line
        GameObject lineDrawing = new GameObject();
        lineDrawing.transform.SetParent(oGameObject.transform);
        
        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Diffuse"));

        lineRender.startWidth = 0.5f;

        //Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        //Make sure the end of the line (1) appears at the new Vector3
        Vector3 center = Vector3.zero;
        lineRender.SetPosition(0, center);
    }

    //make sure the legs never leave the box
    public void CheckLegBoundary()
    {
        //Get the current position of each leg every fixed update. If one of the legs has gone beyond the box, then reset it to be at the box border.
        float xPos = oGameObject.transform.position.x;
        float yPos = oGameObject.transform.position.y;
        float zPos = oGameObject.transform.position.z;

        //x position
        if (xPos > xMax - radius)
        {
            oGameObject.transform.position = new Vector3(xMax - radius, yPos, zPos);
        }
        else if (xPos < xMin + radius)
        {
            oGameObject.transform.position = new Vector3(xMin + radius, yPos, zPos);
        }

        //y position
        if (yPos > yMax - radius)
        {
            oGameObject.transform.position = new Vector3(xPos, yMax - radius, zPos);
        }
        else if (yPos < yMin + radius)
        {
            oGameObject.transform.position = new Vector3(xPos, yMin + radius, zPos);
        }

        //z position
        if (zPos > zMax - radius)
        {
            oGameObject.transform.position = new Vector3(xPos, yPos, zMax - radius);
        }
        else if (zPos < zMin + radius)
        {
            oGameObject.transform.position = new Vector3(xPos, yPos, zMin + radius);
        }

    }
}

