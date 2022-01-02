﻿using AccessAbility.Configuration;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;

namespace AccessAbility
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        public const string HarmonyId = "com.zephyr.BeatSaber.AccessAbility";
        internal static readonly HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(HarmonyId);


        [Init]
        public Plugin(IPALogger logger, Config config)
        {
            Instance = this;
            Plugin.Log = logger;
            Plugin.Log?.Debug("Logger initialized.");

            PluginConfig.Instance = config.Generated<PluginConfig>();
        }

        [OnEnable]
        public void OnEnable()
        {
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += BSEvents_gameSceneLoaded;
            //BS_Utils.Utilities.BSEvents.levelCleared += BSEvents_levelCleared;

            ApplyHarmonyPatches();

            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("AccessAbility", "AccessAbility.ModifierUI.bsml", ModifierUI.instance);
        }

        /*private void BSEvents_levelCleared(StandardLevelScenesTransitionSetupDataSO arg1, LevelCompletionResults arg2)
        {
            Plugin.Log.Debug("GPM - noBombs: " + arg2.gameplayModifiers.noBombs);
            Plugin.Log.Debug("GPM - enabledObstacleType: " + arg2.gameplayModifiers.enabledObstacleType);
            Plugin.Log.Debug("GPM - rawScore: " + arg2.rawScore);
            Plugin.Log.Debug("GPM - modifiedScore: " + arg2.modifiedScore);
            Plugin.Log.Debug("GPM - rank: " + arg2.rank);
            Plugin.Log.Debug("GPM - iswithoutmodifiers: " + arg2.gameplayModifiers.IsWithoutModifiers());
        }*/

        private void BSEvents_gameSceneLoaded()
        {
            Plugin.Log.Debug("Game Scene Loaded");

            if ((PluginConfig.Instance.blue_mode != 0 || PluginConfig.Instance.red_mode != 0) && PluginConfig.Instance.neversubmit_enabled)
            {
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("AccessAbility");
            }

            else if ((PluginConfig.Instance.blue_mode == 2 || PluginConfig.Instance.red_mode == 2) && PluginConfig.Instance.dissolve_distance <= 3)
            {
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("AccessAbility");
            }
        }
        
        [OnDisable]
        public void OnDisable()
        {
            RemoveHarmonyPatches();
        }

        internal static void ApplyHarmonyPatches()
        {
            try
            {
                Plugin.Log?.Debug("Applying Harmony patches.");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error applying Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            try
            {
                harmony.UnpatchSelf();
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error removing Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }
    }
}
