using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{

    private Mesh mesh;                                  // Malla del mapa de influencia
    private Quaternion[] cachedQuaternionEulerArr;      // Cosas de cuaterniones
    private bool updateMesh;                            // Booleano para saber si tenemos que actualizar la malla

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Creamos la malla
        mesh = new Mesh();

        // Recuperamos el Filtro de la malla
        GetComponent<MeshFilter>().mesh = mesh;

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Nos suscribimos a los eventos
        LevelGrid.Instance.OnAnyHeatMapValueChanged += LevelGrid_OnAnyHeatMapValueChanged;

    }

    // @IGM ------------------------------------------------------------
    // LateUpdate is called after all Update functions have been called.
    // -----------------------------------------------------------------
    private void LateUpdate()
    {

        // Comprobamos si tenemos que actualizar el mapa de influencia
        if (updateMesh)
        {

            // Actualizamos el mapa
            updateMesh = false;
            UpdateHeatMapVisual();

        }

    }

    // @IGM ---------------------------------------------------
    // Metodo para actualizar el visual del mapa de influencia.
    // --------------------------------------------------------
    private void UpdateHeatMapVisual()
    {

        // Creamos los arrays de la malla
        CreateEmptyMeshArrays(LevelGrid.Instance.GetWidth() * LevelGrid.Instance.GetHeight(), 
            out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        // Recorremos la malla
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {

            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {

                // Realizamos los calculos necesarios
                int index = x * LevelGrid.Instance.GetHeight() + z;
                Vector3 quadSize = new Vector3(1, 0, 1) * LevelGrid.Instance.GetCellSize();

                // Creamos la posicion seleccionada
                GridPosition gridPosition = new GridPosition(x, z);

                // Recuperamos el valor del mapa de influencia en esta casilla y lo normalizamos
                int gridValue = LevelGrid.Instance.GetHeatMapValueAtGridPosition(gridPosition);
                float gridValueNormalized = (float)gridValue / LevelGrid.Instance.GetMaxHeatMapValue();

                // Creamos el UV del valor del mapa de influencia
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);

                // Calculamos los arrays de la malla
                AddToMeshArrays(vertices, uv, triangles, index, LevelGrid.Instance.GetWorldPosition(gridPosition) 
                    /*+ quadSize * 0.5f*/, 0f, quadSize, gridValueUV, gridValueUV);

            }

        }

        // Actualizamos la malla
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    private void LevelGrid_OnAnyHeatMapValueChanged(object sender, GridPosition gridPosition)
    {

        // Marcamos que tenemos que actualizar el mapa de influenciad
        updateMesh = true;

    }

    // @IGM --------------------------------------------
    // Metodo para crear los arrays vecios de una malla.
    // -------------------------------------------------
    private void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
    {

        vertices = new Vector3[4 * quadCount];
        uvs = new Vector2[4 * quadCount];
        triangles = new int[6 * quadCount];

    }

    // @IGM ------------------------------------
    // Metodo para añadir a la malla los arrays.
    // -----------------------------------------
    public void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
    {

        // Recoloccamos los vertices
        int vIndex = index * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;

        baseSize *= .5f;

        bool skewed = baseSize.x != baseSize.z;

        if (skewed)
        {

            vertices[vIndex0] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, 0, baseSize.z);
            vertices[vIndex1] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, 0, -baseSize.z);
            vertices[vIndex2] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, 0, -baseSize.z);
            vertices[vIndex3] = pos + GetQuaternionEuler(rot) * baseSize;

        }
        else
        {

            vertices[vIndex0] = pos + GetQuaternionEuler(rot - 270) * baseSize;
            vertices[vIndex1] = pos + GetQuaternionEuler(rot - 180) * baseSize;
            vertices[vIndex2] = pos + GetQuaternionEuler(rot - 90) * baseSize;
            vertices[vIndex3] = pos + GetQuaternionEuler(rot - 0) * baseSize;

        }

        // Recolocamos las uvs
        uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
        uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
        uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
        uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

        // Creamos los triangulos
        int tIndex = index * 6;

        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex3;
        triangles[tIndex + 2] = vIndex1;

        triangles[tIndex + 3] = vIndex1;
        triangles[tIndex + 4] = vIndex3;
        triangles[tIndex + 5] = vIndex2;

    }

    // @IGM --------------------------------------------------------
    // Funcion para calcular el quaternion de una rotacion en euler.
    // -------------------------------------------------------------
    private Quaternion GetQuaternionEuler(float rotFloat)
    {

        int rot = Mathf.RoundToInt(rotFloat);
        rot = rot % 360;

        if (rot < 0) { 
            
            rot += 360; 
        
        }

        if (cachedQuaternionEulerArr == null) { 
            
            CacheQuaternionEuler(); 
        
        }

        return cachedQuaternionEulerArr[rot];

    }

    // @IGM -----------------------------------
    // Metodo para hacer cosas de cuaterniones.
    // ----------------------------------------
    private void CacheQuaternionEuler()
    {

        if (cachedQuaternionEulerArr != null) 
        { 
            
            return;
        
        }

        cachedQuaternionEulerArr = new Quaternion[360];

        for (int i = 0; i < 360; i++)
        {

            cachedQuaternionEulerArr[i] = Quaternion.Euler(0, -i, 0);

        }

    }

}

