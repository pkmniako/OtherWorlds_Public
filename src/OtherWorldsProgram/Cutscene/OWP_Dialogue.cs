using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_DialogueField {
			public TextMeshProUGUI text; //Text field where the dialogue gets written into
			public Animator bgAnimator;
			public Button buttonTemplate;
			public List<Animator> charCamAnims;
			public List<string> dialogues; //What the dialogue says. Separated in different dialogues

			double timer; //Timer, used for timing stuff (whoa!)
			int c; //What character of the dialogue comes next
			int d; //Which of the strings in dialogues is currently being displayed

			double timeBetweenLetters = 0.05;
			double timeBetweenCommas = 0.5;
			double timeBetweenSentences = 2.0;
			double timeBetweenDialogues = 10.0;
			double timeBetweenWords = 0.0;
			bool prevSlash = false;

			//Branching Dialogue
			int branchesNumber;
			List<Button> branchesButtons; //The buttons for branching

			// Out Variable
			public int variable {
				private set; get;
			}


			
			//Attributes
			double attributeOverrideTime = -1.0;
			double attributeTimeNext = -1.0;

			bool enabled = false;

			public OWP_DialogueField(TextMeshProUGUI text, Animator bgAnimator, Button buttonTemplate, List<string> dialogues, params Animator[] animators) {
				this.text = text;
				this.text.text = "";
				this.dialogues = dialogues;
				this.bgAnimator = bgAnimator;
				this.buttonTemplate = buttonTemplate;
				this.charCamAnims = new List<Animator>();
				this.variable = 0;
				foreach(var a in animators)
					if(a != null)
						this.charCamAnims.Add(a);
				Debug.Log("[Other Worlds] OWP_Dialogue tracking " + this.charCamAnims.Count + " animators");
			}

			//Also used to reset some stuff
			public void Start() {
				enabled = true;
				c = 0;
				d = 0;
				timer = Time.deltaTime;
				text.text = "";
				branchesNumber = 0;

				DestroyBranchButtons();				
			}

			public void DestroyBranchButtons() {
				if(branchesButtons == null)
					branchesButtons = new List<Button>();
				else {
					foreach(Button b in branchesButtons) {
						GameObject.Destroy(b.gameObject);
					}
					branchesButtons.Clear();
				}
				branchesNumber = 0;
			}

			private void AddBranchingButton(string name, int branchDialogueID) {
				if(buttonTemplate == null) return;

				Button newButton = GameObject.Instantiate(buttonTemplate.gameObject).GetComponent<Button>();
				newButton.transform.SetParent(buttonTemplate.transform.parent, worldPositionStays: false);

				if(branchesButtons == null)
					branchesButtons = new List<Button>();

				branchesButtons.Add(newButton);

				newButton.gameObject.SetActive(true);
				newButton.transform.Find("Text").GetComponent<Text>().text = name;
				newButton.onClick.AddListener(() => BranchingButtonCall(branchDialogueID));
			}

			private void BranchingButtonCall(int branchDialogueID) {
				Start();
				d = branchDialogueID;
			}

			public void Update() {
				timer -= Time.deltaTime;

				//Current text must be finished
				while(timer <= 0 && c < dialogues[d].Length) {
					if(!prevSlash) 
						while(ReadAttributes()) {} //Damn, this line is cursed
					
					if(c >= dialogues[d].Length)
						break;

					prevSlash = false;

					char C = dialogues[d][c];

					//Time till the next letter
					DelayTimeFromCharacter(C);

					if(C.Equals('\\')) {							 
						prevSlash = true;
					} else if(C.Equals('<')) {
						while(!C.Equals('>')) {
							text.text += C;
							c++;
							C = dialogues[d][c];
						}
					}

					text.text += C;

					c++;
				}
				
				//Advance to next dialogue
				/*if(timer <= 0 && c >= dialogues[d].Length) {
					if(d + 1 < dialogues.Count) {
						d++;
						c = 0;
						timer = -1;
						text.text = "";
					} else {
						c = dialogues[d].Length;
					}
				}*/
			}

			private void DelayTimeFromCharacter(char C) {
				if(attributeTimeNext > 0) {
					timer += attributeTimeNext;
					attributeTimeNext = -1;
				} else if(attributeOverrideTime > 0) {
					timer += attributeOverrideTime;
				} else {
					if(C.Equals(' ')) {
						timer += timeBetweenWords;
					} else if(C.Equals(',')) {
						timer += timeBetweenCommas;
					} else if(C.Equals('.') && c + 1 < dialogues[d].Length) {
						timer += timeBetweenSentences;
					} else {
						timer += timeBetweenLetters;
					}
				}
				if(c + 1 >= dialogues[d].Length) {
					timer += timeBetweenDialogues;
				}
			}

			/// List of attributes this class uses to control various aspects of the game, like timings, interacting with the UI, ...
			private void ProcessAttribute(string attribute, string value) {
				//Time Related
				if(attribute.Equals("timing")) {
					attributeOverrideTime = Double.Parse(value);
				} else if(attribute.Equals("timenext")) {
					attributeTimeNext = Double.Parse(value);
				} else if(attribute.Equals("timingreset")) {
					attributeOverrideTime = -1;
				} else if(attribute.Equals("endwait")) {
					timeBetweenDialogues = Double.Parse(value);
				}

				//General text related
				else if(attribute.Equals("clear")) {
					text.text = "";
				}


				//Variable related
				else if(attribute.Equals("var")) {
					variable = Int32.Parse(value);
				}
				
				//UI Related
				else if(attribute.Equals("showui") && value.Equals("true")) {
					if(bgAnimator) bgAnimator.SetBool("FadeWhite", true);
				}
				else if(attribute.Equals("close")) {
					if(value.Equals("long")) {
						if(bgAnimator) bgAnimator.SetInteger("Close", 2);
					} else {
						if(bgAnimator) bgAnimator.SetInteger("Close", 1);
					}
				} else if(attribute.Equals("cameraui")) {
					if(bgAnimator) bgAnimator.SetBool("ShowUI", !Boolean.Parse(value));
				} else if(attribute.Equals("switchanim")) {
					foreach(Animator a in charCamAnims)
						a.SetInteger("sub", Int32.Parse(value));
				} else if(attribute.Equals("ending")) {
					bgAnimator.SetBool("Ending", true);
				} else if(attribute.Equals("switchtext")) {
					Start();
					d = Int32.Parse(value);
				} else if(attribute.Equals("branch")) {
					string[] data = value.Split(';');
					int branchPath = Int32.Parse(data[0]);
					string branchDisplay = data[1];

					AddBranchingButton(branchDisplay, branchPath);
				} else if(attribute.Equals("branchforce")) {
					BranchingButtonCall(Int32.Parse(value));
				}
			}

			private bool ReadAttributes() {
				//Returns true if it had found an attribute
				if(dialogues[d].Length >= c + 4 && dialogues[d][c].Equals('#') && dialogues[d][c + 1].Equals('(')) {
					c += 2;

					while(c < dialogues[d].Length) {
						string attr = "";
						bool attrDone = false;

						//Read the attribute name
						while(c < dialogues[d].Length) {
							char C = dialogues[d][c];
							if(C.Equals('=')) {
								attrDone = true;
								c++;
								break;
							} else if(C.Equals(')')) {
								Debug.LogError("[Other Worlds] Error while reading dialogue. Text " + d + ", character " + c + ": Unexpected ')'. An attribute must have a value!");
								c++;
								return true;
							} else {
								attr += C;
								c++;
							}
						}
						if(!attrDone) {
							Debug.LogError("[Other Worlds] Error while reading dialogue. Text " + d + ", character " + c + ": Attribute never ends!");
							return true;
						}
						attr = attr.ToLower();

						//Read the value
						string value = "";
						bool valueDone = false;
						bool end = false;
						while(c < dialogues[d].Length) {
							char C = dialogues[d][c];
							if(C.Equals(',')) {
								valueDone = true;
								c++;
								break;
							} else if(C.Equals(')')) {
								valueDone = true;
								end = true;
								c++;
								break;
							} else {
								c++;
								value += C;
							}
						}
						if(!valueDone) {
							Debug.LogError("[Other Worlds] Error while reading dialogue. Text " + d + ", character " + c + ": Value for attribute '" + attr + "' never ends!");
							return true;
						}

						//Process attribute+value
						ProcessAttribute(attr, value);

						if(end)
							return true;
					}
					return true;
				}
				return false;
			}
		}
	}
}