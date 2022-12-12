using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilitySystem : MonoBehaviour
{
    public static ProbabilitySystem Instance {get; private set;}

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
    }

    // @EMF -----------------------------------------------------------
    // Metodo, con probabilidad de fallo, que calcula el daño realizado
    // ----------------------------------------------------------------
    public Vector2 CheckDamageProbability(int damage, int criticalProbability, float criticalPercentage, int hitProbability, int distance, int maxShootDistance)
    {
        Vector2 info = new Vector2(0, 0); // int damage + int info (-1 fallo, 0 normal, 1 critico)

        int probability = GetProbabiltyByDistance(hitProbability, distance, maxShootDistance);

        if (CheckHit(probability)) // si no falla
        {
            float rnd = Random.Range(0, 101); // 0 - 101

            if (rnd <= criticalProbability)
            {
                info.x = damage + (int)((float)damage * criticalPercentage);
                info.y = 1; // acierto critico
            }
            else
            {
                info.x = damage;
                info.y = 0; // acierto normal
            }
        }
        else
        {
            info.x = 0;
            info.y = -1; // fallo
        }

        return info;
    }

    // @EMF -------------------------
    // Metodo para comprobar acierto
    // ------------------------------
    public bool CheckHit(int hitProbability)
    {
        float rnd = Random.Range(0, 101);

        if (rnd <= hitProbability){ return true; } // Acierto
        else{ return false; } // Fallo
    }

    public int GetProbabiltyByDistance(int hitProbability, int distance, int maxShootDistance)
    {
        int probability;

        if (hitProbability - GetDistancePercentage(distance) > 0 &&  distance <= maxShootDistance) // Probabilidad positiva y distancia mejor que rango max
        {
            probability = Mathf.RoundToInt(hitProbability - GetDistancePercentage(distance)); 
        }
        else
        {
            probability = 0;
        }

        return probability;
    }

    public int GetDistancePercentage(int distance)
    {
        int percentage = 0;

        if (distance >= 2 && distance < 4)
        {
            percentage = 40;
        }
        else if (distance >= 4)
        {
            percentage = 60;
        }

        return percentage;
    }

    // @EMF -------------------------------------------------
    // Metodo override sin probabilidad de fallo ni distancia
    // ------------------------------------------------------
    public Vector2 CheckDamageProbability(int damage, int criticalProbability, float criticalPercentage)
    {
        Vector2 info = new Vector2(0, 0); // int damage + int info (-1 fallo, 0 normal, 1 critico)

        float rnd = Random.Range(0, 101);

        if (rnd <= criticalProbability)
        {
            info.x = damage + (int)((float)damage * criticalPercentage);
            info.y = 1;
        }
        else
        {
            info.x = damage;
            info.y = 0;
        }

        return info;
    }
}
