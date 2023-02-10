using System;
using System.Collections.Generic;

using UnityEngine;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_LocPillar : OWP_LocData {

			bool alreadyOn = false;
			float activationTimer = 0;

			Animator animator;

			GameObject pillar = null;
			public Transform boneMain = null; // By Default should be -90 0 0
			public Transform boneLens = null; //By Default should be 0 0 0
			public Transform boneLaser = null; //By Default should be 1 1 1, but Y going to -2000 when on
			public double boneAnimationTimer = -1;

			public const float OPENING_TIMER = 300.0f;

			Quaternion _pqsInverseRotation;

			public OWP_LocPillar(string bodyName, string pqsName, float interactionDistance = 2.0f) :
				base(bodyName, pqsName, interactionDistance) {

				}

			//Get closest pillar to active vessel
			public OWP_LocPillar GetClosestPillar() {
				Vessel active = FlightGlobals.ActiveVessel;

				if(active == null || OWP.pillars == null)
					return null;
				
				double dist = 0;
				OWP_LocPillar closestPillar = null;
				foreach(OWP_LocPillar pillarCandidate in OWP.pillars) {
					double dist2 = pillarCandidate.DistanceFrom(active);
					if(closestPillar == null || dist > dist2) {
						dist = dist2;
						closestPillar = pillarCandidate;
					}
				}

				return closestPillar;
			}
			
			public override void OnLoad() {
				Debug.Log("[Other Worlds] Pillar OnLoad()");
				alreadyOn = OWP.GetProgressBit(id);

				//Get Animator
				animator = gameObject.GetComponent<Animator>();
				if(animator == null) {
					Debug.Log("[Other Worlds] Location " + body.name + " / " + pqs.name + " doesn't have an Animator, adding manually");
					RuntimeAnimatorController runtimeAnimation = OWP.LoadAsset<RuntimeAnimatorController>("assets/otherworlds/structures/pillar/spike.controller");
					if(runtimeAnimation == null) {
						Debug.Log("[Other Worlds] Runtime Animation Controller for pillar couldn't be loaded! Check the Bundle and asset path!");
					}
					animator = gameObject.AddComponent<Animator>();
					animator.runtimeAnimatorController = runtimeAnimation;
					animator.updateMode = AnimatorUpdateMode.UnscaledTime; //Make sure KSP doesn't do the funny here by playing animations in the wrong speed
				}

				//Play Corresponding animation
				if(alreadyOn) {
					Debug.Log("[Other Worlds] Location " + body.name + " / " + pqs.name + " is loaded! (Progress is 1)");
					SetAnimation(restorePillarState: false, ifForceOpen: true, instantanious: true);
				} else {
					Debug.Log("[Other Worlds] Location " + body.name + " / " + pqs.name + " is loaded! (Progress is 0)");
					SetAnimation(restorePillarState: false, ifForceOpen: false, instantanious: true);
				}
					boneAnimationTimer = -1;

				//Get bones
				if(boneMain == null) {
					bool abort = false;

					boneMain = gameObject.GetChild("Armature").transform;
					if(boneMain == null) {
						abort = true;
						Debug.LogError("[Other Worlds] Location '" + pqs.name + "' doesn't have bone 'Armature'");
					}

					boneLens = boneMain.gameObject.GetChild("AllPillar").GetChild("Lens 1").transform;
					if(boneLens == null) {
						abort = true;
						Debug.LogError("[Other Worlds] Location '" + pqs.name + "' doesn't have bone 'Armature/AllPillar/Lens 1'");
					}

					boneLaser = boneLens.gameObject.GetChild("Laser 1").transform;
					if(boneLaser == null) {
						abort = true;
						Debug.LogError("[Other Worlds] Location '" + pqs.name + "' doesn't have bone 'Armature/AllPillar/Lens 1/Laser 1'");
					}

					if(abort)
						boneMain = null;
				}
			}

			//restorePillarState when closing a cutscene, ifForceOpen true if you want to open it, instantanious true if fast
			public void SetAnimation(bool restorePillarState, bool ifForceOpen, bool instantanious) {
				if(animator != null) {
					if(restorePillarState) {
						animator.SetBool("Open", false);
						animator.SetBool("OpenFast", !alreadyOn);
					} else {
						animator.SetBool("Open", ifForceOpen && !instantanious);
						animator.SetBool("OpenFast", ifForceOpen && instantanious);
					}
				}
			}

			bool wasNearPillarPreviousFrame = false;
			
			public override void OnLateUpdate() {
				_pqsInverseRotation = Quaternion.Inverse(pqs.PositioningPoint.rotation);
				CheckCircling();
				UpdateLaserPositioning();

				bool closeToPillar = FlightGlobals.ActiveVessel != null ? DistanceFrom(FlightGlobals.ActiveVessel) < 100 : false;
				if(closeToPillar != wasNearPillarPreviousFrame) {
					if(closeToPillar)
						OWP_Music.PlaySong("spike1");
					else
						OWP_Music.StopMusic();
				}
				wasNearPillarPreviousFrame = closeToPillar;
			}

			public bool circlingCloseEnough = false;
			public double circlingLastAngle = 0;
			public double circlingAccumulatedAngle = 0;
			public static bool activeVesselCanActivatePillar = false;

			public static void ResetCircling() {
				foreach(OWP_LocPillar loc in OWP.pillars) {
					loc.circlingLastAngle = 0;
					loc.circlingAccumulatedAngle = 0;
					loc.circlingCloseEnough = false;
				}
				activeVesselCanActivatePillar = false;
			}

			public static void CheckForValidActivationVessel() {
				Vessel active = FlightGlobals.ActiveVessel;
				if(active == null) return;

				activeVesselCanActivatePillar = false;

				//Is EVA Kerbal
				if(active.isEVA && active.evaController != null) {
					ModuleInventoryPart inv = active.evaController.ModuleInventoryPartReference;
					if(inv == null) return;
					foreach(StoredPart sp in inv.storedParts.Values) {
						if(sp.partName.Equals(OWP.artifact2.name) || sp.partName.Equals(OWP.artifact2rec.name)) {
							if(!activeVesselCanActivatePillar)
								ResetCircling();
							activeVesselCanActivatePillar = true;
							break;
						}
					}
				}
			}

			private void CheckCircling() {
				if(OWP.GetProgressBit(id)) return;

				Vessel active = FlightGlobals.ActiveVessel;
				if(activeVesselCanActivatePillar && active != null) {
					double distance = DistanceFrom(active);
					if(distance < 10.0) {
						Vector3 vesselPosition = active.GetWorldPos3D();
						Vector3 vesselRelPosition = _pqsInverseRotation * (vesselPosition - pqs.PositioningPoint.position);

						double angle = Math.Atan2(vesselRelPosition.z, vesselRelPosition.x);

						if(!circlingCloseEnough) {
							circlingAccumulatedAngle = 0;
						} else if((circlingLastAngle < 0) == (angle < 0)){
							double deltaAngle;
							deltaAngle = (circlingLastAngle - angle)%360.0;
							circlingAccumulatedAngle += deltaAngle;
						}

						if(Math.Abs(circlingAccumulatedAngle) >= 2 * Math.PI) {
							Activate();
						}

						circlingLastAngle = angle;

						circlingCloseEnough = true;
					} else {
						circlingCloseEnough = false;
					}
				}
			}

			private void Activate() {
				//Activate if full loop
				OWP.SetProgressBit(id, true);
				alreadyOn = true;
				SetAnimation(restorePillarState: false, ifForceOpen: true, instantanious: false);
				boneAnimationTimer = ACTIVATION_INTRO_TIMER_START;
				OWP_Music.PlaySong("spike2", force: true, once: true);

				if(OWP_PlanetManager.ProgressShowPlanets()) {
					ScreenMessages.PostScreenMessage(OWP_LOC.Get("Event_New_Planet"), 10f);
					OWP_PlanetManager.instance.UpdateMap(); //Might be a null reference lmao
				}
			}

			//TODO: DELETE THESE 3 VARIABLES
			static Vector3 testVector = new Vector3(1, 1, 1);
			public double testAngleAzimuth, testAngleAltitude;
			public const float ACTIVATION_INTRO_TIMER_START = 36.5f;

			private void UpdateLaserPositioning() {
				//Opening animation will run for 10 seconds (16 beats in the music)
				//Lining the laser should also be the same time

				//Pillar has a azimuth of 0 when looking at -Z

				if(boneMain == null) return;

				if(alreadyOn && boneAnimationTimer <= ACTIVATION_INTRO_TIMER_START) {
					Vector3 relativePosition = OWP_PlanetManager.PLANET_OWR1 != null ? _pqsInverseRotation * OWP_PlanetManager.PLANET_OWR1.position : new Vector3(1, 1, 1);
					relativePosition.Normalize();

					double targetAzimuth = Math.Atan2(-relativePosition.x, -relativePosition.z) * Mathf.Rad2Deg;
					double targetAltitude = Math.Acos(relativePosition.y) * Mathf.Rad2Deg;

					double newAzimuth, newAltitude;

					//Pillar still being deployed
 					if(boneAnimationTimer > 0) {
						float lerp = 1 - (float)boneAnimationTimer/ACTIVATION_INTRO_TIMER_START;
						lerp = Mathf.SmoothStep(0, 1, lerp);

						newAzimuth = Mathf.LerpAngle(0, (float)targetAzimuth, lerp);
						newAltitude = Mathf.LerpAngle(0, (float)targetAltitude, lerp);
					}
					//Pillar already deployed (Instant pointing to)
					else {
						newAzimuth = targetAzimuth;
						newAltitude = targetAltitude;
					}

					bool targetUnderHorizon = newAltitude > 90.0;
					newAltitude = targetUnderHorizon ? 90.0 : newAltitude;

					testAngleAltitude = newAltitude;
					testAngleAzimuth = newAzimuth;

					boneMain.localRotation = Quaternion.Euler(-90, (float)newAzimuth, 0);
					boneLens.localRotation = Quaternion.Euler(-(float)newAltitude, 0, 0);

					boneLaser.localScale = new Vector3(1, (targetUnderHorizon || boneAnimationTimer > 0) ? 1 : -2000, 1);
				} else {
					boneMain.localRotation = Quaternion.Euler(-90, 0, 0);
					boneLens.localRotation = Quaternion.Euler(0, 0, 0);
					boneLaser.localScale = new Vector3(1, 1, 1);
				}

				if(boneAnimationTimer > 0) {
					boneAnimationTimer -= Time.deltaTime;
				}
			}
			
			public override void OnPeriodicUpdate() {
				
			}
		}
	}
}