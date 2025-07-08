using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonController : MonoBehaviour
{
    [SerializeField] int _priorityOrder;
    private void Start()
    {
        BackButtonManager._instance._RegisterPanel(gameObject, _priorityOrder);
    }
    private void OnDestroy()
    {
        if (BackButtonManager._instance != null) // to avoid error on game exit
        {
            BackButtonManager._instance._UnRegisterPanel(gameObject);
        }
    }
}