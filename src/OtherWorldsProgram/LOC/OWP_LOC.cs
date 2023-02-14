using System;
using System.Collections.Generic;

using UnityEngine;

/*
	Ah, welcome to the land of spoilers
	Not even moving the text from a .cfg to a .cs stopped you

	Oh well, you know the risk. Please enjoy this hardcoded hellscape.
*/

namespace NiakoKerbalMods
{
	namespace OtherWorIdsProgram
	{
		public class OWP_LOC {
			static Dictionary<string, string> data = null;

			public static bool IsLoaded {
				get {
					return data != null;
				}
			}

			public static string Get(string id) {
				if(data == null || !data.ContainsKey(id)) {
					return "NOLOC(" + id + ")";
				}

				return data[id];
			}

			public static void LoadData(string lang) {
				data = new Dictionary<string, string>();

				if(lang.Equals("en_en")) {
					//Yellow Texts
					data.Add("Event_New_Planet", "A new planet between stars has been discovered<br>Check the tracking station");
					data.Add("Event_Artifact_Added_1", "The moment you touch the artifact, it quickly splits open, revealing what appears to be a screen, and worryingly, text you can read.<br>Open the artifact with the button on the sidebar");
					data.Add("Event_Artifact_Added_2", "Forgotten by even the sands themselves, another small mystery joins your hands, for time gave it another chance");
					data.Add("Event_Artifact_Inventory_Full", "Can't pick up Artifact, Inventory Full");

					//Other
					data.Add("Signal_C2-1_2_Name","SOS CD10Y0");
					data.Add("Signal_C2-1_2_Dial","#(switchanim=1,showui=true,clear=1) The planet#(timing=0.5)..#(cameraui=false).#(timingreset=1) looks pretty from here#(timing=0.5)...#(timingreset=1,timenext=2.0) #(clear=1)I'm paying a small visit to the <b>Sand Sisters</b> again, but after that, I will attempt to go to the <b>Lava planet</b>. Never been there, not even at orbit. It's a very dangerous place, just being near it can heat up a craft and damage some components, so I will have to be very careful.#(timenext=2.0) #(clear=1,switchanim=2,timenext=2.0) Am I really alone?#(timenext=3.0) #(switchanim=3,timenext=1.0) #(close=long)I hope not...");
				}
			}

			private static void GenerateSignalDescriptionLOCs() {
				foreach(string key in new List<string>(data.Keys)) {
					string[] s = key.Split('_');
					if(s.Length == 4 && s[0].Equals("Signal") && s[3].Equals("Dial")) {
						string keyNew = s[0] + "_" + s[1] + "_" + s[2] + "_Desc";

						string value = data[key];
						string valueNew = "";
						bool insideTag = false;
						for(int i = 0; i < value.Length; i++) {
							if(i < value.Length - 1 && value[i] == '#' && value[i + 1] == '(') {
								insideTag = true;
							} else if(value[i] == ')') {
								insideTag = false;
							} else if(!insideTag) {
								valueNew += value[i];
							}
						}

						data.Add(keyNew, valueNew);
					}
				}
			}

			public static List<string> LOCEnding() {
				List<string> o = new List<string>();
				for(int i = 0; i < 23; i++) {
					string loc = $"Ending_{i}";
					string d = Get(loc);

					string nameKerbal = "Jebediah";
					string nameUser = "Me";
					if(FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.isEVA)
						nameKerbal = FlightGlobals.ActiveVessel.GetDisplayName().Split(' ')[0];
					nameUser = Environment.UserName;

					d = d.Replace("PlaceHereKerbalName", nameKerbal);
					d = d.Replace("PlaceHerePCUserName", nameUser);
					o.Add(d);
				}
				return o;
			}
		}
	}
}



























































































































































































































































































































namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_LOC {
			static Dictionary<string, string> data = null;

			public static bool IsLoaded {
				get {
					return data != null;
				}
			}

			public static string Get(string id) {
				if(data == null || !data.ContainsKey(id)) {
					return "NOLOC(" + id + ")";
				}

				return data[id];
			}

			public static void LoadData(string lang) {
				data = new Dictionary<string, string>();
				//TODO: Different languages

				if(lang.Equals("en_en")) {
					//Popouts
					data.Add("Popout_First_Header", "Kerbal Space Agency\nDepartment of Interstellar Studies &\nSprite II Mission Team");
					data.Add("Popout_First_Body", "\n<DATE>\n\nTo the chief of the the Kerbal Space Agency:\n\n\nThe Team for the Sprite II Mission would like to recommend the agency to highly prioritize the exploration of the Cercani exoplanetary system. Our team members have gone through every process or event that could explain the PRP-2611 signal, with the only coherent answer being an artificial origin [1].\n\nTo help future missions, we also calculated the location for the smaller signal PRP-2611B, assuming a tidally locked Vassa, at 15.58 latitude and -174.58 longitude [2]. We added these coordinates to the tracking station's system.\n\nWe know that the efforts by the Department of Interstellar Studies to communicate with this hypothetical alien species have yielded no results, but we believe this doesn't rule out an artificial origin, and in fact, it's to be expected.\n\nBest regards, Mare Kerman, head of the Sprite II Mission Team.");
					data.Add("Popout_First_Refs", "[1] Origins of PRP-2611, an artificial source?; DIS; Sprite II Team\n[2] Orbital analysis & PRP-2611: DIS: Sprite II Team");
					data.Add("Popout_Second_Body", "\n<DATE>\n\nTo the chief of the the Kerbal Space Agency:\n\n\nThe finding of the artifact at Vassa has been such monumental news. All the team is extremely happy, and in fact, wants to keep helping with the agencies other missions using their knowledge they gained from the Sprite II Mission.\n\nWe have taken the liberty to reverse engineer the artifact itself by using the descriptions and measurements the kerbals took while on the exoplanet, and we have made, in collaboration with Intervision, a replica. This way, we can have a backup in case the original one is lost.\n\nIntervision wanted to take all the praise though, so we made them stick some Sprite II stickers on the recreation. You will find a stock of these on the VAB's Cargo Section.\n\nBest regards, Mare Kerman, head of the Sprite II Mission Team.");
					data.Add("Popout_Third_Body", "\n<DATE>\n\nTo the chief of the the Kerbal Space Agency:\n\n\nWe write to inform we have colaborated once again with Intervision to make a new recreation of another alien artifact, the one that was found on Pequar.\n\nThe weak signal, that the original item produced, was picked up as very noisy data in the readouts that were published on the scientific journal 'Skies of stars', but we were able to filter it to find out the patterns that it emmitted. Like the last recreation, this one's objective is to serve as a back up if the original one is lost... or blows up. Like last time, we added a stock of them to the Vehicle Assembly Building's Cargo Section.\n\nThis time, we haven't been able to convince Intervision to add the stickers to the recreation.\n\nBest regards, Mare Kerman, head of the Sprite II Mission Team.");
					data.Add("Popout_SecondThird_Body", "\n<DATE>\n\nTo the chief of the the Kerbal Space Agency:\n\n\nChief, I'm going to ignore formal language in this letter. We can't believe all the findings that have happened on the Cercani system. To know that our mission kickstarted the discovery of intelligent alien life is an honour we will cherish for all of our lives.\n\nWe've been so hyped by the discoveries that we started working with some friends we know from Intervision, the guys that make the laser sails. Between them, us, and the measurements of the original artifacts found on Cercani, we've been able to reverse engineer both of them, and produce replicas that will serve as backups in case the originals are lost.\n\nWe sneaked into the Cargo section of the VAB to leave some of them there.\n\nBest regards, Mare Kerman, head of the Sprite II Mission Team.");
					data.Add("Empty", "");

					//Story
					data.Add("Intro_Waypoint_Vassa", "Radio Signal PRP-2611 B");
					data.Add("Intro_Waypoint_C2-1", "Radio Signal PRP-2611");

					data.Add("Signal_Troni_1_Name","SOS CD301Y2");
					data.Add("Signal_Troni_1_Dial","#(switchanim=1,showui=true,timenext=2.0) New Journal, Day 1031. I'm the first one to ever land on this planet,#(timenext=0.75) and the results are fascinating.#(timenext=2.5) #(clear=1)The ground on the night side of this world looks just#(cameraui=false) like that of an icy moon orbiting a distant gas giant. If I had suddenly woken up here one day, I would think I was in one of those moons, really.#(timenext=3.0) #(clear=1)This place is really nice though, A permanent clear sky perfect for gazing upon the stars, no annoying atmosphere, and enough gravity where you could see yourself making a nice place where to live,#(timenext=1.5) of course if you ignore everything else about the planet that wants to kill you.#(timenext=3.0) #(clear=1,switchanim=2)I don't want to be here for much longer. Going here takes a lot of fuel, so I have to go back to the gas giant to refuel, then I will make a place where to live, at least for some time, at the <b>moon that orbits backwards</b>, specifically <b>in the second biggest plateau</b>, and <b>24 degrees north</b>.#(timenext=0.5) I want to do some sky photography.#(timenext=4.0) #(close=short)");

					data.Add("Signal_Vassa_1_Name","SOS CD8Y0");
					data.Add("Signal_Vassa_1_Dial","#(timenext=1.0) Last Travels Log, I'm lost#(timenext=4.0) #(switchanim=1,showui=true,clear=1) I've been a week stuck in this planet and I'm pretty sure this is <b>Sunset Bay</b>...#(switchanim=2,cameraui=false,timenext=2.0) but as if nothing was ever here.#(timenext=2.0) #(switchanim=3,clear=1)The spacecraft #(switchanim=0)is severely damaged, with multiple leaks that I have to fix.#(timenext=2.0) #(clear=1)Following the Program's policy, in case of being stranded in any star system, I have to setup emergency signals across the system.#(timenext=2.0) #(clear=1)One of them has to be powerful enough for its signal to reach other stars, while also being on a fast-rotating body.#(timenext=4.0) #(switchanim=4,clear=1)I know a perfect place for that.#(timenext=4.0) #(close=short)");

					data.Add("Signal_Vassa_2_Name","SOS CD9Y0");
					data.Add("Signal_Vassa_2_Dial","#(switchanim=1,showui=true,timenext=5.0) Gany..#(cameraui=false). I'm sorry. I just wanted to return home, not destroy your dreams of finding life in these oceans.#(timenext=4.0) I know you will never hear this message, but I wanted to say it regardless, sorry.#(timenext=4.0) #(close=short)");

					data.Add("Signal_Vassa_3_Name","SOS CD91Y2");
					data.Add("Signal_Vassa_3_Dial","#(timenext=0.5) New Journal, Day 821.#(timenext=4.0) #(switchanim=1,showui=true,clear=1)I've come to this planet as fast as I could. With the#(cameraui=false) strange <b>spike being on the desert moon</b>, that means there has to be something left of the <b>city</b> that once stood in this bay.#(timenext=3.0) #(clear=1,switchanim=2)All hope's not lost. If that shit has moved to the top of a mountain since last time, then maybe the city is also somewhere else, maybe on a mountain too.#(timenext=4.0) #(clear=1)I will start the search by going <b>north, following the coast</b>. With the supplies I have, I think I'll be able to be down here for maybe up to a month before I have to return to orbit#(timenext=1.0), #(switchanim=3)if the permanent sunshine doesn't kill me first.#(timenext=4.0) #(close=short)");

					data.Add("Signal_Vassa_4_Name","SOS CD94Y2");
					data.Add("Signal_Vassa_4_Dial","#(switchanim=1,showui=true,timenext=3.0) I remember the time I was here. We were slowly trekking up the hills to go up to the rim of a crater lake. Suddenly I got a call from my cousin#(cameraui=false) back at <b>Sunset Bay</b>. They had accidentally, somehow, <b>activated</b> one of the <b>spikes</b> like the one on the <b>desert moon</b>.#(timenext=1.0) It was such a monumental discovery, we wouldn't stop talking about it for weeks. Though now, with that thing having taken my past away from me, I feel the discovery was more of a curse than a gift.#(timenext=3.0) #(clear=1,switchanim=2) I still haven't found any manmade structure while following the coast, and I doubt I will find any. This planet's ocean and winds are relentless, they will erode anything.#(timenext=3.0) #(clear=1,switchanim=3) Now I will check the rim of a crater in the dark side of the planet, <b>directly west of Sunset Bay</b>, where we had a remote weather station. I will probably not even spot it even if it's there, but I have to try.#(timenext=3.0) #(close=short)");

					data.Add("Signal_Vassa_5_Name","SOS CD10Y2");
					data.Add("Signal_Vassa_5_Dial","#(switchanim=1,showui=true,timenext=5.0) Well you look at that, there's no weather station here#(cameraui=false) either.#(timenext=2.0) #(clear=1)I want to go back, back to before I used that damn <b>spike</b>. I thought using the one at the <b>desert moon</b> could let me go back, but I can not even use this thing to activate it.#(timenext=4.0) #(clear=1)I hate myself so much. This mistake has been too painful#(timing=0.5)...#(timingreset=1,timenext=1.0) not even considering not having anyone with who to talk was already horrible. I feel like a stranger in this land.#(timenext=5.0) #(clear=1,switchanim=2,timenext=3.0) If someone finds this signal, whoever you are, whatever you are, this thing supposedly activates those <b>tall black pillars</b> I call <b>spikes</b>. I'm leaving one of these at the <b>very center of the biggest crater of the desert planet's northern desert</b>. That way it's protected by a magnetic field and it doesn't stop working.#(timenext=4.0) #(clear=1,switchanim=3,timenext=2.75) #(close=long)");

					data.Add("Signal_C2-1_1_Name","SOS CD10Y0");
					data.Add("Signal_C2-1_1_Dial","#(switchanim=1,showui=true,clear=1)This is a distress signal. My ship is in very bad condition and can not #(cameraui=false)travel very far.#(timenext=2.0) #(clear=1,switchanim=2)Please, if you hear this, follow me to the <b>asteroid moon of the desert planet</b> and to the <b>gas giant</b>#(timenext=5.0) #(clear=1,switchanim=3,timenext=3.0)...I'm #(close=long)scared");

					data.Add("Signal_C2-1_2_Name","SOS CD10Y0");
					data.Add("Signal_C2-1_2_Dial","#(switchanim=1,showui=true,clear=1) The planet#(timing=0.5)..#(cameraui=false).#(timingreset=1) looks pretty from here#(timing=0.5)...#(timingreset=1,timenext=2.0) #(clear=1)I'm paying a small visit to the <b>Sand Sisters</b> again, but after that, I will attempt to go to the <b>Lava planet</b>. Never been there, not even at orbit. It's a very dangerous place, just being near it can heat up a craft and damage some components, so I will have to be very careful.#(timenext=2.0) #(clear=1,switchanim=2,timenext=2.0) Am I really alone?#(timenext=3.0) #(switchanim=3,timenext=1.0) #(close=long)I hope not...");

					data.Add("Signal_Pequar_1_Name","SOS CD65Y2");
					data.Add("Signal_Pequar_1_Dial","#(switchanim=1,showui=true)New Journal, Day 795.#(timenext=2.0) This is the first#(cameraui=false) time ever I've landed on this planet. My family and friends always told me it was beyond gorgeous, a place that everyone should visit at least once in their lifetimes.#(timenext=3.0) #(clear=1)Though I have seen many similar landscapes in my travels across the stars, and the gravity here is crushing me to the ground, I find it beautiful too.#(timenext=3.0) #(clear=1)Maybe it's because they always made it sound like the best thing in the universe, but I don't care, I miss them a lot. If they used to like it, I like it too.#(timenext=3.0) #(clear=1,timenext=3.0) I'm going to be here for a while, I don't feel like standing up right now... and I would like to think about #(close=long)them for a bit.");

					data.Add("Signal_C3-1_1_Name","SOS CD43Y0");
					data.Add("Signal_C3-1_1_Dial","New Journal, Day 43.#(timenext=4.0) #(switchanim=1,showui=true,clear=1)I leave this to anyone who might be following me due to the distress signals.#(timenext=2.0) #(clear=1)I have good news. I've been able to fix#(cameraui=false) two of the three fuel leaks my ship had.#(timenext=2.0) #(clear=1)Though it isn't enough to blindly go to other stars, I'm far more relaxed knowing I now have far less chances of getting stuck in orbit.#(timenext=3.0) #(clear=1,switchanim=2)#(timenext=3.0) I have#(cameraui=true) to wait for a transfer window to the <b>gas giant</b>. When I get there, I will also set camp at the <b>smallest of the spherical moons</b>...#(clear=1,timenext=1.0) #(switchanim=3,timenext=3.0) I want to be optimistic but, I still don't know where I'm really at. Everything#(cameraui=false) is familiar, but the abandoned colony back at Sunset Bay isn't there anymore. Same for the science outpost that we had at one of the archipelagos.#(timenext=2.0) #(clear=1)Did the ocean wash them away?#(timenext=1.0) The ice?#(timenext=1.0) The storms?. I don't like when I make mistakes, and I made one...#(timenext=2.0) #(close=short,clear=1)See you soon");

					data.Add("Signal_Disole_1_Name","SOS CD72Y2");
					data.Add("Signal_Disole_1_Dial","#(switchanim=1,showui=true,timenext=0.5) Wandering Log, Day 802.#(timenext=1.0) I've only been to this place twice. Both times I arrived when a global dust storm has raging across#(cameraui=false) the moon. Quite unlucky, but at least today,#(switchanim=2,timenext=5.0) it isn't the case.#(timenext=3.0) #(clear=1)It was still fun though, I got to see people working on trying to find life here, as they could never explain why there was oxygen in the atmosphere.#(timenext=2.0) They would study small bodies of water at the north pole.#(timenext=1.0) Don't bother looking there, I checked and they are gone.#(timenext=2.0) #(clear=1)Well, I could go back to the desert world, but now that the sky is clear...#(switchanim=3) I want to see everything from the top of Curiosity Peak.#(timenext=3.0) #(close=short)");

					data.Add("Signal_Disole_2_Name","SOS CD72Y2 B");
					data.Add("Signal_Disole_2_Dial","#(switchanim=1,showui=true,timenext=4.0) Concha de su#(timing=0.25)...#(timingreset=1)#(clear=1,switchanim=2,timenext=2.0) This shit <b>shouldn't be here</b>. All the accomplishments that my kind did, eroded away, but this#(timenext=0.5) <b><i>thing</i></b>, somehow#(cameraui=false), alive and not even near where it should be!?#(timenext=4.0) #(clear=1,switchanim=3,timenext=3.0) This#(timenext=0.5) <b>spike, monolith</b>, however you want to call it, has already taken so much from me.#(timenext=2.0) #(clear=1)I'm returning to <b>Sunset Bay</b>, if this stupid thing is here, there <b>must</b> be something left of what we did over there.#(timenext=4.0) #(clear=1,switchanim=4,timenext=3.0) #(close=short)");

					data.Add("Signal_Disole_3_Name","SOS Disole 3");
					data.Add("Signal_Disole_3_Dial","#(timenext=1.0) New Journal, Day 908.#(timenext=2.0) #(clear=1,switchanim=1,showui=true,timenext=3.0) #(timenext=3.0,cameraui=false) #(switchanim=2,clear=1)I'm not going to the star, don't worry. I'm going to the <b>lava planet</b>. Instead of going to the horrible day side of the planet, I'm going to go to the <b>night, icy side</b>, specifically <b>7 degrees north</b>, far away from any lava.#(timenext=4.0) #(clear=1)This is very dangerous, in case of an emergency, I can't just land wherever I want, and I don't know how much my orbital craft can stay in orbit without problems.#(timenext=4.0) #(clear=1) I've also thought if this was a good idea at all, I've already told myself I don't have to put antennas anymore, I'm alone. But I've got nothing to lose,#(timenext=0.75) so#(timenext=1.5) #(switchanim=3)<br>#(close=long)Destination:#(timenext=0.6) Hell");

					data.Add("Signal_Kevari_1_Name","SOS CD346Y0");
					data.Add("Signal_Kevari_1_Dial","#(switchanim=1,showui=true,clear=1) New Journal, Day 346. This is going to be a short log as I want to get as far away as I can from this cursed moon.#(timenext=1.0) #(clear=1)I will soon start using #(timenext=0.15)7-#(timenext=0.15)5-#(timenext=0.15)4-#(timenext=0.15)6 linear codification on the signals to save up the good antennas for more pressing emergencies.#(timenext=1.0) #(clear=1)I repeat, #(timenext=0.15)7-#(timenext=0.15)5-#(timenext=0.15)4-#(timenext=0.15)6#(timenext=5.0) #(close=short)");

					data.Add("Signal_Crons_1_Name","SOS CD317Y0");
					data.Add("Signal_Crons_1_Dial","#(switchanim=1,showui=true,clear=1)New Journal, Day#(cameraui=false) 317.#(timenext=2.0) I'm#(switchanim=2) running out of good antennas!#(timenext=2.0) #(clear=1)This has never happened to me, I used to scavenge for them at abandoned places, but they appear to not exist anymore. This means I'm going to have to develop a way to boost the range of the ones I have plenty of.#(timenext=2.0) #(clear=1)I'm not worried about the communications being different, as, if you are listening to this, you probably already cracked the code on how these communications work.#(timenext=3.0) #(clear=1)On other news, welcome to this mess of a moon. It#(switchanim=3) orbits the wrong way around! It doesn't make any sense, but yet#(timenext=0.5) it doesn't care. Traveling here from the outer moons is easy, but exploration between the two inner moons has always been hard.#(timenext=0.5) If you want to save up fuel, you might want to look into Gravity Assists, they are very easy to get around this planet.#(close=short)");

					data.Add("Signal_Crons_2_Name","SOS CD56Y3");
					data.Add("Signal_Crons_2_Dial","#(switchanim=1,showui=true,clear=1,timenext=4.0) I no longer have enough fuel to be able to reach#(cameraui=false) the gas giant and refuel there. From the logs I've been able to download, the cause was...#(switchanim=2) overheating.#(timenext=3.0) #(clear=1)I thought I had escaped that <b>hellish planet</b> in one piece, I was so happy, but no, I didn't listen to what I had learnt about the place and I went straight in without a care in the world.#(timenext=2.0) I'm a disgrace.#(timenext=4.0) #(clear=1)I'm going to go for a stroll across this moon. I need some time to relax and think about how to fix this situation. I'm attaching a file to the message with the details of the trip I want to do, just in case#(timenext=1.0) I never leave this place.#(timenext=3.0) #(close=short)");

					data.Add("Signal_Crons_3_Name","SOS CD58Y3");
					data.Add("Signal_Crons_3_Dial","#(switchanim=1,showui=true,clear=1,timenext=4.0) That <b>spike</b> I found at that mountain, it has broken me.#(timenext=2.0) Before I#(cameraui=false) got stranded here, that structure wasn't in that moon, but at <b>Sunset Bay</b>, next to what were the ruins of my civilization.#(timenext=3.0) #(clear=1)I studied all the information that was available on it, but unlike those who came before me, I had more time that I had ever wanted.#(timenext=1.5) I ended up learning its function, to send whoever uses it to a different world, one with a civilization similar to theirs, awaiting their arrival#(timing=0.5)...#(timingreset=1,timenext=5.0) #(clear=1,switchanim=2)But that didn't happen for me, it was too good to be true. I only saw the memories of my kind erode away, never to be seen again.#(timenext=1.5) Though my ship suffered damages, at least it could still go places,#(timenext=1.5) unlike now.#(timenext=4.0) #(clear=1,switchanim=3,timenext=2.5) #(clear=1)Though, I did some calculations, and there is a pattern of gravity assists that could help me lower my orbit enough to reach the gas giant, but if I make any mistake, I will be stuck in orbit,#(timenext=1.5) forever.#(timenext=4.0) #(clear=1)Dangerous, but unless I want to get fuel by carrying chunks of water ice from here to the spacecraft for centuries, this might be the only way to refuel the ship again.#(timenext=4.0) #(close=short)");

					data.Add("Signal_Crons_4_Name","SOS CD64Y3");
					data.Add("Signal_Crons_4_Dial","#(switchanim=1,showui=true,clear=1,timenext=5.0) Not only I've found <b>another spike</b>, but I've discovered why the#(cameraui=false) other one didn't activate.#(timenext=3.0) #(switchanim=2,timenext=2.0,clear=1) #(clear=1)These ones work differently, the configuration I had on <i>this</i> little <b>emitter</b> was correct after all.#(timenext=3.0) #(clear=1,switchanim=3)A year ago, I would have been furious at the sight of another one of these. Now I'm filled with curiosity, and hope.#(timenext=2.0) I'm going to go back to the <b>other spike</b> and activate it, see if it behaves the same as this one.#(timenext=3.0) #(clear=1)If both end up pointing at the same place in the sky, they could be pointing to the same place, maybe, my ticket out of this nightmare.#(timenext=4.0) #(clear=1,timenext=2.0) #(clear=1)They never figured out what these did. People liked to think of them, perhaps, as a path across the stars.#(timenext=2.5) #(close=long)I can't believe they might be right.");

					data.Add("Signal_Niko_1_Name","SOS CD296Y0");
					data.Add("Signal_Niko_1_Dial","#(switchanim=1,showui=true,clear=1)New Journal, Day#(cameraui=false) 296. My ship is now ready for action! After some very scary attempt, #(switchanim=2)I've been able to refuel it at the Ice Giant without any problems.#(timenext=2.0) #(clear=1)I have enough fuel to go to another star, but one of the tanks still has a fracture I've not been able to fix, so I wouldn't be able to return in case of another emergency.#(timenext=2.0) #(clear=1)Unless I want to spend eons in interstellar space, which I don't, the only option I have is to stay here and continue placing distress signals, even if I'm not in such peril now.#(timenext=2.0) #(clear=1,switchanim=3)#(timenext=2.0) Who knows, even though none of my kind will find these signals, maybe aliens will! I've never met one, so it would be quite fun to do so.#(timenext=1.0) I would probably shit my pants if I saw an alien to be honest.#(timenext=3.0) #(clear=1,switchanim=4)First I will visit the <b>other two moons of the Gas Giant</b>. Then I will go to the <b>twin ice planets</b>, as there's a transfer window very soon. To finish, I will return to the <b>sisters of sand</b>, you should know, the one with the rings.#(timenext=5.0) #(clear=1,switchanim=5,timenext=2.0)See you there aliens.#(close=short)");

					data.Add("Signal_Niko_2_Name","SOS CD176Y3");
					data.Add("Signal_Niko_2_Dial","#(switchanim=1,showui=true,clear=1,timenext=2.0) #(switchanim=2)I've fixed the#(cameraui=false) leak,#(timenext=1.0) #(switchanim=3)I've refueled at the giant,#(timenext=1.0) #(switchanim=4)I checked both spikes,#(timenext=1.0) and now,#(timenext=0.5) #(switchanim=5)I know where they point at.#(timenext=3.0) #(clear=1)I'm going there, curiosity is killing me. I haven't been this excited in so long. If it ends up that there's nothing there, the ship would still have enough fuel to divert the trajectory to that star I initially thought was home but isn't.#(timenext=3.0) #(clear=1,switchanim=6,timenext=3.0) To think that, if I had known that I just had to circle the structures with this thing, I would be doing this one year earlier.#(timenext=3.0) #(clear=1,switchanim=7,timenext=5.0) This is a hopeful signal, if you find it, I'm no longer in this star system. Whoever is reading this, never lose hope, like I#(close=long) almost did.");

					data.Add("Signal_C4-2_1_Name","SOS CD284Y0");
					data.Add("Signal_C4-2_1_Dial","New Journal, Day 284.#(timenext=4.0) #(switchanim=1,showui=true,clear=1)I still have to refuel my craft at the Gas Giant, but at least the long wait of the transfer is over.#(timenext=2.0) #(clear=1,timenext=2.0) These past months I've been thinking#(cameraui=false) about this whole situation. With everything gone at <b>Sunset Bay</b>...#(timenext=2.0) How much time have I been traveling the stars for a whole <b>city</b> to disappear?#(timenext=3.0) #(switchanim=2,clear=1)I know I haven't kept the best records of every single day, but, I really hope that the rest of what my kind did hasn't eroded away too. I'm the last one, I feel I have to preserve as much as I can.#(timenext=3.0) #(switchanim=3,clear=1)I've decided I will set up camp at <b>one of the equatorial craters</b>, probably those south of the <b>giant ringed crater</b>. If I remember correctly, there used to be a small outpost there.#(timenext=2.0) I hope it's still there. #(timenext=3.0) #(close=short,clear=1)");

					data.Add("Signal_Prima_1_Name","SOS CD257Y1");
					data.Add("Signal_Prima_1_Dial","New Journal, Day 621.#(timenext=3.0) #(switchanim=1,showui=true,clear=1)While I was arriving at these two planets, I could see them#(cameraui=false) dancing around.#(timenext=1.0) It was so pretty, but that made me realize I hadn't done any star gazing in the past two years since I started placing antennas like I was crazy.#(timenext=2.0) #(clear=1)But now that I look up into the sky, I realize, I don't recognize any constellation.#(timenext=1.0) One star is as bright as <b>Home</b>, but I doubt it's the same one.#(timenext=1.0) #(clear=1)What I did back at the <b>Spike of Sunset Bay</b> was clearly a mistake, and now the last remnants of our civilization are gone, but I have to move on.#(timenext=4.0) #(clear=1,switchanim=2,timenext=3.0)Time to go there.#(close=short,clear=1)");

					data.Add("Signal_Secunda_1_Name","SOS CD259Y1");
					data.Add("Signal_Secunda_1_Dial","#(switchanim=1,showui=true,timenext=1.0) New Journal, Day 623. I've accepted that, whatever happened at <b>Sunset Bay</b>, didn't make me go back home, but made#(cameraui=false) me go to another time, one where I'm even further away from the place I wanted to be.#(timenext=3.0) #(clear=1)Even if I don't have to setup any more signals, as no one I want will answer, I will keep placing them, as a message in a bottle to those that will stand on our shoulders.#(timenext=3.0,clear=1) #(switchanim=2,timenext=5.5) #(clear=1)Hopefully they won't stand here, this planet is known for its geological activity, producing a lot of earthquakes. They are very strong too, I had the displeasure of having to experience one last time I came here. At least I wasn't in the trenches like right now, I wouldn't like to dodge rocks falling from the sky!#(timenext=2.0) #(clear=1)My next stop is the <b>desert moon</b>, so follow me if you can!#(timenext=3.0) #(close=short,clear=1)");

					data.Add("Signal_OW2_1_Name","NJo 1");
					data.Add("Signal_OW2_1_Dial","#(switchanim=1,showui=true,timenext=2.5) #(cameraui=false,timenext=2.5) #(clear=1)The shadows,#(timenext=1.0) hidden in front of my eyes, all this time.#(timenext=1.5) I've never seen, let alone been to a <b>Rogue Planet</b> before.#(timenext=2.0) #(clear=1)I'm sitting right now in a moon of this sunless world, It's the only one I've discovered, don't know if there are any more.#(timenext=2.5) The moon itself has lakes, oceans, made out of hydrogen. While most of them lie on the southern hemisphere, along with most of the ice, the sea this island sits at is north of the equator.#(timenext=5.0) #(clear=1,switchanim=2,timenext=1.0) I'm going to stay here for a while, I think I deserve to rest after such a long journey. And I better be ready if I want to use the <b>spike</b> here#(timenext=2.0) to once again travel to a better place.#(timenext=4.0) #(clear=1,switchanim=3)I wonder if the signals I left back those years ago are still there#(timing=0.5)...#(timingreset=1,timenext=3.0) #(close=short)");

					GenerateSignalDescriptionLOCs();

					data.Add("CelestialBody_OWR1_Name", "Esperanza's Planet^N");
					data.Add("CelestialBody_OWR1_Desc", "A Rogue planet, a world without a star, that the two \"spikes\" from the Cercani system point at. Though shrouded in darkness, if light was to shine upon it, one would be able to see a white and yellow world without noticable clouds. This rogue planet, a gas giant, is massive, having 25 times more mass than Jool. This actually puts it very near to the limit between planet and star, which no one really has a good concrete separation for. Even though it's very massive, its size is actually not that big as one would think, not even making it to twice the size of our own green gas giant.");
					data.Add("CelestialBody_OWR2_Name", "Esperanza^N");
					data.Add("CelestialBody_OWR2_Desc", "Esperanza is the only known \"moon\" that orbits this Rogue Planet. It has a southern hemisphere of white and red ice, while the northern one has types of rocks you could find on planets like Kerbin and Duna. Its atmosphere, combined with the very little heat this world generates, allows it to have expansive lake systems of liquid hydrogen. A peaceful place, one where the stars will always shine in the night sky, a place where we can remember we aren't alone.");
					data.Add("CelestialBody_OWR3_Name", "Esperanza's Moon^N");
					data.Add("CelestialBody_OWR3_Desc", "A moon shrouded in darkness, orbiting a world without sun. Not much is known about this place.");

					data.Add("Ending_0",  "#(ending=true,var=1,switchanim=2,timenext=12.0) H#(timing=0.5)...#(timingreset=1,timenext=3.0) Hello#(timing=0.5)...#(timingreset=1)<br><br>#(branch=2;Hello,branch=1;...) ");
					data.Add("Ending_1",  "#(switchanim=4)Your suit, it says 'Space Program'#(timing=0.5)...#(timingreset=1,timenext=3.0) #(switchanim=2)Can you understand me?<br><br>#(branch=2;Yes,branch=3;No) ");
					data.Add("Ending_2",  "#(switchanim=3,timenext=4.0) S#(switchanim=0,timing=0.5)...#(timingreset=1,timenext=1.5) Sorry#(timing=0.5)...#(timingreset=1,timenext=1.5) <br>I just didn't expect this to actually be happening#(timenext=3.0) #(branchforce=4)");
					data.Add("Ending_3",  "I never thought I would hear such bad jokes again, specially from an alien.#(timenext=2.0) #(branchforce=4)");
					data.Add("Ending_4",  "#(timing=0.5)...#(timingreset=1)tell me, what's your name.<br><br>#(branch=5;PlaceHereKerbalName,branch=6;Kerman,branch=5;PlaceHerePCUserName) ");
					data.Add("Ending_5",  "That's your name?#(timenext=1.5) #(switchanim=4)Well, that's a very nice one. Happy to meet you.#(timenext=3.0) #(clear=1)My name's Io. I come from a place we call Home, what about you?<br><br>#(branch=7;I come from Kerbin,branch=8;I come from Earth,branch=9;I come from the place you thought was your home) ");
					data.Add("Ending_6",  "Kerman?#(timenext=1.0) That sounds like that 'Space Program' your suit's patch mentions. Is that the name of your people? Your planet?<br><br>#(branch=10;Kerbal is how we call ourselves,branch=10;That's my surname,branch=7;My planet is called Kerbin) ");
					data.Add("Ending_7",  "That's a nice name for a planet. I will try to remember that and your name.#(timenext=1.5) In fact, #(switchanim=5)I'm going to note it down.<br><br>#(branch=11;What is this place?,branch=12;What are those tall spikes?,branch=13;What are you?,branch=9;I also have one of those computers!) ");
					data.Add("Ending_8",  "'Earth'?#(timenext=1.0) Sorry, but that's quite a weird name. Naming your home after the ground?#(timenext=2.0) #(clear=1)I shouldn't be that harsh, 'Home' after all is also a very unoriginal name.#(timenext=1.5) That's the name of the planet where I was born and raised.#(timenext=2.0) #(clear=1)But after time kept going like a stream,#(timenext=1.0) we had to abandon our home and move to a world of water, where we had to live trapped inside colonies.#(timenext=1.0) <br><br>#(branch=14;I'm sorry to hear that,branch=9;I know, I've followed your signals) ");
					data.Add("Ending_9",  "#(timenext=1.0) #(switchanim=6,timenext=0.5) You just made me so happy, to hear that those antennas I placed and the hardware I left on that star system actually paid off.#(timenext=0.75) And not only that, that someone was able to follow them to here, to <b>Esperanza</b>.#(timenext=2.0) #(clear=1,timing=0.5)...#(timingreset=1)I still can't believe I'm talking with an alien#(timing=0.5)...#(timingreset=1)<br><br>#(timenext=2.0) #(branch=11;What is this place?,branch=12;What are those tall spikes?) ");
					data.Add("Ending_10", "Oh#(timing=0.5)...#(timingreset=1,timenext=0.5) I'm sorry#(timing=0.5)...#(timingreset=1,timenext=1.5) That's still a nice name,#(timenext=0.25) I mean surname. Let me note it down.<br><br>#(branch=11;What is this place?,branch=12;What are those tall spikes?,branch=13;What are you?,branch=9;I also have one of those computers!) ");
					data.Add("Ending_11", "What a nice question. This is a <b>Rogue Planet</b>.#(timenext=1.0) Well, a moon of one at least.#(timenext=1.5) It's a mixture of ice and rock, with oceans of liquid hydrogen. Pretty cool if you ask me.#(timenext=2.5) #(clear=1)I even gave this world a name,#(timenext=1.0) <b>Esperanza</b>.#(timenext=3.0) #(clear=1)This island specifically has a <b>structure</b> at its peak, one that lets those who know how to use it travel#(timenext=0.75) to other worlds#(timenext=0.75), to other universes,#(timenext=0.75) where the same structures exists.#(timenext=3.0) #(clear=1)We never figured out who made them, but this is the reason I ended up in here.#(timenext=3.0) #(branchforce=15)");
					data.Add("Ending_12", "What are those spikes?#(timenext=1.0) I was going to ask you!#(timenext=2.0) #(clear=1)My kind didn't build them, and it looks like yours didn't either. They will show you how to get to this planet, <b>Esperanza</b>.#(timenext=3.0) #(clear=1)But one of them, the one at the peak of this mountain, lets those who know how to use it travel#(timenext=0.75) to other worlds#(timenext=0.75), to other universes,#(timenext=0.75) where the same structures exists.#(timenext=3.0) #(clear=1)We never figured out who made them, but this is the reason I ended up in here.#(timenext=3.0) #(branchforce=15)");
					data.Add("Ending_13", "I'm a living being just like you#(timenext=1.0) I think.#(timenext=0.5) We never gave ourselves a name for our kind, apart from 'us', but I'm myself a human.<br><br>#(branch=11;What is this place?,branch=12;What are those tall spikes?,branch=9;I also have one of those computers!) ");
					data.Add("Ending_14", "I didn't expect aliens to be#(timenext=0.75) wholesome.#(timenext=1.25) I always pictured then as beings I would never be able to talk to.#(timenext=2.0) #(clear=1)Thank you a lot for the kind words, I haven't heard those in a long time.#(timenext=1.0) #(switchanim=7,timenext=3.0) #(switchanim=0,clear=1)I've been talking a lot, do you want to ask something?<br><br>#(branch=11;What is this place,branch=12;What are those tall spikes?,branch=13;What are you?,branch=9;I also have one of those computers!) ");
					data.Add("Ending_15", "#(switchanim=2,timenext=1.0) #(switchanim=7,timenext=3.0) #(switchanim=0,clear=1)I have one last question for you#(timing=0.5)...#(timingreset=1,timenext=0.75) What made you get here?<br><br>#(branch=16;Wanted to get to the end of this,branch=17;I wanted to meet you,branch=18;Curiosity itself) ");
					data.Add("Ending_16", "Well, you did quite the good job then. Congratulations on finding me.#(timenext=3.0) #(branchforce=19) ");
					data.Add("Ending_17", "You make me cry inside for the times I have lost, the ones with my family and friends. I am glad someone under these regretful stars cares for me.#(timenext=3.0) #(branchforce=19) ");
					data.Add("Ending_18", "Curiosity, that's what has been keeping me sane the time I have been with no company in space. I'm happy to hear curiosity is not rare among the stars.#(timenext=3.0) #(branchforce=19) ");
					data.Add("Ending_19", "#(timenext=2.0) It's nice talking with you, but sadly#(timenext=2.0) I have to go#(timing=0.5)...#(timingreset=1,timenext=1.0)<br><br>#(branch=20;Where?,branch=21;Why now?) ");
					data.Add("Ending_20", "#(timenext=1.0) #(switchanim=8,timenext=2.0) #(clear=1)I'm going to use a spike again.#(timenext=2.0) #(clear=1)The only thing I could figure out about them was that they 'Sent whoever uses it to a different world, one with a civilization similar to theirs, awaiting their arrival'.#(timenext=3.0) #(clear=1)We might look different, but you and your kind were in this universe, as if someone was actually waiting for me.#(timenext=2.0) #(clear=1)I'm going to use this technology again, non-stop, until by chance, I get back home.#(timenext=2.5) #(clear=1)Hadn't been for you, I might have decided to stick around here, ironically enough.#(timenext=3.5) #(branchforce=22) ");
					data.Add("Ending_21", "#(timenext=1.0) #(switchanim=8,timenext=2.0) #(clear=1)I know, I too wish we could keep having this conversation, but my life now revolves around these pointy structures.#(timenext=2.5) #(clear=1)You showed me that, no matter what happens after I use them, I will have someone to talk with.#(timenext=2.0) And if the next universe I end up in also has the spikes, I can use them again, until I get back home.#(timenext=3.5) #(branchforce=22) ");
					data.Add("Ending_22", "#(var=0,switchanim=9,timenext=3.5) #(clear=1)I know leaving now is very impatient from me, but I've seen that, even after all these years, I was not alone#(timenext=2.0) and I will never be.#(timenext=3.0) #(clear=1)And you,#(timenext=2.0,switchanim=10) are also not alone.#(timenext=3.5) #(switchanim=11,clear=1,timenext=2.0) #(clear=1)Goodbye#(timenext=4.0) #(clear=1,switchanim=-1,timenext=6.25) #(var=2)");

					//Yellow Texts
					data.Add("Event_New_Planet", "A new planet between stars has been discovered<br>Check the tracking station");
					data.Add("Event_Artifact_Added_1", "The moment you touch the artifact, it quickly splits open, revealing what appears to be a screen, and worryingly, text you can read.<br>Open the artifact with the button on the sidebar");
					data.Add("Event_Artifact_Added_2", "Forgotten by even the sands themselves, another small mystery joins your hands, for time gave it another chance");
					data.Add("Event_Artifact_Inventory_Full", "Can't pick up Artifact, Inventory Full");
				}
			}

			private static void GenerateSignalDescriptionLOCs() {
				foreach(string key in new List<string>(data.Keys)) {
					string[] s = key.Split('_');
					if(s.Length == 4 && s[0].Equals("Signal") && s[3].Equals("Dial")) {
						string keyNew = s[0] + "_" + s[1] + "_" + s[2] + "_Desc";

						string value = data[key];
						string valueNew = "";
						bool insideTag = false;
						for(int i = 0; i < value.Length; i++) {
							if(i < value.Length - 1 && value[i] == '#' && value[i + 1] == '(') {
								insideTag = true;
							} else if(value[i] == ')') {
								insideTag = false;
							} else if(!insideTag) {
								valueNew += value[i];
							}
						}

						data.Add(keyNew, valueNew);
					}
				}
			}

			public static List<string> LOCEnding() {
				List<string> o = new List<string>();
				for(int i = 0; i < 23; i++) {
					string loc = $"Ending_{i}";
					string d = Get(loc);

					string nameKerbal = "Jebediah";
					string nameUser = "Me";
					if(FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.isEVA)
						nameKerbal = FlightGlobals.ActiveVessel.GetDisplayName().Split(' ')[0];
					nameUser = Environment.UserName;

					d = d.Replace("PlaceHereKerbalName", nameKerbal);
					d = d.Replace("PlaceHerePCUserName", nameUser);
					o.Add(d);
				}
				return o;
			}
		}
	}
}