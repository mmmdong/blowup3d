using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    #region Singleton
    public static PopupManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private GameObject[] _popupList;
    
    private GameObject _activatedPopup;

    public void ActivePopup(bool active, Enums.PopupName popupName = Enums.PopupName.None)
    {
        if(active == false)
        {
            _activatedPopup.SetActive(active);
            return;
        }

        var name = popupName.ToString();

        for (var i = 0; i < _popupList.Length; i++)
        {
            if(_popupList[i].name == name)
            {
                _activatedPopup = _popupList[i];
                _popupList[i].SetActive(active);
                break;
            }
        }
    }
}
