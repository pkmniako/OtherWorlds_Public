using System;
using System.Collections.Generic;

using UnityEngine;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_LocData {
			public int id; //ID for progression
			public CelestialBody body = null;
			public PQSCity2 pqs = null;
			public float interactionDistance = -1;

			public GameObject gameObject;

			public OWP_LocData(string bodyName, string pqsName, float interactionDistance = 2.0f) {
				this.body = FlightGlobals.GetBodyByName(bodyName);
				this.interactionDistance = interactionDistance;
				if(body != null) {
					foreach(PQSCity2 candidatePQS in body.GetComponentsInChildren<PQSCity2>(true)) { //body.pqsController.gameObject.transform.GetComponents<PQSCity2>()) {
						if(candidatePQS.name.CompareTo(pqsName) == 0) {
							this.pqs = candidatePQS;
							break;
						}
					}
				}
			}

			//Returns if you are close for interaction with the Location
			public bool VesselInRange(Vessel vessel) {
				if(pqs != null && pqs.PositioningPoint != null) {
					double distance = (pqs.PositioningPoint.position - vessel.parts[0].gameObject.transform.position).magnitude;
					return distance < interactionDistance;
				}
				return false;
			}

			//Return the distance in world space to the PQSCity
			public double DistanceFrom(Vessel vessel) {
				Vector3d v = vessel.GetWorldPos3D();
				Vector3d p = pqs.PositioningPoint.position;
				//Vector3d p = pqs.PlanetRelativePosition + body.getTruePositionAtUT(Planetarium.GetUniversalTime());
				//Vector3d p = body.GetWorldSurfacePosition(pqs.lat, pqs.lon, pqs.alt);
				//GetWorldSurfacePositions expects input in degrees, and altitude from alt 0, not center

				return (v - p).magnitude;
			}

			///	<summary>
			///	Retrieve the first GameObject that composes the PQS City (I assume it's the only one, imagine having more than one lmao)
			/// <para>(Duck was later found dead at his house with a note stating someone uses more than 1)</para>
			///	<a href="https://www.youtube.com/watch?v=dQw4w9WgXcQ">How to report an assasination to the police</a>
			///	</summary>
			public GameObject GetGameObject() {
				return (pqs != null && pqs.objects != null && pqs.objects.Length > 0 && pqs.objects[0].objects != null && pqs.objects[0].objects.Length > 0) ? pqs.objects[0].objects[0] : null;
			}

			public GameObject GetGameObject(string name) {
				GameObject mainGameObject = GetGameObject();
				if(mainGameObject == null) {
					Debug.LogError("[Other Worlds] OWP_LocData ID: " + id + " - The PQSCity2 is either null or it has no GameObjects");
				} else {
					GameObject target = mainGameObject.GetChild(name);
					if(target.gameObject == null) {
						Debug.LogError("[Other Worlds] OWP_LocData ID: " + id + " - Couldn't find GameObject Child with name " + name);
					} else {
						return target.gameObject;
					}
				}
				return null;
			}

			/// <summary>
			/// Runs when the location is considered "loaded". Check OWP_Manager for more on this. gameObject is guaranteed to exist and be valid.
			/// </summary>
			public virtual void OnLoad() {}

			/// <summary>
			/// Runs for each late update. gameObject is guaranteed to exist and be valid.
			/// </summary>
			public virtual void OnLateUpdate() {}

			/// <summary>
			/// Runs for every periodic update in OWP_Manager. gameObject is guaranteed to exist and be valid.
			/// </summary>
			public virtual void OnPeriodicUpdate() {}
		}
	}
}