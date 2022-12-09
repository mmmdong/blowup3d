using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class EnemyCollisionController : CollisionController
{
    private Vector3 _originPos;
    private Sequence mySequence;
    public float _value;
    private void Awake()
    {
        _originPos = transform.position;
    }

    private void OnEnable()
    {
        StartCoroutine(CoRotate());
    }


    IEnumerator CoRotate()
    {
        while (true)
        {
            var angles = transform.localRotation.eulerAngles;
            angles.y -= Time.deltaTime * 30f;
            transform.localRotation = Quaternion.Euler(angles);
            yield return null;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        Hit().Forget();

        var collidePos = collision.contacts[0].point;
        if (collision.gameObject.tag.Equals("ball"))
        {
            if (GameManager.instance.isSpdUp.Value)
            {
                FXManager.instance.PlayParticle(collidePos, ball._ballColor, Enums.ParticleName.Hit1);
            }
            else
            {
                FXManager.instance.PlayParticle(collidePos, ball._ballColor, Enums.ParticleName.Hit2);
            }
        }
        

        var value = (long)_value * Mathf.Pow(5, ball._grade - 1);
        var valueStr = $"{GameManager.instance.ToCurrencyString(value)}+";
        EnemyList.instance.Spawn(valueStr);

        GameManager.instance.IncreaseCurrency((long)value);
    }

    public async UniTask Hit()
    {
        if (GameManager.instance.isSpdUp.Value)
        {
            transform.DOScale(4.0f, 0.07f);
        }
        else
        {
            transform.DOScale(3.3f, 0.1f);
        }

        await UniTask.Delay(100);
        transform.DOScale(3f, 0.1f);
    }
}
