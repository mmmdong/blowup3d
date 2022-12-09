using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnNumButton : MonoBehaviour
{
    [SerializeField] private Button[] btns;
    [SerializeField] private ButtonAddBalls btn;

    private void Awake()
    {
        for (var i = 0; i < btns.Length; i++)
        {
            var temp = i;
            btns[temp].onClick.AddListener(() =>
            {
                btn._startGrade = temp + 1;
                btn._btn.onClick.Invoke();
            });
        }

        var asd = btns[0].GetComponent<RectTransform>();
    }
}
