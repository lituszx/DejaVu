using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraColision : MonoBehaviour
{
    public GameObject playerCam;
    private CameraController gm;
    // Update is called once per frame
    void Update()
    {
        gm = transform.parent.GetComponent<CameraController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerControl>().currentAxis = transform.GetChild(0);
            for (int i = 0; i < gm.allCameras.Count; i++)
            {
                gm.allCameras[i].GetComponent<Camera>().enabled = false;
                gm.allCameras[i].GetComponent<AudioListener>().enabled = false;
            }
            playerCam.GetComponent<Camera>().enabled = true;
            playerCam.GetComponent<AudioListener>().enabled = true;
        }
    }
}
