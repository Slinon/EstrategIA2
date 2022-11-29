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
    public Vector2 CheckDamageProbability(int damage, int criticalProbability, float criticalPercentage, int hitProbability)
    {
        Vector2 info = new Vector2(0, 0); // int damage + int info (-1 fallo, 0 normal, 1 critico)

        if (CheckHit(hitProbability))
        {
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
        }
        else
        {
            info.x = 0;
            info.y = -1;
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

    // @EMF -------------------------------------
    // Metodo override sin probabilidad de fallo
    // ------------------------------------------
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
