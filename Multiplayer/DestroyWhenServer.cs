using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (GlobalValues.GetGameMode() == GlobalValues.GameMode.Server)
        {
         //   Destroy(GameObject.Find("UserUICanvas"));
            Destroy(GameObject.Find("MiniMapGlobal"));
            Destroy(GameObject.Find("CountAllValues"));
            Destroy(GameObject.Find("Strava"));
            Destroy(GameObject.Find("testStrava"));
            Destroy(GameObject.Find("Path"));
            Destroy(GameObject.Find("CadenceDisplay"));
            Destroy(GameObject.Find("FitnessEquipmentDisplay"));
            Destroy(GameObject.Find("HeartRateDisplay"));
            Destroy(GameObject.Find("PowerMeterDisplay"));

            //  Destroy(GameObject.Find("GameManager"));

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
