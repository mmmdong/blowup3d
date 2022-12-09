using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    protected BallController ball;
    protected RingController ring;
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("ball"))
        {
            ball = collision.gameObject.GetComponent<BallController>();

            Vector2 income = ball.MovePos; // 입사벡터
            Vector2 normal = collision.contacts[0].normal; // 법선벡터
            ball.MovePos = Vector2.Reflect(income, normal).normalized; // 반사벡터

        }
        /* else if (collision.gameObject.tag.Equals("ring"))
        {
            ring = collision.gameObject.GetComponent<RingController>();

            Vector2 income = ring.MovePos; // 입사벡터
            Vector2 normal = collision.contacts[0].normal; // 법선벡터
            ring.MovePos = Vector2.Reflect(income, normal).normalized; // 반사벡터
        } */
    }
}
