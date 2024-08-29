using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrasManager : MonoBehaviour
{
    public List<GameObject> notas = new List<GameObject>();
    public List<GameObject> logros = new List<GameObject>();


    void Start()
    {
        for (int i = 0; i < notas.Count; i++)
        {
            notas[i].SetActive(false);
        }
        for (int i = 0; i < logros.Count; i++)
        {
            logros[i].SetActive(false);
        }
        SaveLoadGame.SaveContructor(notas.Count, logros.Count);
        LoadGame();
    }
    void Update()
    {

    }
    public void DebugNotas(int _index)
    {
        SaveNotas(notas[_index]);
        LoadGame();
    }
    public void DebugLogros(int _index)
    {
        SaveLogros(logros[_index]);
        LoadGame();
    }
    public void LoadGame()
    {
        SaveLoadGame.LoadGameNotas(notas.Count);
        for (int i = 0; i < SaveLoadGame.activeNotas.Count; i++)
        {
            notas[i].SetActive(SaveLoadGame.activeNotas[i]);
        }
        SaveLoadGame.LoadGameLogros(logros.Count);
        for (int i = 0; i < SaveLoadGame.activeLogros.Count; i++)
        {
            logros[i].SetActive(SaveLoadGame.activeLogros[i]);
        }
    }
    public void SaveNotas(GameObject _nota)
    {
        int tempIndex = GetNotasIndex(_nota);
        if (tempIndex != -1)
        {
            SaveLoadGame.SaveGameNotas(tempIndex, true);
        }
    }
    private int GetNotasIndex(GameObject _nota)
    {
        for (int i = 0; i < notas.Count; i++)
        {
            if (notas[i] == _nota) return i;
        }
        return -1;
    }
    public void SaveLogros(GameObject _logro)
    {
        int tempIndex = GetLogroIndex(_logro);
        if (tempIndex != -1)
        {
            SaveLoadGame.SaveGameLogros(tempIndex, true);
        }
    }
    private int GetLogroIndex(GameObject _logro)
    {
        for (int i = 0; i < logros.Count; i++)
        {
            if (logros[i] == _logro) return i;
        }
        return -1;
    }

}
