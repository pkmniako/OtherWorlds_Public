using UnityEngine;
using UnityEngine.UI;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_UIText {

			public Color TRANSPARENT = new Color(1,1,1,0);
			public Color MAXCOLOR = new Color(0,0,0,0.5f);

			public Text text;
			public Image image;
			string textTarget;
			double timer;
			double timeAfterComma, timeAfterPeriod, timeBetweenCharacters, timeAfterDialogue, timeFade;
			int index;
			int phase = 0;

			public bool hasEnded {
				get; private set;
			}

			public OWP_UIText(Text text, Image image, Color fullColor) {
				if(text == null || image == null) {
					Debug.LogError("[Other Worlds] Something tried to create a OWP_UIText with a null text or image element");
				} else {
					this.text = text;
					this.image = image;
					this.text.gameObject.SetActive(false);
					this.image.gameObject.SetActive(false);
					hasEnded = true;
					MAXCOLOR = fullColor;
				}
			}

			public void Update() {
				if(!hasEnded && text != null && image != null) {
					if(timer > 0) timer -= Time.deltaTime;

					if(phase == 0) { //Fade in
						if(timer <= 0) {
							phase = 1;
							image.color = MAXCOLOR;
							timer = timeBetweenCharacters;
						} else {
							image.color = Color.Lerp(MAXCOLOR, TRANSPARENT, (float)(timer/timeFade)); //Double to float every frame :puke:
						}

					} else if(phase == 1) { //Text Display
						if(timer <= 0) {
							if(index < textTarget.Length) {
								char c;
								do {
									c = textTarget[index++];

									text.text += c;

								} while(c == ' ' && index < textTarget.Length);

								if(c == '.') {
									timer = timeAfterPeriod;
								} else if(c == ',') {
									timer = timeAfterComma;
								} else {
									timer = timeBetweenCharacters;
								}

								if(index == textTarget.Length) {
									phase = 2;
									timer = timeAfterDialogue;
								}
							}
						}
					} else if(phase == 2) { //Wait
						if(timer <= 0) {
							phase = 3;
							timer = timeFade;
						}

					} else if(phase == 3) { //Fade out
						if(timer <= 0) {
							phase = 4;
							image.color = TRANSPARENT;
							timer = timeBetweenCharacters;
							this.text.gameObject.SetActive(false);
							this.image.gameObject.SetActive(false);
							hasEnded = true;
						} else {
							image.color = Color.Lerp(TRANSPARENT, MAXCOLOR, (float)(timer/timeFade)); //Double to float every frame :puke:
						}
					}
				}
			}

			public void SetText(string text, double timeFade = 0.5, double timeAfterDialogue = 4, double timeBetweenCharacters = 0.1, double timeAfterPeriod = 0.75, double timeAfterComma = 0.5) {
				if(text == null || image == null) return;

				index = 0;
				textTarget = text;
				timer = timeFade;
				phase = 0;
				hasEnded = false;

				this.timeFade = timeFade;
				this.text.text = "";
				this.timeBetweenCharacters = timeBetweenCharacters;
				this.timeAfterComma = timeAfterComma;
				this.timeAfterPeriod = timeAfterPeriod;
				this.timeAfterDialogue = timeAfterDialogue;

				this.text.gameObject.SetActive(true);
				this.image.gameObject.SetActive(true);

				Debug.Log("[Other Worlds DEBUG] SetText()");

				image.color = TRANSPARENT;
			}
		}
	}
}