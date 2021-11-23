using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise3_1 : MonoBehaviour
{
    public GameObject Baton;
    float xAngle;
    float yAngle;
    float zAngle;

    //Establish the starting angular velocity and angular rate of acceleration
    Vector3 aVelocity = new Vector3(0f, 0f, 0f);
    Vector3 aAcceleration = new Vector3(0f, 0.001f, 0.001f);


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //every frame, add the acceleration to the velocity
        aVelocity += aAcceleration;
        Baton.transform.Rotate(aVelocity, Space.World);

    }
}
