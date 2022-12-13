using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MondayOFF;
using DG.Tweening;
using UnityEngine;

public class ButtonRvSpecialBallController : ButtonRVController
{
    public RingController _ring;

    protected override void RVAction()
    {
        base.RVAction();
        _btn.interactable = false;
        transform.DOLocalMoveX(_rectTrans.rect.width, 0.5f).SetRelative();
        _ring.gameObject.SetActive(true);
    }

    public override void InitButton()
    {
        base.InitButton();
        transform.DOLocalMoveX(-_rectTrans.rect.width, 0.5f).SetRelative().OnComplete(() =>
                {
                    _btn.interactable = true;
                });
    }
}
