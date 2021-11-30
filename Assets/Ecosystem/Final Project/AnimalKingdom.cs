using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalKingdom : MonoBehaviour
{
    public List<Fly> flies = new List<Fly>();
    public int sporadicFlyPopulation;
    public int sporadicFlyMinPopulation;

    public List<Frog> frogs = new List<Frog>();
    public int frogPopulation;
    public int frogMinPopulation;

    public List<SpiderMover> spiders = new List<SpiderMover>();
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
            Fly fly = new Fly(new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax)), xMin, xMax, yMin, yMax, zMin, zMax);
            flies.Add(fly);

        }
        for (int i = 0; i < frogPopulation; i++)
        {
            Frog frog = new Frog(new Vector3(Random.Range(xMin, xMax), yMin, Random.Range(zMin, zMax)), xMin, xMax, yMin, yMax, zMin, zMax);
            frogs.Add(frog);
        }
        for (int i = 0; i < spiderPopulation; i++)
        {
            SpiderMover spider = new SpiderMover(new Vector3(Random.Range(xMin, xMax), yMin, Random.Range(zMin, zMax)), xMin, xMax, yMin, yMax, zMin, zMax);
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
            }
        }
        foreach (SpiderMover s in spiders)
        {
            s.CheckBoundaries();
            s.Move();
        }
    }
}
