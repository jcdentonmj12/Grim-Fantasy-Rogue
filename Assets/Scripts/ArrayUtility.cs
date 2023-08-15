using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ArrayExtensions
{
    public static T[] To1DArray<T>(this T[,] array2D)
    {
        int width = array2D.GetLength(0);
        int height = array2D.GetLength(1);
        T[] array1D = new T[width * height];
        int index = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                array1D[index++] = array2D[x, y];
            }
        }
        return array1D;
    }

    public static T[,] To2DArray<T>(this T[] array1D, int width, int height)
    {
        T[,] array2D = new T[width, height];
        int index = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                array2D[x, y] = array1D[index++];
            }
        }
        return array2D;
    }
}

