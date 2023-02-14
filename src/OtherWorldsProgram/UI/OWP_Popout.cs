using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using KSP.UI.Screens;
using KSP.Localization;
using FinePrint;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_Popout : MonoBehaviour {
			/*
				Monobehaviour
			*/
			public Button buttonClose;
			public GameObject main;

			public void DeletePopout() {
				Destroy(main);
			}

			/*
				Static Methods
			*/

			public static OWP_Popout instance;
			private const string PREFAB_PATH = "assets/otherworlds/ui/letter/owletterui.prefab";

			public static void OnSceneChange() {
				//Called when on a scene change. Used to delete the UI.
				if(instance == null) return;
				instance.DeletePopout();
			}

			public static void OnFirstKSC() {
				//Runs the first time you see the KSC screen on a savefile, called from OWP
				string date = KSPUtil.PrintDate(Planetarium.GetUniversalTime(), false) + ".";
				SpawnPopout(
					OWP_LOC.Get("Popout_First_Header"),
					OWP_LOC.Get("Popout_First_Body").Replace("<DATE>", date),
					OWP_LOC.Get("Popout_First_Refs")
				);
			}

			public static void OnEnterEditor() {
				bool show1 = !OWP.popout1shown && OWP.GetProgressBit(28) && !OWP.popout1shownNow;
				bool show2 = !OWP.popout2shown && OWP.GetProgressBit(29) && !OWP.popout2shownNow;

				if(show1) { OWP.popout1shown = true; OWP.popout1shownNow = true; }
				if(show2) { OWP.popout2shown = true; OWP.popout2shownNow = true; }

				string loc = "";
				if(show1 && show2) {
					loc = "SecondThird";
				} else if(show1) {
					loc = "Second";
				} else if(show2) {
					loc = "Third";
				} else {
					return;
				}

				string date = KSPUtil.PrintDate(Planetarium.GetUniversalTime(), false) + ".";

				SpawnPopout(
					OWP_LOC.Get("Popout_First_Header"),
					OWP_LOC.Get($"Popout_{loc}_Body").Replace("<DATE>", date),
					OWP_LOC.Get("Empty")
				);
			}

			public static void SpawnPopout(string header, string body, string references) {
				if(instance != null) {
					instance.DeletePopout();
					instance = null;
				}

				GameObject ui = OWP.LoadAsset<GameObject>(PREFAB_PATH);
				ui.transform.SetParent(MainCanvasUtil.MainCanvas.transform);
				ui.AddComponent<OWP_DraggableUI>(); //Add Drag MonoBehaivour to LaserUI
				ui.SetActive(true);

				if(ui == null) {
					Debug.LogError($"[Other Worlds] Couldn't instantiate '{PREFAB_PATH}'. Check Asset Bundle. Popout spawn cancelled.");
					return;
				}

				instance = ui.AddComponent<OWP_Popout>();

				ui.transform.Find("Header").GetComponent<Text>().text = header;
				ui.transform.Find("Body").GetComponent<Text>().text = body;
				ui.transform.Find("References").GetComponent<Text>().text = references;

				instance.buttonClose = ui.transform.Find("ButtonClose").GetComponent<Button>();
				instance.buttonClose.onClick.AddListener(instance.DeletePopout);
				instance.main = ui;

				Debug.Log("[Other Worlds] Spawned Popout");
			}
		}
	}
}