using System;
using System.Collections.Generic;

using UnityEngine;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_LocSignal : OWP_LocData {
			public double signalRange; //Max range of the signal
			public double signalQuality; //The bigger, the closer you must get for a good quality signal
			public bool encoded = false; //Is the signal encoded
			public string name; //Name of the signal
			public string description; //What the file that describes the signal says
			public string dialogue; //What gets fed into OWP_Dialogue
			public OWP_Cutscene cutscene = null; //Cutscene to be played. If there's no cutscene, you shouldn't play it.
			public bool playsSong = true;

			public OWP_LocSignal(string bodyName, string pqsName, double signalRange, double signalQuality, OWP_Cutscene cutscene = null, bool encoded = false, bool playsSong = true) :
				base(bodyName, pqsName, 10.0f) {
					this.signalRange = signalRange;
					this.signalQuality = signalQuality;
					this.encoded = encoded;
					this.cutscene = cutscene;

					string nameLOC = pqsName + "_Name";
					string descriptionLOC = pqsName + "_Desc";
					string dialogueLOC = pqsName + "_Dial";

					this.name = OWP_LOC.Get(nameLOC);
					this.description = OWP_LOC.Get(descriptionLOC);
					this.dialogue = OWP_LOC.Get(dialogueLOC);

					this.playsSong = playsSong;
				}

			public double GetSignalStrength(double distance2) {
				double scale = OWP_PlanetManager.GetRescale();
				double distance = distance2 / scale; //Apply any potential rescale the game has

				if(signalQuality < 0) { //Special case for C2-1 1, so it can be heard from very far away, but has accurate results when close
					if(distance > 7656710000)
						return 0;
					else if(distance > signalRange)
						return (1 + (1 - distance)/(7656710000 - signalRange)) * 0.1;
					else 
						return Math.Abs(Math.Pow(((distance/signalRange) - 1), -signalQuality)) * 0.9 + 0.1;
				}
				if(distance > signalRange)
					return 0;
				return Math.Abs(Math.Pow(((distance/signalRange) - 1), signalQuality));
			}

			public double GetSignalStrength() {
				double distance = DistanceFrom(FlightGlobals.ActiveVessel);
				
				return GetSignalStrength(distance);
			}

			bool closeToArea = false;
			
			public override void OnPeriodicUpdate() {
				if(FlightGlobals.ActiveVessel != null && playsSong) {
					bool closeToAreaNew = DistanceFrom(FlightGlobals.ActiveVessel) < 50;
					if(closeToArea != closeToAreaNew) {
						closeToArea = closeToAreaNew;
						if(closeToArea) {
							OWP_Music.PlaySong("signal2");
						} else {
							OWP_Music.StopMusic();
						}
					}
				}
			}
		}
	}
}