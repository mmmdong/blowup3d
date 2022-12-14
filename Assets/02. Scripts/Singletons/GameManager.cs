using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    /* [HideInInspector] */ public BoolReactiveProperty _canMerge = new BoolReactiveProperty(false);
    [HideInInspector] public BoolReactiveProperty _canAdd = new BoolReactiveProperty(false);
    [HideInInspector] public BoolReactiveProperty _canIncreaseSpeed = new BoolReactiveProperty(false);
    [HideInInspector] public BoolReactiveProperty _canIncreaseLevel = new BoolReactiveProperty(false);
    public FloatReactiveProperty _ballCurrentSpeed = new FloatReactiveProperty(10);
    public IntReactiveProperty _curBallCount = new IntReactiveProperty(0);
    public IntReactiveProperty _blowCount = new IntReactiveProperty(0);
    public LongReactiveProperty _currentCurrency = new LongReactiveProperty(0);
    public BoolReactiveProperty isSpdUp = new BoolReactiveProperty(false);

    public int _bestGrade;
    private readonly string[] CurrencyUnits = new string[] { "", "K", "M", "G", "T", "P", "E", "Z", "Y", };

    public Text _currencyTxt;
    public Text _blowTxt;

    public float _originSpeed;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
    }


    private void Start()
    {
        if (DataManager.instance.isDataExist)
        {
            _currentCurrency.Value = DataManager.instance.player.money;
            _blowCount.Value = DataManager.instance.player.blowCount;
            //_ballFullCount.Value = DataManager.instance.player.totalBallCount;
        }
        else
        {
            DataManager.instance.player.money = _currentCurrency.Value;
            DataManager.instance.player.blowCount = _blowCount.Value;
            //DataManager.instance.player.totalBallCount = _ballFullCount.Value;
        }

        _currencyTxt.text = $"${ToCurrencyString(_currentCurrency.Value)}";

        ReactiveButtonSubscribe();
    }


    public void IncreaseCurrency(long currencyValue)
    {
        _currentCurrency.Value += currencyValue;
    }
    public void DecreaseCurrency(long currencyValue)
    {
        _currentCurrency.Value -= currencyValue;
    }

    public void IncreaseTextEffect(Text text)
    {
        StartCoroutine(CoIncrease(text));
    }

    private void ReactiveButtonSubscribe()
    {
        _canAdd.TakeUntilDestroy(this).Subscribe(x =>
               {
                   ButtonManager.instance._addBall._btn.interactable = x;
               });
        _canMerge.TakeUntilDestroy(this).Subscribe(x =>
        {
            ButtonManager.instance._merge._btn.interactable = x;
        });
        _canIncreaseLevel.TakeUntilDestroy(this).Subscribe(x =>
        {
            ButtonManager.instance._increaseValue._btn.interactable = x;
        });

        _canIncreaseSpeed.TakeUntilDestroy(this).Subscribe(x =>
        {
            ButtonManager.instance._increaseCount._btn.interactable = x;
        });
        
        _blowCount.TakeUntilDestroy(this).Subscribe(x=>{
            DataManager.instance.player.blowCount = x;
            _blowTxt.text = ToCurrencyString(x);
            IncreaseTextEffect(_blowTxt);
        });
    }


    private IEnumerator CoIncrease(Text text)
    {
        text.transform.DOScale(1.3f, 0.05f);
        yield return new WaitForSeconds(0.05f);
        text.transform.DOScale(1f, 0.05f);
    }



    /// <summary>
    /// double ??? ???????????? ????????? ????????? ?????? ????????? ??????
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public string ToCurrencyString(double number)
    {
        string zero = "0";

        if (-1d < number && number < 1d)
        {
            return zero;
        }

        if (double.IsInfinity(number))
        {
            return "Infinity";
        }

        //  ?????? ?????? ?????????
        string significant = (number < 0) ? "-" : string.Empty;

        //  ????????? ??????
        string showNumber = string.Empty;

        //  ?????? ?????????
        string unityString = string.Empty;

        //  ????????? ????????? ????????? ?????? ????????? ?????? ??????????????? ????????? ??? ??????
        string[] partsSplit = number.ToString("E").Split('+');

        //  ??????
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  ?????? (????????? ??????)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  ?????? ????????? ?????????
        int quotient = exponent / 3;

        //  ???????????? ????????? ????????? ????????? ??????(10??? ??????????????? ??????)
        int remainder = exponent % 3;

        //  1A ????????? ?????? ??????
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate(number).ToString();
        }
        else
        {
            //  10??? ??????????????? ????????? ????????? ???????????? ????????? ??????.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  ?????? ????????????????????? ????????????.
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }

}
