using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MondayOFF;
using UniRx;
using UnityEngine;

public class ButtonIncreaseEnemyValue : ButtonController
{
    public EnemyToggles _togs;


    protected override void Start()
    {
        base.Start();
        _btn.interactable = false;
        
        if (DataManager.instance.isDataExist)
        {
            _buttonLev = DataManager.instance.player.buttons.btnLevUpData.buttonLev;
            _costValue = DataManager.instance.player.buttons.btnLevUpData.costValue;
        }
        else
        {
            DataManager.instance.player.buttons.btnLevUpData.buttonLev = _buttonLev;
            DataManager.instance.player.buttons.btnLevUpData.costValue = _costValue;
        }

        ButtonSetting();
        act = ClickAction;
        _btn.interactable = DataManager.instance.player.money >= _costValue && _togs._lev < _togs.toggles.Length;

        if (_togs._lev >= _togs.toggles.Length - 1)
        {
            _cost.text = "MAX";
        }
        EnemyList.instance.currentLev = (int)_buttonLev - 1;

        if (GameManager.instance._bestGrade >= 2)
        {
            MoveButtonPos();
        }
    }

    protected override void ClickAction()
    {
        base.ClickAction();
        AdsManager.ShowInterstitial();
        FXManager.instance.PlayParticle(FXManager.instance.transform.position, Enums.ParticleName.Explosion);
        FXManager.instance.PlayParticle(FXManager.instance.transform.position, Enums.ParticleName.StunExplosion);
        StartCoroutine(CoLevUp());
        _nextValue = _costValue;
        _nextValue += 10 * _nextValue;
        GameManager.instance.DecreaseCurrency(_costValue);
        _costValue = _nextValue;
        _buttonLev++;
        _cost.text = $"$ {GameManager.instance.ToCurrencyString(_costValue)}";

        DataManager.instance.player.buttons.btnLevUpData.buttonLev = _buttonLev;
        DataManager.instance.player.buttons.btnLevUpData.costValue = _costValue;

        if (GameManager.instance._currentCurrency.Value < _costValue)
        {
            GameManager.instance._canIncreaseLevel.Value = false;
        }

        _togs.NextEnemy();
        EnemyList.instance.enemies[EnemyList.instance.currentLev].FullHp();
        if (_togs._lev >= _togs.toggles.Length - 1)
        {
            _cost.text = "MAX";
            GameManager.instance._canIncreaseLevel.Value = false;
            transform.DOLocalMoveX(300f, 0.5f).SetRelative().OnComplete(() =>
        {
            if (GameManager.instance._canIncreaseLevel.Value)
                _btn.interactable = false;
        });
        }
    }

    private IEnumerator CoLevUp()
    {
        EnemyList.instance.currentLev = (int)_buttonLev;
        EnemyList.instance.transform.DOScale(1.2f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        EnemyList.instance.transform.DOScale(1f, 0.1f);
    }

    public void MoveButtonPos()
    {
        transform.DOLocalMoveX(-300f, 0.5f).SetRelative().OnComplete(() =>
        {
            if (GameManager.instance._canIncreaseLevel.Value)
                _btn.interactable = true;
        });
    }
}
