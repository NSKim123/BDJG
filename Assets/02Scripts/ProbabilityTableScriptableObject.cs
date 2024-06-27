using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityTableScriptableObject<T> : ScriptableObject
    where T: struct
{
    public List<TableElement<T>> m_ProbabilityTable = new List<TableElement<T>>();

    public T Trial()
    {
        int total = 0;
        foreach(TableElement<T> element in m_ProbabilityTable)
        {
            total += element.m_Weight;
        }

        T[] table = new T[total];

        int startIndex = 0;
        foreach (TableElement<T> element in m_ProbabilityTable)
        {
            Array.Fill<T>(table, element.m_Value, startIndex, element.m_Weight);
            startIndex += element.m_Weight;
        }

        return table[UnityEngine.Random.Range(0, total)];
    }
}

[Serializable]
public class TableElement<T>
    where T : struct
{
    public T m_Value;
    public int m_Weight;
}