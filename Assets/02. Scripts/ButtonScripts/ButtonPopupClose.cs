using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPopupClose : ButtonController
{

    protected override void Start()
    {
        base.Start();
        act = ClickAction;
    }

    protected override void ClickAction()
    {
        base.ClickAction();
        PopupManager.instance.ActivePopup(false);
    }
}
