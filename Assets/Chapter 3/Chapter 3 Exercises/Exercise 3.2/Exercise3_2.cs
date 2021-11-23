using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise3_2 : MonoBehaviour
{
    Cannon c;
    Ammo a;
    Vector3 cannonForce;

    // Start is called before the first frame update
    void Start()
    {
        a = new Ammo(Vector3.zero, 1f);
        c = new Cannon(a, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            cannonForce = new Vector3(1f, 1f, 0f);
            c.ShootAmmo(a.body, cannonForce);
        }
    }
}

public class Cannon
{
    GameObject cannonObject;
    Vector3 cannonLocation;
    Quaternion cannonAngle;

    public Cannon(Ammo ammo, Vector3 position)
    {
        cannonObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        CapsuleCollider cC = cannonObject.GetComponent<CapsuleCollider>();
        cC.enabled = false;
        cannonObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        cannonLocation = position;
        cannonAngle = Quaternion.Euler(0f, 0f, -45f);

        cannonObject.transform.position = cannonLocation;
        cannonObject.transform.rotation = cannonAngle;
    }

    public void ShootAmmo(Rigidbody b, Vector3 cannonForce)
    {
        //Quaternion cannonKickback = Quaternion.Euler(0f, 0f, -1f);
        
        b.AddForce(cannonForce, ForceMode.Impulse);

        //angular rotation
        b.AddTorque(cannonForce, ForceMode.Impulse);

        //cannonAngle = cannonKickback;
        
    }
}

public class Ammo
{
    GameObject ammoObject;
    public Rigidbody body;
    Vector3 location, velocity, acceleration, aVelocity, aAcceleration;

    public Ammo(Vector3 location, float _mass)
    {
        ammoObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body = ammoObject.AddComponent<Rigidbody>();
        ammoObject.transform.position = location;
        body.mass = _mass;
        body.useGravity = false;
    }
}
