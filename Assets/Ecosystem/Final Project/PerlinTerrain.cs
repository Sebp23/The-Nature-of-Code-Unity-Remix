using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinTerrain : MonoBehaviour
{
    //List of cube positions
    public List<Vector3> terrainPositionList = new List<Vector3>();
    public List<GameObject> terrainObjectList = new List<GameObject>();
    public GameObject terrainItem;
    public int cols, rows;
    public Color color1, color2, color3, color4, color5, color6;

    private float xOff, yOff;

    // Start is called before the first frame update
    void Start()
    {
        GameObject terrain = new GameObject();
        terrain.name = "terrain";

        xOff = 0f;
        for(int i = 0; i < cols; i++)
        {
            yOff = 0f;
            for(int j = 0; j < rows; j++)
            {
                //we add our code here

                //Rotation of cubes
                Quaternion perlinRotation = new Quaternion();
                float rotationTheta = ExtensionMethods.map(Mathf.PerlinNoise(xOff, yOff), 0f, 1f, 0f, 7f);
                Vector3 perlinVectors = new Vector3(Mathf.Cos(rotationTheta), Mathf.Sin(rotationTheta), 0f);
                perlinRotation.eulerAngles = perlinVectors * 100f;
                
                //Dealing with position
                float positionTheta = ExtensionMethods.map(Mathf.PerlinNoise(xOff, yOff), 0f, 1f, 0f, 10f);
                terrainItem = Instantiate(terrainItem, new Vector3(i, positionTheta, j), perlinRotation);

                //Adding a splash of color
                Renderer terrainRenderer = terrainItem.GetComponent<Renderer>();
                terrainRenderer.material.SetColor("_Color", ColorTerrain(terrainItem.transform.position));

                
                yOff += 0.06f; 
                
                terrainObjectList.Add(terrainItem);
            }

            xOff += 0.06f;

            
        }

    }

    void Update()
    {
        foreach(GameObject t in terrainObjectList)
        {
            //Rotation of cubes
            Quaternion perlinRotation = new Quaternion();
            float rotationTheta = ExtensionMethods.map(Mathf.PerlinNoise(xOff, yOff), 0f, 1f, 0f, 7f);
            Vector3 perlinVectors = new Vector3(Mathf.Cos(rotationTheta), Mathf.Sin(rotationTheta), 0f);
            perlinRotation.eulerAngles = perlinVectors;// * 100f * Time.deltaTime;

            t.transform.Rotate(perlinRotation.eulerAngles * 100f, Time.deltaTime);

        }

        yOff += 0.06f;
        xOff += 0.06f;
        
    }

    public Color ColorTerrain(Vector3 terrainItemPosition)
    {
        Color terrainColor = new Vector4(1f, 1f, 1f);

        if (terrainItemPosition.y >= 0f && terrainItemPosition.y <= 2f)
        {
            terrainColor = color1;
        }
        else if (terrainItemPosition.y >= 2f && terrainItemPosition.y <= 3.5f)
        {
            terrainColor = color2;
        }
        else if (terrainItemPosition.y >= 3.5f && terrainItemPosition.y <= 4.5f)
        {
            terrainColor = color3;
        }
        else if (terrainItemPosition.y >= 4.5f && terrainItemPosition.y <= 5f)
        {
            terrainColor = color4;
        }
        else if (terrainItemPosition.y >= 5f && terrainItemPosition.y <= 6f)
        {
            terrainColor = color5;
        }
        else
        {
            terrainColor = color6;
        }

        return terrainColor;
    }
}
