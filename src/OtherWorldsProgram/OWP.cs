using System;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		[KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.SPACECENTER, GameScenes.FLIGHT)]
		public class OWP : ScenarioModule
		{
			/*
				Basic Data
			*/
			private static bool _init = false;
			private static float _timer = 0;
			private const float TIMER_PERIOD = 2.0f;
			public static bool DO_TIMEWARP_TO_PAST_ON_CUTSCENES { get; private set; }

			/*
				Progress Data
			*/
			protected static uint progress = 0b00000000000000000000000000000000;
			private const uint LAST_BIT_MASK = 0b00000000000000000000000000000001;

			public static bool artifact1Spawned = false;
			public static bool artifact2Spawned = false;
			public static bool popout1shown = false, popout1shownNow = false;
			public static bool popout2shown = false, popout2shownNow = false;

			/*
				Object-tracking data
			*/
			public static List<OWP_LocData> locations = null;
			public static List<OWP_LocSignal> signals = null;
			public static List<OWP_LocPillar> pillars = null;

			/*
				Asset-Bundling
			*/
			public static AssetBundle assetBundle = null;
			public static string PATH_OW = "GameData/OtherWorldsReboot/";

			/*
				UI
			*/
			public static OWP_UI ui = null;

			/*
				Custom Part Tracking
			*/
			public static AvailablePart artifact1 = null, artifact2 = null, artifact1rec = null, artifact2rec = null;

			/*
				Events Manager
			*/
			OWP_EventsManager events = null;

			/*
				New-savefile check
			*/
			bool runNewSavefileRoutine = false;
			

			public void Start() {
				Debug.Log("[Other Worlds] OWP Manager - Start()");
				PATH_OW = KSPUtil.ApplicationRootPath + PATH_OW;

				//Load asset bundle once
				if(assetBundle == null) {
					LoadAssetBundle(true);
				}
				if(ui == null) {
					ui = new OWP_UI();
				}
				if(!OWP_LOC.IsLoaded) {
					OWP_LOC.LoadData("en_en");
				}
				
				_init = false;
			}

			/// <summary>
			///	Similar to <c>Start()</c>, but runs once, after (hopefuly) everything has been initialized. Used to get some specific elements
			/// </summary>
			public void LateSingleStart() {
				if(locations == null)
					locations = new List<OWP_LocData>();
				else
					locations.Clear();

				if(signals == null)
					signals = new List<OWP_LocSignal>();
				else
					signals.Clear();

				if(pillars == null)
					pillars = new List<OWP_LocPillar>();
				else
					pillars.Clear();

				if(artifact1 == null)
					artifact1 = PartLoader.getPartInfoByName("ARTIFACT1");
				
				if(artifact2 == null)
					artifact2 = PartLoader.getPartInfoByName("ARTIFACT2");

				if(artifact1rec == null)
					artifact1rec = PartLoader.getPartInfoByName("ARTIFACT1Rec");

				if(artifact2rec == null)
					artifact2rec = PartLoader.getPartInfoByName("ARTIFACT2Rec");

				OWP_Music.LateLoad();

				OWP_LocInit.InitLocations();

				if(events == null) {
					events = new OWP_EventsManager();
					events.SetupEventsListeners();
				}

				if(HighLogic.LoadedSceneIsFlight)
					ui.LoadEverything();

				if(runNewSavefileRoutine) {
					_OnNewSaveFile();
					runNewSavefileRoutine = false;
				}
			}

			public static void AddLocation(OWP_LocSignal loc, int id) {
				AddLocation((OWP_LocData)loc, id);
				signals.Add(loc);
			}

			public static void AddLocation(OWP_LocPillar loc, int id) {
				AddLocation((OWP_LocData)loc, id);
				pillars.Add(loc);
			}
			public static void AddLocation(OWP_LocData loc, int id) {
				loc.id = id;
				locations.Add(loc);
			}

			public void Update() {
				//Static Init
				if(!_init) {
					LateSingleStart();
					_init = true;
				}

				_UnloadLocs(); //Mark locations as unloaded if their gameobjects are, well, unloaded.
				_DeleteVesselsQueue(); //Delete vessels that were marked for destruction. Used in OWPArtifactModule.

				if(ui != null && HighLogic.LoadedSceneIsFlight) {
					ui.Update();
				}
				OWP_Cutscene.Update();
				OWP_Music.Update();

				//Rise Timer
				if(_timer > TIMER_PERIOD) {
					PeriodicUpdate();
					_timer = 0;
				} else {
					_timer += Time.deltaTime;
				}
			}

			public void LateUpdate() {
				foreach(var loc in locations) {
					if(loc.gameObject != null)
						loc.OnLateUpdate();
				}
			}

			/// <summary>
			///	Update that runs every <c>TIMER_PERIOD</c> seconds
			/// </summary>
			public void PeriodicUpdate() {
				_RunLoadMethods(); //Check if a structure is loaded. Run the load function if so.
				_RunPeriodicUpdateMethods();
				//_UpdateProgress();
			}

			public void _UnloadLocs() {
				foreach(var loc in locations) {
					//Mark as not loaded
					if(loc.gameObject != null && (loc.GetGameObject() == null || !loc.GetGameObject().activeInHierarchy)) {
						loc.gameObject = null;
					}
				}
			}

			public void _RunLoadMethods() {
				foreach(var loc in locations) {
					//Load location
					if(loc.GetGameObject() != null && loc.GetGameObject().activeInHierarchy && loc.gameObject == null) {
						loc.gameObject = loc.GetGameObject();
						loc.OnLoad();
					}
				}
			}

			public void _RunPeriodicUpdateMethods() {
				foreach(var loc in locations) {
					if(loc.gameObject != null)
						loc.OnPeriodicUpdate();
				}
			}

			public void OnDisable() {

			}

			private void _OnNewSaveFile() {
				Debug.Log("[Other Worlds] Running new savefile routine...");
				OWP_PlanetManager.SpawnCustomWaypoints();
				OWP_Popout.OnFirstKSC();
			}

			/*
				Save Management
			*/

			public override void OnSave(ConfigNode node) {
				Debug.Log("[Other Worlds] OWP Manager - OnSave()");
				node.AddValue("owProgress", progress);
				node.AddValue("a1s", artifact1Spawned);
				node.AddValue("a2s", artifact2Spawned);
				node.AddValue("popout1shown", popout1shown);
				node.AddValue("popout2shown", popout2shown);
				node.AddValue("newSaveChecksDone", !runNewSavefileRoutine);
			}

			public override void OnLoad(ConfigNode node) {
				DO_TIMEWARP_TO_PAST_ON_CUTSCENES = true;
				try {
					if(node.HasValue("owProgress"))
						progress = UInt32.Parse(node.GetValue("owProgress"));
					else
						Debug.LogWarning("[Other Worlds] No Progress found in savefile, using default value");

					if(node.HasValue("a1s"))
						artifact1Spawned = Boolean.Parse(node.GetValue("a1s"));
					if(node.HasValue("a2s"))
						artifact2Spawned = Boolean.Parse(node.GetValue("a2s"));

					if(node.HasValue("popout1shown"))
						popout1shown = Boolean.Parse(node.GetValue("popout1shown"));
					if(node.HasValue("popout2shown"))
						popout2shown = Boolean.Parse(node.GetValue("popout2shown"));

					if(node.HasValue("newSaveChecksDone"))
						runNewSavefileRoutine = !Boolean.Parse(node.GetValue("newSaveChecksDone"));
					else
						runNewSavefileRoutine = true;
				} catch(Exception e) {
					Debug.LogWarning("[Other Worlds] Error while reading the current progress from savefile: " + e);
				}
				Debug.Log("[Other Worlds] Progress loaded: " + progress + " (0b" + Convert.ToString(progress, 2) + ")");
			}

			/*
				Asset Bundle
			*/

			//Load the asset bundle
			static void LoadAssetBundle(bool verbose = false) {
				string filename = "Bundles/otherworlds_";
				switch(Application.platform) {
					case RuntimePlatform.OSXEditor:
					case RuntimePlatform.OSXPlayer:
						filename += "mac";
						break;
					case RuntimePlatform.LinuxEditor:
					case RuntimePlatform.LinuxPlayer:
						filename += "linux";
						break;
					default:
						filename += "win";
						break;
				}

				assetBundle = AssetBundle.LoadFromFile(PATH_OW + filename);
				if(assetBundle == null) {
					Debug.LogError($"[Other Worlds] Couldn't load bundle '{filename}' (OS: {Application.platform})");
				} else if(verbose) {
					Debug.Log($"[Other Worlds] Loaded Asset Bundle. (OS: {Application.platform})");
					//foreach(var n in assetBundle.GetAllAssetNames())
						//Debug.Log("\t\t" + n);
				}
			}

			//Retrieve a specific asset from the asset bundle
			public static T LoadAsset<T>(string assetName) where T : UnityEngine.Object {
				if(assetBundle == null)
					return null;

				UnityEngine.Object o = assetBundle.LoadAsset(assetName);

				if(o == null) {
					Debug.LogError("[Other Worlds] Couldn't find '" + assetName + "' on the mod's asset bundle. (Laugh at this user, they can't type)");
					return null;
				}
				
				T output = Instantiate(o as T);
				
				return output;
			}

			/*
				Progression
			*/

			public static bool GetProgressBit(int bit) {
				if(bit < 0 || bit > 31) return false;
				
				return LAST_BIT_MASK == ((progress >> bit) & LAST_BIT_MASK);
			}

			public static void SetProgressBit(int bit, bool value) {
				if(bit < 0 || bit > 31) return;

				uint mask = (uint)1 << bit;
				progress |= mask;
			}

			/*
				Safe Deletion
			*/

			static List<Vessel> vesselDeletionQueue = new List<Vessel>();

			public static void DeleteVesselNextFrame(Vessel vessel) {
				vesselDeletionQueue.Add(vessel);
			}

			private static void _DeleteVesselsQueue() {
				foreach(Vessel vessel in vesselDeletionQueue) {
					if(vessel != null)
						vessel.Die();
					else
						Debug.LogError($"[Other Worlds] A vessel was added to the deletion queue but it was actually null");
				}
				vesselDeletionQueue.Clear();
			}

			public static void SetPartVisibilityOnEditor() {
				//Add a custom filter to the VAB's part list, so the artifacts are shown as needed

				EditorPartListFilterList<AvailablePart> filters = KSP.UI.Screens.EditorPartList.Instance.ExcludeFilters;
				//First check if this very filter has already been added
				if(filters != null && filters["owrHideParts"] == null) {
					Func<AvailablePart, bool> func = aPart => {
							if(aPart.partPrefab != null) {
								bool art1 = aPart == artifact1rec;
								bool art2 = aPart == artifact2rec;
								bool art1o = aPart == artifact1;
								bool art2o = aPart == artifact2;
								if(art1o || art2o) {
									return false;
								}
								if(art1 || art2) {
									return (art1 && GetProgressBit(28)) || (art2 && GetProgressBit(29));
								}
								return true;
							} else {
								return true;
							}
						};
					EditorPartListFilter<AvailablePart> filter = new EditorPartListFilter<AvailablePart>("owrHideParts", func, "Cannot be used in the Editor");
					filters.AddFilter(filter);
					Debug.Log("[Other Worlds] Added new filter to parts list");
				}
			}
		}
	}
}