using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public Transform target, pivot;
    public GameObject playerCam;
    public Vector3 offset;
    public bool useOffsetValues, invertY;
    public float rotateSpeed, maxViewAngle, minViewAngle;
    public bool cinematic = false;
    public Vector3 initPos;
    public float currentDistance;
    public Quaternion initRot;
    RaycastHit hitInfo;
    public GameObject gizmo;
    void Start()
    {
        initPos = transform.localPosition;
        initRot = transform.localRotation;
        //if (!useOffsetValues)
        //    offset = target.position - transform.position;
        playerCam = this.gameObject;
        //Cursor.lockState = CursorLockMode.Locked;
        pivot = transform.parent;
    }
    void LateUpdate()
    {
        if (playerCam.GetComponent<Camera>().enabled == true && cinematic == false)
        {
            //Movimiento Camera y aplica al target
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            pivot.Rotate(0, horizontal, 0);
            //float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
            //if (invertY)
            //{
            //    pivot.Rotate(vertical, 0, 0);
            //}
            //else
            //{
            //    pivot.Rotate(-vertical, 0, 0);
            //}
            //Limites
            //if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f)
            //{
            //    pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
            //}
            //if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle)
            //{
            //    pivot.rotation = Quaternion.Euler(360 + minViewAngle, 0, 0);
            //}
            ////Mueve la camera segun la rotacion del target y el offset
            //float desiredYAngle = target.eulerAngles.y;
            //float desiredXAngle = pivot.eulerAngles.x;
            //Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
            //transform.position = target.position - (rotation * offset);
            ////transform.position = target.position - offset;
            //if (transform.position.y < target.position.y)
            //{
            //    transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
            //}
            transform.LookAt(target);
            gizmo.transform.localPosition = offset;
            if (Physics.Linecast(target.transform.position, gizmo.transform.position, out hitInfo))
            {
                transform.position = hitInfo.point;
            }
            else
            {
                transform.localPosition = offset;
            }
        }
    }
    public void Cinematica(int _cinematica, GameObject _pos)
    {
        
        //playerCam.transform.position = _pos.transform.position;
        //playerCam.transform.rotation = _pos.transform.rotation;

        //initPos = transform.localPosition;
        //initRot = transform.localRotation;


        if (_cinematica == 0)
        {
            
            //Cambiar camara como antes
            playerCam.GetComponent<Camera>().enabled = false;
            playerCam.transform.position = initPos;
            playerCam.transform.rotation = initRot;
        }
        else if (_cinematica == 1)
        {
           
            //Cambiar camara
            playerCam.GetComponent<Camera>().enabled = true;

            playerCam.transform.position = _pos.transform.position;
            playerCam.transform.rotation = _pos.transform.rotation;

        }

        //actionar corrutinas en funcion del parametro de la funcion
        //desactivar controles
        //al acabar la corrutina activar la camara actual variable de CameraController activeCam y desactivar la playerCam
        //igualar cinematic a false
        //reposicionar camara playera initpos y rotpos
    }
}
