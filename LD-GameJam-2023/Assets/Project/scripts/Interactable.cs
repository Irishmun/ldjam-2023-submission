using System.Collections.Generic;
using Translations;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private const string NOT_READY_DIALOG = "INTERACT_NO_PACKAGES";

    [SerializeField] protected SpeakerDialog InteractionOverride;
    [SerializeField] private bool hasConversation = true;//unused, need to handle interacting with things that don't have dialog
    [SerializeField] private bool ignoreHasPackages = false;//unused, need to handle interacting with things that don't have dialog
    [SerializeField] protected List<SpeakerDialog> conversation;
    [SerializeField] private UnityEvent OnFinishedInteraction;
    [SerializeField] private DeliveredPackages thisNPC;

    private DialogUI _dialogUI;

    

    private void Awake()
    {
        _dialogUI = GameObject.FindObjectOfType<DialogUI>();
    }

    public void Interact()
    {
        if (ignoreHasPackages == false)
        {
            if (hasConversation == true && GlobalState.HAS_PACKAGES == true)
            {
                _dialogUI.ConversationInteract(this, conversation, 0);
            }
            else if (hasConversation == false && GlobalState.HAS_PACKAGES == true)
            {
                _dialogUI.TextInteract(this, InteractionOverride);
            }
            else
            {
                _dialogUI.TextInteract(this, new SpeakerDialog(NOT_READY_DIALOG));
            }
        }
        else
        {
            if (hasConversation == true)
            {
                _dialogUI.ConversationInteract(this, conversation, 0);
            }
            else
            {
                _dialogUI.TextInteract(this, InteractionOverride);
            }
        }
    }

    public void FinishInteraction()
    {
        if (ignoreHasPackages == true || GlobalState.HAS_PACKAGES == true)
        {
            OnFinishedInteraction.Invoke();
        }
    }

    public void DeliverPackage()
    {
        Debug.Log($"[{gameObject.name}]This NPC: {thisNPC}");
        if (GlobalState.DELIVERED_PACKAGES.HasFlag(thisNPC) == false)
        {
            GlobalState.DELIVERED_PACKAGES = (DeliveredPackages)((int)GlobalState.DELIVERED_PACKAGES + (int)thisNPC);
            Debug.Log("Delivered: " + GlobalState.DELIVERED_PACKAGES);
        }
    }
    public bool HasConversation { get => hasConversation; set => hasConversation = value; }
}

[System.Serializable]
public struct SpeakerDialog
{
    [SerializeField] private string _dialogID;
    public SpeakerDialog(string dialogID)
    {
        _dialogID = dialogID;
    }

    public string DialogID { get => _dialogID; set => _dialogID = value; }
}