using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise2_3 : MonoBehaviour
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

    void FixedUpdate()
    {
        b.CheckBoundaries();
        b.windPush();
        b.gravityPush();
        DynamicMass();
    }

    void Update()
    {
        DynamicScale();
    }

    void DynamicMass()
    {
        b.body.mass = (Random.Range(5f, 10f) / 1f) * Mathf.PI * b.radius * b.radius * b.radius;
        //Debug.Log(b.body.mass);
    }

    void DynamicScale()
    {
        if (b.gameObject.transform.localScale.magnitude < Vector3.zero.magnitude)
        {
            b.gameObject.transform.localScale = Vector3.zero;
        }

        Vector3 changeInScale = new Vector3(b.body.mass, b.body.mass, b.body.mass);
        if(b.body.mass > 25 && b.body.mass <= 32)
        {
            b.gameObject.transform.localScale += changeInScale * Time.deltaTime;
        }
        else if (b.body.mass <= 25 && b.body.mass >= 15)
        {
            b.gameObject.transform.localScale += -changeInScale * Time.deltaTime;
        }
        else
        {
            b.gameObject.transform.localScale = changeInScale * Time.deltaTime;
            Debug.Log(b.body.mass);
        }

    }
}
