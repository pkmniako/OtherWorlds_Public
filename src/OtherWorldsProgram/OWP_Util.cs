using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public static class OWP_Util {
			public static T ReflectionGetField<T>(object obj, string name) {
				var field = obj.GetType().GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				return (T)field?.GetValue(obj);
			}

			public static void ReflectionSetField<T>(object obj, string name, T newValue) {
				var field = obj.GetType().GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				field.SetValue(obj, newValue);
			}

			public static T ReflectionGetStaticField<T>(Type type, string name) {
				FieldInfo field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Static);
				return (T)field?.GetValue(null);
			}

			public static MethodInfo ReflectionGetMethod(object obj, string name) {
				MethodInfo output = obj.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				return output;
			}

			public static MethodInfo ReflectionGetMethod(Type type, string name) {
				return type.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic);
			}

			public static string GetObjectHirearchy(GameObject main = null, string p = "") {
				string output;
				if(main == null) {
					output = "Scene:\n";
					foreach(GameObject go in UnityEngine.Object.FindObjectsOfType(typeof(GameObject))) {
						if(go.transform.parent == null)
							output += GetObjectHirearchy(go, "\t");
					}
					return output;
				}

				output = p + "- '" + main.name + "':\n";
				foreach(Behaviour mb in main.GetComponents<Behaviour>()) {
					output += p + "\t<" + mb.GetType().Name + ">\n";
				}
				foreach(Transform child in main.transform) {
					output += GetObjectHirearchy(child.gameObject, p + "\t");
				}
				return output;
			}

			//Thank you siimav for helping with this. node describes the savefile, saveName is a string to name the save file (Can be anything as far as I know)
			public static void LoadSavefileAsConfigNode(ConfigNode node, string saveName) {
				if(node == null) {
					Debug.LogError("[Other Worlds] Couldn't load savefile as ConfigNode named '" + saveName + "', the ConfigNode is null");
					return;
				}
				
				Version originalVersion = SaveUpgradePipeline.NodeUtil.GetCfgVersion(node, SaveUpgradePipeline.LoadContext.SFS);
				
				if(ROCManager.Instance != null) {
					ROCManager.Instance.ClearROCsCache();
				}

				var onQuickloadPipelineFinished = ReflectionGetMethod(QuickSaveLoad.fetch, "onQuickloadPipelineFinished");
				var onQuickloadPipelineFailed = ReflectionGetMethod(QuickSaveLoad.fetch, "onQuickloadPipelineFailed");
				Callback<ConfigNode> callbackFinished = (Callback<ConfigNode>)(n => onQuickloadPipelineFinished.Invoke(QuickSaveLoad.fetch, new object[]{n, saveName, originalVersion}));
				Callback<KSPUpgradePipeline.UpgradeFailOption, ConfigNode> callbackFailed = (Callback<KSPUpgradePipeline.UpgradeFailOption, ConfigNode>) ((opt, n) => onQuickloadPipelineFailed.Invoke(QuickSaveLoad.fetch, new object[]{opt, n, saveName, originalVersion}));
				KSPUpgradePipeline.Process(node, saveName, SaveUpgradePipeline.LoadContext.SFS, callbackFinished, callbackFailed);
			}
			
			public static ConfigNode SaveSavefileAsConfigNode() {
				GameEvents.onGameAboutToQuicksave.Fire();

				Game game = HighLogic.CurrentGame.Updated();
				game.startScene = GameScenes.FLIGHT;

				ConfigNode output = new ConfigNode();
				game.Save(output);

				game.startScene = GameScenes.SPACECENTER;

				if(output != null)
					Debug.Log("[Other Worlds] Game succesfuly saved");
				else
					Debug.LogError("[Other Worlds] Game couldn't be saved");

				return output;
			}

			//Adjust for time difference. Still in seconds. (CT means Cercani Time. Used in the tablet)
			public static double UT2CT(double UT) => UT + 2554696318;
			public static double CT2UT(double CT) => CT - 2554696318;
			public static double CT() => UT2CT(Planetarium.GetUniversalTime());
			public static double CT2UT(int year, int day) => CT2UT((year * 365 + day) * 24 * 3600);

			public static ScreenShot FindScreenshotManager() {
				ScreenShot output = null;
				try {
					foreach(GameObject go in UnityEngine.Object.FindObjectsOfType(typeof(GameObject))) {
						if(go.name.Equals("GameManagement")) {
							output = go.transform.Find("UI").GetComponent<ScreenShot>();
						}
					}
				} catch(Exception e) {
					Debug.LogError("[Other Worlds] Couldn't find Screenshot component, " + e.Message);
					return null;
				}
				return output;
			}

			public static bool AnimatorHasParameter(Animator animator, string name) {
				foreach(AnimatorControllerParameter param in animator.parameters) {
					if(param.name == name)
						return true;
				}
				return false;
			}

			public static void NewWaypoint(string name, string celestialBody, double longitude, double latitude, double altitude = -1) {
				FinePrint.Waypoint wp = new FinePrint.Waypoint();
				wp.name = name;
				wp.celestialName = celestialBody;
				wp.longitude = longitude;
				wp.latitude = latitude;
				if(altitude < 0) {
					wp.isOnSurface = true;
				} else {
					wp.isOnSurface = false;
					wp.altitude = altitude;
				}
				wp.isCustom = true;
				wp.isNavigatable = true;
				
				ScenarioCustomWaypoints.AddWaypoint(wp);
			}
		}
	}
}