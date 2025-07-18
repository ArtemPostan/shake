using UnityEngine;
using System;
using System.Collections.Generic;
using GamePush;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    [Serializable]
    public class Data
    {
        public string skinName;
        public List<string> levelsEntered = new List<string>();

        public bool surviveModeUnlocked;        

        public int stat_count_deaths;
        public int stat_count_bulletShots;
        public int stat_count_friendlyDeaths;
        public int stat_count_enemyKills;
        public int stat_count_friendlyKills;
        public int stat_count_frindlyKills_UnSaved;
        public int stat_count_friendlyRescued;

        public int stat_levels_finished_01;
        public int stat_levels_finished_02;
        public int stat_levels_finished_03;
        public int stat_levels_finished_04;
        public int stat_levels_finished_05;

        public bool mainGameFinished;
        public bool badAssFinished;
        public bool firstPersonFinished;
        public bool theMythFinished;
        public bool suitManFinished;
        public bool suitManUnlocked;
        public bool theMythDefeated;

        [SerializeField]
        public Dictionary<string, float> customFloatValues;

        [SerializeField]
        public Dictionary<string, int> customIntValues;

        [SerializeField]
        private float _mouseSensitivity;

        public bool holdToAim;

        public float mouseSensitivity
        {
            get
            {
                if (_mouseSensitivity < 0.0001f)
                    _mouseSensitivity = 0.0001f;
                return _mouseSensitivity;
            }
            set
            {
                _mouseSensitivity = Mathf.Max(0.0001f, value);
            }
        }

        public float GetFloat(string key) =>
            customFloatValues.TryGetValue(key, out float val) ? val : 0f;

        public void SetFloat(string key, float value)
        {
            if (customFloatValues.ContainsKey(key))
                customFloatValues[key] = value;
            else
                customFloatValues.Add(key, value);
        }

        public int GetInt(string key) =>
            customIntValues.TryGetValue(key, out int val) ? val : 0;

        public void SetInt(string key, int value)
        {
            if (customIntValues.ContainsKey(key))
                customIntValues[key] = value;
            else
                customIntValues.Add(key, value);
        }

        public Data()
        {
            levelsEntered = new List<string>();
            customFloatValues = new Dictionary<string, float>();
            customIntValues = new Dictionary<string, int>();
            mouseSensitivity = 5f;

            surviveModeUnlocked = false;
        }
    }

    public struct LevelSuccessContext
    {
        public int levelIndex;
        public int teamCount;
    }

    public struct SaveBroContext
    {
        public int teamCount;
    }

    public struct BombDamageContext
    {
        public int hurtCount;
        public int killCount;
        public bool killedPlayer;
    }

    private const string SAVE_KEY = "GameData";
    private static int index;
    private static Data _data;

    public static Data data
    {
        get
        {
            if (_data == null)
                _data = new Data();
            return _data;
        }
        set => _data = value;
    }

    public static bool AchievementAndStatisticsEnabled =>
        FriendlyDamageSwitch.FriendlyDamageOn && !CheatModeSwitch.CheatModeOn;

    public static void Load()
    {
        GP_Init.OnReady += () =>
        {
            try
            {
                GP_Storage.Get(SAVE_KEY, (value) =>
                {
                    if (value is string json && !string.IsNullOrEmpty(json))
                    {
                        try
                        {
                            data = JsonUtility.FromJson<Data>(json);
                            Debug.Log("GamePush data loaded.");
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning("Parsing failed, resetting to defaults: " + e.Message);
                            data = new Data();
                            Save(); // сохранить дефолтные данные
                        }
                    }
                    else
                    {
                        Debug.Log("No saved GamePush data found. Creating default.");
                        data = new Data();
                        Save();
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Load failed: " + e.Message);
                data = new Data();
                Save();
            }
        };
    }

    public static void Save()
    {
        try
        {
            string json = JsonUtility.ToJson(data);
            GP_Storage.Set(SAVE_KEY, json);
            Debug.Log("GamePush data saved.");
        }
        catch (Exception e)
        {
            Debug.LogError("Save error: " + e.Message);
        }
    }

    public static void SetSavesIndex(int newIndex) => index = newIndex;

    public static bool EverEnteredLevel(string levelName) =>
        data.levelsEntered.Contains(levelName);

    public static void SetEnteredLevel(string levelName)
    {
        if (AchievementAndStatisticsEnabled && !data.levelsEntered.Contains(levelName))
        {
            data.levelsEntered.Add(levelName);
        }
    }

    public static void SetAchievement(string name, bool save = true)
    {
        if (AchievementAndStatisticsEnabled && save)
            Save();
    }

    public static void CountPlayerDead()
    {
        if (AchievementAndStatisticsEnabled)
        {
            data.stat_count_deaths++;
            if (data.stat_count_deaths >= 1)
                SetAchievement("count_death_1");
        }
    }

    public static void CountBulletShot()
    {
        if (AchievementAndStatisticsEnabled)
            data.stat_count_bulletShots++;
    }

    public static void CountFriendlyDead()
    {
        if (AchievementAndStatisticsEnabled)
            data.stat_count_friendlyDeaths++;
    }

    public static void CountEnemyKilled()
    {
        if (AchievementAndStatisticsEnabled)
        {
            data.stat_count_enemyKills++;
            if (data.stat_count_enemyKills >= 1) SetAchievement("enemyKilled_1");
            if (data.stat_count_enemyKills >= 50) SetAchievement("enemyKilled_50");
            if (data.stat_count_enemyKills >= 100)
            {
                SetAchievement("enemyKilled_100");
                data.suitManUnlocked = true;
            }
        }
    }

    public static void CountFriendlyRescued()
    {
        if (AchievementAndStatisticsEnabled)
        {
            data.stat_count_friendlyRescued++;
            if (data.stat_count_friendlyRescued > 50)
                SetAchievement("friendlyRescued_50");
        }
    }

    public static void CountFriendlyKilled()
    {
        if (AchievementAndStatisticsEnabled)
        {
            data.stat_count_friendlyKills++;
            if (data.stat_count_friendlyKills >= 20)
                SetAchievement("friendlyKilled_20");
        }
    }

    public static void CountFriendlyKilledUnsaved()
    {
        if (AchievementAndStatisticsEnabled)
        {
            data.stat_count_frindlyKills_UnSaved++;
            if (data.stat_count_frindlyKills_UnSaved >= 10)
                SetAchievement("friendlyKilledUnsaved_10");
        }
    }

    public static void CountLevel1Finished() => data.stat_levels_finished_01++;
    public static void CountLevel2Finished() => data.stat_levels_finished_02++;
    public static void CountLevel3Finished() => data.stat_levels_finished_03++;
    public static void CountLevel4Finished() => data.stat_levels_finished_04++;
    public static void CountLevel5Finished() => data.stat_levels_finished_05++;

    public static void CountLevelSuccess(LevelSuccessContext context)
    {
        if (!AchievementAndStatisticsEnabled) return;

        if (context.teamCount == 3)
            SetAchievement("finishLevelWithFriends_4", save: false);
        if (context.teamCount == 7)
            SetAchievement("finishLevelWithFriends_8", save: false);

        if (context.levelIndex == 7)
        {
            data.mainGameFinished = true;
            SetAchievement("finishedTheWholeGame", save: false);

            if (GameManager.Instance.LevelManager.gameMode == LevelManager.gameModes.single)
            {
                data.badAssFinished = true;
                SetAchievement("badass", save: false);
                string skin = GetSkinName();
                if (skin == "KR") data.suitManFinished = true;
                if (skin == "?") data.theMythFinished = true;
            }

            if (GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
            {
                data.firstPersonFinished = true;
                SetAchievement("firstPersonFinished", save: false);
                string skin = GetSkinName();
                if (skin == "KR") data.suitManFinished = true;
                if (skin == "?") data.theMythFinished = true;
            }
        }

        if (context.levelIndex != 0 && context.levelIndex != 7)
            SetAchievement($"levelFinished_{context.levelIndex}", save: false);

        Save();
        CheckUnlockSurviveMode();
    }

    public static void CheckUnlockSurviveMode()
    {
        if (data.stat_levels_finished_01 > 0 &&
            data.stat_levels_finished_02 > 0 &&
            data.stat_levels_finished_03 > 0 &&
            data.stat_levels_finished_04 > 0 &&
            data.stat_levels_finished_05 > 0)
        {
            if (!data.surviveModeUnlocked)
            {
                UnlockSurviveModePermanent();
                Debug.Log("Survive mode permanently unlocked!");
            }
        }
    }

    public static void SavedABro(SaveBroContext context)
    {
        if (context.teamCount >= 9)
            SetAchievement("snakeLength_10");
    }

    public static void HandleBombDamage(BombDamageContext context)
    {
        if (context.killCount == 4) SetAchievement("kill4withABomb");
        if (context.killedPlayer) SetAchievement("killByABomb");
    }

    public static string GetSkinName() => data?.skinName ?? "Default";

    public static void SetSkinName(string name)
    {
        if (data != null)
        {
            data.skinName = name;
            Save();
        }
    }

    public static bool IsMainGameFinished() => data?.mainGameFinished ?? false;

    public static void DeleteSaveData()
    {
        data = new Data();
        Save();
    }

    public static void ClearAchievement(string name) { /* Stub */ }

    public static void ClearAllAchievements() { /* Stub */ }

    public static void GrantSurviveModeTempAccess()
    {
        SceneManager.LoadScene(10);
    }

    public static void UnlockSurviveModePermanent()
    {
        data.surviveModeUnlocked = true;
        Save();
    }
}
