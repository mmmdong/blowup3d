using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyCollisionController : CollisionController
{
    private Vector3 _originPos;
    public float _value;
    public long _fullHp;
    public Image _hpBar;
    public long _currentHp;
    public bool _isDead;
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
        
        Hit((long)value);
    }

    public void Hit(long value)
    {
        DecreaseHp(value);

        if (_currentHp > 0)
        {
            transform.DOScale(4.0f, 0.07f).OnComplete(() =>
            {
                transform.DOScale(3f, 0.07f);
            });
        }
        else
        {
            _isDead = true;
            GameManager.instance._blowCount.Value++;
            EnemyList.instance.Spawn($"{GameManager.instance.ToCurrencyString(_fullHp * 5)}+");
            GameManager.instance.IncreaseCurrency((long)_fullHp * 5);
            FXManager.instance.PlayParticle(transform.position, Enums.ParticleName.StunExplosion);
            EnemyList.instance.BlowEnemy();
        }


    }

    public void FullHp()
    {
        _isDead = false;
        _hpBar.fillAmount = _fullHp / _fullHp;
        _currentHp = _fullHp;

        transform.DOScale(3f, 0.5f).SetEase(Ease.OutBounce).OnComplete(async () =>
        {
            await UniTask.Delay(100);
            col.isTrigger = false;
        });
    }

    private void DecreaseHp(long value)
    {
        _currentHp -= value;
        _hpBar.fillAmount = (float)_currentHp / (float)_fullHp;
    }    
}
