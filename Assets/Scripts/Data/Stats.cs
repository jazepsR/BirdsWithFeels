using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats Instance { get; private set; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

   public void getTrialDetails(TimedEventData anEvent)
   {
        var steam_acheievements_trial_id = "";
        switch (anEvent.eventName)
        {
            case "Tery's Event":
                steam_acheievements_trial_id = "terry_trial_in_time";
                break;
            case "Rebecca's Event":
                steam_acheievements_trial_id = "rebecca_trial_in_time";
                break;
            case "Alex's Event":
                steam_acheievements_trial_id = "alex_trial_in_time";
                break;
            case "Kim's Event":
                steam_acheievements_trial_id = "kim_trial_in_time";
                break;
            case "Sophie's Event":
                steam_acheievements_trial_id = "sophie_trial_in_time";
                break;
            default:
                steam_acheievements_trial_id = " ";
                break;
        }


        if (steam_acheievements_trial_id != " ")
        {
            switch (anEvent.currentState)
            {

                case TimedEventData.state.completedSuccess:
                    Steamworks.SteamUserStats.SetStat(steam_acheievements_trial_id, 1);
                    bool trialDoneInTime = true;
                    Steamworks.SteamUserStats.GetAchievement(steam_acheievements_trial_id, out trialDoneInTime);
                    Steamworks.SteamUserStats.SetAchievement(steam_acheievements_trial_id);
                    Steamworks.SteamUserStats.StoreStats();
                    Var.trialsSuccessfullCount++;
                    break;
                case TimedEventData.state.completedFail:
                    break;
                case TimedEventData.state.failed:
                    break;
            }
        }

        if(Var.trialsSuccessfullCount == 5)
        {
            Steamworks.SteamUserStats.SetStat("all_trials_complete_in_time", 1);
            bool allTrialsSuccess = true;
            Steamworks.SteamUserStats.GetAchievement("all_trials_complete_in_time", out allTrialsSuccess);
            Steamworks.SteamUserStats.SetAchievement("all_trials_complete_in_time");
        }
        
        Debug.Log(anEvent.eventName);
   }

}
