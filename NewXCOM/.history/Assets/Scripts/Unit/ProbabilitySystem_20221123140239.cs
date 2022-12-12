using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilitySystem : MonoBehaviour
{
    // @EMF -----------------------------------------------------------
    // Método, con probabilidad de fallo, que calcula el daño realizado
    // ----------------------------------------------------------------
    public int CheckDamageProbability(int damage, int criticalProbability, float criticalPercentage, int hitProbability)
    {
        if (CheckHit(hitProbability))
        {
            float rnd = Random.Range(0, 101);

            if (rnd <= criticalProbability){ return damage + (int)((float)damage * criticalPercentage); } // Acierto con crítico.
            else{ return damage; } // Acierto. Daño base.
        }
        else{ return 0; } // Fallo. Daño 0.
    }

    // @EMF -------------------------
    // Método para comprobar acierto
    // ------------------------------
    public bool CheckHit(int hitProbability)
    {
        float rnd = Random.Range(0, 101);

        if (rnd <= hitProbability){ return true; } // Acierto
        else{ return false; } // Fallo
    }

    // @EMF -------------------------------------
    // Método override sin probabilidad de fallo
    // ------------------------------------------
    public int CheckDamageProbability(int damage, int criticalProbability, float criticalPercentage)
    {
        float rnd = Random.Range(0, 101);

        if (rnd <= criticalProbability) { return damage + (int)((float)damage * criticalPercentage); }
        else { return damage; }
    }
}
