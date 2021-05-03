using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calle : MonoBehaviour
{
    // Start is called before the first frame update

    public WayPointManager waypointManager;
    public MotorCarreteras motor { get; set; }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if(motor != null) StartCoroutine(motor.ciclarCalle(this.gameObject));
            StartCoroutine(DestroyStreet());
        }
    }

    public IEnumerator DestroyStreet()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }
}
