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
		public class OWP_UI {
			//Application Button
			ApplicationLauncherButton appButton;

			/*
				Consts
			*/
			const double TIME_LOADING_SCREEN = 1.0;
			const bool DEBUG_MODE = false;
			const float REDUCED_SCALE = 0.6666f;

			/*
				UI Elements
			*/
			//Generic UI Elements
			GameObject ui; //The parent of all UI
			Animator uiAnimator;
			GameObject bgMain, bgComms, bgFiles;
			Image bgLoading;
			Button buttonChangeSize;

			//Debug UI
			Text debugText;
			Button debugButton;

			//Top Bar
			GameObject uiTopBar;
			Text textDate, textHour;
			GameObject uiTopBarCommOn, uiTopBarCommOff;

			//Back Button
			Button buttonBack;

			//Scroll
			Scrollbar scrollbar;

			//Screen Main
			GameObject mainScreen;
			Button mainButtonComms, mainButtonFiles, mainButtonSSSH;

			//Screen Comms
			GameObject commsScreen;
			GameObject commsPrefab, commsGrid;

			//Screen Signal
			GameObject signalScreen;
			Text signalText;
			Button signalButtonWaypoint, signalButtonRead;
			InputField signalDecodeField;
			Button signalDecodeButton;

			//Screen Files
			GameObject filesScreen;
			GameObject filesPrefab, filesGrid;
			Button filesImageButton;

			//Screen File (Just one (1) one)
			GameObject fileScreen;
			Text fileText, fileTitle;

			//Screen Image
			GameObject imageScreen;

			/*
				Dynamic Elements
			*/
			
			List<OWP_UI_Signal> signals;
			List<OWP_UI_File> files;

			/*
				Misc Variables
			*/
			bool isUISmall = false;

			public OWP_UI() {

			}

			public void LoadEverything() {
				DestroyUI();

				LoadUI();
			}

			public void LoadUI() {
				if(ui != null) return;

				isUISmall = false;

				signals = new List<OWP_UI_Signal>();
				files = new List<OWP_UI_File>();

				//Load from asset bundle
				ui = OWP.LoadAsset<GameObject>("assets/otherworlds/ui/tablet/owtabletui.prefab");
                ui.transform.SetParent(MainCanvasUtil.MainCanvas.transform);
            	ui.AddComponent<OWP_DraggableUI>(); //Add Drag MonoBehaivour to LaserUI
				ui.SetActive(false);

				//Obtain Elements
				bgMain = GetChild(ui, "BGMain").gameObject;
				bgComms = GetChild(ui, "BGComms").gameObject;
				bgFiles = GetChild(ui, "BGFiles").gameObject;
				bgLoading = GetChild(ui, "LoadingIcon").GetComponent<Image>();
				buttonBack = GetChild(ui, "ButtonBack").GetComponent<Button>();
				uiAnimator = ui.GetComponent<Animator>();
				scrollbar = GetChild(ui, "Scrollbar").GetComponent<Scrollbar>();
				buttonChangeSize = GetChild(ui, "ButtonChangeSize").GetComponent<Button>();

				debugText = GetChild(ui, "DebugText").GetComponent<Text>();
				debugText.gameObject.SetActive(DEBUG_MODE);
				debugButton = GetChild(ui, "DebugButton").GetComponent<Button>();
				debugButton.gameObject.SetActive(DEBUG_MODE);

				uiTopBar = GetChild(ui, "TopBar").gameObject;
				textDate = GetChild(uiTopBar, "Date").GetComponent<Text>();
				textHour = GetChild(uiTopBar, "Hour").GetComponent<Text>();
				uiTopBarCommOn = GetChild(uiTopBar, "SSSHYes").gameObject;
				uiTopBarCommOff = GetChild(uiTopBar, "SSSHNo").gameObject;

				mainScreen = GetChild(ui, "ScreenMain").gameObject;
				mainButtonComms = GetChild(mainScreen, "ButtonComms").GetComponent<Button>();
				mainButtonFiles = GetChild(mainScreen, "ButtonFiles").GetComponent<Button>();
				mainButtonSSSH = GetChild(mainScreen, "ButtonSSH").GetComponent<Button>();

				commsScreen = GetChild(ui, "ScreenComms").gameObject;
				commsGrid = GetChild(GetChild(commsScreen, "MaskAndScroll").gameObject, "Grid").gameObject;
				commsPrefab = GetChild(commsGrid, "CommSignalPrefab").gameObject;

				signalScreen = GetChild(ui, "ScreenSignal").gameObject;
				signalText = GetChild(signalScreen, "Text").GetComponent<Text>();
				signalButtonWaypoint = GetChild(signalScreen, "ButtonWaypoint").GetComponent<Button>();
				signalButtonRead = GetChild(signalScreen, "ButtonReadSignal").GetComponent<Button>();
				signalDecodeField = GetChild(signalScreen, "DecodeField").GetComponent<InputField>();
				signalDecodeButton = GetChild(signalScreen, "DecodeButton").GetComponent<Button>();

				filesScreen = GetChild(ui, "ScreenFiles").gameObject;
				filesGrid = GetChild(GetChild(filesScreen, "MaskAndScroll").gameObject, "Grid").gameObject;
				filesPrefab = GetChild(filesGrid, "FilePrefab").gameObject;
				filesImageButton = filesGrid.transform.Find("FileImage").GetComponent<Button>();

				fileScreen = GetChild(ui, "ScreenFile").gameObject;
				fileText = GetChild(fileScreen, "MaskAndScroll").Find("Text").GetComponent<Text>();
				fileTitle = GetChild(fileScreen, "Title").GetComponent<Text>();

				imageScreen = ui.transform.Find("ScreenImage").gameObject;

				//Add Listeners
				buttonChangeSize.onClick.AddListener(ChangeUISize);
				buttonBack.onClick.AddListener(StateGoBack);
				mainButtonComms.onClick.AddListener(StateToComms);
				mainButtonFiles.onClick.AddListener(StateToFiles);
				signalButtonWaypoint.onClick.AddListener(ButtonPlaceWaypoint);
				signalButtonRead.onClick.AddListener(ButtonReadSignal);
				debugButton.onClick.AddListener(ButtonDebug);
				signalDecodeButton.onClick.AddListener(ButtonDecode);
				filesImageButton.onClick.AddListener(StateToImage);

				//Generate buttons and managers for each signal
				foreach(OWP_LocSignal signal in OWP.signals) {
					OWP_UI_Signal sg = new OWP_UI_Signal(commsGrid, commsPrefab, signal);
					sg.button.onClick.AddListener(delegate{ StateToSignal(sg); });
					signals.Add(sg);
					sg.Update();
				}
				commsPrefab.SetActive(false);

				//Generate buttons for each file
				foreach(OWP_LocSignal signal in OWP.signals) {
					OWP_UI_File f = new OWP_UI_File(filesGrid, filesPrefab, signal);
					f.button.onClick.AddListener(delegate{ StateToFile(f); });
					files.Add(f);
				}
				filesPrefab.SetActive(false);

				//Misc
				timer = 0;
				nextSignalToUpdate = 0;
				state = State.LOADING;

				Debug.Log("[Other Worlds] Created UI");
			}

			public void DestroyUI() {
				StockButtonDisable();

				if(ui == null) return;

				//DESTROY, DESTROY, DESTROY (*Laser noises*)
				ui.DestroyGameObject();
				ui = null;

				signals.Clear();
				files.Clear();

				Debug.Log("[Other Worlds] Deleted UI");
			}

			private Transform GetChild(GameObject go, string name) {
				Transform t = null;
				try {
					t = go.transform.Find(name);
				} catch (Exception e) {
					Debug.LogError("[Other Worlds] Can't find element '" + name + "' as child of '" + go.name + "' in the UI! (Exception type: " + e.GetType().Name + ")");
					return null;
				}
				if(t == null) {
					Debug.LogError("[Other Worlds] Can't find element '" + name + "' as child of '" + go.name + "' in the UI!");
					return null;
				}
				return t;
			}

			/*
				State Machine
				Control Functions
			*/

			enum State {
				LOADING,
				MAIN,
				COMMS,
				SIGNAL,
				FILES,
				FILE,
				IMAGE
			}

			State state = State.LOADING;
			double timer = 1.0;

			OWP_UI_Signal currentSignal = null;
			
			public void Update() {
				if(ui == null || !ui.activeInHierarchy) return;

				if(FlightGlobals.ActiveVessel != null)
					debugText.text = "Distance to Trigger: " + OWP_Ending.DistanceToTrigger(FlightGlobals.ActiveVessel.gameObject.transform);
				else
					debugText.text = "Active Vessel is null... somehow...";

				if(timer > 0) timer -= Time.deltaTime;

				if(state == State.LOADING) {
					if(timer <= 0) {
						StateToMain();
					} else {
						Color aux = bgLoading.color;
						double a = (2 * timer) / TIME_LOADING_SCREEN;
						a = a > 1 ? 1 : a;
						aux.a = (float)a;
						bgLoading.color = aux;
					}
				} else if(state == State.COMMS) {
					UpdateSignals();
				} else if(state == State.SIGNAL) {
					if(currentSignal != null) {
						bool outOfRangeOfSignal = currentSignal.UpdateSignalScreen(signalText, signalButtonWaypoint, signalButtonRead, signalDecodeField, signalDecodeButton);
						if(outOfRangeOfSignal)
							StateGoBack();
					}
				}

				UpdateTopBar();
			}

			public void ButtonDebug() {
				//Debug.LogWarning(OWP_Util.GetObjectHirearchy());
				OWP_Cutscene.DEBUG_CUTSCENE = !OWP_Cutscene.DEBUG_CUTSCENE;
			}

			int nextSignalToUpdate = 0;

			void ChangeUISize() {
				isUISmall = !isUISmall;
				ui.transform.localScale = isUISmall ? new Vector3(REDUCED_SCALE, REDUCED_SCALE, 1) : new Vector3(1, 1, 1);
			}

			public void UpdateSignals() {
				OWP_UI_Signal sg = signals[nextSignalToUpdate++];
				nextSignalToUpdate = nextSignalToUpdate >= signals.Count ? 0 : nextSignalToUpdate;

				sg.Update();
			}
			
			void StateGoBack() {
				if(state == State.FILES) {
					StateToMain();
					return;
				}
				if(state == State.FILE) {
					StateToFiles();
					return;
				}
				if(state == State.COMMS) {
					StateToMain();
					return;
				}
				if(state == State.SIGNAL) {
					StateToComms();
					foreach(OWP_UI_Signal signal in signals) {
						signal.Update();
					}
					currentSignal = null;
					return;
				}
				if(state == State.IMAGE) {
					StateToFiles();
					return;
				}
				Debug.Log("[Other Worlds] [Tablet UI] Back");
			}

			void StateToMain() {
				//Deactivate
				filesScreen.SetActive(false);
				commsScreen.SetActive(false);
				scrollbar.gameObject.SetActive(false);
				bgLoading.gameObject.SetActive(false);
				bgFiles.gameObject.SetActive(false);
				bgComms.gameObject.SetActive(false);
				buttonBack.gameObject.SetActive(false);

				//Activate
				uiTopBar.SetActive(true);
				mainScreen.SetActive(true);
				bgMain.SetActive(true);

				state = State.MAIN;
			}

			void StateToComms() {
				//Deactivate
				mainScreen.SetActive(false);
				signalScreen.SetActive(false);
				bgMain.SetActive(false);

				//Activate
				commsScreen.SetActive(true);
				bgComms.SetActive(true);
				buttonBack.gameObject.SetActive(true);
				scrollbar.gameObject.SetActive(true);

				state = State.COMMS;
			}

			void StateToSignal(OWP_UI_Signal signal) {
				//Deactivate
				commsScreen.SetActive(false);
				scrollbar.gameObject.SetActive(false);

				//Activate
				signalScreen.SetActive(true);

				//Bonus
				currentSignal = signal;
				currentSignal.UpdateSignalScreen(signalText, signalButtonWaypoint, signalButtonRead, signalDecodeField, signalDecodeButton);

				state = State.SIGNAL;
			}

			void StateToFiles() {
				//Deactivate
				mainScreen.SetActive(false);
				fileScreen.SetActive(false);
				bgMain.SetActive(false);
				imageScreen.SetActive(false);

				//Activate
				filesScreen.SetActive(true);
				bgFiles.SetActive(true);
				buttonBack.gameObject.SetActive(true);
				scrollbar.gameObject.SetActive(true);

				//Bonus, show only known files
				foreach(OWP_UI_File file in files) {
					file.ui.SetActive(OWP.GetProgressBit(file.signal.id));
				}
				filesImageButton.gameObject.SetActive(OWP.GetProgressBit(21));

				state = State.FILES;
			}

			void StateToFile(OWP_UI_File file) {
				if(file == null) return;

				//Deactivate
				filesScreen.SetActive(false);

				//Activate
				fileScreen.SetActive(true);
				scrollbar.gameObject.SetActive(true);

				//Insert file data into text fields
				file.UpdateTextFields(fileTitle, fileText);

				state = State.FILE;
			}

			void StateToImage() {
				//Deactivate
				filesScreen.SetActive(false);
				
				//Activate
				imageScreen.SetActive(true);

				state = State.IMAGE;
			}

			void UpdateTopBar() {
				if(uiTopBar.activeSelf) {
					double CT = OWP_Util.CT(); //Assuming it's in seconds

					double year = Math.Floor(CT/31536000);
					double day = Math.Floor((CT/31536000 - year) * 365);
					double hours = Math.Floor(CT/3600.0);
					double minutes = Math.Floor((CT/3600.0 - hours) * 60);
					string sHours = (int)hours%24 >= 10 ? "" + (int)hours%24 : "0" + (int)hours%24;
					string sMinutes = (int)minutes >= 10 ? "" + (int)minutes : "0" + (int)minutes;

					textDate.text = "Y" + (int)year + ", D" + (int)day;
					textHour.text = sHours + ":" + sMinutes;
				}
			}

			/*
				UI On/Off Managment
				Including Stock Toolbar Button
			*/
			
			public void StockButtonEnable() {
				if(appButton != null) return;

				appButton = ApplicationLauncher.Instance.AddModApplication(
					ShowUI, // onTrue
					HideUI, // onFalse
					() => { }, // onHover
					() => { }, // onHoverOut
					() => { }, // onEnable
					() => { }, // onDisable
					ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW, // visibleInScenes
					GameDatabase.Instance.GetTexture("OtherWorldsReboot/Plugins/icon", false) // texture
				);
				Debug.Log("[Other Worlds] Added ModApp Button");
			}

			public void StockButtonDisable() {
				if(appButton == null) return;

				ApplicationLauncher.Instance.RemoveModApplication(appButton);
				appButton = null;
			}

			public void ShowUI() {
				if(ui == null || ui.activeSelf) return;
				ui.SetActive(true);
				Debug.Log("[Other Worlds] Showing UI...");
			}

			public void HideUI() {
				if(ui == null || !ui.activeSelf) return;
				ui.SetActive(false);
				Debug.Log("[Other Worlds] Hiding UI...");
			}

			/*
				Non Navigation Buttons
			*/

			void ButtonPlaceWaypoint() {
				if(currentSignal == null) return;

				Vessel active = FlightGlobals.ActiveVessel;
				Color color = Color.red;

				/*
				Waypoint wp = new Waypoint();
				wp.name = "Unknown Signal " + currentSignal.signal.id + "\nSignal Strength: " + currentSignal.signalStrengthText;
				wp.celestialName = active.mainBody.GetName();
				wp.longitude = active.longitude;
				wp.latitude = active.latitude;
				if(active.Landed) {
					wp.isOnSurface = true;
				} else {
					wp.isOnSurface = false;
					wp.altitude = active.altitude;
				}
				wp.isCustom = true;
				wp.isNavigatable = true;
				
				ScenarioCustomWaypoints.AddWaypoint(wp);
				*/
				OWP_Util.NewWaypoint(
					$"Unknown Signal {currentSignal.signal.id}\nSignal Strength: {currentSignal.signalStrengthText}"
					, active.mainBody.GetName()
					, active.longitude
					, active.latitude
					, active.Landed ? -1 : active.altitude
				);

				//Noup, this ain't work fam
				//I would have to do a custom waypoint loader/saver for this.
				//wp.id = "dish"; //Dear lord I hope this works, AHHHH
				//wp.seed = 1;
				//wp.SetupMapNode();
				//wp.node = KSP.UI.Screens.Mapview.MapNode.Create(wp.name, color, ContractDefs.sprites["dish"].texture.width, true, false, wp.blocksInput); //Maybe it works after adding and setting fade
			}

			void ButtonReadSignal() {
				if(currentSignal != null) {
					if(currentSignal.signal.cutscene == null) {
						Debug.LogWarning("[Other Worlds] The signal '" + currentSignal.signal.name + "' you've just tried to access has no cutscene");
						return;
					}

					int id = currentSignal.signal.id;
					OWP.SetProgressBit(id, true);

					OWP_Cutscene.StartCutscene(currentSignal.signal.cutscene, currentSignal.signal.pqs.objects[0].objects[0].transform);

					Debug.Log("[Other Worlds] Progress updated for signal " + id);
				}
			}

			void ButtonDecode() {
				if(signalDecodeField.text.Equals("7546")) {
					Debug.Log("[Other Worlds] Decode code inserted correctly!");
					OWP.SetProgressBit(31, true);
					StateToSignal(currentSignal);
				} else {
					Debug.Log("[Other Worlds] The code '" + signalDecodeField.text + "' is not correct (But at least you tried!)");
				}
			}

			/*
				Outside Control
			*/

			public void CheckIfArtifactOnCraft() {
				Vessel active = FlightGlobals.ActiveVessel;
				if(active == null || OWP.artifact1 == null || OWP.artifact1rec == null) return;

				bool hasArtifact = false;

				//Is EVA Kerbal
				if(active.isEVA) {
					if(active.evaController == null || active.evaController.ModuleInventoryPartReference == null) return;
					ModuleInventoryPart inv = active.evaController.ModuleInventoryPartReference;
					if(inv.storedParts == null) return;
					foreach(StoredPart sp in inv.storedParts.Values) {
						if(sp.partName.Equals(OWP.artifact1.name) || sp.partName.Equals(OWP.artifact1rec.name)) {
							hasArtifact = true;
							break;
						}
					}
				}

				//Not a EVA Kerbal, check every goddamn part :skull:
				else {
					if(active.GetVesselCrew() == null) return;
					foreach(ProtoCrewMember pCrew in active.GetVesselCrew()) {
						ModuleInventoryPart inv = pCrew.KerbalInventoryModule;
						foreach(StoredPart sp in inv.storedParts.Values) {
							if(sp.partName.Equals(OWP.artifact1.name) || sp.partName.Equals(OWP.artifact1rec.name)) {
								hasArtifact = true;
								break;
							}
						}
					}
				}

				if(hasArtifact) {
					if(ui != null && !ui.activeSelf && appButton == null)
						StockButtonEnable();
				} else {
					HideUI();
					StockButtonDisable();
				}
			}
		}

		class OWP_UI_File {
			public GameObject ui;
			public Text text;
			public Button button;
			public OWP_LocSignal signal;

			public OWP_UI_File(GameObject grid, GameObject prefab, OWP_LocSignal signal) {
				this.signal = signal;
				ui = OWP.Instantiate(prefab, grid.transform);
				ui.transform.SetAsFirstSibling();
				ui.SetActive(false);

				button = ui.GetComponent<Button>();
				text = ui.transform.Find("Text").GetComponent<Text>();

				text.text = signal.name + ".txt";
			}

			public void UpdateTextFields(Text title, Text body) {
				title.text = "./sig_data/" + signal.name + ".txt";
				
				body.text = "Signal: \"" + signal.name + "\"\nLocation: " + Localizer.Format("<<1>>", signal.body.bodyDisplayName) + "\nCoordinates: ???????\n\nExtract: " + signal.description;
			}
		}

		class OWP_UI_Signal {
			//UI Elements
			public GameObject ui;
			public Button button; //Component of ui
			public Image bg;
			public Text text, textSmall;

			//Signal linked with this UI element
			public OWP_LocSignal signal;

			public string signalStrengthText = "";

			public OWP_UI_Signal(GameObject grid, GameObject prefab, OWP_LocSignal signal) {
				this.signal = signal;
				ui = OWP.Instantiate(prefab, grid.transform);
				ui.transform.SetAsFirstSibling();
				ui.SetActive(false);

				bg = ui.GetComponent<Image>();

				text = ui.transform.Find("TextMain").GetComponent<Text>();
				textSmall = ui.transform.Find("TextSmall").GetComponent<Text>();
				button = ui.transform.GetComponent<Button>();
			}

			public void Update() {
				ui.SetActive(true);
				double distance = signal.DistanceFrom(FlightGlobals.ActiveVessel);
				double strength = signal.GetSignalStrength(distance);
				strength = strength > 0.9999 ? 0.9999 : strength;

				if(strength <= 0) {
					ui.SetActive(false);
				} else {
					ui.SetActive(true);
					if(strength < 0.25) {
						text.text = "Unknown Signal (ID " + signal.id + ")\nStrength: " + String.Format("{0:0.000}", strength * 100) + "%";
						bg.color = Color.grey;
						textSmall.text = "Click for Information and Tracking";
					} else {
						if(signal.VesselInRange(FlightGlobals.ActiveVessel)) {
							text.text = "Signal (ID " + signal.id + "): " + signal.name + "\nStrength: 100%";
							bg.color = Color.green;
							textSmall.text = "Click to Read the Signal";
						} else {
							text.text = "Signal (ID " + signal.id + "): " + signal.name + "\nStrength: " + String.Format("{0:0.000}", strength * 100) + "%";
							bg.color = Color.white;
							textSmall.text = "Click for more Information";
						}
					}
				}
			}

			//Returns true if signal is no longer in range (must be kicked out)
			public bool UpdateSignalScreen(Text text, Button buttonWaypoint, Button buttonRead, InputField decodeField, Button decodeButton) {
				Vessel active = FlightGlobals.ActiveVessel;

				//Calculate strength
				double distance = signal.DistanceFrom(active);
				double strength = signal.GetSignalStrength(distance);

				if(strength <= 0) return true; //Out of range, why are you even updating this info?

				signalStrengthText = String.Format("{0:0.000}", strength * 100) + "%";

				bool canReadEncoded = OWP.GetProgressBit(31);
				bool canReadSignal = canReadEncoded || !signal.encoded;
				bool close25percent = strength >= 0.25;

				//Build the main info text dump
				text.text = "Signal ID: " + signal.id + "\n";
				if(close25percent && canReadSignal)
					text.text += "Signal Name: " + signal.name + "\n";
				if(!signal.VesselInRange(active))
					text.text += "\nSignal Strength: " + signalStrengthText + "\n";
				else
					text.text += "\nSignal Strength: 100%\n";
				if(close25percent) {
					if(!signal.encoded) //No Encoding
						text.text += "Location: " + Localizer.Format("<<1>>", signal.body.bodyDisplayName) + "\n\tLatitude: ????\n\tLongitude: ????\n\nEncoding: No\n";
					else if(!canReadEncoded) //Encoding, Unknown
						text.text += "Location: ????????\n\nEncoding: Yes (Unknown Code)\n";
					else //Encoding, Known
						text.text += "Location: " + Localizer.Format("<<1>>", signal.body.bodyDisplayName) + "\n\tLatitude: ????\n\tLongitude: ????\n\nEncoding: Yes (Decoded)\n";
				} else {
					text.text += "Location: ????????\n\nEncoding: ????????\n";
				}
				
				//Manage buttons
				bool interactRead = (canReadSignal && signal.VesselInRange(active) && active.Landed && 
									(TimeWarp.CurrentRate < 1.1 && TimeWarp.CurrentRate > 0.9) && !MapView.MapIsEnabled && 
									CameraManager.Instance.currentCameraMode != CameraManager.CameraMode.IVA && active.GetCrewCount() > 0);
				bool interactWayp = (!(close25percent && canReadSignal && signal.body != active.mainBody));// && active.Landed;

				//Show the decoding text field here
				decodeField.gameObject.SetActive(!canReadSignal && close25percent);
				decodeButton.gameObject.SetActive(!canReadSignal && close25percent);

					//Should update text color here

				buttonRead.interactable = interactRead;
				buttonWaypoint.interactable = interactWayp; //For the moment, restricted to landed

				return false;
			}
		}
	}
}