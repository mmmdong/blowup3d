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
    public Text _text;

    private Color _oriColor;
    protected override async UniTask Awake()
    {
        await base.Awake();
        _oriColor = _text.color;
        GameManager.instance._ballCurrentSpeed.TakeUntilDestroy(this).Subscribe(x =>
                {
                    _text.text = string.Format("Speed {0:0.0}", x);
                    if (GameManager.instance.isSpdUp.Value == false)
                        DataManager.instance.player.speed = GameManager.instance._ballCurrentSpeed.Value * 0.5f;
                    else
                        DataManager.instance.player.speed = GameManager.instance._ballCurrentSpeed.Value;
                });
        GameManager.instance._curBallCount.TakeUntilDestroy(this).Subscribe(x =>
        {
            DataManager.instance.player.liveBalls = x;

            GameManager.instance.IncreaseTextEffect(_text);
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
        if (_text.color == Color.red)
            _text.color = _oriColor;

        if (!GameManager.instance.isSpdUp.Value)
            GameManager.instance._ballCurrentSpeed.Value += 0.2f;
        else
        {
            GameManager.instance._ballCurrentSpeed.Value *= 0.5f;
            GameManager.instance._ballCurrentSpeed.Value += 0.2f;
        }

        GameManager.instance._originSpeed = GameManager.instance._ballCurrentSpeed.Value;

        DataManager.instance.player.speed = GameManager.instance._ballCurrentSpeed.Value;

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

    public void ReturnColor()
    {
        _text.color = _oriColor;
    }
}
