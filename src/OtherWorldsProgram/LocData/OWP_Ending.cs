using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_Ending : OWP_LocData {
			//Instance
			public static OWP_Ending fetch;

			//Constants
			private const float TRIGGER_DISTANCE = 2.0f;
			private const float CLOSE_TO_DISTANCE = 10000f;
			private const string PREFAB_WAND = "assets/otherworlds/signalobjects/wanderer/wanderer_end.prefab";
			private const string PREFAB_DOOR_ANIMATOR = "";
			private const string PREFAB_UI = "";
			public const string INPUTLOCK_NAME = "owp_ending";

			//Scene objects
			GameObject trigger;
			GameObject light;
			GameObject door;
			Animator doorAnim;
			GameObject wanderer;
			Animator wandAnim;
			Transform wandLook;

			//UI
			GameObject ui;
			Animator uiAnimator;
			TextMeshProUGUI uiText;
			Button uiButtonTemplate;
			OWP_DialogueField dialogueField;

			//Variables
			bool closeToArea = false;
			bool playing = false;
			
			public OWP_Ending() : base("OWR2", "Island_OW2", 5.0f) {}

			public override void OnLoad() {
				fetch = null;

				trigger = gameObject.transform.Find("Trigger").gameObject;
				if(trigger == null) {
					Debug.LogError("[Other Worlds] Ending Area: Can't find object 'Trigger'");
					return;
				}
				light = gameObject.transform.Find("LIGHT_LAST/GlowyBits").gameObject;
				if(light == null) {
					Debug.LogError("[Other Worlds] Ending Area: Can't find object 'LIGHT_LAST/GlowyBits'");
					return;
				}
				door = gameObject.transform.Find("DOOR/DoorHinge").gameObject;
				if(door == null) {
					Debug.LogError("[Other Worlds] Ending Area: Can't find object 'DOOR/DoorHinge'");
					return;
				}

				fetch = this;
				playing = false;
			}
			
			public override void OnLateUpdate() {
				Vessel active = FlightGlobals.ActiveVessel;
				if(active == null) return;

				//Make sure certain things are correct
				bool closeToAreaNew = DistanceToTrigger(active.gameObject.transform) < CLOSE_TO_DISTANCE;
				if(closeToArea != closeToAreaNew) {
					closeToArea = closeToAreaNew;
					if(closeToArea) {
						OnCloseToArea();
					} else {
						OWP_Music.StopMusic();
					}
				}

				//Trigger
				if(active.isEVA && !playing && closeToArea && DistanceToTrigger(active.gameObject.transform) < TRIGGER_DISTANCE && !OWP.GetProgressBit(26)) {
					StartCutscene();
				}

				if(playing) {
					_PointCharAtVessel(active);
					if(dialogueField != null)
						dialogueField.Update();
				}

				_RunCutscene();
			}

			private void _PointCharAtVessel(Vessel vessel) {
				if(wandLook != null && vessel != null) {
					if(dialogueField != null && dialogueField.variable == 1) {
						Vector3 posRel = wandLook.parent.InverseTransformPoint(vessel.gameObject.transform.position);
						float leftRightAngle = Mathf.Atan2(posRel.x, posRel.z) * Mathf.Rad2Deg;
						leftRightAngle = Mathf.Clamp(leftRightAngle, -25, 25);
						float upDownAngle = -Mathf.Asin(posRel.y / posRel.magnitude) * Mathf.Rad2Deg * 0.5f;
						if(upDownAngle < 0) upDownAngle *= 0.25f;
						upDownAngle = Mathf.Clamp(upDownAngle, -15, 7);
						wandLook.localRotation = Quaternion.Euler(upDownAngle, leftRightAngle, 0);
					} else {
						wandLook.localRotation = Quaternion.Euler(0, 0, 0);
					}
				}
			}

			/*
				All transitions of the cutscene
			*/

			double timer = 0;
			double timeStampEnding = -1;

			void _RunCutscene() {
				try {
					double timerPrev = timer;
					if(playing || doorAnim != null)
						timer += Time.deltaTime;

					if(playing) {
						if(timerPrev < 1.0 && timer >= 1.0)
							_CutsceneTransition1();

						if(dialogueField != null && dialogueField.variable == 2 && timeStampEnding < 0) {
							timeStampEnding = timer;
							StopCutscene(dontDeleteDoorAnim: true);
						}
					}

					if(doorAnim != null && (timeStampEnding >= 0) && (timer - timeStampEnding) > 5.0)
						StopCutscene();
				} catch(NullReferenceException) {
					Debug.LogError($"[Other Worlds] [Ending Debug] NulLReferenceException\n\tIs doorAnim null?: {doorAnim == null}\n\tIs dialogueField null?: {dialogueField == null}\n\tplaying?: {playing}");
				}
			}

			void _CutsceneTransition0() {
				//Spawn Char
				wanderer = OWP.LoadAsset<GameObject>(PREFAB_WAND);
				wanderer.transform.parent = gameObject.transform;
				wanderer.transform.localPosition = new Vector3(32.208f, 51.15402f, 239.116f);
				wanderer.transform.localRotation = Quaternion.Euler(0, -29.1062f, 0);
				wanderer.transform.parent = null;
				wanderer.SetActive(false);
				wandAnim = wanderer.transform.Find("Armature").GetComponent<Animator>();
				wandLook = wanderer.transform.Find("Armature/MainLower/MainUpperTurning");

				//Set input lock
				InputLockManager.SetControlLock(ControlTypes.TIMEWARP | ControlTypes.VESSEL_SWITCHING | ControlTypes.QUICKSAVE | ControlTypes.PAUSE, INPUTLOCK_NAME);

				//Set warp to x1
				TimeWarp.SetRate(0, false, false);

				//Animator for the door
				RuntimeAnimatorController runtimeAnimationDoor = OWP.LoadAsset<RuntimeAnimatorController>("assets/otherworlds/structures/island/dooranimator.controller");
				doorAnim = door.AddComponent<Animator>();
				doorAnim.runtimeAnimatorController = runtimeAnimationDoor;
				doorAnim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				doorAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
				doorAnim.enabled = true;
				doorAnim.SetInteger("sub", 1);
			}

			void _CutsceneTransition1() {
				//Turn on light
				light.SetActive(true);

				//Show them
				wanderer.SetActive(true);

				//Hide KSP's UI
				OWP_Cutscene.HideKSPUI();

				//Show custom UI
				fetch.ui = OWP.LoadAsset<GameObject>("assets/otherworlds/ui/cutscene/cutscenecanvas.prefab");
				fetch.ui.SetActive(true);
				fetch.uiAnimator = fetch.ui.transform.GetComponent<Animator>();
				fetch.uiText = fetch.ui.transform.Find("LowerBar").Find("Dialogue").GetComponent<TextMeshProUGUI>();
				fetch.uiAnimator.enabled = true;
				fetch.uiButtonTemplate = fetch.ui.transform.Find("LowerBar").Find("ButtonLayer").Find("ButtonTemplate").GetComponent<Button>();

				//Start Dialogue
				fetch.dialogueField = new OWP_DialogueField(fetch.uiText, fetch.uiAnimator, fetch.uiButtonTemplate,
				OWP_LOC.LOCEnding(), wandAnim, doorAnim);
				fetch.dialogueField.Start();

				//Yes
				OWP_Music.PlaySong("us", force: true);
			}

			/*
				Other
			*/

			void StartCutscene() {
				playing = true;
				timer = 0;
				timeStampEnding = -1;
				_CutsceneTransition0();

				Debug.Log("[Other Worlds] Hello");
			}

			void OnCloseToArea() {
				//Has the ending already been played?
				bool done = OWP.GetProgressBit(26);

				light.SetActive(done);

				OWP_Music.PlaySong("dark");

				//Debug.Log("[Other Worlds] Now close to the ending area");
			}

			public static float DistanceToTrigger(Transform t) {
				if(fetch != null && fetch.gameObject != null && fetch.trigger != null) {
					return (t.position - fetch.trigger.transform.position).magnitude;
				}
				return -1.0f;
			}

			//Stop the goddamn cutscene
			public static void StopCutscene(bool dontDeleteDoorAnim = false) {
				if(fetch != null && fetch.gameObject != null && fetch.playing) {
					Debug.Log("[Other Worlds] [Ending Debug] Stopping ending...");
					fetch.playing = false;
					fetch.timer = 0;
					InputLockManager.RemoveControlLock(INPUTLOCK_NAME);

					GameObject.Destroy(fetch.wanderer);
					fetch.wanderer = null;

					OWP_Cutscene.ShowKSPUI();

					GameObject.Destroy(fetch.ui);
					fetch.dialogueField = null;
					fetch.uiAnimator = null;
					fetch.uiText = null;

					OWP_Music.PlaySong("dark", force: true);

					OWP.SetProgressBit(26, true);
				}

				if(!dontDeleteDoorAnim && (fetch.doorAnim != null)) {
					Debug.Log("[Other Worlds] [Ending Debug] Removed Door Animation...");
					GameObject.Destroy(fetch.doorAnim);
					fetch.doorAnim = null;
					fetch.door.transform.localRotation = Quaternion.Euler(0, -90, 0);
				}
			}
		}
	}
}