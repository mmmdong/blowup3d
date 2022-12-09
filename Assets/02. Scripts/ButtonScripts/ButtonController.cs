using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;


[Serializable]
public class ButtonController : MonoBehaviour
{
    public Button _btn;

    public Text _cost;
    public long _costValue;
    [HideInInspector] public long _nextValue;
    public long _buttonLev = 0;
    public Action act;
    protected virtual async UniTask Awake()
    {
        await UniTask.WaitUntil(() => GameManager.instance != null && DataManager.instance != null);
        _btn.onClick.AddListener(() =>
        {
            act?.Invoke();
        });
        _nextValue = _costValue;
    }

    virtual protected void Start()
    {
        //pass
    }

    virtual public void ButtonSetting()
    {
        _cost.text = $"$ {GameManager.instance.ToCurrencyString(_costValue)}";
    }

    virtual protected void ClickAction()
    {
        //pass
        Haptic(HapticTypes.MediumImpact);
    }

    private void Haptic(HapticTypes type)
{
        MMVibrationManager.Haptic(type);
}
}
