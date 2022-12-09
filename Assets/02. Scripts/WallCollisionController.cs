using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionController : CollisionController
{
    private void Awake()
    {
        SettingWalls();
    }

    private void SettingWalls()
    {
        var pos = Camera.main.WorldToViewportPoint(transform.position);

        switch (gameObject.name)
        {
            case "Top":
                pos.x = 0.5f;
                pos.y = 1f;
                break;
            case "Bottom":
                pos.x = 0.5f;
                pos.y = 0.15f;
                break;
            case "Left":
                pos.x = 0f;
                pos.y = 0.5f;
                break;
            case "Right":
                pos.x = 1f;
                pos.y = 0.5f;
                break;
        }
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
