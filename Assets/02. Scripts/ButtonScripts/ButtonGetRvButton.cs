using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MondayOFF;

public class ButtonGetRvButton : ButtonController
{

    protected override void Start()
    {
        act = () =>
        {
            AdsManager.ShowRewarded(ClickAction);
        };
    }

    protected override void ClickAction()
    {
        base.ClickAction();

        BallObjPool.instance.Spawn(GameManager.instance._bestGrade);
        FXManager.instance.PlayParticle(BallObjPool.instance.transform.position, Color.yellow, Enums.ParticleName.Spawn);

        PopupManager.instance.ActivePopup(false);
    }
}
