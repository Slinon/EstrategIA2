using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilitySystem : MonoBehaviour
{

    public static ProbabilitySystem Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        
        Instance = this;
    }


    // @EMF -----------------------------------------------------------
    // M�todo, con probabilidad de fallo, que calcula el da�o realizado
    // ----------------------------------------------------------------
    public int CheckDamageProbability(int damage, int criticalProbability, float criticalPercentage, int hitProbability)
    {
        if (CheckHit(hitProbability))
        {
            float rnd = Random.Range(0, 101);

            if (rnd <= criticalProbability){ return damage + (int)((float)damage * criticalPercentage); } // Acierto con cr�tico.
            else{ return damage; } // Acierto. Da�o base.
        }
        else{ return 0; } // Fallo. Da�o 0.
    }

    // @EMF -------------------------
    // M�todo para comprobar acierto
    // ------------------------------
    public bool CheckHit(int hitProbability)
    {
        float rnd = Random.Range(0, 101);

        if (rnd <= hitProbability){ return true; } // Acierto
        else{ return false; } // Fallo
    }

    // @EMF -------------------------------------
    // M�todo override sin probabilidad de fallo
    // ------------------------------------------
    public int CheckDamageProbability(int damage, int criticalProbability, float criticalPercentage)
    {
        float rnd = Random.Range(0, 101);

        if (rnd <= criticalProbability) { return damage + (int)((float)damage * criticalPercentage); }
        else { return damage; }
    }
}
