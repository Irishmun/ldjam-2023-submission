using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeliveryChecklistUI : MonoBehaviour
{
    [SerializeField] private GameObject BobCheck;
    [SerializeField] private GameObject DaveCheck;
    [SerializeField] private GameObject FredCheck;
    [SerializeField] private GameObject JimCheck;
    [SerializeField] private GameObject GreggCheck;
    [SerializeField] private GameObject FishermanCheck;
    [SerializeField] private GameObject ConductorCheck;

    [SerializeField] private UnityEvent OnCheckListComplete;

    private void Awake()
    {
        DisableAll();
    }
    public void UpdateChecklist()
    {
        Array delivered = Enum.GetValues(typeof(DeliveredPackages));
        foreach (DeliveredPackages check in delivered)
        {
            if ((GlobalState.DELIVERED_PACKAGES & check) == check)
            {
                switch (check)
                {
                    case DeliveredPackages.BOB:
                        BobCheck.SetActive(true);
                        break;
                    case DeliveredPackages.DAVE:
                        DaveCheck.SetActive(true);
                        break;
                    case DeliveredPackages.FRED:
                        FredCheck.SetActive(true);
                        break;
                    case DeliveredPackages.JIM:
                        JimCheck.SetActive(true);
                        break;
                    case DeliveredPackages.GREGG:
                        GreggCheck.SetActive(true);
                        break;
                    case DeliveredPackages.FISHERMAN:
                        FishermanCheck.SetActive(true);
                        break;
                    case DeliveredPackages.CONDUCTOR:
                        ConductorCheck.SetActive(true);
                        break;
                    case DeliveredPackages.NONE:
                    default:
                        DisableAll();
                        break;
                }
            }
        }
        Debug.Log((int)GlobalState.DELIVERED_PACKAGES);
        if ((int)GlobalState.DELIVERED_PACKAGES == 127)
        {
            OnCheckListComplete.Invoke();
        }
    }

    private void DisableAll()
    {
        BobCheck.SetActive(false);
        DaveCheck.SetActive(false);
        FredCheck.SetActive(false);
        JimCheck.SetActive(false);
        GreggCheck.SetActive(false);
        FishermanCheck.SetActive(false);
        ConductorCheck.SetActive(false);
    }
}
