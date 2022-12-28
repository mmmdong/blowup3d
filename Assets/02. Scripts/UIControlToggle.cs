using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControlToggle : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] objs;

    private Toggle tog;

    private void Awake()
    {
        tog = GetComponentInChildren<Toggle>();
        tog.onValueChanged.AddListener(Action);
    }

    private void Action(bool isOn)
    {
        for(var i = 0; i < objs.Length; i++)
        {
            if (isOn)
                objs[i].alpha = 1f;
            else
                objs[i].alpha = 0f;
        }
    }
}
