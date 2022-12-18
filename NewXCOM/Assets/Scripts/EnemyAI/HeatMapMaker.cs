using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapMaker : MonoBehaviour
{

    [Header("Grenade Action")]
    [SerializeField] private int enemyGrenadeValue;             // Valor que tiene el enemigo al lanzar una granada
    [SerializeField] private int allyGrenadeValue;              // Valor que tiene un aliado al lanzar una granada
    [SerializeField] private int grenadeCenterInfluence;        // Influencia desde el centro de los puntos de influencia
    [SerializeField] private int grenadeExpansion;              // Expansion de los puntos de influencia
    [Space(10)]

    [Header("BuildStructureAction")]
    [SerializeField] private int enemyStructureValue;           // Valor que tiene el enemigo al construir una torre
    [SerializeField] private int sphereStructureValue;          // Valor que tiene un punto de toma al construir una torre
    [SerializeField] private int baseStructureValue;            // Valor que tiene la base aliada al construir una torre
    [SerializeField] private int allyTurretsStructureValue;     // Valor que tiene una torre aliada al construir una torre
    [SerializeField] private int structureCenterInfluence;      // Influencia desde el centro de los puntos de influencia
    [SerializeField] private int allyStructuresExpansion;       // Expansion de los puntos de influencia estructuras aliadas
    [SerializeField] private int enemiesStructuresExpansion;    // Expansion de los puntos de influencia enemigos
    [Space(10)]

    [Header("MoveAction")]
    [SerializeField] private int enemyBaseMoveValue;            // Valor que tiene la base enemiga al moverse
    [SerializeField] private int allyBaseMoveValue;             // Valor que tiene la base aliada al moverse
    [SerializeField] private int sphereMoveValue;               // Valor que tiene la esfera de influencia al moverse
    [SerializeField] private int enemyMoveValue;                // Valor que tiene el enemigo al moverse
    [SerializeField] private int berserkMoveValue;              // Valor que tiene el berserk al moverse
    [SerializeField] private int allyMoveValue;                 // Valor que tiene un aliado al moverse
    [SerializeField] private int coverMoveValue;                // Valor que tiene una cobertura al moverse
    [SerializeField] private int moveCenterInfluence;           // Influencia desde el centro de los puntos de influencia
    [SerializeField] private int coverMoveCenterInfluence;      // Influencia desde el centro de las coberturas
    [SerializeField] private int sphereMoveExpansion;           // Expansion de los puntos de influencia de las esferas
    [SerializeField] private int baseMoveExpansion;             // Expansion de los puntos de influencia de las bases
    [SerializeField] private int coverMoveExpansion;            // Expansion de los puntos de influencia de la cobertura

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {



    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    void Start()
    {

        UnitAIManager.OnAnyUnitThrowGrenade += UnitAIManager_OnAnyUnitThrowGrenade;
        UnitAIManager.OnAnyUnitBuildStructure += UnitAIManager_OnAnyUnitBuildStructure;
        UnitAIManager.OnAnyUnitMoveAction += UnitAIManager_OnAnyUnitMoveAction;
        BaseAIManager.OnAnyUnitSpawnedAction += BaseAIManager_OnAnyUnitSpawnedAction;

    }

    // @IGM ----------------------------------------------------------
    // Handler del evento cuando una unidad decide lanzar una granada.
    // ---------------------------------------------------------------
    private void UnitAIManager_OnAnyUnitThrowGrenade(object sender, Unit unit)
    {

        // Limpiamos el mapa de influencia
        LevelGrid.Instance.ClearHeatMap();

        // Asignamos los valores de las acciones
        MakeGrenadeActionHeatMap(unit);

    }

    // @IGM -----------------------------------------------------------
    // Handler del evento cuando una unidad decide construir una torre.
    // ----------------------------------------------------------------
    private void UnitAIManager_OnAnyUnitBuildStructure(object sender, Unit unit)
    {

        // Limpiamos el mapa de influencia
        LevelGrid.Instance.ClearHeatMap();

        // Asignamos los valores de las acciones
        MakeBuildStructureActionHeatMap(unit);

    }

    // @IGM -----------------------------------------------
    // Handler del evento cuando una unidad decide moverse.
    // ----------------------------------------------------
    private void UnitAIManager_OnAnyUnitMoveAction(object sender, Unit unit)
    {

        // Limpiamos el mapa de influencia
        LevelGrid.Instance.ClearHeatMap();

        // Asignamos los valores de las acciones
        MakeMoveActionHeatMap(unit);

    }

    // @IGM ---------------------------------------------------
    // Handler del evento cuando se decide spawnear una unidad.
    // --------------------------------------------------------
    private void BaseAIManager_OnAnyUnitSpawnedAction(object sender, Unit unit)
    {

        // Limpiamos el mapa de influencia
        LevelGrid.Instance.ClearHeatMap();

        // Asignamos los valores de las acciones
        MakeMoveActionHeatMap(unit);

    }

    // @IGM ------------------------------------------------------------------
    // Metodo para crear el mapa de influencia de la accion de lanzar granada.
    // -----------------------------------------------------------------------
    private void MakeGrenadeActionHeatMap(Unit unit)
    {

        // Recorremos la malla
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {

            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {

                // Creamos la posicion deseada
                GridPosition gridPosition = new GridPosition(x, z);

                // Comprobamos si la posicion no tiene unidades dentro
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition))
                {

                    // La saltamos
                    continue;

                }

                // Recuperamos el target de la posicion
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

                // Comprobamos si el target esta en el mismo equipo que la unidad que va a disparar
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {

                    // Generamos un punto de peligro
                    LevelGrid.Instance.AddValue(gridPosition, allyGrenadeValue, grenadeCenterInfluence, grenadeExpansion);
                    continue;

                }
                else
                {

                    // El target es un enemigo
                    LevelGrid.Instance.AddValue(gridPosition, enemyGrenadeValue, grenadeCenterInfluence, grenadeExpansion);
                    continue;

                }

            }

        }

    }

    // @IGM ---------------------------------------------------------------
    // Metodo para crear el mapa de influencia de la accion de crear torre.
    // --------------------------------------------------------------------
    private void MakeBuildStructureActionHeatMap(Unit unit)
    {

        // Recorremos la malla
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {

            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {

                // Creamos la posicion deseada
                GridPosition gridPosition = new GridPosition(x, z);

                // Comprobamos si hay un elemento interactuable en la posicion
                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
                if (interactable != null)
                {

                    // Si es un objeto de posesion comprobamos si el estado es diferente al suyo
                    InteractSphere interactSphere = interactable as InteractSphere;
                    if ((int)interactSphere.GetInControlState() == Convert.ToInt32(unit.IsEnemy()))
                    {

                        // Generamos un punto de influencia
                        LevelGrid.Instance.AddValue(gridPosition, sphereStructureValue, structureCenterInfluence, allyStructuresExpansion);
                        continue;

                    }

                }

                // Comprobamos si la posicion tiene unidades dentro
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition))
                {

                    // Recuperamos el target de la posicion
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

                    // Comprobamos si el target esta en el mismo equipo que la unidad que va a disparar
                    if (targetUnit.IsEnemy() == unit.IsEnemy())
                    {
                       
                        // Comprobamos si es una torre aliada
                        if (targetUnit.tag == "Turret")
                        {
                         
                            // Generamos un punto de peligro
                            LevelGrid.Instance.AddValue(gridPosition, allyTurretsStructureValue, structureCenterInfluence,
                                targetUnit.GetAction<ShootAction>().GetMaxShootDistance());
                            continue;

                        }
                        else if (targetUnit.tag == "EnemyMainBase")
                        {

                            // Es la base aliada
                            // Generamos un punto de influencia
                            LevelGrid.Instance.AddValue(gridPosition, baseStructureValue, structureCenterInfluence, allyStructuresExpansion);
                            continue;

                        }
                   
                    }
                    else
                    {

                        // La unidad es enemiga
                        // Generamos un punto de influencia
                        LevelGrid.Instance.AddValue(gridPosition, enemyStructureValue, structureCenterInfluence, enemiesStructuresExpansion);
                        continue;

                    }

                }

            }

        }

    }

    // @IGM ----------------------------------------------------------
    // Metodo para crear el mapa de influencia de la accion moviiento.
    // ---------------------------------------------------------------
    private void MakeMoveActionHeatMap(Unit unit)
    {

        // Recorremos la malla
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {

            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {

                // Creamos la posicion deseada
                GridPosition gridPosition = new GridPosition(x, z);

                // Comprobamos si hay un elemento interactuable en la posicion
                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
                if (interactable != null)
                {

                    // Si es un objeto de posesion comprobamos si el estado es diferente al suyo
                    InteractSphere interactSphere = interactable as InteractSphere;
                    if ((int)interactSphere.GetInControlState() != Convert.ToInt32(unit.IsEnemy()))
                    {

                        // Generamos un punto de influencia
                        LevelGrid.Instance.AddValue(gridPosition, sphereMoveValue, moveCenterInfluence, sphereMoveExpansion);

                    }

                }
                else if (LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition))
                {

                    // La posicion tiene unidades dentro
                    // Recuperamos el target de la posicion
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

                    // Comprobamos si el target esta en el mismo equipo que la unidad que va a disparar
                    if (targetUnit.IsEnemy() == unit.IsEnemy())
                    {
                       
                        // Comprobamos si es la base aliada
                        if (targetUnit.tag == "EnemyMainBase")
                        {

                            // Generamos un punto de influencia
                            LevelGrid.Instance.AddValue(gridPosition, allyBaseMoveValue, moveCenterInfluence, baseMoveExpansion);

                        }
                        else
                        {

                            // Es una unidad aliada
                            LevelGrid.Instance.AddValue(gridPosition, allyMoveValue, moveCenterInfluence,
                                targetUnit.GetAction<ShootAction>().GetMaxShootDistance());

                        }

                    }
                    else
                    {

                        // La unidad es enemiga
                        // Comprobamos si es la base enemiga
                        if (targetUnit.tag == "MainBase")
                        {

                            // Generamos un punto de influencia
                            LevelGrid.Instance.AddValue(gridPosition, enemyBaseMoveValue, moveCenterInfluence, baseMoveExpansion);

                        }
                        else
                        {

                            // Comprobamos que la unidad pueda pegar espadazo
                            if (unit.TryGetComponent(out SwordAction swordAction))
                            {

                                // Es un berserk
                                LevelGrid.Instance.AddValue(gridPosition, berserkMoveValue, moveCenterInfluence,
                                    targetUnit.GetAction<ShootAction>().GetMaxShootDistance());

                            }
                            else if (unit.TryGetComponent(out BuildStructureAction buildStructureAction))
                            {

                                // La unidad puedde construir torres
                                // Añadimos el mapa de influencia de la torre para que la unidad se mueva hacia la posición donde construir torre
                                MakeBuildStructureActionHeatMap(unit);

                            }
                            else
                            {

                                // Es una unidad enemiga
                                LevelGrid.Instance.AddValue(gridPosition, enemyMoveValue, moveCenterInfluence,
                                    targetUnit.GetAction<ShootAction>().GetMaxShootDistance());
                               
                            }

                        }

                    }

                }

                // Comprobamos si en esa posición la unidad no esta a tiro de nadie
                if (CheckLineOfSightOfAllUnits(unit))
                {
                    // Generamos un punto de influencia
                    LevelGrid.Instance.AddValue(gridPosition, coverMoveValue, coverMoveCenterInfluence, coverMoveExpansion);
                }

            }

        }

    }

    // @IGM ------------------------------------------
    // Setter del valor de la base enemiga al moverse.
    // -----------------------------------------------
    public void SetEnemyBaseMoveValue(int enemyBaseMoveValue)
    {

        this.enemyBaseMoveValue = enemyBaseMoveValue;

    }

    // @IGM ------------------------------------------
    // Getter del valor de la base enemiga al moverse.
    // -----------------------------------------------
    public int GetEnemyBaseMoveValue()
    {

        return enemyBaseMoveValue;

    }

    // @IGM -----------------------------------------
    // Setter del valor de la base aliada al moverse.
    // ----------------------------------------------
    public void SetAllyBaseMoveValue(int allyBaseMoveValue)
    {

        this.allyBaseMoveValue = allyBaseMoveValue;

    }

    // @IGM --------------------------------------
    // Setter del valor de las esferas al moverse.
    // -------------------------------------------
    public void SetSpheresMoveValue(int sphereMoveValue)
    {

        this.sphereMoveValue = sphereMoveValue;

    }

    // @IGM -----------------------------------------
    // Getter del valor de la base aliada al moverse.
    // ----------------------------------------------
    public int GetAllyBaseMoveValue()
    {

        return allyBaseMoveValue;

    }

    public bool CheckLineOfSightOfAllUnits(Unit unit)
    {
        foreach (Unit playerUnit in UnitManager.Instance.GetFriendlyUnitList())
        {
            if (unit.ThisUnitIsInSight(playerUnit))
            {
                return true;
            }
        }
        return false;
    }
}
