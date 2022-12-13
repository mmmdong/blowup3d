using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RingController : MonoBehaviour
{
    public Vector3 originPos;
    public float speed;

    public float collTime;
    public EnemyCollisionController enemy;

    [SerializeField] private ButtonRvSpecialBallController _rv;
    private void Awake()
    {
        originPos = transform.position;
    }

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        transform.Rotate(Vector3.up, 5000f * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            enemy = other.GetComponent<EnemyCollisionController>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            collTime += Time.deltaTime;

            if (collTime >= 0.08f)
            {
                var value = enemy._value.ToString();
                FXManager.instance.PlayParticle(transform.position + Vector3.down, Enums.ParticleName.StunStarExplosion);
                EnemyList.instance.Spawn(value);

                enemy.Hit(long.Parse(value));
                GameManager.instance.IncreaseCurrency(long.Parse(value));
                collTime = 0f;
            }
        }
    }


    public void Init()
    {
        collTime = 0f;
        _rv.InitButton();
        transform.position = originPos;
        gameObject.SetActive(false);
    }
}
