using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIWindow : MonoBehaviour {

    public Canvas holder;
    public bool remainOpen = false;
    public string windowPath;

    public void CloseWindow()
    {
        GUIManagement.Close(this);
    }

    public void LoadGUISetupSingular(string setup)
    {
        GUIManagement.LoadGUISetupSingular(setup);
    }

    public void LoadGUISetupAdditive(string setup)
    {
        GUIManagement.LoadGUISetupAdditive(setup);
    }
}
