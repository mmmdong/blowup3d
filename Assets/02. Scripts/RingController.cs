using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RingController : MonoBehaviour
{
    public Vector3 originPos;
    public Collider _collider;
    public float speed;
    public float _lifeTime;

    private bool isPassing;
    private float originSpeed;
    private void Awake()
    {
        originPos = transform.position;
        originSpeed = speed;
    }
    private void OnEnable()
    {
        StartBall();
    }

    public void StartBall()
    {
        StartCoroutine(CoMove());
    }


    private IEnumerator CoMove()
    {
        while (true)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            transform.Rotate(Vector3.up, 5000f * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator CoDrilling(EnemyCollisionController enemy)
    {
        var value = enemy._value.ToString();
        while (isPassing)
        {
            FXManager.instance.PlayParticle(transform.position + Vector3.down, Enums.ParticleName.StunStarExplosion);
            EnemyList.instance.Spawn(value);
            enemy.Hit().Forget();
            yield return new WaitForSeconds(0.08f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            var enemy = other.GetComponent<EnemyCollisionController>();
            
            isPassing = true;
            StartCoroutine(CoDrilling(enemy));
            
            speed = 3f;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("enemy"))
        {

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            speed = originSpeed;
            isPassing = false;
            StartCoroutine(CoInitialize());
        }
    }

    private IEnumerator CoInitialize()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = originPos;
        gameObject.SetActive(false);
    }
}
