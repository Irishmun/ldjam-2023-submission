using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalState
{
    public static bool IS_PAUSED { get; set; } = false;
    public static bool HAS_PACKAGES { get; set; } = false;
    public static bool IS_INTERACTING { get; set; } = false;
    public static bool HAS_CHECKLIST { get; set; } = false;
    public static bool BRAM_MODE { get; set; } = false;

    public static DeliveredPackages DELIVERED_PACKAGES { get; set; } = DeliveredPackages.NONE;
}
