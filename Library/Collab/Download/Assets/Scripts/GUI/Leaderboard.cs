using System.Collections;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System;

public class Leaderboard : GUIWindow {
    
    public GameObject forceSignIn;
    public Text loadingText;

    public Text nameText;
    public Text scoreText;

    private static bool initialized = false;
    public static UserAuthenticationStatus currentUserStatus = UserAuthenticationStatus.Unauthenticated;

    public GameObject[] sortButtons;

	public void Awake()
    {
        if(!initialized) { Initialize(); }
        Update();
    }

    public static void Initialize()
    {
        if(initialized) { return; }
        PlayGamesPlatform.InitializeInstance(GetConfiguration());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        if (PlayerBoxController.currentPlayer.playerSignedInAlready) {
            SignIn(true, true);
        }        
        initialized = true;
    }

    public void Update()
    {
        forceSignIn.SetActive(currentUserStatus == UserAuthenticationStatus.Unauthenticated);
        if(currentUserStatus == UserAuthenticationStatus.Authenticated)
        {
            PlayerBoxController.currentPlayer.playerSignedInAlready = true;
            if (currentLeaderboard == null)
            {
                displayLeaderboard(ScoreContext.Top);
            }
        }
        else if(currentUserStatus == UserAuthenticationStatus.InProgress)
        {
            loadingText.text = "Signing in...";
        }
        else
        {
            loadingText.text = string.Empty;
        }
    }

    private ILeaderboard currentLeaderboard;

    public void displayLeaderboard(string context)
    {
        displayLeaderboard((ScoreContext)Enum.Parse(typeof(ScoreContext), context));
    }

    public void displayLeaderboard(ScoreContext context)
    {
        nameText.text = string.Empty;
        scoreText.text = string.Empty;
        loadingText.text = "Loading scores...";
        foreach(GameObject o in sortButtons)
        {
            o.SetActive(false);
        }
        ILeaderboard leaderboard = PlayGamesPlatform.Instance.CreateLeaderboard();
        leaderboard.id = GooglePlayIDs.leaderboard_high_scores;
        if (context == ScoreContext.Top)
        {
            PlayGamesPlatform.Instance.LoadScores(GooglePlayIDs.leaderboard_high_scores, LeaderboardStart.TopScores, 30, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (collection) =>
            {
                string[] userIds = new string[collection.Scores.Length];
                for (int x = 0; x < userIds.Length; x++)
                {
                    userIds[x] = collection.Scores[x].userID;
                }
                Social.LoadUsers(userIds, (IUserProfile[] profiles) =>
                {
                    ShowLeaders(collection.Scores, profiles);
                });
            });
        }
        else if(context == ScoreContext.AroundPlayer)
        {
            PlayGamesPlatform.Instance.LoadScores(GooglePlayIDs.leaderboard_high_scores, LeaderboardStart.PlayerCentered, 30, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (collection) =>
            {
                string[] userIds = new string[collection.Scores.Length];
                for (int x = 0; x < userIds.Length; x++)
                {
                    userIds[x] = collection.Scores[x].userID;
                }
                Social.LoadUsers(userIds, (IUserProfile[] profiles) =>
                {
                    ShowLeaders(collection.Scores, profiles);
                });
            });
        }
        else if(context == ScoreContext.TopFriends)
        {
            PlayGamesPlatform.Instance.LoadScores(GooglePlayIDs.leaderboard_high_scores, LeaderboardStart.TopScores, 30, LeaderboardCollection.Social, LeaderboardTimeSpan.AllTime, (collection) =>
            {
                string[] userIds = new string[collection.Scores.Length];
                for (int x = 0; x < userIds.Length; x++)
                {
                    userIds[x] = collection.Scores[x].userID;
                }
                Social.LoadUsers(userIds, (IUserProfile[] profiles) =>
                {
                    ShowLeaders(collection.Scores, profiles);
                });
            });
        }
        else
        {
            PlayGamesPlatform.Instance.LoadScores(GooglePlayIDs.leaderboard_high_scores, LeaderboardStart.PlayerCentered, 30, LeaderboardCollection.Social, LeaderboardTimeSpan.AllTime, (collection) =>
            {
                string[] userIds = new string[collection.Scores.Length];
                for (int x = 0; x < userIds.Length; x++)
                {
                    userIds[x] = collection.Scores[x].userID;
                }
                Social.LoadUsers(userIds, (IUserProfile[] profiles) =>
                {
                    ShowLeaders(collection.Scores, profiles);
                });
            });
        }
        currentLeaderboard = leaderboard;
    }

    private void ShowLeaders(IScore[] data, IUserProfile[] profiles)
    {
        foreach (GameObject o in sortButtons)
        {
            o.SetActive(true);
        }
        foreach (IScore score in data)
        {
            nameText.text += score.rank + ". " + getUserFromID(score.userID, profiles) + "\n";
            scoreText.text += score.value + "\n";
        }
        loadingText.text = string.Empty;
    }
    
    private string getUserFromID(string id, IUserProfile[] profiles)
    {
        foreach(IUserProfile profile in profiles)
        {
            if(id == profile.id)
            {
                if (Social.localUser.id == profile.id)
                {
                    return "<color=yellow>" + profile.userName + "</color>";
                }
                else {
                    return profile.userName;
                }
            }
        }
        return null;
    }

    public static void SignIn(bool silent, bool forceIt)
    {
        if(currentUserStatus == UserAuthenticationStatus.Unauthenticated)
        {
            currentUserStatus = UserAuthenticationStatus.InProgress;
            PlayGamesPlatform.Instance.Authenticate((success, data) => { OnAuthenticated(success, data, forceIt); }, silent);
            //Social.localUser.Authenticate(OnAuthenticated);
        }
    }

    public void LocalSignIn()
    {
        SignIn(false, false);
    }

    private static void OnAuthenticated(bool success, string data, bool forceIt)
    {
        Debug.Log("User was authenticated? " + success + "\n" + data);
        if(success)
        {
            currentUserStatus = UserAuthenticationStatus.Authenticated;
            Social.ReportScore(PlayerBoxController.highestScore, GooglePlayIDs.leaderboard_high_scores, succ => { Debug.Log("Posting user's highest score successful: " + succ); });
        }
        else
        {
            if (GUIManagement.openWindows.Count > 0 && !(GUIManagement.openWindows[0] is InGameScreen))
            {
                GUIManagement.LoadGUISetupSingular("LeaderboardFailure");
            }
            currentUserStatus = UserAuthenticationStatus.Unauthenticated;
            if (PlayerBoxController.currentPlayer.playerSignedInAlready)
            {
                PlayerBoxController.currentPlayer.playerSignedInAlready = forceIt;
            }
        }
    }

    private static PlayGamesClientConfiguration GetConfiguration()
    {
        PlayGamesClientConfiguration.Builder bee = new PlayGamesClientConfiguration.Builder();
            //.RequestIdToken();
        Debug.Log("my dad " + bee.IsHidingPopups());
        return bee
            .Build();
    }

    public enum UserAuthenticationStatus { Unauthenticated, InProgress, Authenticated }
    public enum ScoreContext { Top, AroundPlayer, TopFriends, Friends }
}
