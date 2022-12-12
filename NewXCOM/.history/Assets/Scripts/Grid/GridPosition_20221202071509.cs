using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridPosition : IEquatable<GridPosition>
{

    public int x;                           // Posicion x de la malla
    public int z;                           // Posicion z de la malla

    // @IGM ------------------------
    // Constructor de la estructura.
    // -----------------------------
    public GridPosition(int x, int z)
    {

        // Asignamos lo atributos
        this.x = x;
        this.z = z;

    }

    // @IGM ------------------
    // Funcion operador igual.
    // -----------------------
    public override bool Equals(object obj)
    {

        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;

    }

    // @IGM ------------------
    // Funcion operador igual.
    // -----------------------
    public bool Equals(GridPosition other)
    {

        return this == other;

    }
    // @IGM ---------------------
    // Funcion operador HashCode.
    // --------------------------
    public override int GetHashCode()
    {

        return HashCode.Combine(x, z);

    }

    // @IGM --------------------------
    // Funcion sustituda del ToString.
    // -------------------------------
    public override string ToString()
    {

        return "x: " + x + "; " + "z: " + z;

    }

    // @IGM ---------------
    // Funcion operador ==.
    // --------------------
    public static bool operator ==(GridPosition a, GridPosition b)
    {

        return a.x == b.x && a.z == b.z;

    }

    // @IGM ---------------
    // Funcion operador !=.
    // --------------------
    public static bool operator !=(GridPosition a, GridPosition b)
    {

        return !(a == b);

    }

    // @IGM --------------
    // Funcion operador +.
    // -------------------
    public static GridPosition operator +(GridPosition a, GridPosition b)
    {

        return new GridPosition(a.x + b.x, a.z + b.z);

    }

    // @IGM --------------
    // Funcion operador -.
    // -------------------
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {

        return new GridPosition(a.x - b.x, a.z - b.z);

    }

    // @VJT -----------------------------
    // Conversion implicita a Vector2Int
    // ----------------------------------
    public static implicit operator Vector2Int(GridPosition gp) => new Vector2Int(gp.x, gp.z);

    // @VJT -----------------------------
    // Conversion implicita a Vector2Int
    // ----------------------------------
    public static implicit operator GridPosition(Vector2Int v) => new Grid


}
