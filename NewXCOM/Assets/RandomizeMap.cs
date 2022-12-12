using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMap : MonoBehaviour
{
    [SerializeField] private bool useRandomSeed;
    [SerializeField] private string seed;

    [Range(0,100)]
    [SerializeField] private int obstaclesPercentage;
    [SerializeField] GameObject[] obstacles;
    [SerializeField] GameObject[] interactableSpheres;

    void Start()
    {
        if (useRandomSeed) seed = Time.deltaTime.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        int width = LevelGrid.Instance.GetWidth();
        int height = LevelGrid.Instance.GetHeight();
        float cellSize = LevelGrid.Instance.GetCellSize();

        for (int i = 0; i < width; i++)
        {
            for (int j = 2; j < height - 2; j++) //El bucle va de 2 a height-2 porque evitamos las dos primeras/últimas filas, que es donde va la base.
            {
                if (pseudoRandom.Next(0, 100) < obstaclesPercentage)
                {
                    //Si la posición del obstaculo coincide con la posición de una esfera, saltamos la interación
                    if (interactableSpheres[0].transform.position.x == cellSize * i && interactableSpheres[0].transform.position.z == cellSize * j
                        || interactableSpheres[1].transform.position.x == cellSize * i && interactableSpheres[1].transform.position.z == cellSize * j)
                    {

                        continue;

                    }

                    //Colocamos un obstaculo random
                    else
                    {
                        
                        int rand = Random.Range(0, obstacles.Length);
                        Instantiate(obstacles[rand], new Vector3(i * cellSize, 0, j * cellSize), Quaternion.identity);

                    }
                    
                }
            }
        }
    }

}
