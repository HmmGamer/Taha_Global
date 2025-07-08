using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BackButtonManager : Singleton_Abs<BackButtonManager>
{
    [SerializeField] MessageBoxController _exitMsgBox;

    [Tooltip("if true => the game is closed after 2 backButtons")]
    bool _isDoubleClickExit;

    [Tooltip("minimum delay before 2 clicks")]
    [SerializeField, ConditionalField(nameof(_isDoubleClickExit))]
    float _doubleClickExitDelay;

    [Tooltip("time before the first click is expired (match it with notification)")]
    [SerializeField, ConditionalField(nameof(_isDoubleClickExit))]
    float _clickExpireTime;

    [SerializeField, ReadOnly] List<_PanelsClass> _registeredPanels = new List<_PanelsClass>();
    bool _isFirstClickActive = false;
    Coroutine _exitTimerCoroutine;

    private void Start()
    {
        DontDestroyOnLoad(transform.root);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _OnBackButtonPressed();
        }
    }
    public void _OnBackButtonPressed()
    {
        // Find the first active panel (highest priority since list is sorted)
        for (int i = 0; i < _registeredPanels.Count; i++)
        {
            if (_registeredPanels[i]._panel != null && _registeredPanels[i]._panel.activeInHierarchy)
            {
                _registeredPanels[i]._panel.SetActive(false);
                return;
            }
        }

        // No active panels found, handle exit logic
        if (_isDoubleClickExit)
        {
            if (_isFirstClickActive)
            {
                // Second click within delay time
                if (_exitTimerCoroutine != null)
                {
                    StopCoroutine(_exitTimerCoroutine);
                }
                Application.Quit();
            }
            else
            {
                // First click
                _isFirstClickActive = true;
                _exitTimerCoroutine = StartCoroutine(_ExitTimer());
            }
        }
        else
        {
            _exitMsgBox._StartMsg();
        }
    }
    IEnumerator _ExitTimer()
    {
        yield return new WaitForSeconds(_clickExpireTime);
        _isFirstClickActive = false;
        _exitTimerCoroutine = null;
    }
    public void _RegisterPanel(GameObject iPanel, int iOrder)
    {
        _registeredPanels.Add(new _PanelsClass(iPanel, iOrder));
        _registeredPanels = _registeredPanels.OrderByDescending(p => p._priorityOrder).ToList();
    }
    public void _UnRegisterPanel(GameObject iPanel)
    {
        for (int i = _registeredPanels.Count - 1; i >= 0; i--)
        {
            if (_registeredPanels[i]._panel == iPanel)
            {
                _registeredPanels.RemoveAt(i);
                break;
            }
        }
    }

    [System.Serializable]
    public class _PanelsClass
    {
        public GameObject _panel;
        public int _priorityOrder;

        public _PanelsClass(GameObject iPanel, int iOrder)
        {
            _panel = iPanel;
            _priorityOrder = iOrder;
        }
    }
}