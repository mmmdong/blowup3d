using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ButtonCheatButton : ButtonController
{
    private float _originSpeed;

    [SerializeField] private long _value;

    protected override async UniTask Awake()
    {
        await base.Awake();

        act = ClickAction;
    }

    protected override void ClickAction()
    {
        base.ClickAction();
        GameManager.instance.IncreaseCurrency(_value);
        //StartCoroutine(RVSpeedUp());
    }

    protected override void Start()
    {
        _originSpeed = GameManager.instance._ballCurrentSpeed.Value;
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        //재화 치트
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            act.Invoke();
        }
    }
#endif


    
}
