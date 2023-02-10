using System;
using System.Collections.Generic;

using UnityEngine;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		//Oh yeah, it's ~~morbing~~ camera hijacking time
		/*
			Not only handles the camera, but also the light
										 /\ LAST SENTENCE, AN UTTER LIE
		*/
		public class OWP_Camera {
			private static FlightCamera cam;

			private static OWP_Camera_Backup camBackup = null;

			public static Animator camAnimation = null;

			private static GameObject camAnchor, camAnimObject;
			private static bool usingCustomCamera = false;

			public static void StartCameraAnimation(Transform sceneCenter, Vector3 camPosition, Quaternion camRotation, float FOV, int initialTransition = 0) {
				if(usingCustomCamera) return;

				BackupStockCamera();

				usingCustomCamera = true;

				Debug.Log("[Other Worlds] Scene's center at " + sceneCenter.position + ", " + sceneCenter.rotation);

				camAnchor = new GameObject("Cutscene Camera Anchor");
				camAnchor.transform.parent = sceneCenter;
				camAnchor.transform.localPosition = camPosition;
				camAnchor.transform.localRotation = camRotation;

				camAnimObject = new GameObject("Cutscene Camera Anchor for Animations");
				camAnimObject.transform.parent = camAnchor.transform;
				camAnimObject.transform.localPosition = new Vector3(0, 0, 0);
				camAnimObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

				cam.transform.parent = camAnimObject.transform;
				cam.transform.localPosition = new Vector3(0, 0, 0);
				cam.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
				cam.SetFoV(FOV);
				cam.SetDistanceImmediate(0);

				AttachAnimator(camAnimObject, initialTransition);
			}

			private static void AttachAnimator(GameObject anchor, int initialTransition) {
				RuntimeAnimatorController runtimeAnimation = OWP.LoadAsset<RuntimeAnimatorController>("assets/otherworlds/signalobjects/cameraanimations/cameracontroller.controller");
				if(runtimeAnimation == null) {
					Debug.Log("[Other Worlds] Runtime Animation Controller for camera couldn't be loaded! Check the Bundle and asset path!");
					return;
				}
				camAnimation = anchor.AddComponent<Animator>();
				camAnimation.runtimeAnimatorController = runtimeAnimation;
				camAnimation.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				camAnimation.updateMode = AnimatorUpdateMode.UnscaledTime; //Make sure KSP doesn't do the funny here by playing animations in the wrong speed
				camAnimation.applyRootMotion = false;
				camAnimation.speed = 1;

				camAnimation.SetInteger("main", initialTransition);
				camAnimation.enabled = true;
				Debug.Log("[Other Worlds] Added animator to camera, initial transition: " + initialTransition);
			}

			//Camera Tools teaches well
			public static void BackupStockCamera() {
				cam = FlightCamera.fetch;

				cam.SetTargetNone();
				camBackup = new OWP_Camera_Backup(cam);
				cam.DeactivateUpdate();
			}

			public static void RestoreStockCamera() {
				if(!usingCustomCamera) return;
				usingCustomCamera = false;

				camBackup.Restore(cam);
				camBackup = null;
				cam.ActivateUpdate();
				cam.SetTargetVessel(FlightGlobals.ActiveVessel);

				if(camAnimObject != null)
					GameObject.Destroy(camAnimObject);

				if(camAnchor != null)
					GameObject.Destroy(camAnchor);

				camAnimation = null;
			}
		}

		public class OWP_Camera_Backup {
			Vector3 origPosition;
			Quaternion origRotation;
			Vector3 origLocalPosition;
			Quaternion origLocalRotation;
			Transform origParent;
			float origNearClip;
			float origDistance;
			FlightCamera.Modes origMode;
			float origFov = 60;
			FlightCamera flightCamera;

			public OWP_Camera_Backup(FlightCamera cam) {
				origPosition = cam.transform.position;
				origRotation = cam.transform.rotation;
				origLocalPosition = cam.transform.localPosition;
				origLocalRotation = cam.transform.localRotation;
				origParent = cam.transform.parent;
				origNearClip = cam.mainCamera.nearClipPlane;
				origDistance = cam.Distance;
				origMode = cam.mode;
				origFov = cam.FieldOfView;
			}

			public void Restore(FlightCamera cam) {
				cam.transform.position = origPosition;
				cam.transform.rotation = origRotation;
				cam.transform.localPosition = origLocalPosition;
				cam.transform.localRotation = origLocalRotation;
				cam.transform.parent = origParent;
				cam.mainCamera.nearClipPlane = origNearClip;
				cam.SetDistanceImmediate(origDistance);
				cam.mode = origMode;
				cam.SetFoV(origFov);
			}
		}
	}
}