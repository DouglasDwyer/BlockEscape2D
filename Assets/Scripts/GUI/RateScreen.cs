using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateScreen : GUIWindow {

	public void Rate()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.KinglyStudios.SimpleVectors");
#endif
        GUIManagement.LoadGUISetupSingular("Title");
    }
}
