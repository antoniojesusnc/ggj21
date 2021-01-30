using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISillouettesContoller : MonoBehaviour
{
    private CollectableManager _collectableManager;

    [SerializeField]
    private List<UISillouettesContollerInfo> _sillouettesContollerInfos;
    void Start()
    {
        _collectableManager = FindObjectOfType<CollectableManager>();
        SetCollectables();
    }

    private void SetCollectables()
    {
        foreach (var collectable in _collectableManager.Collectables)
        {
            for (int i = 0; i < _sillouettesContollerInfos.Count; i++)
            {
                if (_sillouettesContollerInfos[i].type == collectable.Value.CollectableType)
                {
                    _sillouettesContollerInfos[i].gameObject.SetActive(true);
                }
            }
        }
    }

    public void SetSillouteAsComplete(ECollectableType type)
    {
        for (int i = 0; i < _sillouettesContollerInfos.Count; i++)
        {
            if (_sillouettesContollerInfos[i].type == type)
            {
                SetAsActive(_sillouettesContollerInfos[i]);
            }
        }
    }

    private void SetAsActive(UISillouettesContollerInfo sillouettesContollerInfo)
    {
        Image image = sillouettesContollerInfo.gameObject.GetComponentInChildren<Image>();
        if (image != null)
        {
            image.color = Color.white;
        }
    }
}

[System.Serializable]
public class UISillouettesContollerInfo
{
    public ECollectableType type;
    public GameObject gameObject;
}
