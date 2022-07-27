using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static int IndexOf<T>(T[] arr, T element)
    {
        
        for (int i = 0; i < arr.Length; i++)
            if (arr[i].ToString() == element.ToString())
                return i;
        return -1;
    }

}
