using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleDayNightSun : MonoBehaviour
{
    public float time;
    public Transform SunTransform;
    public Light Sun;
    public float intensity;
    public Color fogDay = Color.grey;
    public Color fogNight = Color.black;

    private Vector3 rot = new Vector3(0,180,0);
    public int speed;

    void Start()
    {
        if(GlobalVariables.Instance.gameMode != GameMode.MAINMENU &&
        GlobalVariables.Instance.gameMode != GameMode.WOLRDMAINMENU)
            time = GlobalVariables.Instance.GetLvlTime();
        Sun = GetComponent<Light>();
        SunTransform = transform;
        ChangeTime();   
    }

    // Update is called once per frame
    void Update()
    {
        if(speed > 0) 
            ChangeTime();   
    }

    public void ChangeTime(){
        time += Time.deltaTime * speed;
        if(time > 86400) time = 0;
        rot.x = (time-21600)/86400 * 360;
        SunTransform.rotation = Quaternion.Euler(rot);

        if(time < 43200)
            intensity = 1 - ((43200 - time) / 43200);
        else 
            intensity = 1 - ((43200 - time) / 43200 * -1);

        RenderSettings.fogColor = Color.Lerp(fogNight, fogDay, intensity * intensity);

        Sun.intensity = intensity;
        GlobalVariables.Instance.currentLight = intensity;
    }
}
