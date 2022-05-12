using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Achievements
{
    public static int stats_total_trial_count = 5;
    public static int max_bird_level_count = 5;
    public static int hours_to_play = 3;
    public static int weeks_to_beat_king =80;
    public static int weeks_to_pass = 50;
    public static int total_Narrative_Events = 24;
    public static int total_Levels = 42;



    public static void Start()
    {

        
    }

    /*void Update()
    {
    }*/

   public static void getTrialDetails(TimedEventData anEvent, bool forStats = false)
   {
        var steam_acheievements_trial_id = "";
        switch (anEvent.eventName)
        {
            case "Terry's farm is in trouble":
                steam_acheievements_trial_id = "terry_trial_in_time";
                break;
            case "Rebecca's family is leaving":
                steam_acheievements_trial_id = "rebecca_trial_in_time";
                break;
            case "Talonport under siege":
                steam_acheievements_trial_id = "alex_trial_in_time";
                break;
            case "Kim's boyfriend in peril":
                steam_acheievements_trial_id = "kim_trial_in_time";
                break;
            case "A fight for knowledge":
                steam_acheievements_trial_id = "sophie_trial_in_time";
                break;
            default:
                steam_acheievements_trial_id = "";
                break;
        }


        
            if (steam_acheievements_trial_id != "")
            {
                Debug.Log("event " + anEvent.currentState);
                switch (anEvent.currentState)
                {
                    case TimedEventData.state.completedSuccess:

                    if (!forStats)
                    {
                        Var.trialsSuccessfullCount++;
                        SetAchievement(steam_acheievements_trial_id, "");
                    }
                        break;
                    case TimedEventData.state.completedFail:
                        break;
                    case TimedEventData.state.failed:
                        break;
                    default:
                        Debug.Log("none can be found");
                        break;
                }
            }
        

        if(Var.trialsSuccessfullCount == stats_total_trial_count)
        {
            SetAchievement("all_trials_complete_in_time", "");
        }
        
   }

    public static void vultureKingFightStatus(bool isbeat)
    {
        if (isbeat == true)
        { 
            SetAchievement("beat_vulture_king", "");
        }
        
    }

    public static void amountOfWeeks(int weeks)
    {
        if (weeks >= weeks_to_pass)
        { 
            SetAchievement("reach_amount_of_weeks", "");
        }
    }

    public static void birdDiedFirstTime()
    {
        SetAchievement("birds_death", "");
    }

    public static void checkBirdInjuredInTrial(bool isbirdInjured)
    {

        if (!isbirdInjured)
        {
            SetAchievement("trial_no_bird_injured", "");
        }

        else
        {
            Var.birdInjuredInTrial = false;
        }

        
    }
    public static void SetAchievement(string achievementName, string statname= "")
    {
        if (Debug.isDebugBuild)
        {
            Debug.Log("------Achievement triggered: " + achievementName + " stat name: " + statname);
            return;
        }
        
        if (statname != "")
        {
            Steamworks.SteamUserStats.SetStat(statname, 1);
        }
        bool completedAchievement = true;
        Steamworks.SteamUserStats.GetAchievement(achievementName, out completedAchievement);
        if (completedAchievement == false)
        {
            Steamworks.SteamUserStats.SetAchievement(achievementName);
            Debug.Log("ACHIEVEMENT UNLOCKED: " + achievementName);
            Steamworks.SteamUserStats.StoreStats();
        }
        else
        {
            Debug.Log("ACHIEVEMENT ALREADY CLAIMED: " + achievementName);
        }
    }
    public static void levelCompletionTracker()
    {

        Debug.Log("Level Completed: " + Var.levelsCompleted + " out of 42");
        if (Var.levelsCompleted == total_Levels)
        {
            SetAchievement("complete_all_levels","");
        }
    }

    public static void narrativeEventCompletionTracker()
    {
        Debug.Log("Narrative Events Completed: " + Var.narrativeEventsCompleted + " out of 24");
        if (Var.narrativeEventsCompleted == total_Narrative_Events)
        {
            SetAchievement("narrative_events_unlocked", "");
        }
    }

    public static void checkBirdLevelUp(Bird bird, bool inMap)
    {
        if (inMap == false)
        {
            if (bird.data.level > 1)
            {
                SetAchievement("bird_level_up", "");
            }

            if (bird.data.level == max_bird_level_count)
            {
                SetAchievement("bird_level_up_max", "");
            }
        }

        if (inMap == true)
        {
            if (Var.birdsMaxLevelCount == max_bird_level_count)
            {
                SetAchievement("level_up_all_birds", "");
            }
        }
    }

    public static void vulture_king_in_time()
    {
        if (Var.currentWeek <= weeks_to_beat_king)
        {
            SetAchievement("vulture_king_in_time", "");
        }
    }

    public static void BeatGameInTime()
    {
        if (Var.totalTimeHours <= hours_to_play)
        {
            SetAchievement("beat_game_in_hours", "");
        }
    }

    



}
