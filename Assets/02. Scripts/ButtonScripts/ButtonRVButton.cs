using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ButtonRVButton : ButtonController
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


    private IEnumerator RVSpeedUp()
    {
        if (GameManager.instance._ballCurrentSpeed.Value == _originSpeed && BallObjPool.instance._spawnBallList.Count > 0)
        {
            GameManager.instance._ballCurrentSpeed.Value *= 3f;
            GameManager.instance.isSpdUp.Value = true;
            _btn.interactable = false;
            yield return new WaitForSeconds(3f);
            GameManager.instance._ballCurrentSpeed.Value = _originSpeed;
            GameManager.instance.isSpdUp.Value = false;
            _btn.interactable = true;
        }
    }
}
