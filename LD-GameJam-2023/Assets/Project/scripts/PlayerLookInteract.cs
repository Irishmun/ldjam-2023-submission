using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLookInteract : MonoBehaviour
{
    private const float MAX_LOOK_DISTANCE = 2f;

    [SerializeField] private GameObject interactableIndicator;

    private Interactable _currentInteractable;


    // Update is called once per frame
    void LateUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, MAX_LOOK_DISTANCE))
        {
            //Debug.Log($"Hit:{hit.transform.gameObject.name}");
            if (hit.transform.TryGetComponent<Interactable>(out Interactable obj) == true)
            {
                interactableIndicator.SetActive(true);
                _currentInteractable = obj;
            }
            else
            {
                interactableIndicator.SetActive(false);
                _currentInteractable = null;
            }
        }
        else
        {
            interactableIndicator.SetActive(false);
            _currentInteractable = null;
        }
    }

    public void TryInteract()
    {
        if (_currentInteractable == null)
        { return; }
        Debug.Log($"Interacted with:{_currentInteractable.gameObject.name}");
        //TODO: rework for InteractState (Create InteractionState)
        //can't remember how this was supposed to happen
        _currentInteractable.Interact();
    }
}
