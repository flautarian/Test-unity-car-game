using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    public Transform MinimapCam;
	public float MinimapSize;
	Vector3 TempV3;
    Vector3 newPos;

    private void Start() {
        newPos = new Vector3(0f, 0f, 0f);
    }

	void Update () {
		TempV3 = transform.parent.transform.position;
		TempV3.y = transform.position.y;
		transform.position = TempV3;
	}

	void LateUpdate () {
        newPos.x = Mathf.Clamp(transform.position.x, MinimapCam.position.x-MinimapSize, MinimapSize + MinimapCam.position.x);
        newPos.y = transform.position.y;
        newPos.z = Mathf.Clamp(transform.position.z, MinimapCam.position.z-MinimapSize, MinimapSize + MinimapCam.position.z);
		transform.position = newPos;
	}
}
