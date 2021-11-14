using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise1_1 : MonoBehaviour
{
    //pig location and speed
    Vector2 location = new Vector2(0, 0); //Vector2.zero
    Vector2 velocity = new Vector2(0.01f, 0.01f);

    //Screen information
    Vector2 minimumPosition, maximumPosition;

    GameObject pig;


    // Start is called before the first frame update
    void Start()
    {
        CameraSetup();

        //Create our Pig
        pig = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    // Update is called once per frame
    void Update()
    {
        // Each frame, we will check to see if the mover has touched a boarder
        // We check if the X/Y position is greater than the max position OR if it's less than the minimum position
        bool xHitBorder = location.x > maximumPosition.x || location.x < minimumPosition.x;
        bool yHitBorder = location.y > maximumPosition.y || location.y < minimumPosition.y;

        // If the mover has hit at all, we will mirror it's speed with the corrisponding boarder

        if (xHitBorder)
        {
            velocity.x = -velocity.x;
        }

        if (yHitBorder)
        {
            velocity.y = -velocity.y;
        }

        location += velocity;

        // Now we apply the positions to the mover to put it in it's place
        pig.transform.position = new Vector2(location.x, location.y);
    }

    void CameraSetup()
    {
        Camera.main.orthographic = true;
        minimumPosition = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
