using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            var ball = other.GetComponent<BallController>();
            var grade = ball._grade;

            BallObjPool.instance.DestroyBall(ball);
            BallObjPool.instance.Spawn(grade);
        }
        else if (other.CompareTag("ring"))
        {
            var ring = other.GetComponent<RingController>();
        }
    }
}
