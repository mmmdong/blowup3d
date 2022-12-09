using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallPopupController : MonoBehaviour
{
    [SerializeField] private BallController _renderBall;
    [SerializeField] private ParticleSystem[] _particles;
    private void OnEnable()
    {
        _renderBall._ballMesh.material = BallObjPool.instance._ballColor[GameManager.instance._bestGrade - 1];
        for (var i = 0; i < _particles.Length; i++)
        {
            _particles[i].Play();
        }
    }

    private void OnDisable() {
        for (var i = 0; i < _particles.Length; i++)
        {
            _particles[i].Stop();
        }
    }
}
