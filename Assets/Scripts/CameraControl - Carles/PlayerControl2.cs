using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



#if UNITY_EDITOR
using UnityEditor;
#endif
/*

public enum ViewType { FPS, TPS }

[CustomEditor(typeof(PlayerControl))]
public class PlayerControlEditor : Editor
{
    SerializedProperty speed, speedRotation, jumpForce, gravity;
    SerializedProperty canJump, multyJumps;
    SerializedProperty totalJumps;
    SerializedProperty height, lookHeight, distance, speedMove;
    SerializedProperty type;

    bool showGeneral, showJumps, showCamera;

    void OnEnable()
    {
        speed = serializedObject.FindProperty("speed");
        speedRotation = serializedObject.FindProperty("speedRotation");
        jumpForce = serializedObject.FindProperty("jumpForce");
        gravity = serializedObject.FindProperty("gravity");

        canJump = serializedObject.FindProperty("canJump");
        multyJumps = serializedObject.FindProperty("multyJumps");

        totalJumps = serializedObject.FindProperty("totalJumps");

        height = serializedObject.FindProperty("height");
        lookHeight = serializedObject.FindProperty("lookHeight");
        distance = serializedObject.FindProperty("distance");
        speedMove = serializedObject.FindProperty("speedMove");

        type = serializedObject.FindProperty("type");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUIStyle tittle = new GUIStyle();
        tittle.fontStyle = FontStyle.Bold;
        GUIStyle foldout = EditorStyles.foldout;
        foldout.fontStyle = FontStyle.Bold;

        showGeneral = EditorGUILayout.Foldout(showGeneral, "General values", true, foldout);
        if (showGeneral == true)
        {
            speed.floatValue = EditorGUILayout.FloatField("   Speed", speed.floatValue);
            speedRotation.floatValue = EditorGUILayout.FloatField("   Speed rotation", speedRotation.floatValue);
            gravity.floatValue = EditorGUILayout.FloatField("   Gravity", gravity.floatValue);
        }
        EditorGUILayout.Space();
        showJumps = EditorGUILayout.Foldout(showJumps, "Jump values", true);
        if (showJumps)
        {
            canJump.boolValue = EditorGUILayout.Toggle("   Can jump", canJump.boolValue);
            if (canJump.boolValue == true)
            {
                jumpForce.floatValue = EditorGUILayout.FloatField("   Jump foce", jumpForce.floatValue);

                multyJumps.boolValue = EditorGUILayout.Toggle("   Multi jumps", multyJumps.boolValue);
                if (multyJumps.boolValue == true)
                {
                    totalJumps.intValue = EditorGUILayout.IntField(new GUIContent("   Total jumps", "Total jumps in air"), totalJumps.intValue);
                    if (totalJumps.intValue < 1) { totalJumps.intValue = 1; }
                }
            }
        }

        EditorGUILayout.Space();

        showCamera = EditorGUILayout.Foldout(showCamera, "Camera values", true);
        if (showCamera)
        {
            type.enumValueIndex = (int)(ViewType)EditorGUILayout.EnumPopup("   View type",
                (ViewType)Enum.GetValues(typeof(ViewType)).GetValue(type.enumValueIndex));

            height.floatValue = EditorGUILayout.FloatField("   Height", height.floatValue);
            speedMove.floatValue = EditorGUILayout.FloatField("   Speed", speedMove.floatValue);

            if (type.enumValueIndex == 1)
            {
                lookHeight.floatValue = EditorGUILayout.FloatField("   Look height", lookHeight.floatValue);
                distance.floatValue = EditorGUILayout.FloatField("   Distance", distance.floatValue);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif


[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public ViewType type;

    public float speed = 12, speedRotation = 150, jumpForce = 8, gravity = 20;
    public bool canJump = true, multyJumps = false;
    public int totalJumps = 1, currentJumps;

    public float height, lookHeight, distance, speedMove;


    private CharacterController control;
    private Vector3 moveDir;
    private float rotX, rotY;

    private RaycastHit hitCamera;
    private Camera mainCamera;
    private GameObject cameraParent;
    public Transform currenAxis;

    void Start()
    {
        control = GetComponent<CharacterController>();
        moveDir = Vector3.zero;
        rotX = rotY = 0;
        InitCamera();
    }
    public void InitCamera()
    {
        Camera cameraFind = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
        cameraParent = new GameObject("CameraParent");
        if (cameraFind != null) mainCamera = cameraFind;
        else
        {
            GameObject newCamera = new GameObject("FPC_camera");
            mainCamera = newCamera.AddComponent<Camera>();
        }
        AudioListener audioFind = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;
        if (audioFind == null) mainCamera.gameObject.AddComponent<AudioListener>();

        mainCamera.transform.SetParent(cameraParent.transform);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);

        cameraParent.transform.SetParent(transform);
    }

    void Update()
    {
        Movement();
        CameraControl();
    }
    public void Movement()
    {
        if (!control)
        {
            return;
        }

        if (control.isGrounded)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDir = currenAxis.TransformDirection(moveDir);
            moveDir *= speed;

            currentJumps = 0;
            if (canJump == true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    moveDir.y = jumpForce;
                }
            }
        }
        else
        {
            if (canJump == true && multyJumps == true)
            {
                if (Input.GetKeyDown(KeyCode.Space) == true && currentJumps < totalJumps)
                {
                    moveDir.y = jumpForce; currentJumps++;
                }
            }
        }
        moveDir.y -= gravity * Time.deltaTime;
        control.Move(moveDir * Time.deltaTime);
    }
    public void CameraControl()
    {
        if (!mainCamera)
        {
            return;
        }

        if (type == ViewType.TPS)
        {
            mainCamera.transform.position = new Vector3(0, 0.8f, -2.6f);
        }
        else
        {
            if (cameraParent.transform.localRotation != Quaternion.Euler(Vector3.zero))
            {
                cameraParent.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }

            cameraParent.transform.localPosition = new Vector3(0, height, 0);

            //rotY += Input.GetAxis("Mouse Y") * speedMove;
            rotY = Mathf.Clamp(rotY, -90, 90);
            cameraParent.transform.localEulerAngles = new Vector3(-rotY, 0, 0);

            if (GetComponent<Renderer>().enabled)
            {
                GetComponent<Renderer>().enabled = false;
            }
        }
        rotX += Input.GetAxis("Mouse X") * speedMove;
        transform.rotation = Quaternion.Euler(0, rotX, 0);

    }
    
}
*/
