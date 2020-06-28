using System;
using System.Collections.Generic;
using UnityEngine;

public static class GUIManagement {

    internal static List<GUIWindow> openWindows = new List<GUIWindow>();
	
    public static GUIWindow LoadGUISetupSingular(string setup)
    {
        CloseAll();
        GUIWindow newWindow = GameObject.Instantiate(Resources.Load<GameObject>("GUI/Setups/" + setup)).GetComponent<GUIWindow>();
        newWindow.windowPath = setup;
        openWindows.Add(newWindow);
        return openWindows[openWindows.Count - 1];
    }

    public static GUIWindow LoadGUISetupAdditive(string setup)
    {
        GUIWindow newWindow = GameObject.Instantiate(Resources.Load<GameObject>("GUI/Setups/" + setup)).GetComponent<GUIWindow>();
        newWindow.windowPath = setup;
        openWindows.Add(newWindow);
        return openWindows[openWindows.Count - 1];
    }

    public static void Close(GUIWindow window)
    {
        if(openWindows.Contains(window))
        {
            GameObject.Destroy(window.gameObject);
            openWindows.Remove(window);
        }
        else { throw new ArgumentException("Parameter window was not instantiated using GUIManagement!"); }
    }

    public static void CloseAll()
    {
        CloseAll(false);
    }

    public static void CloseAll(bool closeUnclosables)
    {
        foreach(GUIWindow window in openWindows.ToArray())
        {
            if (closeUnclosables || !window.remainOpen)
            {
                Close(window);
            }
        }
    }
}
