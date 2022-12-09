using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [HideInInspector] public Vector3 MovePos; // 이동벡터

    public Color _ballColor;
    public MeshRenderer _ballMesh;
    public TrailRenderer _trail;
    public Collider _collider;
    public int _grade;
    public int _index;
    private Coroutine _curCoroutine;

    public void StartBall()
    {
        var x = Random.Range(0, 2);
        var y = Random.Range(0, 2);

        if (x == 0)
            x = -1;
        else
            x = 1;

        if (y == 0)
            y = -1;
        else
            y = 1;

        MovePos = new Vector2(x, y).normalized;


        _curCoroutine = StartCoroutine(CoMove());
    }

    public void InitializeBall()
    {
        StopCoroutine(_curCoroutine);
        _curCoroutine = null;
    }


    private IEnumerator CoMove()
    {
        while (true)
        {
            transform.position += MovePos * GameManager.instance._ballCurrentSpeed.Value * Time.deltaTime; 
            yield return null;
        }
    }

    public void StartMoveTo(Vector3 targetPos)
    {
        _collider.isTrigger = true;
        StartCoroutine(CoMove(targetPos));
    }

    private IEnumerator CoMove(Vector3 targetPos)
    {
        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, GameManager.instance._originSpeed * 10f * Time.deltaTime);

            yield return null;
        }
        _collider.isTrigger = false;
    }
}
