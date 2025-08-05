using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;


#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class BackButtonManager : Singleton_Abs<BackButtonManager>
{
    [SerializeField] bool _autoGameQuit = true;
    [SerializeField, ConditionalField(nameof(_autoGameQuit))] MessageBoxController _exitMsgBox;

    [Tooltip("if true => the game is closed after 2 backButtons")]
    [SerializeField] bool _isDoubleClickExit;

    [Tooltip("minimum delay before 2 clicks")]
    [SerializeField, ConditionalField(nameof(_isDoubleClickExit))]
    float _doubleClickExitDelay;

    [Tooltip("time before the first click is expired (match it with notification)")]
    [SerializeField, ConditionalField(nameof(_isDoubleClickExit))]
    float _clickExpireTime;

    List<_PanelsClass> _registeredPanels = new List<_PanelsClass>();
    bool _isFirstClickActive = false;
    Coroutine _exitTimerCoroutine;

    private void Start()
    {
        DontDestroyOnLoad(transform.root);
    }
    private void Update()
    {
        #region Old Input System
#if !UNITY_ANDROID || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _OnBackButtonPressed();
        }
#else
        if (Input.backButtonLeavesApp)
        {
            _OnBackButtonPressed();
        }
#endif
#endregion
    }
    #region New Input System Event
#if ENABLE_INPUT_SYSTEM
    public void _OnBackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _OnBackButtonPressed();
        }
    }
#endif
#endregion
    public void _OnBackButtonPressed()
    {
        // Find the first active panel (highest priority since list is sorted)
        for (int i = 0; i < _registeredPanels.Count; i++)
        {
            if (_registeredPanels[i]._panel != null && _registeredPanels[i]._panel.activeInHierarchy)
            {
                _registeredPanels[i]._panel.SetActive(false);
                _registeredPanels[i]._optionalEvent?.Invoke();
                return;
            }
        }

        // No active panels found, handle exit logic
        if (_isDoubleClickExit && _autoGameQuit)
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
    public void _RegisterPanel(GameObject iPanel, int iOrder,UnityEvent iEvent)
    {
        _registeredPanels.Add(new _PanelsClass(iPanel, iOrder, iEvent));
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
        public UnityEvent _optionalEvent;

        public _PanelsClass(GameObject iPanel, int iOrder, UnityEvent optionalEvent)
        {
            _panel = iPanel;
            _priorityOrder = iOrder;
            _optionalEvent = optionalEvent;
        }
    }
}