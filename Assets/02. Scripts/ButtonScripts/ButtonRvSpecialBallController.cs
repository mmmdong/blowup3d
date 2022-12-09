using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ButtonRvSpecialBallController : ButtonController
{
    public RingController _ring;

    protected override async UniTask Awake()
    {
        await base.Awake();
        act = ClickAction;
    }

    protected override void ClickAction()
    {
        base.ClickAction();

        _ring.gameObject.SetActive(true);
    }
}
