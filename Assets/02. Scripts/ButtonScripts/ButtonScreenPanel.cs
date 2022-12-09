using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScreenPanel : ButtonController
{
    [HideInInspector] public float originSpeed;

    public float _curTime;

    private bool _auto;
    protected override async UniTask Awake()
    {
        await base.Awake();
        act = ClickAction;

    }

    protected override void Start()
    {
        originSpeed = GameManager.instance._originSpeed;
        StartCoroutine(CoSpeedUp());
    }

    protected override void ClickAction()
    {
        base.ClickAction();
        _curTime = 0.5f;
    }


#if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        //자동 스피드업
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_auto)
                _auto = true;
            else
            {
                _curTime = 0f;
                _auto = false;
            }
        }

        if (_auto)
            _curTime = 0.5f;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_btn.interactable)
                act.Invoke();
        }
    }

#endif

    private IEnumerator CoSpeedUp()
    {
        while (true)
        {
            _curTime -= Time.deltaTime;

            if (_curTime > 0f)
            {
                GameManager.instance._ballCurrentSpeed.Value = GameManager.instance._originSpeed * 2f;
                GameManager.instance.isSpdUp.Value = true;
            }
            else
            {
                _curTime = 0f;
                GameManager.instance._ballCurrentSpeed.Value = GameManager.instance._originSpeed;
                GameManager.instance.isSpdUp.Value = false;
            }

            yield return null;
        }
    }
}
