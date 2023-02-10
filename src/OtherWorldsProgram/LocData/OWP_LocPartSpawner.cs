using System;
using System.Collections.Generic;

using UnityEngine;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_LocPartSpawner : OWP_LocData {
			int artifactID; //Which of the two artifacts does this spawn, 1 or 2
			double latitude, longitude;

			public OWP_LocPartSpawner(string bodyName, string pqsName, int artifactID, double latitude, double longitude) :
				base(bodyName, pqsName, 5.0f) {
					this.artifactID = artifactID;
					this.latitude = latitude;
					this.longitude = longitude;
				}

			public bool IsSpawned() {
				if(artifactID == 1)
					return OWP.artifact1Spawned;
				return OWP.artifact2Spawned;
			}

			public void SetAsSpawned() {
				if(artifactID == 1)
					OWP.artifact1Spawned = true;
				else
					OWP.artifact2Spawned = true;
			}

			public string GetPartName() {
				if(artifactID == 1)
					return "ARTIFACT1";
				return "ARTIFACT2";
			}

			public override void OnPeriodicUpdate() {
				Vessel active = FlightGlobals.ActiveVessel;
				if(active != null && !IsSpawned() && DistanceFrom(active) < 50.0) {
					SetAsSpawned();
					SpawnPartAsVessel();
				}
			}

			private void SpawnPartAsVessel() {
				//Basing (BASED?) this off's Kopernicus Asteroid Spawner
				Orbit orbit = new Orbit(0, 0, 0, 0, 0, 0, 0, body);
				Vector3d pos = pqs.PlanetRelativePosition + pqs.PlanetRelativePosition.normalized * 0.5;
				orbit.UpdateFromStateVectors(pos, body.getRFrmVel(pos), body, Planetarium.GetUniversalTime());

				ConfigNode vesselCfg = ProtoVessel.CreateVesselNode(
					"Alien Artifact",
					VesselType.DroppedPart,
					orbit,
					0,
					new[] {
						ProtoVessel.CreatePartNode(
							GetPartName(),
							0
						)
					},
					new ConfigNode("ACTIONGROUPS")
				);

				vesselCfg.SetValue("landed", true);
				vesselCfg.SetValue("landedAt", body.name);
				vesselCfg.SetValue("splashed", false);
				vesselCfg.SetValue("lat", latitude);
				vesselCfg.SetValue("lon", longitude);
				vesselCfg.SetValue("sit", Vessel.Situations.LANDED.ToString());

				ProtoVessel pVessel = new ProtoVessel(vesselCfg, HighLogic.CurrentGame);
				pVessel.launchedFrom = "????????";

				pVessel.Load(HighLogic.CurrentGame.flightState);
            	GameEvents.onNewVesselCreated.Fire(pVessel.vesselRef);
				Debug.Log("[Other Worlds] Spawned Artifact (?)");

				//GetWorldSurfacePosition(lat, lon, alt) is the same as body.BodyFrame.LocalToWorld(body.GetRelSurfacePosition(lat, lon, alt).xzy).xzy + body.position;
			}
		}
	}
}