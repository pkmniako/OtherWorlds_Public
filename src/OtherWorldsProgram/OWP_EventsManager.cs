using System;
using System.Collections.Generic;

using UnityEngine;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_EventsManager {
			/*
				Add listeners to events
			*/
			public void SetupEventsListeners() {
				if(GameEvents.OnGameSettingsApplied != null)
					GameEvents.OnGameSettingsApplied.Add(OnGameSettingsApplied);
				if(GameEvents.onVesselSituationChange != null)
					GameEvents.onVesselSituationChange.Add(OnVesselSituationChange);
				if(GameEvents.onGamePause != null)
					GameEvents.onGamePause.Add(OnGamePause);
				if(GameEvents.onGameUnpause != null)
					GameEvents.onGameUnpause.Add(OnGameUnpause);
				if(GameEvents.onGameSceneLoadRequested != null)
					GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
				if(GameEvents.onGameSceneSwitchRequested != null)
					GameEvents.onGameSceneSwitchRequested.Add(OnGameSceneSwitchRequested);
				if(GameEvents.onSceneConfirmExit != null)
					GameEvents.onSceneConfirmExit.Add(OnSceneConfirmExit);
				if(GameEvents.onFlightReady != null)
					GameEvents.onFlightReady.Add(OnFlightReady);

				//Next one used for part filtering editing
				if(GameEvents.onEditorShowPartList != null)
					GameEvents.onEditorShowPartList.Add(OnEditorShowPartList);
				
				//To check if there's kerbals with the artifacts
				if(GameEvents.onModuleInventoryChanged != null)
					GameEvents.onModuleInventoryChanged.Add(OnInventoryChange);
				if(GameEvents.onModuleInventorySlotChanged != null)
					GameEvents.onModuleInventorySlotChanged.Add(OnInventorySlotChange);
				if(GameEvents.onVesselChange != null)
					GameEvents.onVesselChange.Add(OnVesselChange);
				if(GameEvents.onVesselSwitching != null)
					GameEvents.onVesselSwitching.Add(OnVesselSwitch);
			}

			/*
				Functions to hook
			*/
			private void OnGameSettingsApplied() {
				OWP_Music.UpdateVolume();
			}
			private void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> a) {
				OnVesselOrInventoryChange();

				if(a.from == Vessel.Situations.ESCAPING && a.to == Vessel.Situations.ORBITING && a.host.orbit.referenceBody != null && a.host.orbit.referenceBody.name.Equals("Cercani") && a.host.isActiveVessel) {
					OWP_Music.PlaySong("cercani1", force: true, once: true);
				}
			}
			private void OnGamePause() {
				OWP_Music.PausePlaying();
			}
			private void OnGameUnpause() {
				OWP_Music.ContinuePlaying();
			}
			private void OnGameSceneLoadRequested(GameScenes scene) {
				//Debug.Log("[Other Worlds] OnGameSceneLoadRequested()");
			}
			private void OnGameSceneSwitchRequested(GameEvents.FromToAction<GameScenes, GameScenes> scenes) {
				//Debug.Log("[Other Worlds] OnGameSceneSwitchRequested()");
				OWP_Ending.StopCutscene();
				OWP_Music.StopMusic();
				OWP_Popout.OnSceneChange();
			}
			private void OnSceneConfirmExit(GameScenes scenes) {
				OWP_LocPillar.ResetCircling();
				OWP.ui.DestroyUI(); //Destroy the tablet UI
				OWP_Cutscene.EndCutscene();
				OWP_Ending.StopCutscene();
				OWP_Music.StopMusic();
			}
			private void OnInventoryChange(ModuleInventoryPart inv) {
				OnVesselOrInventoryChange();
			}
			private void OnInventorySlotChange(ModuleInventoryPart inv, int index) {
				OnVesselOrInventoryChange();
			}
			private void OnVesselChange(Vessel vessel) {
				OnVesselOrInventoryChange();
			}
			private void OnVesselSwitch(Vessel orig, Vessel target) {
				OnVesselOrInventoryChange();
			}
			private void OnEditorShowPartList() {
				OnSceneConfirmExit(GameScenes.FLIGHT);
				OWP.SetPartVisibilityOnEditor();
				OWP_Popout.OnEnterEditor();
			}

			private void OnFlightReady() {
				/*
				if(OWP.locations != null) {
					foreach(OWP_LocData loc in OWP.locations) {
						if(loc.gameObject != null) {
							loc.OnLoad();
						}
					}
				}
				*/
			}

			//Amalgamation of multiple events
			public void OnVesselOrInventoryChange() {
				OWP.ui.CheckIfArtifactOnCraft();
				OWP_LocPillar.CheckForValidActivationVessel();
			}
		}
	}
}