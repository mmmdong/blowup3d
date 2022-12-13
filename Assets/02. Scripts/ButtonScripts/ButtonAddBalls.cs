using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.IO;
using MondayOFF;

public class ButtonAddBalls : ButtonController
{
    public int _startGrade = 1;

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
            _buttonLev = DataManager.instance.player.buttons.btnAddBallData.buttonLev;

            _costValue = DataManager.instance.player.buttons.btnAddBallData.costValue;
        }
        else
        {
            DataManager.instance.player.buttons.btnAddBallData.buttonLev = _buttonLev;
            DataManager.instance.player.buttons.btnAddBallData.costValue = _costValue;
        }
        ButtonSetting();
    }

    protected override void ClickAction()
    {
        base.ClickAction();
//#if !UNITY_STANDALONE && !UNITY_EDITOR
        AdsManager.ShowInterstitial();
//#endif
        var ball = BallObjPool.instance.Spawn(_startGrade);

        FXManager.instance.PlayParticle(BallObjPool.instance.transform.position, Color.yellow, Enums.ParticleName.Spawn);


        _nextValue = _costValue;
        _nextValue += 20 + _buttonLev * 5 * (DataManager.instance.player.level + 1);
        GameManager.instance.DecreaseCurrency(_costValue);
        _costValue = _nextValue;
        _buttonLev++;
        _cost.text = $"$ {GameManager.instance.ToCurrencyString(_costValue)}";

        DataManager.instance.player.buttons.btnAddBallData.buttonLev = _buttonLev;
        DataManager.instance.player.buttons.btnAddBallData.costValue = _costValue;

        if (GameManager.instance._currentCurrency.Value < _costValue)
            GameManager.instance._canAdd.Value = false;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_btn.interactable)
                act.Invoke();
        }
    }
#endif
}
