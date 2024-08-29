using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColision : MonoBehaviour
{
    private CameraController gm;
    private GameObject cam;
    void Start()
    {
        gm = transform.parent.GetComponent<CameraController>();
        cam = transform.GetChild(0).gameObject;
    }    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerControl>().currentAxis = transform.GetChild(1);
            for (int i = 0; i < gm.allCameras.Count; i++)
            {
                gm.allCameras[i].GetComponent<Camera>().enabled = false;
                gm.allCameras[i].GetComponent<AudioListener>().enabled = false;
            }
            cam.transform.GetComponent<Camera>().enabled = true;
            cam.transform.GetComponent<AudioListener>().enabled = true;
            gm.activeCamera = cam.transform.gameObject;
        }
    }  
}
