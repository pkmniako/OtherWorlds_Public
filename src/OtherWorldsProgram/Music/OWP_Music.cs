using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NiakoKerbalMods
{
	namespace OtherWorldsProgram
	{
		public class OWP_Music {
			private static string ASSET_BUNDLE_PATH = "assets/otherworlds/bgm/";
			private static float MUSIC_VOLUME_MULT = 1.5f;
			private static MusicLogic music = null;
			protected static List<AudioClip> stockSongs = null;
			protected static List<AudioClip> emptySongsList = null;
			protected static AudioSource source = null;
			static bool first = true;

			static GameObject gameObject = null;

			public static void LateLoad() {
				if(first) {
					first = false;

					gameObject = new GameObject();
					GameObject.DontDestroyOnLoad(gameObject);

					source = gameObject.AddComponent<AudioSource>();

					// Disable positional effects.
					source.spatialBlend = 0;
					source.dopplerLevel = 0;
					source.loop = false;

					music = MusicLogic.fetch;
					stockSongs = music.spacePlaylist;
					emptySongsList = new List<AudioClip>();
					emptySongsList.Add(AudioClip.Create("none", 44100, 1, 44100, false));
				}
			}

			public static void PlaySong(string name, bool force = false, bool once = false) {
				if(!force && source.isPlaying) return;

				AudioClip clip = OWP.LoadAsset<AudioClip>(ASSET_BUNDLE_PATH + name + ".mp3");

				if(clip == null) {
					Debug.LogError("[Other Worlds] Song '" + name + "' doesn't exist or wasn't loaded correctly");
					return;
				}

				if(source == null) {
					Debug.LogError("[Other Worlds] Can't play music when there is no audio source silly");
					return;
				}

				//Disable stock music
				//Fading for the stock songs would be pretty cool ngl
				//music.audio1.Stop();
				//music.spacePlaylist = emptySongsList;

				//Play custom music
				if(source.isPlaying) source.Stop();
				source.clip = clip;
				source.loop = true;
				source.time = 0;
				UpdateVolume();
				if(once)
					source.PlayOneShot(clip);
				else
					source.Play();
				
				Debug.Log("[Other Worlds] Playing song '" + name + "' (" + clip.length + " seconds)...");
			}

			public static void UpdateVolume() {
				if(source != null)
					source.volume = GameSettings.MUSIC_VOLUME * MUSIC_VOLUME_MULT;
			}

			public static void PausePlaying() {
				if(source != null && source.isPlaying)
					source.Pause();
			}

			public static void ContinuePlaying() {
				if(source != null)
					source.UnPause();
			}

			protected static float stockVolumeMultiplier = 1.0f;

			public static void Update() {
				if(source == null || music.audio1 == null) return;

				music.audio1.volume = GameSettings.MUSIC_VOLUME * stockVolumeMultiplier;
				if(source.isPlaying)
					stockVolumeMultiplier -= Time.fixedDeltaTime;
				else if(!FlightDriver.Pause)
					stockVolumeMultiplier += Time.fixedDeltaTime;
				stockVolumeMultiplier = Mathf.Clamp(stockVolumeMultiplier, 0, 1);
			}
			
			public static void StopMusic() {
				if(source != null)
					source.Stop();
				if(music.audio1 != null)
					music.audio1.volume = GameSettings.MUSIC_VOLUME;
			}
		}
	}
}