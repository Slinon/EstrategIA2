using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMap : MonoBehaviour
{
    [SerializeField] private bool useRandomSeed;
    [SerializeField] private string seed;

    [Range(0, 100)]
    [SerializeField] private int obstaclesPercentage;
    [SerializeField] GameObject[] obstacles;
    [SerializeField] GameObject[] interactableSpheres;

    private int width;
    private int height;
    private float cellSize;
    private int[,] myGrid;

    private void Awake()
    {
        width = LevelGrid.Instance.GetWidth();
        height = LevelGrid.Instance.GetHeight();
        cellSize = LevelGrid.Instance.GetCellSize();
        myGrid = new int[width, height]; //inicializo una matriz de 0 del tamaño del mapa

    }


    public void FillWithObstacles()
    {
        if (useRandomSeed) seed = Time.deltaTime.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());


        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 4; j < height - 4; j++) //El bucle va de 4 a height-4 porque evitamos donde va la base.
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
                        int randObstacle = pseudoRandom.Next(0, obstacles.Length) + 1; //modifico el 0 de la matriz a 1 si es caja y a 2 si es columna (por eso el +1, sé que es cutre.)
                        myGrid[i, j] = randObstacle;

                    }


                }
            }
        }

        ModifyGrid(myGrid);
        InstantiateObstacles();

    }


    private void ModifyGrid(int[,] grid)
    {
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 2; j < height - 2; j++)
            {
                if (grid[i, j] == 2)
                {
                    if (hasWallDiagonally(i, j) && !hasWallVerticallyOrHorizontally(i, j))
                    {
                        grid[i, j] = 0;
                    }
                }
            }
        }
    }

    private bool hasWallDiagonally(int i, int j)
    {
        if (myGrid[i + 1, j + 1] == 2
            || myGrid[i - 1, j - 1] == 2
            || myGrid[i - 1, j + 1] == 2
            || myGrid[i + 1, j - 1] == 2) return true;

        else return false;
    }

    private bool hasWallVerticallyOrHorizontally(int i, int j)
    {
        if (myGrid[i + 1, j] == 2
            || myGrid[i - 1, j] == 2
            || myGrid[i, j + 1] == 2
            || myGrid[i, j - 1] == 2) return true;

        else return false;
    }

    private void InstantiateObstacles()
    {
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 2; j < height - 2; j++)
            {
                if (myGrid[i, j] != 0)
                {
                    Instantiate(obstacles[myGrid[i, j] - 1], new Vector3(i * cellSize, 0, j * cellSize), Quaternion.identity);
                }
            }
        }
    }
}

