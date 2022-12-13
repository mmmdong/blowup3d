using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRvValue2xController : ButtonRVController
{
    public float usedTime;
    [SerializeField] private GameObject back;
    [SerializeField] private Image gauge;

    protected override void RVAction()
    {
        base.RVAction();
        _btn.interactable = false;
        back.SetActive(true);
        StartCoroutine(CoGauge());
    }

    private IEnumerator CoGauge()
    {
        for (var i = 0; i < EnemyList.instance.enemies.Length; i++)
        {
            EnemyList.instance.enemies[i]._value *= 2;
        }

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

        for (var i = 0; i < EnemyList.instance.enemies.Length; i++)
        {
            EnemyList.instance.enemies[i]._value *= 0.5f;
        }

        back.SetActive(false);
    }
}
