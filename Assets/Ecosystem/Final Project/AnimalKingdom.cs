using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalKingdom : MonoBehaviour
{
    public List<Fly> flies = new List<Fly>();
    public Fly fly;
    public int flyCurrentPopulation;
    public int sporadicFlyMaxPopulation = 15;
    public int sporadicFlyMinPopulation = 10;

    public List<Frog> frogs = new List<Frog>();
    public Frog frog;
    public int frogCurrentPopulation;
    public int frogMaxPopulation = 5;
    public int frogMinPopulation = 1;

    public List<SpiderMover> spiders = new List<SpiderMover>();
    public SpiderMover spider;
    public int spiderCurrentPopulation;
    public int spiderMaxPopulation = 2;
    public int spiderMinPopulation = 1;

    //Terrain
    public PerlinTerrain terrain;
    public float terrainMin;

    //Boundaries
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public float zMin;
    public float zMax;

    public bool frogHuntSuccessful;
    public bool spiderHuntSuccessful;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sporadicFlyMaxPopulation; i++)
        {
            fly = new Fly(new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax)), xMin, xMax, yMin, yMax, zMin, zMax);
            flies.Add(fly);

        }
        for (int i = 0; i < frogMaxPopulation; i++)
        {
            frog = new Frog(new Vector3(Random.Range(xMin, xMax), yMin, Random.Range(zMin, zMax)), xMin, xMax, yMin, yMax, zMin, zMax);
            frogs.Add(frog);
        }
        for (int i = 0; i < spiderMaxPopulation; i++)
        {
            spider = new SpiderMover(new Vector3(Random.Range(xMin, xMax), yMin, Random.Range(zMin, zMax)), xMin, xMax, yMin, yMax, zMin, zMax);
            spiders.Add(spider);
        }

        flyCurrentPopulation = flies.Count;
        frogCurrentPopulation = frogs.Count;
        spiderCurrentPopulation = spiders.Count;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Fly f in flies)
        {
            if (f.flyAlive)
            {
                f.CheckBoundaries();
                f.MoveFly();
            }
        }
        foreach (Frog fr in frogs)
        {
            if (fr.frogAlive)
            {
                fr.CheckBoundaries();
                //only jump every 2 seconds
                if (fr.jumpCooldownElapsed && fr.frogAlive)
                {
                    fr.jumpCooldownElapsed = false;
                    StartCoroutine(fr.JumpCooldown());
                    foreach (Fly f in flies)
                    {
                        if (f.fly != null)
                        {
                            fr.flyPrey.Add(f.fly.transform);
                        }
                    }
                    Transform preyToTarget = fr.GetClosestPrey(fr.flyPrey);

                    if(preyToTarget != null)
                    {
                        int flyPreyID = preyToTarget.GetHashCode();
                        frogHuntSuccessful = fr.HuntClosestPrey(fr.frogObject, preyToTarget);
                        if (frogHuntSuccessful)
                        {
                            frogHuntSuccessful = false;
                            flyCurrentPopulation--;
                        }
                        else
                        {
                            frogHuntSuccessful = fr.HuntClosestPrey(fr.frogObject, preyToTarget);
                        }
                    }
                    else
                    {
                        preyToTarget = fr.GetClosestPrey(fr.flyPrey);
                    }

                }
            }
        }
        foreach (SpiderMover s in spiders)
        {
            s.CheckBoundaries();
            s.MoveLegs();

            foreach (Frog fr in frogs)
            {
                if(fr.frogObject != null)
                {
                    s.frogPrey.Add(fr.frogObject.transform);
                }
            }
            Transform preyToTarget = s.GetClosestPrey(s.frogPrey);

            if (preyToTarget != null)
            {
                spiderHuntSuccessful = s.HuntClosestPrey(s.mover, preyToTarget);
                if (spiderHuntSuccessful)
                {
                    spiderHuntSuccessful = false;
                    frogCurrentPopulation--;
                }
                else
                {
                    spiderHuntSuccessful = s.HuntClosestPrey(s.mover, preyToTarget);
                }
            }
            else
            {
                preyToTarget = s.GetClosestPrey(s.frogPrey);
            }

        }

        //check the current population size of each animal to see if it needs to be increased.
        StartCoroutine(circleOfLife(fly.fly, new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax))));
        StartCoroutine(circleOfLife(frog.frogObject, new Vector3(Random.Range(xMin, xMax), yMin, Random.Range(zMin, zMax))));
        StartCoroutine(circleOfLife(spider.mover, new Vector3(Random.Range(xMin, xMax), yMin, Random.Range(zMin, zMax))));
    }

    IEnumerator circleOfLife(GameObject prey, Vector3 position)
    {
        yield return new WaitForSeconds(2f);

        if (flyCurrentPopulation < sporadicFlyMaxPopulation)
        {
            Fly f = new Fly(position, xMin, xMax, yMin, yMax, zMin, zMax);
            flies.Add(f);
            Debug.Log(flyCurrentPopulation);
            flyCurrentPopulation++;
            Debug.Log(flyCurrentPopulation);
            StopCoroutine("circleOfLife()");
        }

        if (frogCurrentPopulation < frogMaxPopulation)
        {
            Frog fr = new Frog(position, xMin, xMax, yMin, yMax, zMin, zMax);
            frogs.Add(fr);
            frogCurrentPopulation++;
            StopCoroutine("circleOfLife()");
        }

        if (spiderCurrentPopulation < spiderMaxPopulation)
        {
            SpiderMover s = new SpiderMover(position, xMin, xMax, yMin, yMax, zMin, zMax);
            spiders.Add(s);
            spiderCurrentPopulation++;
            StopCoroutine("circleOfLife()");
        }
    }
}
