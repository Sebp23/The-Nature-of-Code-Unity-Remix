using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalKingdom : MonoBehaviour
{
    public List<Fly> flies = new List<Fly>();
    public Fly fly;
    public int sporadicFlyPopulation;
    public int sporadicFlyMinPopulation;

    public List<Frog> frogs = new List<Frog>();
    public Frog frog;
    public int frogPopulation;
    public int frogMinPopulation;

    public List<SpiderMover> spiders = new List<SpiderMover>();
    public SpiderMover spider;
    public int spiderPopulation;
    public int spiderMinPopulation;

    //Terrain
    public PerlinTerrain terrain;
    public float terrainMin;

    //Walls (Temporary)
    public float xMin = -10f;
    public float xMax = 10f;
    public float yMin = 0f;
    public float yMax = 20f;
    public float zMin = -10f;
    public float zMax = 10f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sporadicFlyPopulation; i++)
        {
            fly = new Fly(new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax)), xMin, xMax, yMin, yMax, zMin, zMax);
            flies.Add(fly);

        }
        for (int i = 0; i < frogPopulation; i++)
        {
            frog = new Frog(new Vector3(Random.Range(xMin, xMax), yMin, Random.Range(zMin, zMax)), xMin, xMax, yMin, yMax, zMin, zMax);
            frogs.Add(frog);
        }
        for (int i = 0; i < spiderPopulation; i++)
        {
            spider = new SpiderMover(new Vector3(Random.Range(xMin, xMax), yMin, Random.Range(zMin, zMax)), xMin, xMax, yMin, yMax, zMin, zMax);
            spiders.Add(spider);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Fly f in flies)
        {
            f.CheckBoundaries();
            f.MoveFly();
        }
        foreach (Frog fr in frogs)
        {
            fr.CheckBoundaries();
            //only jump every 2 seconds
            if (fr.jumpCooldownElapsed)
            {
                fr.jumpCooldownElapsed = false;
                StartCoroutine(fr.JumpCooldown());
                List<Transform> flyPrey = new List<Transform>();
                foreach (Fly f in flies)
                {
                    flyPrey.Add(f.fly.transform);
                }
                Transform preyToTarget = GetClosestPrey(flyPrey);
                Debug.Log(preyToTarget);
                HuntClosestPrey(fr.frogObject, preyToTarget);

            }
        }
        foreach (SpiderMover s in spiders)
        {
            s.CheckBoundaries();
            s.MoveLegs();
            s.Move();
        }

        if(flies.Count <= sporadicFlyMinPopulation && flies.Count < sporadicFlyPopulation)
        {
            StartCoroutine(circleOfLife(fly.fly, new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax))));
        }
        if(frogs.Count <= frogMinPopulation && frogs.Count < frogPopulation)
        {
            StartCoroutine(circleOfLife(frog.frogObject, new Vector3(Random.Range(xMin, xMax), yMin, Random.Range(zMin, zMax))));
        }
        if (spiders.Count <= spiderMinPopulation && spiders.Count < spiderPopulation)
        {
            StartCoroutine(circleOfLife(spider.mover, new Vector3(Random.Range(xMin, xMax), yMin, Random.Range(zMin, zMax))));
        }
    }

    IEnumerator circleOfLife(GameObject prey, Vector3 position)
    {
        yield return new WaitForSeconds(2f);
        //Fly
        if(prey.name == fly.fly.name || prey.name == fly.fly.name + "(Clone)")
        {
            Fly f = new Fly(position, xMin, xMax, yMin, yMax, zMin, zMax);
            flies.Add(f);
        }
        //Frog
        if(prey.name == frog.frogObject.name || prey.name == frog.frogObject.name + "(Clone)")
        {
            Frog fr = new Frog(position, xMin, xMax, yMin, yMax, zMin, zMax);
            frogs.Add(fr);
        }
        //Spider
        if(prey.name == spider.mover.name || prey.name == spider.mover.name + "(Clone)")
        {
            SpiderMover s = new SpiderMover(position, xMin, xMax, yMin, yMax, zMin, zMax);
            spiders.Add(s);
        }
    }

    Transform GetClosestPrey(List<Transform> prey)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
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

    void HuntClosestPrey(GameObject predator, Transform preyPos)
    {
        Vector3 relativePos = preyPos.position - predator.transform.position;
        predator.GetComponent<Rigidbody>().AddForce(100 * relativePos);
    }

    //void Hunt(GameObject predator, Transform predatorPos, Transform preyPos)
    //{
    //    Vector3 relativePos = preyPos.position - predatorPos.position;
    //    predator.GetComponent<Rigidbody>().AddForce(100 * relativePos);
    //    //Invoke("ApplyFlightDirection", 1.0f);
    //}
}
