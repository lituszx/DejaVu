using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public GameObject menuPrincipal, pressStart, menuPausa;
    public GameObject controllerPanels, extraPanels;
    public GameObject select, click, back;
    public MenuButon[] extraPanelImages;
    public GameObject panelNotas;
    public float contador = 0;

    private bool controlsVisibles = false;
    private Vector3 controllerImageInitialPosition;
    static public bool isPaused;

    private bool CanSpace;

    private void Awake()
    {
        CanSpace = true;

        Transform panel = panelNotas.transform;
        extraPanelImages = new MenuButon[panel.childCount];

        for (int i = 0; i < panel.childCount; i++)
        {
            extraPanelImages[i].obj = panel.GetChild(i).gameObject;
            extraPanelImages[i].initPos = extraPanelImages[i].obj.transform.localPosition;
        }
    }
    void Update()
    {
        contador += Time.deltaTime;
        //Tecla definida start
        if (CanSpace)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button9))
            {
                pressStart.SetActive(false);
                menuPrincipal.SetActive(true);
            }
        }


        /*
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            menuPausa.SetActive(true);
        }
        */

    }
    public void ActiveMenu(GameObject _menu)
    {
        menuPrincipal.SetActive(false);
        _menu.SetActive(true);
        CanSpace = false;
    }
    public void MainMenu(GameObject _menu)
    {
        if (!isPaused)
        {
            _menu.SetActive(false);
            menuPrincipal.SetActive(true);
            menuPausa.SetActive(false);
            CanSpace = true;
        }
        else
        {
            _menu.SetActive(false);
            menuPausa.SetActive(true);
            menuPrincipal.SetActive(false);
        }
    }
    public void MenuResume(GameObject _menu)
    {
        CanSpace = false;
        _menu.SetActive(false);
        isPaused = !isPaused;
    }
    public void PauseMainMenu()
    {
        CanSpace = false;
        menuPausa.SetActive(false);
        isPaused = false;
        menuPrincipal.SetActive(true);
    }
    public void PauseMovement(GameObject _menu)
    {
        CanSpace = false;
        _menu.SetActive(true);
        menuPausa.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void InitGame(int _scene)
    {
        SceneManager.LoadScene(_scene);
    }
    public void Select()
    {
        GameObject tempSound = Instantiate(select);
        Destroy(tempSound, 2);
    }
    public void PasCursor()
    {
        if (contador > 0.5f)
        {
            GameObject tempSound1 = Instantiate(click);
            Destroy(tempSound1, 2);
            contador = 0;
        }
    }
    public void MainMenu()
    {
        GameObject tempSound2 = Instantiate(back);
        Destroy(tempSound2, 2);
    }
    public void ActiveControl(int _control)
    {
        for (int i = 0; i < controllerPanels.transform.childCount; i++)
        {
            if (i == _control)
            {
                controllerPanels.transform.GetChild(i).gameObject.SetActive(!controllerPanels.transform.GetChild(i).gameObject.activeSelf);
            }
            else
            {
                controllerPanels.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void ActiveExtraPanels(int _control)
    {
        for (int i = 0; i < extraPanels.transform.childCount; i++)
        {
            if (i == _control)
            {
                extraPanels.transform.GetChild(i).gameObject.SetActive(!extraPanels.transform.GetChild(i).gameObject.activeSelf);
            }
            else
            {
                extraPanels.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private Coroutine ButtonsCoroutine;

    public void ShowController(int index)
    {

        if (ButtonsCoroutine == null)
        {
            controlsVisibles = !controlsVisibles;
            extraPanelImages[index].obj.SetActive(true);
            if (controlsVisibles)
            {
                HideImages(index, false);
            }

            if (controlsVisibles)
            {
                t = 0;
                ButtonsCoroutine = StartCoroutine(MoveUIButtons(extraPanelImages[index], extraPanelImages[index].obj.transform.localPosition, Vector3.zero, index));


            }
            else
            {

                t = 0;
                ButtonsCoroutine = StartCoroutine(MoveUIButtons(extraPanelImages[index], extraPanelImages[index].obj.transform.localPosition, extraPanelImages[index].initPos, index));


            }
        }
    }
    float t = 0;
    IEnumerator MoveUIButtons(MenuButon button, Vector3 initPos, Vector3 endPos, int index)
    {
        t = Mathf.Clamp(t + Time.deltaTime, 0, 1f);

        yield return new WaitForEndOfFrame();
        button.obj.transform.localPosition = Vector3.Lerp(initPos, endPos, t);

        Debug.Log(t);

        if (controlsVisibles)
            button.obj.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(4f, 4f, 4f), t);
        else
            button.obj.transform.localScale = Vector3.Lerp(new Vector3(4f, 4f, 4f), Vector3.one, t);

        if (t < 1)
            StartCoroutine(MoveUIButtons(button, initPos, endPos, index));
        else
            HideImages(index, !controlsVisibles);

        if (t == 1)
        {
            ButtonsCoroutine = null;
        }
    }
    private void HideImages(int index, bool show)
    {
        for (int i = 0; i < extraPanelImages.Length; i++)
        {
            if (i != index)
                extraPanelImages[i].obj.SetActive(show);
        }
    }
}
[System.Serializable]
public struct MenuButon
{
    public GameObject obj;
    public Vector3 initPos;
}