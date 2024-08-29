using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadGame : MonoBehaviour
{
    static public List<bool> activeNotas = new List<bool>();
    static public List<bool> activeLogros = new List<bool>();

    static public void SaveContructor(int _notas, int _logros)
    {
        for (int i = 0; i < _notas; i++)
        {
            activeNotas.Add(false);
        }
        for (int i = 0; i < _logros; i++)
        {
            activeLogros.Add(false);
        }
    }
    static public void SaveGameNotas(int _nota, bool _active)
    {
        activeNotas[_nota] = _active;
        PlayerPrefsX.SetBoolList("activeNotas", activeNotas);
    }
    static public void SaveGameLogros(int _logro, bool _active)
    {
        activeLogros[_logro] = _active;
        PlayerPrefsX.SetBoolList("activeLogros", activeLogros);
    }
    static public void LoadGameNotas(int _notas)
    {
        if (PlayerPrefs.HasKey("activeNotas"))
            activeNotas = PlayerPrefsX.GetBoolList("activeNotas");
        else
        {
            activeNotas = new List<bool>();
            for (int i = 0; i < _notas; i++)
            {
                activeNotas.Add(false);
            }
        }
    }
    static public void LoadGameLogros(int _logros)
    {
        if (PlayerPrefs.HasKey("activeLogros"))
            activeLogros = PlayerPrefsX.GetBoolList("activeLogros");
        else
        {
            activeLogros = new List<bool>();
            for (int i = 0; i < _logros; i++)
            {
                activeLogros.Add(false);
            }
        }
    }
    static public void DeleteData(string _key)
    {
        PlayerPrefs.DeleteKey(_key);

    }
}
