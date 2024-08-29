using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonControl : MonoBehaviour
{
    public float speed, speedRotation, jumpForce, gravity;
    public bool canJump, multyJumps;
    public int totalJumps, currentJumps;

    private CharacterController control;
    private Vector3 moveDir;
    private float rotX, rotY;

    public float height, lookHeight, distance, speedMove;
    public bool isThirdPerson;
    private RaycastHit hitCamera;
    private Vector3 posCamera, lookAtPos;
    private Camera mainCamera;
    private GameObject cameraParent;

    //Crear un current axis de transform y en moveDir = transofmr.transformDirection(moveDir)
    public Transform currenAxis;
    public GameObject playerCamera;
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        Movement();
    }
    public void Init()
    {
        control = GetComponent<CharacterController>();
        moveDir = Vector3.zero;
        rotX = rotY = 0;
    }
    public void Movement()
    {
        if (!control) return;

        if (control.isGrounded)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (playerCamera.GetComponent<Camera>().enabled == false)
                moveDir = currenAxis.TransformDirection(moveDir);
            else
            {
                moveDir = playerCamera.transform.TransformDirection(moveDir);
            }
            moveDir *= speed;
            currentJumps = 0;
            if (canJump) if (Input.GetKeyDown(KeyCode.Space)) moveDir.y = jumpForce;
        }
        else
        {
            if (canJump && multyJumps) if (Input.GetKeyDown(KeyCode.Space) && currentJumps < totalJumps)
                { moveDir.y = jumpForce; currentJumps++; }
        }
        moveDir.y -= gravity * Time.deltaTime;
        control.Move(moveDir * Time.deltaTime);
    }

}

#region INSPECTOR PARAMETERS
#if UNITY_EDITOR
[CustomEditor(typeof(FirstPersonControl))]
public class FirstPersonControlEditor : Editor
{

    public override void OnInspectorGUI()
    {
        FirstPersonControl FPC_script = (FirstPersonControl)target;
        GUIStyle headerStyle = new GUIStyle();
        headerStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Move parameters", headerStyle);
        FPC_script.speed = EditorGUILayout.FloatField("Speed", FPC_script.speed);
        FPC_script.speedRotation = EditorGUILayout.FloatField("Speed rotation", FPC_script.speedRotation);
        FPC_script.gravity = EditorGUILayout.FloatField("Gravity", FPC_script.gravity);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Jump parameters", headerStyle);
        FPC_script.canJump = EditorGUILayout.Toggle("Can jump", FPC_script.canJump);
        if (FPC_script.canJump)
        {
            FPC_script.jumpForce = EditorGUILayout.FloatField("    Jump force", FPC_script.jumpForce);
        }
        FPC_script.multyJumps = EditorGUILayout.Toggle("Multy jumps", FPC_script.multyJumps);
        if (FPC_script.multyJumps)
        {
            FPC_script.totalJumps = EditorGUILayout.IntField("    Number of jumps", FPC_script.totalJumps);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Camera parameters", headerStyle);
        FPC_script.height = EditorGUILayout.FloatField("Height", FPC_script.height);
        FPC_script.speedMove = EditorGUILayout.FloatField("Speed rotate", FPC_script.speedMove);
        FPC_script.isThirdPerson = EditorGUILayout.Toggle("Is Third Person", FPC_script.isThirdPerson);
        if (FPC_script.isThirdPerson)
        {
            FPC_script.lookHeight = EditorGUILayout.FloatField("    Look height", FPC_script.lookHeight);
            FPC_script.distance = EditorGUILayout.FloatField("    Distance", FPC_script.distance);
        }
        DrawDefaultInspector();
    }
}
#endif
#endregion