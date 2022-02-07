using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkLog : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI log;

    public enum Color
    {
        red, yellow, green
    };

    private void Start()
    {
        SetLog("Conectando...", Color.yellow);
    }

    public void SetLog(string text, Color color)
    {
        switch (color)
        {
            case Color.red :
                log.text += "\n<color=#FF0000>" + text + "</color>";
                break;
            case Color.yellow:
                log.text += "\n<color=#DFFF00>" + text + "</color>";
                break;
            case Color.green:
                log.text += "\n<color=#00FF00>" + text + "</color>";
                break;
            default:
                log.text += "\n<color=#DFFF00>" + text + "</color>";
                break;
        }
    }
}
