using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calle : MonoBehaviour
{
    // Start is called before the first frame update

    public WayPointManager waypointManager;
    public MotorCarreteras motor { get; set; }

    void Start()
    {
        /*MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Debug.Log("total meshes to combine in street: " + meshFilters.Length);
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);*/
    }

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
