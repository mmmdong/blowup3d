using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyToggles : MonoBehaviour
{
    public Toggle[] toggles;

    [SerializeField] private SpriteRenderer bg;
    [SerializeField] private Sprite[] backGrounds;
    public int _lev;

    private void Awake()
    {
        for (var i = 0; i < toggles.Length; i++)
        {
            toggles[i].onValueChanged.AddListener(EnemyList.instance.array[i].SetActive);
        }

        _lev = DataManager.instance.player.level;
        bg.sprite = backGrounds[_lev % 6];
        toggles[_lev].isOn = true;
    }

    private void Start()
    {
    }

    public void NextEnemy()
    {

        if(_lev >= toggles.Length - 1)
        return;
        _lev++;
        bg.sprite = backGrounds[_lev % 6];
        toggles[_lev].isOn = true;
        DataManager.instance.player.level = _lev;
    }
}
