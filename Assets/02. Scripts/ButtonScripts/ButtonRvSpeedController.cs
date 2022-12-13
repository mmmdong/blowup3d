using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRvSpeedController : ButtonRVController
{
    public float usedTime;
    [SerializeField] private GameObject back;
    [SerializeField] private Image gauge;
    [SerializeField] ButtonScreenPanel panel;

    protected override void RVAction()
    {
        base.RVAction();
        _btn.interactable = false;
        back.SetActive(true);
        StartCoroutine(CoGauge());
    }

    private IEnumerator CoGauge()
    {
        panel._curTime = usedTime;
        panel._btn.interactable = false;
        while (true)
        {
            gauge.fillAmount -= 1f / usedTime * Time.deltaTime;
            
            if (gauge.fillAmount <= 0f)
                break;
            yield return null;
        }
        InitButton();
    }

    public override void InitButton()
    {
        base.InitButton();
        gauge.fillAmount = 1f;
        _btn.interactable = true;
        panel._btn.interactable= true;
        back.SetActive(false);
    }
}
