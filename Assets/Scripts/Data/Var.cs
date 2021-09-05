using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
public static class Var {
	public static AudioGroup ambientSounds;
	public static AudioMixerSnapshot snapshot;
	public static Bird clickedBird;
	public static string currentSaveSlot = "debug";
	public static bool SophieUnlocked = false;
	public static bool KimUnlocked = false;
	public static bool yesFinalChoice = false;
	public static bool StartedNormally = false;
	public static bool CanShowHover = true;
	public static int currentWeek = 0;     
	public static int currentBG = 0;
	public static List<string> shownDialogs = new List<string>();
	public static List<string> shownEvents = new List<string>();
	public enum Em { Neutral,Solitary,SuperLonely, Social, SuperFriendly,Confident,SuperConfident,Cautious, SuperScared,finish,Random, Shield};
	public enum PowerUps { heal,dmg,emotion,obstacle,shield};
	public static bool isTutorial = false;
    public static bool isEnding = false;
    public static bool tutorialCompleted = true;
	public static bool isBoss = false;
	public static int loadChapterID = -1;
	public static GameObject dustCloud = null;
	public static int MoveGraphBy = 1315;
	public static int maxMentalHealth = 2;
	public static int emPowerTileStrength = 3;
	public static int DangerZoneStart = 14;
	public static int lvl1 = 5;
	public static bool fled = false;
	public  static int lvl2 = 10;
	public static int confLoseFight = -3;
	public static int confWinFight = 2;
	public static int confWinAll = 1;
	public static int confLoseAll = -1;
	public static int oneFriendStraightGain = 2;
	public static int twoFriendStraightGain = 4;
	public static int noFriendGain = -3;
	public static int friendDiagGain = 0;
	public static int[] friendTable = new int[] { -2, 0, 2, 4, 6 };
	public static Bird[] enemies = new Bird[8];
	public static Bird[,] playerPos = new Bird[4,4];
	public static Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
	public static Text birdInfo;
	public static Text birdInfoHeading;
	public static Text birdInfoFeeling;
	public static Image powerBar;
	public static Text powerText;
	public static int health = 3;
	public static List<BattleData> map = new List<BattleData>();
	public static List<MapSaveData>mapSaveData = new List<MapSaveData>();
	public static GameObject selectedBird;
	public static List<Bird> activeBirds = new List<Bird>();
	public static List<Bird> availableBirds = new List<Bird>();
	public static int currentStageID = -1;
    public static int currentBackgroundProgressAnim = -1;
	public static Sprite[] lvlSprites = null;
	public static Sprite[] skillIcons = null;
	public static Sprite[] startingLvlSprites = null;
	public static List<Sprite> hatSprites = null;
	public static List<GameObject> enemySprites = null;
	public static List<GameObject> wizardEnemySprites = null;
	public static List<GameObject> drillEnemySprites = null;
	public static List<GameObject> sueprEnemySprites = null;
	public static GameObject emotionParticles = null;
	public static bool Infight = false;
	[HideInInspector]
	public static TimedEventData selectedTimeEvent;
	public static bool shouldDoMapEvent = false;
	public static Settings gameSettings = new Settings(true);
	public static List<TimedEventData> timedEvents = new List<TimedEventData>();
	public static string[] maleNames = {"Noah", "Liam", "Mason", "Jacob", "William","Ethan", "James","Alexander","Michael","Benjamin","Elijah",
		"Daniel", "Aiden", "Logan", "Matthew","Lucas","Jackson","David","Oliver","Jayden","Joseph","Gabriel","Samuel","Carter","Anthony","John",
		"Dylan", "Luke", "Henry","Andrew","Isaac","Christopher","Joshua","Wyatt","Sebastian","Owen","Caleb","Nathan","Ryan","Jack","Hunter","Levi",
		"Christian","Jaxon","Julian","Landon","Grayson","Jonathan","Isaiah","Charles","Rory", "Eric"};
	public static string[] femaleNames = {"Emma","Olivia","Sophia","Ava","Isabella","Mia","Abigail","Emily", "Charlotte", "Harper","Madison", "Amelia",
		"Elizabeth","Sofia","Evelyn","Chloe","Ella","Grace","Victoria","Aubrey","Scarlett","Zoey","Addison","Lily","Lillian","Natalie","Hannah","Aria","Layla","Lulu"};
	public static string[] deathSignoffs = { "Get well soon, <name>!", "Tis but a scratch!", "I'm sure <name> won't be down for long!" };
	public static bool isDragControls = true;
	public static bool freezeEmotions = false;
	public static int eventTextCharLimit = 200;
	public static int maxLevel = 2;
	public static int enemyMaxLevel = 5;
	/// Tutorial IDs
	public static int battlePlanningTutorialID = 13;
    public static int levelTutorialID = 21;
	public static bool cheatsEnabled = true;
	public static float levelPopupDelay = 0.3f;
	public static int wizardEffectStrength = 6;

