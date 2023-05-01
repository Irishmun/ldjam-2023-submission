using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonEnabler : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().enabled = false;
    }

    public void EnableButton()
    {
        GetComponent<Button>().enabled = true;
    }
}
