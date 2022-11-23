using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{

    T[] items;                  // Array de elementos
    int currentItemCount;       // Contador de elementos

    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    public Heap(int maxHeapSize)
    {

        items = new T[maxHeapSize];

    }

    // @IGM ------------------------------------
    // Metodo para añadir un elemento al monton.
    // -----------------------------------------
    public void Add(T item)
    {

        // Asignamos el elemento al array
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;

        // Ordenamos el elemento en el array
        SortUp(item);

        // Incrementamos el contador de elementos
        currentItemCount++;

    }

    // @IGM -----------------------------------------------------
    // Metodo para ordenear un elemento hacia abajo en el monton.
    // ----------------------------------------------------------
    void SortDown(T item)
    {

        // Mientras no este ordenado el monton
        while (true)
        {

            // Comprobamos los indices de los elementos hijos
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            // Comprobamos que el hijo de la izquierda no es el ultimo
            if (childIndexLeft < currentItemCount)
            {

                // Cambiamos el indice
                swapIndex = childIndexLeft;

                // Comprobamos que el hijo de la derecha no sea el ultimo
                if (childIndexRight < currentItemCount)
                {

                    // Comparamos los hijos
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {

                        // Cambiamos el indice
                        swapIndex = childIndexRight;

                    }

                }

                // Comparamos el padre con el hijo menos
                if (item.CompareTo(items[swapIndex]) < 0)
                {

                    // Intercambiamos los elementos
                    Swap(item, items[swapIndex]);

                }
                else
                {

                    // El monton esta ordenado
                    return;

                }

            }
            else
            {

                // El monton esta ordenado 
                return;

            }

        }

    }

    // @IGM -----------------------------------------------------
    // Metodo para ordenar un elemento hacia arriba en el monton.
    // ----------------------------------------------------------
    void SortUp(T item)
    {

        // Recuperamos el indice del padre del elemento
        int parentIndex = (item.HeapIndex - 1) / 2;

        // Mientras no este ordenado el monton
        while (true)
        {

            // Recuperamos el padre del elemento
            T parentItem = items[parentIndex];

            // Comprobamos que el elemento es mayor a 0
            if (item.CompareTo(parentItem) > 0)
            {

                // Intercambiamos los elementos
                Swap(item, parentItem);

            }
            else
            {

                // El monton esta ordenado
                break;

            }

            // Actualizamos el indice del padre al nuevo padre
            parentIndex = (item.HeapIndex - 1 / 2);

        }

    }

    // @IGM ---------------------------------------------
    // Metodo para intercambiar dos elementos del monton.
    // --------------------------------------------------
    void Swap(T itemA, T itemB)
    {

        // Intercambiamos los elementos
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        // Intercambiamos los indices
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;

    }

    // @IGM -----------------------------------------
    // Metodo para actualizar un elemento del monton.
    // ----------------------------------------------
    public void UpdateItem(T item)
    {

        // Ordenamos hacia arriba el elemento
        SortUp(item);

    }

    // @IGM ------------------------------------------------
    // Funcion para saber si el monton contiene un elemento.
    // -----------------------------------------------------
    public bool Contains(T item)
    {

        // Comprobamos si el elemento esta en el monton
        return Equals(items[item.HeapIndex], item);

    }

    // @IGM -------------------------------------------------
    // Funcion para saber cuantos elementos hay en el monton.
    // ------------------------------------------------------
    public int Count
    {

        // No se puede modificar
        get
        {

            return currentItemCount;

        }

    }

    // @IGM -----------------------------------------------
    // Funcion para eliminar el primer elemento del monton.
    // ----------------------------------------------------
    public T RemoveFirst()
    {

        // Seleccionamos el primer elemento del monton
        T firstItem = items[0];

        // Restames el numero de elementos
        currentItemCount--;

        // Movemos el ultimo elemento al principio
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;

        // Ordenamos el elemento
        SortDown(items[0]);

        // Devolvemos el primer elemento
        return firstItem;

    }

}

// @IGM ----------------------------
// Interfaz del elemento del monton.
// ---------------------------------
public interface IHeapItem<T> : IComparable<T>
{

    int HeapIndex
    {

        get;
        set;

    }

}
