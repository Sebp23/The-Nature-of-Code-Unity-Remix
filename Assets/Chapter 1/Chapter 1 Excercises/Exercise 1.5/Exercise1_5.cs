using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise1_5 : MonoBehaviour
{
    //instance of car class
    Car c;

    // Start is called before the first frame update
    void Start()
    {
        //construct the object
        c = new Car();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up"))
        {
            //make sure it's recognizing the input
            //Debug.Log("accelerate");

            c.acceleration += new Vector2(0.01f, 0.01f);
        }

        if (Input.GetKey("down"))
        {
            //Debug.Log("brake");
            c.acceleration += new Vector2(-0.01f, -0.01f);
        }

        c.MoveCar();
        c.CheckEdges();
    }
}

public class Car
{
    public Vector2 velocity, location, acceleration;
    float topSpeed;

    //window limits
    Vector2 minimumPos, maximumPos;

    //car
    GameObject engine = GameObject.CreatePrimitive(PrimitiveType.Cube);

    //constructor for the class
    public Car()
    {
        CameraSetup();
        location = Vector2.zero;
        velocity = Vector2.zero;
        acceleration = new Vector2(0.01f, 0.01f);
        topSpeed = 10f;
    }

    //Accelerate/brake the car
    public void MoveCar()
    {

        //speed up the car
        velocity += acceleration * Time.deltaTime;

        //limit the velocity to the topSpeed
        velocity = Vector2.ClampMagnitude(velocity, topSpeed);

        location += velocity * Time.deltaTime;

        if (velocity.x > Vector2.zero.x || velocity.y > Vector2.zero.y)
        {
            engine.transform.position = new Vector2(location.x, location.y);
        }
        else
        {
            velocity = Vector2.zero;
        }

        Debug.Log(velocity);
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x -= maximumPos.x - minimumPos.x;
        }
        else if (location.x < minimumPos.x)
        {
            location.x += maximumPos.x - minimumPos.x;
        }
        if (location.y > maximumPos.y)
        {
            location.y -= maximumPos.y - minimumPos.y;
        }
        else if (location.y < minimumPos.y)
        {
            location.y += maximumPos.y - minimumPos.y;
        }
    }

    void CameraSetup()
    {
        Camera.main.orthographic = true;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

}