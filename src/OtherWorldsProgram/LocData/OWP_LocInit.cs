using System;
using System.Collections.Generic;

using UnityEngine;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		/*
			BEHOLD
			HARDCODE HELL
			(It's funny, because this could be done with .cfg files, but I just don't)
		*/
		public class OWP_LocInit {

			public static void InitLocations() {
				//Spikes
				OWP.AddLocation(new OWP_LocPillar("Disole", "Spike_Disole", 5), 13);
				OWP.AddLocation(new OWP_LocPillar("Crons", "Spike_Crons", 5), 24);

				//Part Spawners
				OWP.AddLocation(new OWP_LocPartSpawner("Vassa", "Signal_Vassa_1_Box_1", 1, 15.58933, -174.5783), -1);
				OWP.AddLocation(new OWP_LocPartSpawner("Pequar", "Signal_Pequar_2_Box_1", 2, 79.141644362038, 175.134878499381), -1);

				//Change some PQSCities to use non-convex mesh colliders
				OWP.AddLocation(new OWP_LocNonConvexCol("OWR2", "Island_OW2"), -1);

				/*
					SIGNALS
				*/
				//Troni
				OWP.AddLocation(new OWP_LocSignal("Troni", "Signal_Troni_1", 20000, 8,
							new OWP_Cutscene(
								19,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(2, 301) + 13.53453 * 3600
							), true), 19);
							
				//Vassa
				OWP.AddLocation(new OWP_LocSignal("Vassa", "Signal_Vassa_1", 2700000, 3,
							new OWP_Cutscene(
								0,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(82807 + 7871 * (85/360.0f)) //7871 (~79200 actually?) period of C2-1 in the sky
							), false), 0);
				OWP.AddLocation(new OWP_LocSignal("Vassa", "Signal_Vassa_2", 130000, 7,
							new OWP_Cutscene(
								1,
								new Vector3(-2, -1.98f * 2, -8), new Vector3(0, 0, 0), //Char
								new Vector3(-2, -1.88f * 2, -8), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(0, 9) + 1.5634 * 3600
							), false), 1);
				OWP.AddLocation(new OWP_LocSignal("Vassa", "Signal_Vassa_3", 4000, 3,
							new OWP_Cutscene(
								14,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(2, 91) + 31.54 * 60
							), false), 14);
				OWP.AddLocation(new OWP_LocSignal("Vassa", "Signal_Vassa_4", 15000, 6,
							new OWP_Cutscene(
								15,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(2, 94) + 18.4752 * 3600, 40
							), false), 15);
				OWP.AddLocation(new OWP_LocSignal("Vassa", "Signal_Vassa_5", 30000, 2,
							new OWP_Cutscene(
								16,
								new Vector3(0.131f, -0.132f, -0.201f), new Vector3(0, 0, 0), //Char
								new Vector3(0.131f, -0.132f, -0.201f), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(2, 100) + 4.734 * 3600
							), false), 16);

				//C2-1
				OWP.AddLocation(new OWP_LocSignal("C2-1", "Signal_C2-1_1", 1143000 * 2.1, -2,
							new OWP_Cutscene(
								2,
								new Vector3(0, -2, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(255607 + 7871 * 0.66666), 30
							), false), 2);
				OWP.AddLocation(new OWP_LocSignal("C2-1", "Signal_C2-1_2", 20000, 6,
							new OWP_Cutscene(
								17,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(2, 103)
							), true), 17);

				//Pequar
				OWP.AddLocation(new OWP_LocSignal("Pequar", "Signal_Pequar_1", 450000, 4,
							new OWP_Cutscene(
								10,
								new Vector3(0, 0.13f, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0.13f, 0), new Vector3(0, 0, 0), //Camera
								(OWP_Util.CT2UT(2, 65) + 9.13 * 3600)
							), true), 10);

				//C3-1
				OWP.AddLocation(new OWP_LocSignal("C3-1", "Signal_C3-1_1", 7000 * 5, 2,
							new OWP_Cutscene(
								3,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(3715200), 30
							), false), 3);

				//Disole
				OWP.AddLocation(new OWP_LocSignal("Disole", "Signal_Disole_1", 6228000, 4,
							new OWP_Cutscene(
								11,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								(OWP_Util.CT2UT(2, 72) + 107760 * 222/360.0)
							), true), 11);
				OWP.AddLocation(new OWP_LocSignal("Disole", "Signal_Disole_2", 60000, 2,
							new OWP_Cutscene(
								12,
								new Vector3(-1.28f, 1.105f, 0.42f), new Vector3(0, 0, 0), //Char
								new Vector3(-1.28f, 1.105f, 0.42f), new Vector3(0, 0, 0), //Camera
								(OWP_Util.CT2UT(2, 72) + 107760 * 222/360.0 + 1800)
							), true, playsSong: false), 12);
				OWP.AddLocation(new OWP_LocSignal("Disole", "Signal_Disole_3", 120000, 4,
							new OWP_Cutscene(
								18,
								new Vector3(0, -0.47f, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, -0.47f, 0), new Vector3(0, 0, 0), //Camera
								(OWP_Util.CT2UT(2, 178) + (85 / 360.0) * 107760)
							), true), 18);

				//Kevari
				OWP.AddLocation(new OWP_LocSignal("Kevari", "Signal_Kevari_1", 1500000, 7,
							new OWP_Cutscene(
								9,
								new Vector3(0, -0.1f, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, -0.1f, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(0, 346)
							), false), 9);

				//Crons
				OWP.AddLocation(new OWP_LocSignal("Crons", "Signal_Crons_1", 400000, 5,
							new OWP_Cutscene(
								8,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								(OWP_Util.CT2UT(0, 317) + 129/360.0f * 232260 + 4 * 3600)
							), false), 8);
				OWP.AddLocation(new OWP_LocSignal("Crons", "Signal_Crons_2", 10000, 6,
							new OWP_Cutscene(
								21,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(3, 56) + 232260 * 0.5
							), true), 21);
				OWP.AddLocation(new OWP_LocSignal("Crons", "Signal_Crons_3", 2000, 8,
							new OWP_Cutscene(
								22,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(3, 58) + 232260 * 0.11584
							), true), 22);
				OWP.AddLocation(new OWP_LocSignal("Crons", "Signal_Crons_4", 2000, 9,
							new OWP_Cutscene(
								23,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(-0.1114472f, 0.1234691f, -0.3525128f), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(3, 64) + 37.682777777 * 3600
							), true, playsSong: false), 23);

				//Niko
				OWP.AddLocation(new OWP_LocSignal("Niko", "Signal_Niko_1", 132000 * 5, 2,
							new OWP_Cutscene(
								5,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(0, 296)
							), false), 5);
				OWP.AddLocation(new OWP_LocSignal("Niko", "Signal_Niko_2", 108284000, 4,
							new OWP_Cutscene(
								20,
								new Vector3(0, 0.31f, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0.31f, 0), new Vector3(0, 0, 0), //Camera
								OWP_Util.CT2UT(3, 176) + 7560 * (1 - 130/360.0)
							), true), 20);

				//C4-2
				OWP.AddLocation(new OWP_LocSignal("C4-2", "Signal_C4-2_1", 132000 * 10, 4,
							new OWP_Cutscene(
								4,
								new Vector3(0, 1.4f, 0), new Vector3(0, -135, 0), //Char
								new Vector3(0, 1.4f, 0), new Vector3(0, -135, 0), //Camera
								OWP_Util.CT2UT(24537600 + 154.1f/360 * (16 * 6 * 3600 + 47 * 60)), 30 //Last parentheis period of C4-2
							), false), 4);

				//Prima-Secunda
				OWP.AddLocation(new OWP_LocSignal("Prima", "Signal_Prima_1", 210000, 2,
							new OWP_Cutscene(
								6,
								new Vector3(0, -0.1f, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								(OWP_Util.CT2UT(1, 257)), 25 //When, when you already have the rotation period of Prima-Secunda
							), false), 6);
				OWP.AddLocation(new OWP_LocSignal("Secunda", "Signal_Secunda_1", 150000, 8,
							new OWP_Cutscene(
								7,
								new Vector3(0.1498061f, 0.436f, 0.5480636f), new Vector3(0, 120, 0), //Char
								new Vector3(0.1498061f, 0.436f, 0.5480636f), new Vector3(0, 120, 0), //Camera
								(OWP_Util.CT2UT(1, 259) + 0.25 * 56801.5005151051), 32
							), false), 7);

				//Ending
				OWP.AddLocation(new OWP_LocSignal("OWR2", "Signal_OW2_1", 1000000, 8,
							new OWP_Cutscene(
								25,
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Char
								new Vector3(0, 0, 0), new Vector3(0, 0, 0), //Camera
								(OWP_Util.CT2UT(138, 20)), 25 //When, when you already have the rotation period of Prima-Secunda
							), false, playsSong: false), 25);


				foreach(OWP_LocSignal signal in OWP.signals) {
					signal.cutscene.SetText(signal.dialogue);
				}







				//...I just didn't expect this to actually be happening
				OWP.AddLocation(new OWP_Ending(), -1);
			}
		}
	}
}