	//variables for stats
	public static int trialsSuccessfullCount = 0;//Serialize this variable!
	public static bool birdInjuredInTrial = false;
	public static int narrativeEventsCompleted = 0;
	public static int levelsCompleted = 0;
	public static int birdsMaxLevelCount = 0;
	public static int totalTimeSeconds = 0;
	public static int totalTimeDays = 0;
	public static int totalTimeMinutes = 0;
	public static int totalTimeHours = 0;
	public static int totalPlayTime = 0;
}

[Serializable]
public class Settings
{
	public bool shownFirstLevelUp = true;
	public bool shownBattlePlanningTutorial = true;
	public bool shownLevelTutorial = true;
	public bool shownMapTutorial = true;
	public bool shownMapTutorial2 = true;
	// map feature tutorials
	public bool shownSwordTutorial = true;
	public bool shownShieldTutorial = true;
	public bool shownEmoSquareTutorial = true;
	public bool shownWizardTutorial = true;
	public bool shownHeartTutorial = true;
	public bool shownOneHealthTutorial = true;
	public bool shownTrialTutorial = true;
	public bool shownLevelCapTutorial = true;
	public bool useMHP = true;
	public Settings(bool tutorialsCompleted)
   {
        shownFirstLevelUp = tutorialsCompleted;
        shownBattlePlanningTutorial = tutorialsCompleted;
        shownLevelTutorial = tutorialsCompleted;
		shownMapTutorial = tutorialsCompleted;
		shownMapTutorial2 = tutorialsCompleted;
		// map feature tutorials
		shownSwordTutorial =  tutorialsCompleted;
		shownShieldTutorial = tutorialsCompleted;
		shownEmoSquareTutorial = tutorialsCompleted;
		shownWizardTutorial = tutorialsCompleted;		
		shownHeartTutorial = tutorialsCompleted;
		shownOneHealthTutorial = tutorialsCompleted;
		shownTrialTutorial = tutorialsCompleted;
		shownLevelCapTutorial = tutorialsCompleted;
		useMHP = tutorialsCompleted;
	}
}


[Serializable]
public class MapSaveData
{
	public string areaName;
	public int ID;
	public int trialID;
	public List<int> targets;
	public bool completed;
	public bool available;
	public Var.Em emotion;
	public bool firstCompletion;
	public MapSaveData(bool completed, bool available,bool firstCompletion, int ID, List<int> targets, Var.Em emotion, int trialID, string areaName)
	{
		this.firstCompletion = firstCompletion;
		this.completed = completed;
		this.available = available;
		this.ID = ID;
		this.targets = targets;
		this.emotion = emotion;
		this.trialID = trialID;
		this.areaName = areaName;
	}
}
/*
[Serializable]
public class StatSaveData
{
	public int trialsSuccessfullCount;
	public bool birdInjuredInTrial;
	public int narrativeEventsCompleted;
	public int levelsCompleted;
	public int birdsMaxLevelCount;
	public int TotalTimeSeconds;
	public int TotalTimeDays;
	public int TotalTimeMinutes;
	public int TotalTimeHours;
	public int TotalPlayTime;

	public StatSaveData(int succesCount, bool bInjureTrial, int narrEvent, int lvlCompleted, int birdsMax, int ttSeconds, int ttDays, int ttMins, int ttHours, int ttPlayTime)
	{
		this.trialsSuccessfullCount = succesCount;
		this.birdInjuredInTrial = bInjureTrial;
		this.narrativeEventsCompleted = narrEvent;
		this.levelsCompleted = lvlCompleted;
		this.birdsMaxLevelCount = birdsMax;
		this.TotalTimeSeconds = ttSeconds;
		this.TotalTimeDays = ttDays;
		this.TotalTimeMinutes = ttMins;
		this.TotalTimeHours =ttHours;
		this.TotalPlayTime = ttPlayTime;
	}
}*/

