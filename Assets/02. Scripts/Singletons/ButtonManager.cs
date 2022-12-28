using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;

    [Header("Buttons")]
    public ButtonAddBalls _addBall;
    public ButtonIncreaseBallSpeed _increaseCount;
    public ButtonIncreaseEnemyValue _increaseValue;
    public ButtonMergeBalls _merge;

    private void Awake()
    {
        if (instance == null)
            instance = this;



        GameManager.instance._currentCurrency.TakeUntilDestroy(this).Subscribe(x =>
                {
                    GameManager.instance._currencyTxt.text = $"${GameManager.instance.ToCurrencyString(x)}";
                    GameManager.instance.IncreaseTextEffect(GameManager.instance._currencyTxt);
                    DataManager.instance.player.money = x;



                    if (x >= _addBall._costValue)
                    {
                        GameManager.instance._canAdd.Value = true;
                    }
                    else if (x < _addBall._costValue)
                    {
                        GameManager.instance._canAdd.Value = false;
                    }
                    if (x >= _increaseValue._costValue && DataManager.instance.player.level <= _increaseValue._togs.toggles.Length)
                    {
                        GameManager.instance._canIncreaseLevel.Value = true;
                    }
                    else
                    {
                        GameManager.instance._canIncreaseLevel.Value = false;
                    }
                    if (x >= _increaseCount._costValue)
                    {
                        GameManager.instance._canIncreaseSpeed.Value = true;
                    }
                    else
                    {
                        GameManager.instance._canIncreaseSpeed.Value = false;
                    }
                    if (x >= _merge._costValue)
                    {
                        if (BallObjPool.instance.CanMergeCheck())
                            GameManager.instance._canMerge.Value = true;
                        else
                            GameManager.instance._canMerge.Value = false;

                    }
                    else
                    {
                        GameManager.instance._canMerge.Value = false;
                    }
                });
    }
}
