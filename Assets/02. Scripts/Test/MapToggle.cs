using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MapToggle : MonoBehaviour
{
    [SerializeField] private Button[] btns;
    [SerializeField] private Sprite[] backGrounds;
    [SerializeField] private SpriteRenderer bg;

    private void Awake()
    {
        for (var i = 0; i < btns.Length; i++)
        {
            var chosen = i;
            btns[chosen].onClick.AddListener(() => {
                bg.sprite = backGrounds[chosen];
            });
        }

    }
}
