using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBoxController : MonoBehaviour {

    public static PlayerBoxController instance;
    public static PlayerCustomizationSettings currentPlayer;

    public Rigidbody2D playerCube;
    public Transform lookCalculatorCenterTransform;
    public Transform lookCalculatorOutsideTransform;
    public Transform fireTransform;

    public Die deathKiller;
    public int deathReviveTimes = 0;

    public float currentScore = 0;
    public float difficulty = 0;
    public static int highestScore = 0;
    public bool playing = false;

    public float distanceAwayFromLavaBonus = 0;

    public void Awake() {
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        instance = this;
        GUIManagement.LoadGUISetupSingular("Title");
        SoundManager.soundEffectSource = GetComponent<AudioSource>();
        SoundManager.musicSource = SoundManager.soundEffectSource;

        if (PlayerPrefs.HasKey("HighScore")) {
            highestScore = PlayerPrefs.GetInt("HighScore");
        }
        if (currentPlayer == null)
        {
            if (PlayerPrefs.HasKey("Customization"))
            {
                currentPlayer = SerializationManagement.XMLToObject<PlayerCustomizationSettings>(PlayerPrefs.GetString("Customization"));
            }
            else {
                currentPlayer = new PlayerCustomizationSettings();
            }
            currentPlayer.numberOfTimesOpened += 1;
            if(currentPlayer.numberOfTimesOpened == 4)
            {
                GUIManagement.LoadGUISetupSingular("RateMe");
            }
        }
        if (currentPlayer.activeColor == Color.clear) { currentPlayer.activeColor = new Color32(0, 255, 0, 255); }
        DrawPlayerCube();
        Leaderboard.Initialize();
    }

	public void Update () {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, float.PositiveInfinity, LayerMask.GetMask("PlayerCollider"));
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) && hit.transform != null)
        {
            ApplyTouchPhysic(hit);
            if(!(GUIManagement.openWindows.Count == 0 || !(GUIManagement.openWindows[0] is TitleScreen || GUIManagement.openWindows[0] is InGameScreen)) && !IsPointerOverUIObject() && Input.GetMouseButtonDown(0))
            {
                SoundManager.PlaySoundEffect("Tap");
            }
        }
#else
        for (int i = 0; i < Input.touchCount; ++i)
        {
                if (hit.transform != null)
                {
                    ApplyTouchPhysic(hit);
                    if(!(GUIManagement.openWindows.Count == 0 || !(GUIManagement.openWindows[0] is TitleScreen || GUIManagement.openWindows[0] is InGameScreen)) && !IsPointerOverUIObject()  && Input.GetTouch(i).phase.Equals(TouchPhase.Began)) {
                        SoundManager.PlaySoundEffect("Tap");
                    }
                }
        }
#endif
        playerCube.angularVelocity = Mathf.Clamp(playerCube.angularVelocity, -60, 60);
        UpdateCameraAndFire();
        trackLatestScore();
    }

    /*public void OnGUI()
    {
        GUI.Label(new Rect(100, 100, 50, 300), new GUIContent("GPS Status: " + Leaderboard.currentUserStatus));
    }*/

    public void UpdateCameraAndFire() {
        if (!playing) { return; }
        if (playerCube.transform.localPosition.y - 0.16 > transform.localPosition.y)
        {
            Vector3 newPos = new Vector3(transform.localPosition.x, playerCube.transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * 2);
        }
        fireTransform.transform.localPosition += new Vector3(0, Time.deltaTime * Mathf.Clamp(difficulty - 0.8f, 1, 1.2f) * 0.7f);
        Time.timeScale = Mathf.Clamp((difficulty - 0.2f) * 0.66f, 1, 1.2f);
        if (fireTransform.transform.localPosition.y + 4.52 < transform.localPosition.y) {
            fireTransform.localPosition = new Vector3(fireTransform.localPosition.x, transform.position.y - 4.52f, fireTransform.localPosition.z);
        }
    }

    public void ApplyTouchPhysic(RaycastHit2D hit) {
        if (GUIManagement.openWindows.Count == 0 || IsPointerOverUIObject() || !(GUIManagement.openWindows[0] is TitleScreen || GUIManagement.openWindows[0] is InGameScreen)) { return; }
        if (!playing) {
            playing = true;
            currentScore = 0;
            distanceAwayFromLavaBonus = 0;
            GUIManagement.LoadGUISetupSingular("InGame");
        }
        lookCalculatorCenterTransform.LookAt(hit.point);
        Vector3 motionToAdd = lookCalculatorCenterTransform.position - lookCalculatorOutsideTransform.position;
        playerCube.velocity = motionToAdd * 5;
        if (playerCube.angularVelocity > 2.5f)
        {
            playerCube.angularVelocity = Mathf.Clamp(playerCube.angularVelocity * 1.25f, -60, 60);
        }
        else {
            playerCube.angularVelocity = Random.Range(-30, 30);
        }
    }

    public void trackLatestScore() {
        if (playing)
        {
            difficulty = Mathf.Clamp(transform.localPosition.y / 200 + 1, 1, 2);
            distanceAwayFromLavaBonus += Time.deltaTime * 0.3f * (transform.localPosition.y - (fireTransform.transform.localPosition.y));
            currentScore = (float)(2 * ((1 + (transform.localPosition.y) * 0.75) * difficulty)) + distanceAwayFromLavaBonus - 2;
        }
    }

    public void KillPlayer()
    {
        if(!playing) { return; }
        if (currentScore > highestScore) {
            highestScore = (int)currentScore;
            PlayerPrefs.SetInt("HighScore", highestScore);
        }
        GUIManagement.CloseAll();
        playerCube.isKinematic = true;
        playerCube.velocity = Vector2.zero;
        playerCube.angularVelocity = 0;
        playing = false;
        deathKiller.enabled = true;
    }

    private GameObject destructTrail;
    private GameObject destructHat;
    private GameObject destructSkin;

    public void DrawPlayerCube()
    {
        try
        {
            if (destructTrail != null) { Destroy(destructTrail); destructTrail = null; }
            if (destructHat != null) { Destroy(destructHat); destructHat = null; }
            if (destructSkin != null) { Destroy(destructSkin); destructSkin = null; }
            playerCube.GetComponent<SpriteRenderer>().color = currentPlayer.activeColor;
            if (!string.IsNullOrEmpty(currentPlayer.activeTrail)) { destructTrail = (GameObject)GameObject.Instantiate(Resources.Load("Store/Trails/" + currentPlayer.activeTrail), playerCube.transform, false); }
            if (!string.IsNullOrEmpty(currentPlayer.activeHat)) { destructHat = (GameObject)GameObject.Instantiate(Resources.Load("Store/Hats/" + currentPlayer.activeHat), playerCube.transform, false); }
            if (!string.IsNullOrEmpty(currentPlayer.activeSkin)) { destructSkin = (GameObject)GameObject.Instantiate(Resources.Load("Store/Skins/" + currentPlayer.activeSkin), playerCube.transform, false); }
        }
        catch(System.Exception e)
        {
            Debug.Log("Error while drawing cube: " + e);
        }
    }

    public void DestructTrail()
    {
        if (destructTrail != null) { Destroy(destructTrail); destructTrail = null; }
    }

    public void OnApplicationPause(bool useless)
    {
        PlayerPrefs.SetString("Customization", SerializationManagement.ObjectToXML(currentPlayer));
    }

    public void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}