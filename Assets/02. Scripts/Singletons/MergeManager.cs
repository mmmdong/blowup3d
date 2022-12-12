using MondayOFF;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class MergeManager : MonoBehaviour
{
    #region SingleTon
    public static MergeManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

    }
    #endregion

    public Transform[] _objArr = new Transform[3];
    public float _speed;
    public Vector3[] _pos = new Vector3[3];

    Coroutine _coRot;

    private bool[] _isCome = new bool[3];
    private void Start()
    {
        for (var i = 0; i < _objArr.Length; i++)
        {
            _pos[i] = _objArr[i].position;
            _objArr[i] = null;
        }
    }

    public void StartMerge(List<BallController> balls)
    {
        StartCoroutine(CoStartMerge(balls));
    }

    public IEnumerator CoStartMerge(List<BallController> balls)
    {
        var ballArr = new BallController[_objArr.Length];
        for (var i = 0; i < _objArr.Length; i++)
        {
            ballArr[i] = balls[i];


            ballArr[i].InitializeBall();
            ballArr[i]._collider.isTrigger = true;
            _objArr[i] = ballArr[i].transform;

            if (ballArr[i]._trail.enabled == false)
                ballArr[i]._trail.enabled = true;

            ballArr[i].StartMoveTo(_pos[i]);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.1f);

        _coRot = StartCoroutine(CoRotate());

        yield return new WaitUntil(() => _coRot == null);

        StartCoroutine(CoMerge(ballArr));
    }

    private void Initialized()
    {
        for (var i = 0; i < _objArr.Length; i++)
        {
            _objArr[i].position = _pos[i];
            _objArr[i].rotation = Quaternion.identity;
        }
        _objArr = new Transform[3];
    }

    private IEnumerator CoRotate()
    {
        var moveSpeed = 200f * Time.deltaTime;
        var tempSpd = _speed;
        while (true)
        {
            for (var i = 0; i < _objArr.Length; i++)
            {
                tempSpd += 50f * Time.deltaTime; ;
                _objArr[i].RotateAround(transform.position, Vector3.back, tempSpd * Time.deltaTime);


                _objArr[i].position = Vector3.MoveTowards(_objArr[i].position, transform.position, moveSpeed * Time.deltaTime);
            }
            if (_objArr[0].position == transform.position)
                break;
            yield return null;
        }
        Initialized();
        StopCoroutine(_coRot);
        _coRot = null;
    }

    private IEnumerator CoMerge(BallController[] balls)
    {
        var grade = balls[0]._grade + 1;

        for (var i = 0; i < balls.Length; i++)
        {
            BallObjPool.instance.DestroyBall(balls[i]);
            BallObjPool.instance._spawnBallList.Remove(balls[i]);
            ButtonManager.instance._increaseCount.ReturnColor();
        }
        var ball = BallObjPool.instance.Spawn(grade, transform);
        FXManager.instance.PlayParticle(transform.position, Enums.ParticleName.Firework);

#if !UNITY_STANDALONE && !UNITY_EDITOR
        AdsManager.ShowInterstitial();
#endif

        ball.transform.DOScale(1.5f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        ball.transform.DOScale(0.8f, 0.1f);

        yield return new WaitForSeconds(0.5f);
        ball.StartMoveTo(BallObjPool.instance.transform.position);

        ball._trail.enabled = true;
        yield return new WaitForSeconds(0.5f);
        ball._trail.enabled = GameManager.instance.isSpdUp.Value;

        ball.StartBall();
        ButtonManager.instance._merge._btn.interactable = GameManager.instance._canMerge.Value;
    }

}
