using System;
using System.Collections.Generic;
using UnityEngine;

using KSP.UI.Screens.Mapview;

using TMPro;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		[KSPAddon(KSPAddon.Startup.MainMenu, true)]
		public class OWP_PlanetManager : MonoBehaviour
		{
			public static OWP_PlanetManager instance;
			public static List<CelestialBody> PLANETS;
			public static CelestialBody PLANET_OWR1 {
				get { return (PLANETS != null && PLANETS.Count > 1) ? PLANETS[0] : null; }
			}
			public static CelestialBody PLANET_OWR2 {
				get { return (PLANETS != null && PLANETS.Count > 2) ? PLANETS[1] : null; }
			}

			private static string[] PLANETS_NAMES = {"OWR1", "OWR2"};

			private static bool FirstStart = true;

			private static bool spawnCustomWaypoints = false;

			private static double planets_rescale = -1.0;

			public void Start() {
				if(FirstStart) {
					if(GameEvents.OnMapEntered != null)
						GameEvents.OnMapEntered.Add(UpdateMap);

					//Get the funny planets
					PLANETS = new List<CelestialBody>();
					foreach(string cbName in PLANETS_NAMES)
						foreach(CelestialBody cb in FlightGlobals.Bodies)
							if(cbName.Equals(cb.name)) {
								PLANETS.Add(cb);
								Debug.Log("[Other Worlds] Planet Manager, adding planet " + cbName);
							}

					FirstStart = false;
					Debug.Log("[Other Worlds] Planet Manager started");
				} else {
					//Debug.Log("[Other Worlds] Planet Manager already started");
				}
			}

			public void UpdateMap() {
				//Hide (Or show!) specific planets

				bool visibility = ProgressShowPlanets();
				bool namesVisibility = ProgressShowNames();

				foreach(CelestialBody cb in PLANETS) {
					PlanetariumSetPlanetVisibility(cb, visibility);
					ManageHiddenName(cb, visibility, namesVisibility);
				}

				if(spawnCustomWaypoints) {
					OWP_Util.NewWaypoint(OWP_LOC.Get("Intro_Waypoint_Vassa"), "Vassa", -174.5783, 15.58833);
					//OWP_Util.NewWaypoint(OWP_LOC.Get("Intro_Waypoint_C2-1"), "C2-1", 78.021667, -19.89889, 10000); //Confused a lot of beta testers
					spawnCustomWaypoints = false;
				}
			}

			public static bool ProgressShowPlanets() {
				//If true, the player can see the new planets
				return (OWP.GetProgressBit(13) && OWP.GetProgressBit(24)) || OWP.GetProgressBit(26);
			}

			public static bool ProgressShowNames() {
				return OWP.GetProgressBit(26);
			}

			public static void PlanetariumSetPlanetVisibility(CelestialBody cb, bool visible) {
				PlanetariumCamera cam = PlanetariumCamera.fetch;
				if(cam != null && cam.targets != null) {
					if(!visible && cam.targets.Contains(cb.MapObject))
						cam.targets.Remove(cb.MapObject);
					else if(visible && !cam.targets.Contains(cb.MapObject))
						cam.targets.Add(cb.MapObject); 
				}
				cb.DiscoveryInfo.SetLevel(visible ? DiscoveryLevels.Owned : DiscoveryLevels.None);
			}

			public static void ManageHiddenName(CelestialBody cb, bool visibility, bool showRealName) {
				if(showRealName) {
					string name = cb.name;
					cb.bodyDisplayName = OWP_LOC.Get("CelestialBody_" + cb.name + "_Name");
					cb.bodyDescription = OWP_LOC.Get("CelestialBody_" + cb.name + "_Desc");
				} else if(visibility) {
					cb.bodyDisplayName = "?????????^N";
					cb.bodyDescription = "?????????";
				} else {
					cb.bodyDisplayName = "DEBUG Barycenter C^N";
					cb.bodyDescription = "Debug thing DO NOT TOUCH >:(";
				}
			}

			public static void SpawnCustomWaypoints() {
				spawnCustomWaypoints = true;
			}

			public static double GetRescale() {
				//Try to guess what the scale of the user's planets is, like x2.7
				//This is done by comparing the size of object 1 (homeworld) to that of what Kerbin would be (600km)
				if(planets_rescale > 0) return planets_rescale;

				if(PLANET_OWR1 != null) {
					planets_rescale = PLANET_OWR1.Radius / 10800000;
					Debug.Log($"[Other Worlds] Planet Manager guess of the rescale factor: x{planets_rescale}");
					return planets_rescale;
				} else {
					return 1.0; //I don't trust KSP
				}
			}
		}
	}
}