using System.Collections;
using System.Collections.Generic;
using Translations;
using UnityEngine;

public class DialogUI : MonoBehaviour
{
    private const float TIME_BEFORE_HIDE = 10f;

    [SerializeField] private List<SpeakerDialog> dialogOrder;
    [SerializeField] private LanguageText languageText;
    [SerializeField] private GameObject finishedIndicator;

    private GameObject _firstChild;
    private float _timeSinceFinished = 0;
    private Interactable _currentInteractedObject;
    private int _dialogIndex = 0;

    private void Awake()
    {
        if (transform.childCount == 0)
        { return; }
        _firstChild = transform.GetChild(0).gameObject;
        _firstChild.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_firstChild == null)
        { return; }
        if (_firstChild.activeSelf == true)
        {
            if (languageText.FinishedTyping == true)
            {
                finishedIndicator.SetActive(true);
                if (_timeSinceFinished >= TIME_BEFORE_HIDE)
                {
                    if (dialogOrder != null && dialogOrder.Count > 0 && _dialogIndex < dialogOrder.Count)
                    {
                        ProceedConversation();
                    }
                    else
                    {
                        FinishInteraction();
                    }
                }
                else
                {
                    _timeSinceFinished += Time.deltaTime;
                }
            }
        }
    }

    private void SetTextActive(bool active, string textId = default)
    {
        _firstChild.SetActive(active);
        if (active == true)
        {
            finishedIndicator.SetActive(false);
            GlobalState.IS_INTERACTING = true;
            languageText.TextID = textId;
            languageText.UpdateText();
        }
    }

    private void FinishInteraction()
    {
        SetTextActive(false);
        _timeSinceFinished = 0;
        _currentInteractedObject.FinishInteraction();
        _currentInteractedObject = null;
        GlobalState.IS_INTERACTING = false;
    }

    private void ProceedConversation(bool isStart = false, int startIndex = 0)
    {
        if (isStart == true)
        {
            _dialogIndex = startIndex;
        }
        else
        {
            _dialogIndex += 1;
            _timeSinceFinished = 0;
        }
        if (_dialogIndex < dialogOrder.Count)
        {
            SetTextActive(true, dialogOrder[_dialogIndex].DialogID);
        }
        else
        {
            FinishInteraction();
        }
    }

    public void TextInteract(Interactable interactedObject, SpeakerDialog dialog)
    {
        if (_currentInteractedObject != interactedObject)
        {
            _currentInteractedObject = interactedObject;
            dialogOrder = null;
            SetTextActive(true, dialog.DialogID);
        }
        else
        {
            if (languageText.FinishedTyping == false)
            {
                languageText.ForceFinishText();
            }
            else
            {
                FinishInteraction();
            }
        }
    }
    public void ConversationInteract(Interactable interactedObject, List<SpeakerDialog> conversation, int startIndex)
    {
        if (_currentInteractedObject != interactedObject)
        {
            _currentInteractedObject = interactedObject;
            dialogOrder = conversation;
            ProceedConversation(true, startIndex);
        }
        else
        {
            if (languageText.FinishedTyping == false)
            {
                languageText.ForceFinishText();
            }
            else
            {
                ProceedConversation();
            }
        }
    }
}
