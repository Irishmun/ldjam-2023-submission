using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeOutScript : MonoBehaviour
{
    [SerializeField] private UnityEvent OnStart;
    [SerializeField] private UnityEvent OnEnd;

    public void OnStarted()
    {
        OnStart.Invoke();
    }

    public void OnEnded()
    {
        OnEnd.Invoke();
        Cursor.lockState = CursorLockMode.Confined;
    }
}
