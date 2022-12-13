using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using MondayOFF;

public class ButtonRVController : ButtonController
{
    protected RectTransform _rectTrans;
    protected override async UniTask Awake()
    {
        await base.Awake();
        _rectTrans = GetComponent<RectTransform>();
        act = () => RVClickAction(null);
    }

    protected virtual void RVClickAction(Action act = null)
    {
        //Pass
        AdsManager.ShowRewarded(RVAction);
    }

    protected virtual void RVAction()
    {
        //Pass
    }

    public virtual void InitButton()
    {
        //Pass
        
    }
}
