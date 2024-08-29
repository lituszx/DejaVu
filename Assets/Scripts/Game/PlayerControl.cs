using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    
    
    public GameObject Dild;
    private bool isLogrosActive, isLogrosActive3, isLogrosActive2, isLogrosActive4;
    public GameObject mirrorDraw;

    private float SoundWalk, SoundWaterWalk;
    public Text PADTEXT;
    public GameObject PanelPad;


    //Sonidos aleatorios de varias cosas
    public List<GameObject> HeySound = new List<GameObject>();
    public List<GameObject> WalkSounds = new List<GameObject>();
    public List<GameObject> LaunchSound = new List<GameObject>();
    public GameObject CoverSound, MidCoverSound;
    public List<GameObject> LogrosSound = new List<GameObject>();
    public List<GameObject> ButtonDoorsSound = new List<GameObject>();
    public GameObject RainSound;
    public GameObject OpenDoorSound, CloseDoorSound;
    public GameObject Weed1, Weed2;
    public GameObject PC_ClickSound, PC_StartSound, PC_FanSound;
    public GameObject OpenInventorySound;
    public GameObject TakeNoteSound;
    public List<GameObject> WaterWalkSound = new List<GameObject>();


    //Controles basicos
    private CharacterController control;
    private Vector3 moveDir;
    public float speed, speedRotate, gravity;

    //Rayo coger un objeto
    private RaycastHit hitObject;

    private GameObject point;
    private GameObject point2;
    private bool CanThrow;

    private bool Crouch;

    //Rayo cobertura
    private RaycastHit HitWall;
    private bool isCover;


    //Mecanica de dialogo mejorada
    [System.Serializable]
    public class Talk
    {
        [System.Serializable]
        public class Talk2
        {
            public string frase;
        }
        public List<Talk2> Frases = new List<Talk2>();
    }
    public List<Talk> Dialogos = new List<Talk>();
    private int currentDialogue, currentfrase;
    public GameObject dialogoCanvas;
    public Text dialogueText;
    private bool StartSpeak, StartSpeak2, StartSpeak3, StartSpeak4, StartSpeak5, StartSpeak6, StartSpeak7, StartSpeak8, StartSpeak9, StartSpeak10;


    //public Image Tasser, Object1, Object2;

    //PAD
    private RaycastHit HitPad;
    private bool CanWrite, CanWrite2, CanWrite3, CanWrite4, CanWrite5;
    public Text PadText, PadText2, PadText3, PadText4, PadText5;
    public Animator DoorAnim, DoorAnim2, DoorAnim3, DoorAnim4, DoorAnim5;
    public GameObject EmptyDoorPad;

    //UI Imagenes
    public GameObject[] UIImage;

    //Animator del player
    public Animator playerAnim;
    private float ContadorIdle2;

    private RaycastHit HitStairs;


    public Transform currentAxis;
    public GameObject playerCamera;

    private RaycastHit HitDoor;
    public GameObject Dialogue5, Dialogue6;
    public GameObject Cinematica3, Cinematica3_2;

    private bool TakeNote1, TakeNote2, TakeNote3, TakeNote4, TakeNote5, TakeNote6;
    public GameObject Paper1, Paper2, Paper3, Paper4, Paper5, Paper6;

    private int contador = 0;

    private void Awake()
    {
        //playerAnim.SetInteger("wakeup", 0);
    }

    void Start()
    {
        
        isLogrosActive = true;

        Time.timeScale = 1;
        control = GetComponent<CharacterController>();
        point = transform.GetChild(1).gameObject;
        point2 = transform.GetChild(2).gameObject;
        //Desactivar empty;
        point.SetActive(false);
        point2.SetActive(false);

        Crouch = false;
        isCover = false;
        StartSpeak = false;
        CanWrite = false;

        //Desactivar cursor en la pantalla
        Cursor.visible = false;

        PadText.text = "0000";
        PadText2.text = "0000";
        PadText3.text = "0000";
        PadText4.text = "0565";
        PadText5.text = "0000";

        //PlayerPrefs.DeleteAll();


        playerAnim.SetInteger("wakeup", 0);
        StartCoroutine(WakeUp("wakeup", 1));

        //Luvia de fondo
        Instantiate(RainSound);
    }

    void Update()

    {
        if (MenuManager2.isPaused == false)
        {
            Cursor.visible = true;

            SoundWalk += Time.deltaTime;
            SoundWaterWalk += Time.deltaTime;

            if (Physics.Raycast(transform.position, transform.forward, out HitDoor, 4))
            {

                if (HitDoor.collider.tag == "Door")
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        //Sonido Puerta
                        GameObject NewDoorOpenSound = Instantiate(OpenDoorSound);
                        Destroy(NewDoorOpenSound, 1);

                        Quaternion initRot = HitDoor.transform.rotation;
                        Vector3 initPos = HitDoor.transform.position;
                        if (contador == 0)
                        {
                            HitDoor.transform.Rotate(0, 0, +90);
                            contador = 1;

                        }
                        else
                        {
                            HitDoor.transform.Rotate(0, 0, -90);
                            HitDoor.transform.position = initPos;
                            contador = 0;

                        }

                    }
                }
            }



            //Rayo para la animacion de las escaleras
            if (Physics.Raycast(transform.position, Vector3.down, out HitStairs, 4))
            {
                if (HitStairs.collider.tag == "stairs")
                {
                    if (moveDir.z > 0)
                    {
                        playerAnim.SetInteger("stairs", 1);
                    }
                    else if (moveDir.z < 0)
                    {
                        playerAnim.SetInteger("stairs", 2);
                    }

                }
                else
                {
                    playerAnim.SetInteger("stairs", 0);
                }

                if (HitStairs.collider.tag == "Bath")
                {

                    if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0 ||
                        Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") < 0)
                    {
                        if (SoundWaterWalk > 1)
                        {
                            GameObject NewWalkBathSound = Instantiate(WaterWalkSound[Random.Range(0, 4)]);
                            Destroy(NewWalkBathSound, 1);
                            SoundWaterWalk = 0;
                        }
                    }
                }
            }

            //Animacion idle2
            ContadorIdle2 += Time.deltaTime;
            if (ContadorIdle2 >= Random.Range(15f, 30f))
            {
                playerAnim.SetBool("idle2", true);

            }

            if (ContadorIdle2 >= Random.Range(15f, 30f))
            {
                playerAnim.SetBool("idle2", false);
                ContadorIdle2 = 0;
            }



            //Imagenes UI
            for (int i = 0; i < UIImage.Length; i++)
            {
                if (Vector3.Distance(transform.position, UIImage[i].transform.position) < 4f)
                {
                    UIImage[i].gameObject.SetActive(true);
                }
                else
                {
                    UIImage[i].gameObject.SetActive(false);
                }
            }

            //Mecanica de dialogo

            //Dialogo01
            if (StartSpeak)
            {
                dialogueText.text = Dialogos[0].Frases[currentDialogue].frase;

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    currentDialogue++;

                    if (currentDialogue >= Dialogos[0].Frases.Count)
                    {
                        currentDialogue = 0;
                        dialogoCanvas.SetActive(false);
                        StartSpeak = false;
                    }

                }
            }
            //Dialogo02
            if (StartSpeak2)
            {
                dialogueText.text = Dialogos[1].Frases[currentDialogue].frase;

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    currentDialogue++;

                    if (currentDialogue >= Dialogos[1].Frases.Count)
                    {
                        currentDialogue = 0;
                        dialogoCanvas.SetActive(false);
                        StartSpeak2 = false;
                    }

                }
            }

            //Dialogo03
            if (StartSpeak3)
            {
                dialogueText.text = Dialogos[2].Frases[currentDialogue].frase;

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    currentDialogue++;

                    if (currentDialogue >= Dialogos[2].Frases.Count)
                    {
                        currentDialogue = 0;
                        dialogoCanvas.SetActive(false);
                        StartSpeak3 = false;
                    }

                }
            }

            //Dialogo04
            if (StartSpeak4)
            {
                dialogueText.text = Dialogos[3].Frases[currentDialogue].frase;

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    currentDialogue++;

                    if (currentDialogue >= Dialogos[3].Frases.Count)
                    {
                        currentDialogue = 0;
                        dialogoCanvas.SetActive(false);
                        StartSpeak4 = false;
                    }

                }
            }

            //Dialogo05
            if (StartSpeak5)
            {
                dialogueText.text = Dialogos[4].Frases[currentDialogue].frase;

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    currentDialogue++;

                    if (currentDialogue >= Dialogos[4].Frases.Count)
                    {
                        currentDialogue = 0;
                        //Desactivar un dialogo y poner otro
                        Dialogue5.SetActive(false);
                        Dialogue6.SetActive(true);
                        //Descativar una animacion de la cinematica y poner otra
                        Cinematica3.SetActive(false);
                        Cinematica3_2.SetActive(true);

                        dialogoCanvas.SetActive(false);
                        StartSpeak5 = false;
                    }

                }
            }

            //Dialogo06
            if (StartSpeak6)
            {
                dialogueText.text = Dialogos[5].Frases[currentDialogue].frase;

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    currentDialogue++;

                    if (currentDialogue >= Dialogos[5].Frases.Count)
                    {
                        currentDialogue = 0;
                        dialogoCanvas.SetActive(false);
                        StartSpeak6 = false;
                    }

                }
            }


            //Dialogo07
            if (StartSpeak7)
            {
                dialogueText.text = Dialogos[6].Frases[currentDialogue].frase;

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    currentDialogue++;

                    if (currentDialogue >= Dialogos[6].Frases.Count)
                    {
                        currentDialogue = 0;
                        dialogoCanvas.SetActive(false);
                        StartSpeak7 = false;
                    }

                }
            }


            //Dialogo08
            if (StartSpeak8)
            {
                dialogueText.text = Dialogos[7].Frases[currentDialogue].frase;

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    currentDialogue++;

                    if (currentDialogue >= Dialogos[7].Frases.Count)
                    {
                        currentDialogue = 0;
                        dialogoCanvas.SetActive(false);
                        StartSpeak8 = false;
                    }

                }
            }


            //Dialogo09
            if (StartSpeak9)
            {
                dialogueText.text = Dialogos[8].Frases[currentDialogue].frase;

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    currentDialogue++;

                    if (currentDialogue >= Dialogos[8].Frases.Count)
                    {
                        currentDialogue = 0;
                        dialogoCanvas.SetActive(false);
                        StartSpeak9 = false;
                    }

                }
            }


            //Dialogo10
            if (StartSpeak10)
            {
                dialogueText.text = Dialogos[9].Frases[currentDialogue].frase;

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    currentDialogue++;

                    if (currentDialogue >= Dialogos[9].Frases.Count)
                    {
                        currentDialogue = 0;
                        dialogoCanvas.SetActive(false);
                        StartSpeak10 = false;
                    }

                }
            }


            //Mecanica de abrir puerta con Keypad //Pad1
            if (CanWrite)
            {
                DoorAnim.SetBool("Open", false);
                PADTEXT.text = PadText.text;
                CanWrite2 = false;
                CanWrite3 = false;
                CanWrite4 = false;
                CanWrite5 = false;
                PanelPad.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    PadText.text += "0";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);

                }
                else if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    PadText.text += "1";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    PadText.text += "2";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    PadText.text += "3";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    PadText.text += "4";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    PadText.text += "5";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    PadText.text += "6";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad7))
                {
                    PadText.text += "7";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad8))
                {
                    PadText.text += "8";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    PadText.text += "9";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }

                if (PadText.text.Length > 4 && PadText.text != "1154")
                {
                    PadText.text = "";
                    PanelPad.SetActive(false);
                    CanWrite = false;
                }
                else if (PadText.text == "1154")
                {
                    PadText.text = "OPEN";
                    DoorAnim.SetBool("Open", true);
                    CanWrite = false;
                    PanelPad.SetActive(false);
                }
            }

            //Pad 3
            if (CanWrite2)
            {
                DoorAnim2.SetBool("Open", false);
                PADTEXT.text = PadText2.text;
                CanWrite = false;
                CanWrite3 = false;
                CanWrite4 = false;
                CanWrite5 = false;
                PanelPad.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    PadText2.text += "0";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);

                }
                else if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    PadText2.text += "1";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    PadText2.text += "2";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    PadText2.text += "3";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    PadText2.text += "4";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    PadText2.text += "5";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    PadText2.text += "6";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad7))
                {
                    PadText2.text += "7";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad8))
                {
                    PadText2.text += "8";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    PadText2.text += "9";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }

                if (PadText2.text.Length > 4 && PadText2.text != "1996")
                {
                    PadText2.text = "";
                    PanelPad.SetActive(false);
                    CanWrite2 = false;
                }
                else if (PadText2.text == "1996")
                {
                    PadText2.text = "OPEN";
                    DoorAnim2.SetBool("Open", true);
                    CanWrite2 = false;
                    PanelPad.SetActive(false);
                }
            }

            if (CanWrite3)
            {
                DoorAnim3.SetBool("Open", false);
                PADTEXT.text = PadText3.text;
                CanWrite2 = false;
                CanWrite = false;
                CanWrite4 = false;
                CanWrite5 = false;
                PanelPad.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    PadText3.text += "0";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);

                }
                else if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    PadText3.text += "1";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    PadText3.text += "2";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    PadText3.text += "3";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    PadText3.text += "4";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    PadText3.text += "5";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    PadText3.text += "6";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad7))
                {
                    PadText3.text += "7";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad8))
                {
                    PadText3.text += "8";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    PadText3.text += "9";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }

                if (PadText3.text.Length > 4 && PadText3.text != "3014")
                {
                    PadText3.text = "";
                    PanelPad.SetActive(false);
                    CanWrite3 = false;
                }
                else if (PadText3.text == "3014")
                {
                    PadText3.text = "OPEN";
                    DoorAnim3.SetBool("Open", true);
                    CanWrite3 = false;
                    PanelPad.SetActive(false);
                }
            }

            if (CanWrite4)
            {
                DoorAnim4.SetBool("Open", false);
                PADTEXT.text = PadText4.text;
                CanWrite2 = false;
                CanWrite3 = false;
                CanWrite = false;
                CanWrite5 = false;
                PanelPad.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    PadText4.text += "0";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);

                }
                else if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    PadText4.text += "1";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    PadText4.text += "2";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    PadText4.text += "3";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    PadText4.text += "4";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    PadText4.text += "5";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    PadText4.text += "6";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad7))
                {
                    PadText4.text += "7";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad8))
                {
                    PadText4.text += "8";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    PadText4.text += "9";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }

                if (PadText4.text.Length > 4 && PadText4.text != "0000")
                {
                    PadText4.text = "";
                    PanelPad.SetActive(false);
                    CanWrite4 = false;
                }
                else if (PadText4.text == "0000")
                {
                    PadText4.text = "OPEN";
                    DoorAnim4.SetBool("Open", true);
                    CanWrite4 = false;
                    PanelPad.SetActive(false);
                    transform.GetChild(8).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    StartCoroutine(LoadScene("Menu"));
                }
            }

            if (CanWrite5)
            {
                DoorAnim5.SetBool("Open", false);
                PADTEXT.text = PadText5.text;
                CanWrite2 = false;
                CanWrite3 = false;
                CanWrite4 = false;
                CanWrite = false;
                PanelPad.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    PadText5.text += "0";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);

                }
                else if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    PadText5.text += "1";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    PadText5.text += "2";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    PadText5.text += "3";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    PadText5.text += "4";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    PadText5.text += "5";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    PadText5.text += "6";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad7))
                {
                    PadText5.text += "7";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad8))
                {
                    PadText5.text += "8";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    PadText5.text += "9";
                    GameObject NewSoundButton = Instantiate(ButtonDoorsSound[Random.Range(0, 3)]);
                    Destroy(NewSoundButton, 1);
                }

                if (PadText5.text.Length > 4 && PadText5.text != "1933")
                {
                    PadText5.text = "";
                    PanelPad.SetActive(false);
                    CanWrite5 = false;
                }
                else if (PadText5.text == "1933")
                {
                    PadText5.text = "OPEN";
                    DoorAnim5.SetBool("Open", true);
                    PanelPad.SetActive(false);
                    CanWrite5 = false;
                }
            }


            Debug.DrawRay(EmptyDoorPad.transform.position, transform.forward, Color.yellow);

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                if (Physics.Raycast(EmptyDoorPad.transform.position, transform.forward, out HitPad, 3))
                {
                    if (HitPad.collider.tag == "Pad")
                    {
                        print("Hola");


                        CanWrite = !CanWrite;
                    }

                    if (HitPad.collider.tag == "Pad2")
                    {
                        print("Hola");
                        CanWrite2 = !CanWrite2;
                    }

                    if (HitPad.collider.tag == "Pad3")
                    {
                        print("Hola");
                        CanWrite3 = !CanWrite3;
                    }

                    if (HitPad.collider.tag == "Pad4")
                    {
                        print("Hola");
                        CanWrite4 = !CanWrite4;
                    }

                    if (HitPad.collider.tag == "Pad5")
                    {
                        print("Hola");
                        CanWrite5 = !CanWrite5;
                    }
                }
            }




            //Mirar rayo de cubrirte en el debug
            //Debug.DrawRay(transform.position, Camera.main.transform.forward, Color.black);
            //Mecanica de cubrirse
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                if (Physics.Raycast(transform.position, Camera.main.transform.forward, out HitWall, 3))
                {
                    if (HitWall.collider.tag == "Wall" || HitWall.collider.tag == "mid_wall")
                    {
                        isCover = !isCover;
                    }
                }
            }

            //Control al cubrirse
            if (isCover)
            {

                if (Input.GetAxis("Horizontal") >= 0.3 || Input.GetAxis("Horizontal") <= 0.3 ||
                    Input.GetAxis("Vertical") >= 0.3 || Input.GetAxis("Vertical") <= 0.3)
                {
                    moveDir = new Vector3(Input.GetAxis("Horizontal") * speed, 0, 0);
                    moveDir = transform.TransformDirection(moveDir);

                    //Rotacion del player al cubrirse
                    Vector3 normalDir = HitWall.normal;
                    normalDir *= -1;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(normalDir), 5);

                }
                if (HitWall.collider.tag == "Wall")
                {
                    playerAnim.SetInteger("cover", 1);
                    //Sonido cobertura
                    /*
                    GameObject NewCoverSound = Instantiate(CoverSound);
                    Destroy(NewCoverSound, 1);
                    */

                    if (Input.GetAxis("Horizontal") < 0)
                    {
                        playerAnim.SetInteger("move", 2);
                    }
                    else if (Input.GetAxis("Horizontal") > 0)
                    {
                        playerAnim.SetInteger("move", 1);
                    }
                    else
                    {
                        playerAnim.SetInteger("move", 0);
                    }

                }

                if (HitWall.collider.tag == "mid_wall")
                {
                    playerAnim.SetInteger("cover", 2);

                    //Sonido media cobertura
                    GameObject NewMidCoverSound = Instantiate(MidCoverSound);
                    Destroy(NewMidCoverSound, 1);

                    if (Input.GetAxis("Horizontal") < 0)
                    {
                        playerAnim.SetInteger("move", 2);


                    }
                    else if (Input.GetAxis("Horizontal") > 0)
                    {
                        playerAnim.SetInteger("move", 1);


                    }
                    else
                    {
                        playerAnim.SetInteger("move", 0);
                    }

                }
            }
            else
            {
                playerAnim.SetInteger("cover", 0);

            }


            //Mecanica de correr
            /*
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 20;
            }
            else
            {
                speed = 5;
            }
            */


            //Mecanica de agacaharse
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                Crouch = !Crouch;
            }


            if (Crouch)
            {
                control.height = 1;
                speed = 2;
                //Recolocar FBX de player

                transform.GetChild(6).transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);

                //Animacion de agacharse
                playerAnim.SetBool("crouch", true);

            }
            else
            {

                control.height = 2;

                //Recolocar FBX del player
                transform.GetChild(6).transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);

                //Quitar animacion de agacharse
                playerAnim.SetBool("crouch", false);
            }

            //Lanzar Objeto
            if (CanThrow == true)
            {
                if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.JoystickButton6))
                {
                    hitObject.collider.GetComponent<LineRenderer>().enabled = true;

                    if (playerAnim.GetInteger("aim") != 1)
                    {
                        playerAnim.SetInteger("aim", 1);
                        //StartCoroutine(SetAnimValue("aim", 0));
                        //playerAnim.SetBool("aimbool", true);
                        StartCoroutine(SetAnimValue("aimbool", true));
                    }

                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton7))
                    {

                        //Funcion lanzar objeto
                        hitObject.collider.GetComponent<Parabola>().Launch();
                        //Reposicionar objeto
                        hitObject.collider.transform.GetComponent<BoxCollider>().isTrigger = false;
                        //hitObject.collider.GetComponent<Rigidbody>().isKinematic = true;

                        //Sonido Lanzar objeto
                        GameObject newLaunchSound = Instantiate(LaunchSound[Random.Range(0, 4)]);
                        Destroy(newLaunchSound, 1);

                        playerAnim.SetBool("throw", true);
                        playerAnim.SetInteger("aim", 0);
                        playerAnim.SetBool("aimbool", false);

                        hitObject.collider.GetComponent<LineRenderer>().enabled = false;
                        CanThrow = false;
                        point.SetActive(false);
                        point2.SetActive(false);
                        hitObject.collider.transform.SetParent(null);

                    }
                }
                else
                {

                    playerAnim.SetInteger("aim", 0);
                    playerAnim.SetBool("aimbool", false);
                    hitObject.collider.transform.GetComponent<LineRenderer>().enabled = false;

                }
            }

            //Mirar rayo en el debug
            Debug.DrawRay(transform.GetChild(6).GetChild(0).GetChild(1).GetChild(0).transform.position, transform.forward, Color.red);
            //Coger Objeto
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                if (Physics.Raycast(transform.GetChild(6).GetChild(0).GetChild(1).GetChild(0).transform.position, transform.forward, out hitObject, 3))
                {
                    if (hitObject.collider.tag == "Obj")
                    {

                        //Hacer hijo al objeto
                        hitObject.collider.transform.SetParent(transform);
                        hitObject.collider.transform.position = transform.GetChild(6).GetChild(0).GetChild(1).GetChild(0).transform.position;
                        hitObject.collider.transform.GetComponent<BoxCollider>().isTrigger = true;
                        //hitObject.collider.GetComponent<Rigidbody>().isKinematic = false;

                        //Activar empty
                        point.SetActive(true);
                        point2.SetActive(true);
                        CanThrow = true;

                        //Objetos del inventario
                        //Object1.enabled = true;
                        //Object2.enabled = true;

                    }
                }
            }

            //Soltar Objeto
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                if (Physics.Raycast(transform.position, transform.forward, out hitObject, 3))
                {
                    if (hitObject.collider.tag == "Obj")
                    {
                        print("sASDA");

                        playerAnim.SetInteger("aim", 0);
                        playerAnim.SetBool("aimbool", false);
                        hitObject.collider.transform.GetComponent<BoxCollider>().isTrigger = false;
                        hitObject.collider.GetComponent<LineRenderer>().enabled = false;
                        //Hacer hijo al objeto
                        hitObject.collider.transform.GetChild(10).SetParent(null);
                        //Activar empty
                        point.SetActive(false);
                        CanThrow = false;

                        hitObject.collider.GetComponent<LineRenderer>().enabled = false;

                        //Objetos del inventario
                        //Object1.enabled = false;
                        //Object2.enabled = false;

                    }
                }
            }

            //Transicion animacion caminar


            //Movimiento personaje
            if (isCover == false)
            {
                if (control.isGrounded)
                {
                    if (PADTEXT.transform.parent.gameObject.activeSelf == false)
                    {
                        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0 ||
                        Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") < 0)
                        {
                            playerAnim.SetBool("walk", true);

                            //Sonido pasos
                            if (SoundWalk > 1)
                            {
                                GameObject newWalkSound = Instantiate(WalkSounds[Random.Range(0, 3)]);
                                Destroy(newWalkSound, 1);
                                SoundWalk = 0;
                            }

                        }
                        else
                        {
                            playerAnim.SetBool("walk", false);
                        }


                        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                        if (playerCamera.GetComponent<Camera>().enabled == false)
                        {
                            moveDir = currentAxis.TransformDirection(moveDir);
                        }
                        else
                        {
                            moveDir = playerCamera.transform.TransformDirection(moveDir);
                        }

                        moveDir *= speed;
                    }



                    /*
                    //Recolocar FBX del player en funcion de donde mira
                    if (Input.GetAxis("Horizontal") > 0)
                    {
                        //Recolocar FBX de player
                        transform.GetChild(6).transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + 90, transform.localRotation.z);
                    }
                    else if (Input.GetAxis("Horizontal") < 0)
                    {
                        //Recolocar FBX de player
                        transform.GetChild(6).transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y - 90, transform.localRotation.z);
                    }                   
                    else
                    {
                        transform.GetChild(6).transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z);
                    }
                    */

                }
                else
                {

                    if (Input.GetAxis("Horizontal") >= 0.3 || Input.GetAxis("Horizontal") <= 0.3 ||
                        Input.GetAxis("Vertical") >= 0.3 || Input.GetAxis("Vertical") <= 0.3)
                    {

                        moveDir = new Vector3(Input.GetAxis("Horizontal") * speed, moveDir.y, Input.GetAxis("Vertical") * speed);
                        moveDir = transform.TransformDirection(moveDir);

                    }

                }
                //Rotacion personaje
                transform.Rotate(Vector3.up * speedRotate * Input.GetAxis("Mouse X"));
                //Rotacion con el mando
                //transform.Rotate(Vector3.up * speedRotate * Input.GetAxis("RightJoystick"));


            }
            moveDir.y -= gravity * Time.deltaTime;
            control.Move(moveDir * Time.deltaTime);

            /*
            if (Input.GetKeyDown(KeyCode.N))
            {
                SaveLoadGame.DeleteData("activeNotas");
                SaveLoadGame.DeleteData("activeLogros");
                GetComponent<ExtrasManager>().LoadGame();
            }
            */


            //No problem
            if (TakeNote1)
            {
                transform.GetComponent<ExtrasManager>().DebugNotas(0);
                Paper1.SetActive(false);
                GameObject NewTakeNoteSound = Instantiate(TakeNoteSound);
                Destroy(NewTakeNoteSound, 1);
                TakeNote1 = false;

            }
            //No problem
            if (TakeNote2)
            {
                transform.GetComponent<ExtrasManager>().DebugNotas(5);
                Paper2.SetActive(false);
                GameObject NewTakeNoteSound = Instantiate(TakeNoteSound);
                Destroy(NewTakeNoteSound, 1);
                TakeNote2 = false;

            }

            if (TakeNote3)
            {
                transform.GetComponent<ExtrasManager>().DebugNotas(2);
                Paper3.SetActive(false);
                GameObject NewTakeNoteSound = Instantiate(TakeNoteSound);
                Destroy(NewTakeNoteSound, 1);
                TakeNote3 = false;

            }

            if (TakeNote4)
            {
                transform.GetComponent<ExtrasManager>().DebugNotas(3);
                Paper4.SetActive(false);
                GameObject NewTakeNoteSound = Instantiate(TakeNoteSound);
                Destroy(NewTakeNoteSound, 1);
                TakeNote4 = false;

            }

            if (TakeNote5)
            {
                transform.GetComponent<ExtrasManager>().DebugNotas(4);
                Paper5.SetActive(false);
                GameObject NewTakeNoteSound = Instantiate(TakeNoteSound);
                Destroy(NewTakeNoteSound, 1);
                TakeNote5 = false;

            }

            if (TakeNote6)
            {
                transform.GetComponent<ExtrasManager>().DebugNotas(1);
                Paper6.SetActive(false);
                GameObject NewTakeNoteSound = Instantiate(TakeNoteSound);
                Destroy(NewTakeNoteSound, 1);
                TakeNote6 = false;

            }

        }
    }

    IEnumerator SetAnimValue(string animName, int animId)
    {
        yield return new WaitForSeconds(0.1f);
        playerAnim.SetInteger(animName, animId);
    }

    IEnumerator SetAnimValue(string animName, bool animId)
    {
        yield return new WaitForSeconds(0.1f);
        playerAnim.SetBool(animName, animId);
    }
    /*
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    */


    private IEnumerator LoadScene(string levelName)
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(levelName);
    }

    private IEnumerator WakeUp(string animName, int animId)
    {
        yield return new WaitForSeconds(2f);
        playerAnim.SetInteger("wakeup", 1);
    }




    private void OnTriggerEnter(Collider col)
    {
        //Dildo      
        if (col.tag == "Dildo")
        {
            transform.GetComponent<ExtrasManager>().DebugLogros(2);
            Dild.SetActive(false);
            if (isLogrosActive4)
            {
                //Sonidos logros
                GameObject NewSoundLogros = Instantiate(LogrosSound[Random.Range(0, 4)]);
                Destroy(NewSoundLogros, 2);
                isLogrosActive4 = false;
            }
        }


        //Nota1       
        if (col.tag == "Note1")
        {
            TakeNote1 = true;
        }

        //Nota2       
        if (col.tag == "Note2")
        {
            TakeNote2 = true;
        }

        //Nota3       
        if (col.tag == "Note3")
        {
            TakeNote3 = true;
        }

        //Nota4     
        if (col.tag == "Note4")
        {
            TakeNote4 = true;
        }

        //Nota5     
        if (col.tag == "Note5")
        {
            TakeNote5 = true;
        }

        //Nota6      
        if (col.tag == "Note6")
        {
            TakeNote6 = true;
        }

        //Huevo sala secreta
        if (col.tag == "Egg")
        {
            transform.GetComponent<ExtrasManager>().DebugLogros(6);

            if (isLogrosActive3)
            {
                //Sonidos logros
                GameObject NewSoundLogros = Instantiate(LogrosSound[Random.Range(0, 4)]);
                Destroy(NewSoundLogros, 2);
                isLogrosActive3 = false;
            }
        }

        //Cinematica 1
        if (col.tag == "Cinematica1")
        {

            transform.GetChild(8).GetComponent<CameraPlayer>().Cinematica(1, transform.GetChild(8).gameObject);
            StartCoroutine(SetAnimValue("mirror", true));
            transform.GetComponent<ExtrasManager>().DebugLogros(0);

            if (isLogrosActive)
            {
                //Sonidos logros
                GameObject NewSoundLogros = Instantiate(LogrosSound[Random.Range(0, 4)]);
                Destroy(NewSoundLogros, 2);
                isLogrosActive = false;
            }


            //Destruyo Colision
            //Destroy(col.gameObject);
            //StartCoroutine(SetAnimValue("mirror", false));
        }

        //Cinematica 2
        if (col.tag == "Cinematica2")
        {
            transform.GetChild(8).GetComponent<CameraPlayer>().Cinematica(1, transform.GetChild(8).gameObject);
            StartCoroutine(SetAnimValue("WC_Masc", true));
            mirrorDraw.SetActive(true);

        }

        //Cinematica 3
        if (col.tag == "Cinematica3")
        {
            transform.GetChild(8).GetComponent<CameraPlayer>().Cinematica(1, transform.GetChild(8).gameObject);
            StartCoroutine(SetAnimValue("WC_Fem", 1));
        }

        //Cinematica 3.1
        if (col.tag == "Cinematica3.1")
        {
            transform.GetChild(8).GetComponent<CameraPlayer>().Cinematica(1, transform.GetChild(8).gameObject);
            StartCoroutine(SetAnimValue("WC_Fem", 2));
        }

        //Cinematica 4
        if (col.tag == "Cinematica4")
        {
            transform.GetChild(8).GetComponent<CameraPlayer>().Cinematica(1, transform.GetChild(8).gameObject);
            StartCoroutine(SetAnimValue("Weed", true));
            //Sonido calada
            GameObject NewWeedSound = Instantiate(Weed1);
            Destroy(NewWeedSound, 1);

            transform.GetComponent<ExtrasManager>().DebugLogros(5);
            if (isLogrosActive2)
            {
                //Sonidos logros
                GameObject NewSoundLogros = Instantiate(LogrosSound[Random.Range(0, 4)]);
                Destroy(NewSoundLogros, 2);
                isLogrosActive2 = false;
            }

        }

        //Salir de la cobertura
        if (col.tag == "Out")
        {
            isCover = false;
        }

        //Iniciar dialogo1
        if (col.tag == "Dialogue1")
        {
            dialogoCanvas.SetActive(true);
            StartSpeak = true;
        }

        //Iniciar dialogo 2
        if (col.tag == "Dialogue2")
        {
            dialogoCanvas.SetActive(true);
            StartSpeak2 = true;
        }

        //Iniciar dialogo 3
        if (col.tag == "Dialogue3")
        {
            dialogoCanvas.SetActive(true);
            StartSpeak3 = true;
        }

        //Iniciar dialogo 4
        if (col.tag == "Dialogue4")
        {
            dialogoCanvas.SetActive(true);
            StartSpeak4 = true;
        }

        //Iniciar dialogo 5
        if (col.tag == "Dialogue5")
        {
            dialogoCanvas.SetActive(true);
            StartSpeak5 = true;
        }

        //Iniciar dialogo 6
        if (col.tag == "Dialogue6")
        {
            dialogoCanvas.SetActive(true);
            StartSpeak6 = true;
        }

        //Iniciar dialogo 7
        if (col.tag == "Dialogue7")
        {
            dialogoCanvas.SetActive(true);
            StartSpeak7 = true;
        }

        //Iniciar dialogo 8
        if (col.tag == "Dialogue8")
        {
            dialogoCanvas.SetActive(true);
            StartSpeak8 = true;
        }

        //Iniciar dialogo 9
        if (col.tag == "Dialogue9")
        {
            dialogoCanvas.SetActive(true);
            StartSpeak9 = true;
        }

        //Iniciar dialogo 10
        if (col.tag == "Dialogue10")
        {
            dialogoCanvas.SetActive(true);
            StartSpeak10 = true;
        }

        //Inventario
        if (col.tag == "Tasser")
        {
            Destroy(col.gameObject);
            //Tasser.enabled = true;
        }

        //Si te pillan los enemigos
        if (col.tag == "enemyVision")
        {
            
           
            

            StartCoroutine(SetAnimValue("wasted", true));

            GameObject newHeySound = Instantiate(HeySound[Random.Range(0, 2)]);
            Destroy(newHeySound, 1);

            //Animacion de primera vez
            PlayerPrefs.SetInt("Load", 1);

            if (PlayerPrefs.GetInt("Load") == 1)
            {
                //Animacion de primera vez
                PlayerPrefs.SetInt("Load", 2);
                playerAnim.SetInteger("wakeup", 1);

            }
            else if (PlayerPrefs.GetInt("Load") == 2)
            {
                //Animacion de segunda vez
                PlayerPrefs.SetInt("Load", 3);
                playerAnim.SetInteger("wakeup", 2);
            }
            else if (PlayerPrefs.GetInt("Load") == 3)
            {
                //Animacion de tercera vez
                playerAnim.SetInteger("wakeup", 3);
            }

            //Reinciar escena
            StartCoroutine(LoadScene("Game"));
        }
    }


    private void OnTriggerExit(Collider col)
    {
        //Silla      
        if (col.tag == "Chair")
        {
            col.GetComponent<BoxCollider>().isTrigger = false;
        }


        //Cinematica 1       
        if (col.tag == "Cinematica1")
        {
            transform.GetChild(8).GetComponent<CameraPlayer>().Cinematica(0, transform.GetChild(8).gameObject);
            StartCoroutine(SetAnimValue("mirror", false));
        }

        //Cinematica 2
        if (col.tag == "Cinematica2")
        {
            transform.GetChild(8).GetComponent<CameraPlayer>().Cinematica(0, transform.GetChild(8).gameObject);
            StartCoroutine(SetAnimValue("WC_Masc", false));
        }

        //Cinematica 3
        if (col.tag == "Cinematica3")
        {
            transform.GetChild(8).GetComponent<CameraPlayer>().Cinematica(0, transform.GetChild(8).gameObject);
            StartCoroutine(SetAnimValue("WC_Fem", 0));

        }

        //Cinematica 3
        if (col.tag == "Cinematica3.1")
        {
            transform.GetChild(8).GetComponent<CameraPlayer>().Cinematica(0, transform.GetChild(8).gameObject);
            StartCoroutine(SetAnimValue("WC_Fem", 0));
        }

        //Cinematica 4
        if (col.tag == "Cinematica4")
        {
            transform.GetChild(8).GetComponent<CameraPlayer>().Cinematica(0, transform.GetChild(8).gameObject);
            StartCoroutine(SetAnimValue("Weed", false));
        }

        //Salir del dialogo
        if (col.tag == "Dialogue1")
        {
            dialogoCanvas.SetActive(false);
            StartSpeak = false;
        }
        //Salir del dialogo 2
        if (col.tag == "Dialogue2")
        {
            dialogoCanvas.SetActive(false);
            StartSpeak2 = false;
        }

        //Salir del dialogo 3
        if (col.tag == "Dialogue3")
        {
            dialogoCanvas.SetActive(false);
            StartSpeak3 = false;
        }

        //Salir del dialogo 4
        if (col.tag == "Dialogue4")
        {
            dialogoCanvas.SetActive(false);
            StartSpeak4 = false;
        }

        //Salir del dialogo 5
        if (col.tag == "Dialogue5")
        {
            dialogoCanvas.SetActive(false);
            StartSpeak5 = false;
        }

        //Salir del dialogo 6
        if (col.tag == "Dialogue6")
        {
            dialogoCanvas.SetActive(false);
            StartSpeak6 = false;
        }

        //Salir del dialogo 7
        if (col.tag == "Dialogue7")
        {
            dialogoCanvas.SetActive(false);
            StartSpeak7 = false;
        }

        //Salir del dialogo 8
        if (col.tag == "Dialogue8")
        {
            dialogoCanvas.SetActive(false);
            StartSpeak8 = false;
        }

        //Salir del dialogo 9
        if (col.tag == "Dialogue9")
        {
            dialogoCanvas.SetActive(false);
            StartSpeak9 = false;
        }

        //Salir del dialogo 10
        if (col.tag == "Dialogue10")
        {
            dialogoCanvas.SetActive(false);
            StartSpeak10 = false;
        }
    }
}
