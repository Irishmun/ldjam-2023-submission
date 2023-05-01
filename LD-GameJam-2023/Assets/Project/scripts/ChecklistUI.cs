using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecklistUI : MonoBehaviour
{
    [SerializeField] private GameObject IndicatorArrows;

    public void ShowArrows()
    {
        if (GlobalState.BRAM_MODE == true)
        {
            IndicatorArrows.SetActive(true);
        }
    }

    public void HideArrows()
    {
        if (GlobalState.BRAM_MODE == true)
        {
            IndicatorArrows.SetActive(false);
        }
    }
}
