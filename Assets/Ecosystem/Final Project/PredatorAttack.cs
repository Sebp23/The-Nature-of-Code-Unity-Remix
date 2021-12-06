using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorAttack : MonoBehaviour
{
    public float gravityForce = 6.7f;
    public float mass;
    Vector3 location;
    public GameObject predator;
    public string predatorTag;
    public bool alive = true;

    AnimalKingdom ecosystem;
    List<GameObject> chapterCreatures;

    // Start is called before the first frame update
    void Start()
    {
        ecosystem = GameObject.Find("Scripts").GetComponent<AnimalKingdom>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            GameObject[] predators = GameObject.FindGameObjectsWithTag(predatorTag);

            if (predators.Length > 0)
            {
                foreach(GameObject predator in predators)
                {
                    location = this.gameObject.transform.position;

                    predator.transform.GetComponent<Rigidbody>().AddForce(predator.transform.forward, ForceMode.Acceleration);
                    predator.transform.GetComponent<Rigidbody>().AddForce(Attract(predator), ForceMode.Acceleration);

                    float dist = Vector3.Distance(predator.transform.position, location);

                    if (dist <= 4f)
                    {
                        alive = false;
                        if (predatorTag == "FlyPredator")
                        {
                            ecosystem.flies.Remove(this.ecosystem.fly);
                        }
                        else if (predatorTag == "FrogPredator")
                        {
                            ecosystem.frogs.Remove(this.ecosystem.frog);
                        }
                        else if (predatorTag == "SpiderPredator")
                        {
                            ecosystem.spiders.Remove(this.ecosystem.spider);
                        }

                        //Destroy(gameObject);
                    }
                }
            }
        }
        else
        {
            //do nothing, you are dead :/
        }
    }

    public Vector3 Attract(GameObject predator)
    {
        Vector3 difference = location - predator.transform.position;
        float dist = difference.magnitude;
        Vector3 gravityDirection = difference.normalized;
        float gravity = gravityForce * (mass * predator.GetComponent<Rigidbody>().mass / (dist * dist));
        Vector3 gravityVector = gravityDirection * gravity;

        return gravityVector;
    }
}
