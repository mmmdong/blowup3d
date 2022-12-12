using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using MondayOFF;

public class BallObjPool : MonoBehaviour
{
    public List<BallController> _spawnBallList = new List<BallController>();

    public static BallObjPool instance;
    public int _capacity;

    public Material[] _ballColor;
    [SerializeField] private GameObject _ballPref;
    [SerializeField] private bool _autoQueueGrow;

    private Queue<BallController> _ballQue = new Queue<BallController>();
    private Vector3 _defaultSpawnPos;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        _defaultSpawnPos = transform.position;
    }

    private void Start()
    {
        for (var i = 0; i < _capacity; i++)
        {
            AddBallToQueue();
        }
        GameManager.instance.isSpdUp.TakeUntilDestroy(this).Subscribe(SetActiveTrail);

        if (DataManager.instance.isDataExist)
        {
            SpawnDataBalls().Forget();
        }
        else
        {
            Spawn(1);
        }
    }

    private async UniTask SpawnDataBalls()
    {
        var balls = DataManager.instance.player.liveBallGrade;
        var fullLength = DataManager.instance.player.liveBalls;
        GameManager.instance._canAdd.Value = false;
        DataManager.instance.isCalling = true;

        await UniTask.WaitUntil(() => FXManager.instance != null);
        for (var i = 0; i < balls.Length; i++)
        {
            var ball = Spawn(balls[i]);
            ball.transform.position = new Vector3(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-1f, 1f), 0);
        }
        DataManager.instance.isCalling = false;
    }

    private void AddBallToQueue()
    {
        var ball = Instantiate(_ballPref, _defaultSpawnPos, Quaternion.identity, transform).GetComponent<BallController>();

        ball.gameObject.SetActive(false);
        ball.transform.eulerAngles = new Vector2(90f, 0);
        _ballQue.Enqueue(ball);
    }

    public BallController Spawn(int grade, Transform spawnPos = null)
    {
        if (_ballQue.Count == 0)
        {
            _capacity++;
            AddBallToQueue();
        }

        var ball = _ballQue.Dequeue();
        ball._collider.isTrigger = false;
        SetBall(ball, grade, spawnPos);

        //화면에 돌아다니고 있는 공 리스트
        _spawnBallList.Add(ball);
        GameManager.instance._curBallCount.Value++;
        var length = _spawnBallList.Count;
        DataManager.instance.player.liveBallGrade = new int[length];
        for (var i = 0; i < length; i++)
        {
            _spawnBallList[i]._index = i;
            DataManager.instance.player.liveBallGrade[i] = _spawnBallList[i]._grade;
        }



        //공이 생성된 후에 머지할 갯수가 되는지 체크
        //GameManager.instance._canMerge.Value = CanMergeCheck();
        return ball;
    }

    public void DestroyBall(BallController ball)
    {
        //큐브가 파괴되는 것이 아닌 풀로 돌려보냄
        ball.transform.position = Vector2.zero;
        ball._grade = 0;
        ball._index = 0;
        ball.gameObject.SetActive(false);
        _ballQue.Enqueue(ball);
        _spawnBallList.Remove(ball);
        GameManager.instance._curBallCount.Value--;
    }

    public bool MergeBalls()
    {
        var minGradeList = _spawnBallList.GroupBy(x => x._grade).OrderBy(x => x.Key).ToList();

        for (var i = 0; i < minGradeList.Count; i++)
        {
            if (minGradeList[i].Count() >= 3)
            {
                var mergeList = minGradeList[i].OrderBy(x => x._index).ToList();

                MergeManager.instance.StartMerge(mergeList);

                return true;
            }
            /* else
            {
                Debug.Log("cannot merge");
                continue;
            } */
        }
        return false;
    }

    public bool CanMergeCheck()
    {
        var minGradeList = _spawnBallList.GroupBy(x => x._grade).OrderBy(x => x.Key).ToList();

        for (var i = 0; i < minGradeList.Count; i++)
        {
            if (minGradeList[i].Count() >= 3)
            {
                if (minGradeList[i].Key == BallObjPool.instance._ballColor.Length)
                    return false;

                return true;
            }
            /* else
            {
                Debug.Log("cannot merge");
                continue;
            } */
        }
        return false;
    }

    private void SetActiveTrail(bool isOn)
    {
        for (var i = 0; i < _spawnBallList.Count; i++)
        {
            var ball = _spawnBallList[i];
            ball._trail.enabled = isOn;
        }
    }

    private async void SetBall(BallController ball, int grade, Transform spawnPos = null)
    {
        ball.gameObject.SetActive(true);
        if (spawnPos == null)
        {
            ball.StartBall();
            ball.transform.position = transform.position;
        }
        else
            ball.transform.position = spawnPos.position;

        ball._grade = grade;


        if (grade == 0)
        {
            grade = 1;
            ball._grade = grade;
        }

        ball._ballMesh.material = _ballColor[grade - 1];
        ball._ballColor = _ballColor[grade - 1].color;
        var trailColor = _ballColor[grade - 1].color;

        ball._trail.startColor = new Color(trailColor.r, trailColor.g, trailColor.b, 1f);
        ball._trail.endColor = new Color(trailColor.r, trailColor.g, trailColor.b, 0f);
        ball._trail.enabled = GameManager.instance.isSpdUp.Value;

#if !UNITY_STANDALONE && !UNITY_EDITOR
        if (grade > GameManager.instance._bestGrade)
        {
            await UniTask.Delay(500);
            GameManager.instance._bestGrade = grade;
            DataManager.instance.player.bestGrade = grade;
            EventTracker.ClearStage(grade - 1);
            EventTracker.TryStage(grade);
            PopupManager.instance.ActivePopup(true, Enums.PopupName.NewBallPopup);
        }
#endif
    }
}



