using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPartsIndicator : MonoBehaviour
{

    public GameObject iconRepair;
    public int numberParts;
    public int actualNumberParts;
    public List<GameObject> repairItems;
    
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
            repairItems.Add(ri);
        }
    }

    public void startGameOver()
    {
        foreach(GameObject item in repairItems)
        {
            item.SetActive(false);
        }
    }

    public void decrementPart()
    {
        if(actualNumberParts >= 0)
        {
            repairItems[actualNumberParts].GetComponent<Animator>().SetBool("action", true);
            actualNumberParts--;
        }
    }

    public void resetIndicator()
    {
        foreach (GameObject item in repairItems)
        {
            item.GetComponent<Animator>().SetBool("action", false);
            item.GetComponent<Animator>().Play("StartIconAnimation");
            item.SetActive(true);
        }
        actualNumberParts = repairItems.Count - 1;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
