using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPartsIndicator : MonoBehaviour
{

    public GameObject iconRepair;
    public int numberParts;
    public int actualNumberParts;
    public List<Animator> repairItems;
    
    public void startGame(int np)
    {
        numberParts = np;
        actualNumberParts = np-1;
        Vector3 pos = this.transform.position;
        for (int i =0; i< numberParts; i++)
        {
            GameObject ri = GameObject.Instantiate(iconRepair);
            ri.transform.parent = this.transform;
            ri.transform.position = pos;
            ri.SetActive(true);
            pos.x -= 0.12f;
            repairItems.Add(ri.GetComponent<Animator>());
        }
    }

    public void startGameOver()
    {
        foreach(Animator item in repairItems)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void decrementPart()
    {
        if(actualNumberParts >= 0)
        {
            repairItems[actualNumberParts].SetBool("action", true);
            actualNumberParts--;
        }
    }

    public void resetIndicator()
    {
        foreach (Animator item in repairItems)
        {
            item.SetBool("action", false);
            item.gameObject.SetActive(true);
            item.Play("StartIconAnimation");
        }
        actualNumberParts = repairItems.Count - 1;
    }
}
