using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        Screen.SetResolution(1242, 2688, false);
#endif
    }
}
