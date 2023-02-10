using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_Cutscene {
			public const string PREFAB_WAND = "assets/otherworlds/signalobjects/wanderer/wanderer.prefab";
			public static bool userInputEnabled = true;
			public static bool vesselsVisible = true;
			public static bool stockUIshowing = true;
			public static bool inThePresentUT = true;
			public static bool playing = false;
			public const bool PAUSE_FLIGHT_DRIVER = false;

			public static OWP_Cutscene cut = null;

			private static List<Renderer> invisibleMeshes;
			private static ScreenShot screenshotModule;

			private static GameObject cutsceneCenter;

			public static bool DEBUG_CUTSCENE = false; //Doesn't work anymore lol, If true, the cutscene will play, but the game will still behave like normal KSP Gameplay (No camera change, ui hiding, craft hiding etc.)

			/*
				Cutscene UI
			*/

			private static GameObject ui;
			private static Animator uiAnimator;
			private static TextMeshProUGUI uiText;
			private static OWP_DialogueField dialogueField = null;

			public static void StartUI() {
				ui = OWP.LoadAsset<GameObject>("assets/otherworlds/ui/cutscene/cutscenecanvas.prefab");
				ui.SetActive(true);
				uiAnimator = ui.transform.GetComponent<Animator>();
				uiText = ui.transform.Find("LowerBar").Find("Dialogue").GetComponent<TextMeshProUGUI>();

				uiAnimator.enabled = true;
			}

			private static void StartUIDialogue() {
				dialogueField = new OWP_DialogueField(uiText, uiAnimator, null, new List<string>{cut.text},
														cut.charAnimator, OWP_Camera.camAnimation);
				dialogueField.Start();
			}

			public static void DestroyUI() {
				GameObject.Destroy(ui);
				dialogueField = null;
				uiAnimator = null;
				uiText = null;
			}

			/*
				Update the Cutscene
			*/

			private static int phase = 0;
			private static double timer = 0;

			public static void Update() {
				if(cut != null) {
					//Text shows, instantiate character and start updating the dialogue
					if(phase == 0 && uiAnimator != null && uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeWait")) {
						Debug.Log("[Other Worlds] Cutscene entering to Phase 1");

						timer = 0;
						cut.charObject.SetActive(true);
						cut.PlayCharAnimation();
						TimeTravel(cut.targetUT);
						if(!DEBUG_CUTSCENE) {
							OWP_Camera.StartCameraAnimation(cutsceneCenter.transform, cut.cameraPosition, cut.cameraRotation, cut.cameraFOV, cut.animationTriggerTransition);
							HideKSPUI();
							MakeVesselsInvisible();
						}
						StartUIDialogue();

						phase = 1;
					}
					//Text has dissapeared, go back to the normal camera and time
					else if(phase == 1 && uiText != null && !uiText.gameObject.activeInHierarchy && uiAnimator.GetInteger("Close") > 0) {
						Debug.Log("[Other Worlds] Cutscene entering to Phase 2");

						cut.DestroyChar();
						if(!DEBUG_CUTSCENE) { 
							OWP_Camera.RestoreStockCamera();
							MakeVesselsVisible();
							ShowKSPUI();
							UserInputEnable();
						}
						DetimeTravel();
						if(cut == null) {
							return;
						}

						phase = 2;
					}
					//Final UI Animation has ended, start a timer to stop for good the cutscene logic
					else if(phase == 2 && uiAnimator != null && (uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeCloseLong") || uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeCloseQuick"))) {
						Debug.Log("[Other Worlds] Cutscene entering to Phase 3");

						phase = 3;
						timer = 6.0;
					}

					if(phase == 1) {
						dialogueField.Update();
					}

					if(phase == 3 && timer <= 0) {
						Debug.Log("[Other Worlds] Ending Cutscene, Timer reached");
						EndCutscene(); //End cutscene, it has made it to the end
					} else if(phase == 3) {
						timer -= Time.deltaTime;
					}
				}
			}

			/*
				Toggle Functions
			*/

			public static void StartCutscene(OWP_Cutscene cutscene, Transform center) {
				if(playing) return;
				playing = true;

				phase = 0;

				//Create a new GameObject to which attach the camera
				cutsceneCenter = new GameObject("Cutscene Center");
				cutsceneCenter.transform.localScale	= new Vector3(1, 1, 1);
				cutsceneCenter.transform.parent = center;
				cutsceneCenter.transform.localPosition = new Vector3(0, 0, 0);
				cutsceneCenter.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

				//Create the animation object
				cut = cutscene;

				Debug.Log("[Other Worlds] Starting cutscene " + cut);

				//Kidnap KSP
				cut.InstantiateChar(center);
				cut.charObject.SetActive(false);
				if(!DEBUG_CUTSCENE) {
					UserInputDisable();
				}
				StartUI();
			}

			//To use by other classes
			public static void EndCutscene() {
				if(!playing) return;
				playing = false;

				cut.DestroyChar();
				cut = null;
				
				if(!DEBUG_CUTSCENE) OWP_Camera.RestoreStockCamera();

				GameObject.Destroy(cutsceneCenter);

				//Un-kidnap KSP
				DetimeTravel();
				if(!DEBUG_CUTSCENE) {
					MakeVesselsVisible();
					UserInputEnable();
				}
				DestroyUI();
				ShowKSPUI();
			}

			/*
				Aux Functions
			*/

			public static void MakeVesselsInvisible() {
				if(!vesselsVisible) return;
				vesselsVisible = false;
				invisibleMeshes = new List<Renderer>();

				foreach(Vessel vessel in FlightGlobals.fetch.vesselsLoaded) {
					//It's a EVA Kerbal
					if(vessel.isEVA) {
						KerbalEVA eva = vessel.evaController;
						Part part = eva.part;
						Renderer[] rs = part.GetComponentsInChildren<Renderer>(true);
						foreach(Renderer r in rs) {
							if(r.enabled) {
								invisibleMeshes.Add(r);
								r.enabled = false;
							}
						}
					//It's a normal vessel
					} else {
						foreach(Part part in vessel.Parts) {
							Renderer[] rs = part.GetComponentsInChildren<Renderer>(true);
							foreach(Renderer r in rs) {
								if(r.enabled) {
									invisibleMeshes.Add(r);
									r.enabled = false;
								}
							}
						}
					}
				}

				Debug.Log("[Other Worlds] Made Vessels Invisible");
			}

			public static void MakeVesselsVisible() {
				if(vesselsVisible) return;
				vesselsVisible = true;

				foreach(Renderer r in invisibleMeshes) {
					if(r.enabled) Debug.LogWarning("[Other Worlds] [MakeVesselsInvisible] Renderer of object " + r.gameObject.name + " was supposed to be invisible, but it wasn't!");
					r.enabled = true;
				}

				Debug.Log("[Other Worlds] Made Vessels visible");

				invisibleMeshes.Clear();
				invisibleMeshes = null;
			}

			public static void UserInputDisable() {
				if(!userInputEnabled) return;

				userInputEnabled = false;

				InputLockManager.SetControlLock(ControlTypes.All, "owp");
			}

			public static void UserInputEnable() {
				if(userInputEnabled) return;

				InputLockManager.RemoveControlLock("owp");

				userInputEnabled = true;
			}

			//Warning, this hides the UI permanently, this means you won't be able to use F2 to unhide it. Use ShowUI for that
			public static void HideKSPUI() {
				if(!stockUIshowing) return;
				stockUIshowing = false;

				screenshotModule = OWP_Util.FindScreenshotManager();
				if(screenshotModule != null) screenshotModule.allowUiHidingWithF2 = false;

				GameEvents.onHideUI.Fire(); //Using a GameEvent to hide the UI, savage (But kinda based ngl)
			}

			public static void ShowKSPUI() {
				if(stockUIshowing) return;
				stockUIshowing = true;

				if(screenshotModule != null) {
					screenshotModule.allowUiHidingWithF2 = true;
					screenshotModule = null;
				}
				GameEvents.onShowUI.Fire();
			}

			static double UTBackup;
			static double ThermalMaxIntegratorWarpBackup;
			static double thermalIntegrationMinStepBackup;

			static ConfigNode deTimeTravelQuicksave;

			public static void TimeTravel(double UTNew) {
				//Will make FlightIntegrator complain, saying it has to recalculate thermal stuff (It thinks vessels are unloaded due to time difference)
				if(!inThePresentUT || !OWP.DO_TIMEWARP_TO_PAST_ON_CUTSCENES) return;

				//Make "quicksave" so we can go back to where everything was not fucked
				deTimeTravelQuicksave = OWP_Util.SaveSavefileAsConfigNode();

				if(deTimeTravelQuicksave == null) {
					//Don't time travel. The game couldn't be quicksave, wouldn't have anything to go back to
					inThePresentUT = true;
					return;
				}

				inThePresentUT = false;
	
				Debug.LogWarning("[Other Worlds] Get out of the way Nomai, we going **past** past, not 20 minutes past");

				CollisionEnhancer.bypass = true;
				FlightGlobals.overrideOrbit = true;
				HighLogic.CurrentGame.Parameters.Flight.CanAutoSave = false; //No autosave in cutscene please thank you
				UTBackup = Planetarium.GetUniversalTime();
				Planetarium.SetUniversalTime(UTNew);

				//Try to stop the spam
				thermalIntegrationMinStepBackup = PhysicsGlobals.ThermalIntegrationMinStep;
				PhysicsGlobals.ThermalIntegrationMinStep = Double.PositiveInfinity;

				//Disable autosave, must restart coroutine for autosave, either manually or by switching "scene"
				//HighLogic.CurrentGame.Parameters.Flight.CanAutoSave = canAutoSaveBackup;
			}

			public static void DetimeTravel() {
				if(inThePresentUT || !OWP.DO_TIMEWARP_TO_PAST_ON_CUTSCENES) return;
				inThePresentUT = true;

				CollisionEnhancer.bypass = false;
				FlightGlobals.overrideOrbit = false;
				PhysicsGlobals.ThermalIntegrationMinStep = thermalIntegrationMinStepBackup;
				//Planetarium.SetUniversalTime(UTBackup);
				
				EndCutscene(); //Will call DetimeTravel again, but will not end on a loop	

				ConfigNode node = deTimeTravelQuicksave;
				deTimeTravelQuicksave = null;
				OWP_Util.LoadSavefileAsConfigNode(node, "cutscene_quicksave");			
			}




















			/*
				Specific Cutscene Data
			*/

			double targetUT = 0;
			Vector3 cameraPosition;
			Quaternion cameraRotation;
			float cameraFOV;
			string charPrefab;
			int animationTriggerTransition;
			Vector3 charPosition;
			Quaternion charRotation;

			//On-Cutscene objects
			GameObject charObject;
			public Animator charAnimator;
			string text = "#(showui=true)Hello!!!!\nIt's me, bob.#(timenext=2.5) #(switchanim=1,clear=1)\n<b>Yoo</b>oo#(timenext=2.5) It's me#(close=2)"; //Default text, just in case

			public OWP_Cutscene(int animationTriggerTransition, Vector3 charPosition, Vector3 charRotation, Vector3 camPosition, Vector3 camRotation, double targetUT, float FOV = 45) {
				this.charPrefab = PREFAB_WAND;
				this.animationTriggerTransition = animationTriggerTransition;
				this.charPosition = charPosition;
				this.charRotation = Quaternion.Euler(charRotation);
				this.cameraFOV = FOV;
				this.targetUT = targetUT;

				cameraPosition = camPosition;
				cameraRotation = Quaternion.Euler(camRotation);
			}

			public void SetText(string text) {
				this.text = text;
			}

			public override string ToString()
			{
				return "(Cutscene)";
			}

			public void InstantiateChar(Transform center) {
				charObject = OWP.LoadAsset<GameObject>(charPrefab);
				charObject.transform.parent = center;
				charObject.transform.localPosition = charPosition;
				charObject.transform.localRotation = charRotation;
				charObject.transform.parent = null;
				charObject.SetActive(false);

				charAnimator = charObject.transform.Find("Armature").GetComponent<Animator>();
				charAnimator.enabled = true;
			}

			public void PlayCharAnimation() {
				if(!OWP_Util.AnimatorHasParameter(charAnimator, "main")) {
					Debug.LogError("[Other Worlds] Error! Couldn't find parameter 'main' in the Wanderer's animation");
				} else {
					charAnimator.SetInteger("main", animationTriggerTransition);
					Debug.Log("[Other Worlds] Playing Animation main=" + animationTriggerTransition);
				}
			}

			public void DestroyChar() {
				if(charObject != null) {
					GameObject.Destroy(charObject);
					charObject = null;
					charAnimator = null;
				}
			}
		}
	}
}