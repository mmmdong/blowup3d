using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ButtonMergeBalls : ButtonController
{
    protected override async UniTask Awake()
    {
        await base.Awake();


        act = ClickAction;
    }

    protected override void Start()
    {
        base.Start();
        if (DataManager.instance.isDataExist)
        {
            _buttonLev = DataManager.instance.player.buttons.btnMergeBallData.buttonLev;
            _costValue = DataManager.instance.player.buttons.btnMergeBallData.costValue;
        }
        else
        {
            DataManager.instance.player.buttons.btnMergeBallData.buttonLev = _buttonLev;
            DataManager.instance.player.buttons.btnMergeBallData.costValue = _costValue;
        }
        ButtonSetting();
    }

    protected override void ClickAction()
    {
        base.ClickAction();

        _btn.interactable = false;
        BallObjPool.instance.MergeBalls();

        _nextValue = _costValue;
        _nextValue += 20 + _buttonLev * 15 * (DataManager.instance.player.level + 1);
        GameManager.instance.DecreaseCurrency(_costValue);
        _costValue = _nextValue;
        _buttonLev++;
        _cost.text = $"$ {GameManager.instance.ToCurrencyString(_costValue)}";

        DataManager.instance.player.buttons.btnMergeBallData.buttonLev = _buttonLev;
        DataManager.instance.player.buttons.btnMergeBallData.costValue = _costValue;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_btn.interactable && gameObject.activeSelf)
                act.Invoke();
        }
    }
#endif
}
