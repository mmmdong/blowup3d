using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using MondayOFF;

public class ButtonIncreaseBallSpeed : ButtonController
{
    private Color _oriColor;
    protected override async UniTask Awake()
    {
        await base.Awake();
        /* GameManager.instance._ballCurrentSpeed.TakeUntilDestroy(this).Subscribe(x =>
                {
                    if (GameManager.instance.isSpdUp.Value == false)
                        DataManager.instance.player.speed = GameManager.instance._ballCurrentSpeed.Value * 0.5f;
                    else
                        DataManager.instance.player.speed = GameManager.instance._ballCurrentSpeed.Value;
                }); */
        GameManager.instance._curBallCount.TakeUntilDestroy(this).Subscribe(x =>
        {
            DataManager.instance.player.liveBalls = x;
        });

        if (GameManager.instance._currentCurrency.Value < _costValue)
            GameManager.instance._canIncreaseSpeed.Value = false;

        act = ClickAction;
    }

    protected override void Start()
    {
        base.Start();
        if (DataManager.instance.isDataExist)
        {
            _buttonLev = DataManager.instance.player.buttons.btnBallCountData.buttonLev;
            _costValue = DataManager.instance.player.buttons.btnBallCountData.costValue;
        }
        else
        {
            DataManager.instance.player.buttons.btnBallCountData.buttonLev = _buttonLev;
            DataManager.instance.player.buttons.btnBallCountData.costValue = _costValue;
        }
        ButtonSetting();

        if (_buttonLev >= 16)
        {
            _cost.text = "MAX";
            GameManager.instance._canIncreaseSpeed.Value = false;
        }
    }

    protected override void ClickAction()
    {
        base.ClickAction();
        AdsManager.ShowInterstitial();

        if (!GameManager.instance.isSpdUp.Value)
            GameManager.instance._ballCurrentSpeed.Value += 0.2f;
        else
        {
            GameManager.instance._ballCurrentSpeed.Value *= 0.5f;
            GameManager.instance._ballCurrentSpeed.Value += 0.2f;
        }

        GameManager.instance._originSpeed = GameManager.instance._ballCurrentSpeed.Value;

        _nextValue = _costValue;
        _nextValue = (long)Math.Pow(10, _buttonLev + 3);
        GameManager.instance.DecreaseCurrency(_costValue);
        _costValue = _nextValue;
        _buttonLev++;
        _cost.text = $"$ {GameManager.instance.ToCurrencyString(_costValue)}";

        DataManager.instance.player.buttons.btnBallCountData.buttonLev = _buttonLev;
        DataManager.instance.player.buttons.btnBallCountData.costValue = _costValue;

        if (GameManager.instance._currentCurrency.Value < _costValue)
            _btn.interactable = false;

        if (_buttonLev >= 16)
        {
            _cost.text = "MAX";
            GameManager.instance._canIncreaseSpeed.Value = false;
        }
    }
}
