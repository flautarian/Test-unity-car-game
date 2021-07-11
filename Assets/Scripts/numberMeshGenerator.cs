using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class numberMeshGenerator : MonoBehaviour
{
    private int number;
    public List<GameObject> panelNumbers;
    public List<Mesh> representalNumbers;

    public void updateNumber(int newNumber)
    {
        number = newNumber;
        updateNumberPanel();
    }

    private void updateNumberPanel()
    {
        int[] newNumbersProcessed = getNumberVectorByNumber(number);
        GetComponent<Animator>().SetBool("changed", true);
        for (int i = 0; i < panelNumbers.Count; i++)
        {
            Mesh actualMesh = panelNumbers[i].GetComponent<MeshFilter>().mesh;
            if (actualMesh != representalNumbers[newNumbersProcessed[i]])
            {
                panelNumbers[i].GetComponent<MeshFilter>().mesh = representalNumbers[newNumbersProcessed[i]];
                // animar numero que cambia
            }
        }
    }

    public int[] getNumberVectorByNumber(int number)
    {
        int[] result = new int[7];
        int index = 0;
        while(number >= 10)
        {
            result[index] = number % 10;
            number /= 10;
            index++;
        }
        result[index] = number;
        return result;
    }
}
