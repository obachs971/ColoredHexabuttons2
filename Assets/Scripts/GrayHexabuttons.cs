using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayHexabuttons {

	private ColorfulButtonSeries coloredHexabuttons;
	private KMAudio Audio;
	private int moduleId;
	private KMSelectable[] hexButtons;
	private MeshRenderer[] buttonMesh;
	private TextMesh[] buttonText;
	private Material[] ledColors;
	private MeshRenderer[] ledMesh;
	private Light[] lights;
	private Transform transform;
	private char[] submission;
	private int numButtonPresses;
	private bool moduleSolved;
	private bool flag;
	private int tableIndex;
	private string buttonLetters;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private string[][] tables =
	{
		new string[]{
			"DCEABF",
			"ADBCFE",
			"BECFDA",
			"FBDEAC",
			"EAFDCB",
			"CFABED"
		},
		new string[]{
			"EDCFBA",
			"CFEDAB",
			"DBFACE",
			"ACBEFD",
			"BEACDF",
			"FADBEC"
		},
		new string[]{
			"CDAFEB",
			"DEBACF",
			"EAFDBC",
			"BFCEDA",
			"FBDCAE",
			"ACEBFD"
		},
		new string[]{
			"FAEDCB",
			"BFACED",
			"CDBEAF",
			"EBDAFC",
			"ACFBDE",
			"DECFBA"
		},
		new string[]{
			"BCEDFA",
			"FBACDE",
			"AEDFBC",
			"DFCEAB",
			"CAFBED",
			"EDBACF"

		},
		new string[]{
			"AEBFCD",
			"EBFCDA",
			"FDCBAE",
			"CFDAEB",
			"DCAEBF",
			"BAEDFC"
		},
		new string[]{
			"CBFADE",
			"EADCBF",
			"DECFAB",
			"FCABED",
			"BFEDCA",
			"ADBEFC"
		},
		new string[]{
			"DCBFEA",
			"BDECAF",
			"AFDBCE",
			"EBFADC",
			"FACEBD",
			"CEADFB"
		},
		new string[]{
			"FCEDBA",
			"CBFEAD",
			"BDCAFE",
			"AEBFDC",
			"EADBCF",
			"DFACEB"
		},
		new string[]{
			"BDEACF",
			"ACFEDB",
			"FABCED",
			"CFADBE",
			"DECBFA",
			"EBDFAC"
		},
		new string[]{
			"AFEBCD",
			"FEBDAC",
			"EDACBF",
			"DCFAEB",
			"CBDEFA",
			"BACFDE"
		},
		new string[]{
			"EFBDCA",
			"DAECBF",
			"CDFAEB",
			"BECFAD",
			"ACDBFE",
			"FBAEDC"
		}
	};
	public GrayHexabuttons(ColorfulButtonSeries m, KMAudio aud, int MI, KMSelectable[] HB, MeshRenderer[] BM, TextMesh[] BT, Material[] LC, MeshRenderer[] LM, Light[] L, Transform T)
	{
		coloredHexabuttons = m;
		Audio = aud;
		moduleId = MI;
		hexButtons = HB;
		buttonMesh = BM;
		buttonText = BT;
		ledColors = LC;
		ledMesh = LM;
		lights = L;
		transform = T;
	}
	public void run()
	{
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color Generated: Gray", moduleId);
		tableIndex = UnityEngine.Random.Range(0, tables.Length);
		string choices = "ABCDEF";
		buttonLetters = "";
		foreach(int i in buttonIndex)
		{
			buttonLetters = buttonLetters + "" + choices[UnityEngine.Random.Range(0, choices.Length)];
			choices = choices.Replace(buttonLetters[i] + "", "");
			hexButtons[i].OnInteract = delegate { pressedGray(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { pressedGrayRelease(i); };
		}
		hexButtons[6].OnInteract = delegate { pressedGrayCenter(); return false; };
		hexButtons[6].OnInteractEnded = delegate { pressedGrayCenterRelease(); };
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Button Letters: {1}", moduleId, buttonLetters);
		buttonText[6].text = "ABCDEF"[UnityEngine.Random.Range(0, 6)] + "";
		lights[6].color = Color.white;
		submission = new char[6];
	}
	
	void pressedGray(int n)
	{
		if(!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos2 = buttonMesh[n].transform.localPosition;
			pos2.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos2.x, pos2.y, pos2.z);
			buttonText[6].text = tables[tableIndex]["ABCDEF".IndexOf(buttonLetters[n])]["ABCDEF".IndexOf(buttonText[6].text)] + "";
		}
	}
	void pressedGrayRelease(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
			Vector3 pos2 = buttonMesh[n].transform.localPosition;
			pos2.y = 0.0169f;
			buttonMesh[n].transform.localPosition = new Vector3(pos2.x, pos2.y, pos2.z);
		}
	}
	void pressedGraySubmit(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			hexButtons[n].OnInteract = null;
			ledMesh[n].material = ledColors[1];
			submission[numButtonPresses] = buttonLetters[n];
			numButtonPresses++;
			if (numButtonPresses == 6)
			{
				Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User Submission: {1}{2}{3}{4}{5}{6}", moduleId, submission[0], submission[1], submission[2], submission[3], submission[4], submission[5]);
				bool correct = true;
				for(int aa = 0; aa < 6; aa++)
				{
					if(submission[aa] != "ABCDEF"[aa])
					{
						correct = false;
						break;
					}
				}
				if(correct)
				{
					flag = false;
					moduleSolved = true;
					coloredHexabuttons.Solve();
				}
				else
				{
					coloredHexabuttons.Strike();
					resetInput();
				}
			}
		}
	}
	void pressedGrayCenter()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			if (flag)
			{
				resetInput();
			}
			else
			{
				flag = true;
				numButtonPresses = 0;
				foreach (int i in buttonIndex)
				{
					Vector3 pos2 = buttonMesh[i].transform.localPosition;
					pos2.y = 0.0169f;
					buttonMesh[i].transform.localPosition = new Vector3(pos2.x, pos2.y, pos2.z);
					ledMesh[i].material = ledColors[0];
					hexButtons[i].OnInteract = delegate { pressedGraySubmit(i); return false; };
					hexButtons[i].OnInteractEnded = null;
				}
				buttonText[6].text = "";
				coloredHexabuttons.StartCoroutine(grayFlasher());
			}
		}
	}
	void pressedGrayCenterRelease()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	IEnumerator grayFlasher()
	{
		yield return new WaitForSeconds(1.0f);
		while (flag)
		{
			lights[6].enabled = true;
			yield return new WaitForSeconds(1.0f);
			lights[6].enabled = false;
			yield return new WaitForSeconds(1.0f);
		}
		lights[6].enabled = false;
	}
	void resetInput()
	{
		flag = false;
		buttonText[6].text = "ABCDEF"[UnityEngine.Random.Range(0, 6)] + "";
		foreach (int i in buttonIndex)
		{
			Vector3 pos2 = buttonMesh[i].transform.localPosition;
			pos2.y = 0.0169f;
			buttonMesh[i].transform.localPosition = new Vector3(pos2.x, pos2.y, pos2.z);
			hexButtons[i].OnInteract = delegate { pressedGray(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { pressedGrayRelease(i); };
			ledMesh[i].material = ledColors[0];
		}
	}
}

