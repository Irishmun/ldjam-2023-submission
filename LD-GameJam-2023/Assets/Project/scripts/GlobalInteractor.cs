using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInteractor : MonoBehaviour
{
    public bool IsPaused { get => GlobalState.IS_PAUSED; set => GlobalState.IS_PAUSED = value; }
    public bool HasPackages { get => GlobalState.HAS_PACKAGES; set => GlobalState.HAS_PACKAGES = value; }
    public bool HasChecklist { get => GlobalState.HAS_CHECKLIST; set => GlobalState.HAS_CHECKLIST = value; }
    public bool IsInteracting { get => GlobalState.IS_INTERACTING; set => GlobalState.IS_INTERACTING = value; }
}
