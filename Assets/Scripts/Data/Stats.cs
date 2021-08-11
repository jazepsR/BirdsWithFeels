using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Stats
{

    public static void Start()
    {  
    }

    /*void Update()
    {
    }*/

   public static void getTrialDetails(TimedEventData anEvent)
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
                steam_acheievements_trial_id = " ";
                break;
        }


        if (steam_acheievements_trial_id != " ")
        {
            switch (anEvent.currentState)
            {

                case TimedEventData.state.completedSuccess:
                  /*  Steamworks.SteamUserStats.SetStat(steam_acheievements_trial_id, 1);
                    bool trialDoneInTime = true;
                    Steamworks.SteamUserStats.GetAchievement(steam_acheievements_trial_id, out trialDoneInTime);
                    Steamworks.SteamUserStats.SetAchievement(steam_acheievements_trial_id);
                    Steamworks.SteamUserStats.StoreStats();*/
                    Var.trialsSuccessfullCount++;
                    Debug.Log("ACHIEVEMENT UNLOCKED: " + steam_acheievements_trial_id);
                    break;
                case TimedEventData.state.completedFail:
                    break;
                case TimedEventData.state.failed:
                    break;
            }
        }

        if(Var.trialsSuccessfullCount == 5)
        {
            /*Steamworks.SteamUserStats.SetStat("all_trials_complete_in_time", 1);
            bool allTrialsSuccess = true;
            Steamworks.SteamUserStats.GetAchievement("all_trials_complete_in_time", out allTrialsSuccess);
            Steamworks.SteamUserStats.SetAchievement("all_trials_complete_in_time");*/
            Debug.Log("ACHIEVEMENT UNLOCKED: all_trials_complete_in_time");
        }
        
   }

    public static void vultureKingFightStatus(bool isbeat)
    {


        if (isbeat == true)
        { /*Steamworks.SteamUserStats.SetStat("beat_vulture_king", 1);
           bool kingbeaten = true;
           Steamworks.SteamUserStats.GetAchievement("beat_vulture_king", out kingbeaten);
           Steamworks.SteamUserStats.SetAchievement("beat_vulture_king");*/
            Debug.Log("ACHIEVEMENT UNLOCKED: beat_vulture_king");
        }
        
    }

    public static void amountOfWeeks(int weeks)
    {
        if (weeks >= 50)
        { /*Steamworks.SteamUserStats.SetStat("reach_amount_of_weeks", 1);
           bool amountOfWeeks = true;
           Steamworks.SteamUserStats.GetAchievement("reach_amount_of_weeks", out amountOfWeeks);
           Steamworks.SteamUserStats.SetAchievement("reach_amount_of_weeks");*/
            Debug.Log("ACHIEVEMENT UNLOCKED: reach_amount_of_weeks");
        }
    }

    public static void birdDiedFirstTime()
    {
        /*Steamworks.SteamUserStats.SetStat("birds_death", 1);
           bool birddeath = true;
           Steamworks.SteamUserStats.GetAchievement("birds_death", out birddeath);
           Steamworks.SteamUserStats.SetAchievement("birds_death");*/
        Debug.Log("ACHIEVEMENT UNLOCKED: birds_death");
    }

    public static void checkBirdInjuredInTrial(bool isbirdInjured)
    {

        if (!isbirdInjured)
        {
            /*Steamworks.SteamUserStats.SetStat("trial_no_bird_injured", 1);
               bool nobirdInjured = true;
               Steamworks.SteamUserStats.GetAchievement("trial_no_bird_injured", out nobirdInjured);
               Steamworks.SteamUserStats.SetAchievement("trial_no_bird_injured");*/
            Debug.Log("ACHIEVEMENT UNLOCKED: trial_no_bird_injured");
        }

        else
        {
            Var.birdInjuredInTrial = false;
        }

        
    }

    public static void levelCompletionTracker()
    {

        Debug.Log("Level Completed: " + Var.levelsCompleted + " out of 42");
        if (Var.levelsCompleted == 42)
        {
            /*Steamworks.SteamUserStats.SetStat("complete_all_levels", 1);
            bool completedalllevels = true;
            Steamworks.SteamUserStats.GetAchievement("complete_all_levels", out completedalllevels);
            Steamworks.SteamUserStats.SetAchievement("complete_all_levels");*/
            Debug.Log("ACHIEVEMENT UNLOCKED: narrative_events_unlocked");
        }
    }

    public static void narrativeEventCompletionTracker()
    {
        Debug.Log("Narrative Events Completed: " + Var.narrativeEventsCompleted + " out of 24");
        if (Var.narrativeEventsCompleted == 24)
        {
            /*Steamworks.SteamUserStats.SetStat("narrative_events_unlocked", 1);
            bool didAllNarEvents = true;
            Steamworks.SteamUserStats.GetAchievement("narrative_events_unlocked", out didAllNarEvents);
            Steamworks.SteamUserStats.SetAchievement("narrative_events_unlocked");*/
            Debug.Log("ACHIEVEMENT UNLOCKED: narrative_events_unlocked");
        }
    }

    public static void checkBirdLevelUp(Bird bird, bool inMap)
    {
        if (inMap == false)
        {
            if (bird.data.level > 1)
            {
                /*Steamworks.SteamUserStats.SetStat("bird_level_up", 1);
                bool birdlevelup = true;
                Steamworks.SteamUserStats.GetAchievement("bird_level_up", out birdlevelup);
                Steamworks.SteamUserStats.SetAchievement("bird_level_up");*/
                Debug.Log("ACHIEVEMENT UNLOCKED: bird_level_up");
            }

            if (bird.data.level == Var.maxLevel)
            {
                /*Steamworks.SteamUserStats.SetStat("bird_level_up_max", 1);
               bool birdMaxLevel = true;
               Steamworks.SteamUserStats.GetAchievement("bird_level_up_max", out birdMaxLevel);
               Steamworks.SteamUserStats.SetAchievement("bird_level_up_max");*/
                Debug.Log("ACHIEVEMENT UNLOCKED: bird_level_up_max");
            }
        }

        if (inMap == true)
        {
            if (Var.birdsMaxLevelCount == 5)
            {
                /*Steamworks.SteamUserStats.SetStat("level_up_all_birds", 1);
               bool birdsAllMaxLevel = true;
               Steamworks.SteamUserStats.GetAchievement("level_up_all_birds", out birdsAllMaxLevel);
               Steamworks.SteamUserStats.SetAchievement("level_up_all_birds");*/
                Debug.Log("ACHIEVEMENT UNLOCKED: level_up_all_birds");
            }
        }
    }

    public static void vulture_king_in_time()
    {
        if (Var.currentWeek <= 30)
        {
            /*Steamworks.SteamUserStats.SetStat("vulture_king_in_time", 1);
            bool vultureKingInTime = true;
            Steamworks.SteamUserStats.GetAchievement("vulture_king_in_time", out vultureKingInTime);
            Steamworks.SteamUserStats.SetAchievement("vulture_king_in_time");*/
            Debug.Log("ACHIEVEMENT UNLOCKED: vulture_king_in_time");
            
        }
    }

    public static void BeatGameInTime()
    {
        if (Var.TotalTimeHours <= 3)
        {
            /*Steamworks.SteamUserStats.SetStat("beat_game_in_hours", 1);
            bool gameBeatInTime = true;
            Steamworks.SteamUserStats.GetAchievement("beat_game_in_hours", out gameBeatInTime);
            Steamworks.SteamUserStats.SetAchievement("beat_game_in_hours");*/
            Debug.Log("ACHIEVEMENT UNLOCKED: beat_game_in_hours");
        }
    }
}
