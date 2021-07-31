using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenHexabuttons {

	private ColorfulButtonSeries coloredHexabuttons;
	private KMAudio Audio;
	private AudioClip[] notes;
	private int moduleId;
	private KMSelectable[] hexButtons;
	private MeshRenderer[] buttonMesh;
	private TextMesh[] buttonText; 
	private Material[] ledColors;
	private MeshRenderer[] ledMesh;
	private Light[] lights;
	private Transform transform;
	private int[] solution;
	private int[] submission;
	private int numButtonPresses;
	string greenNotes;
	private bool moduleSolved;
	private bool flag;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private string[] noteNames = { "C", "C♯/D♭", "D", "D♯/E♭", "E", "F", "F♯/G♭", "G", "G♯/A♭", "A", "A♯/B♭", "B"};
	private string[] table1 =
	{
		"13","03","12","45","01","31",
		"23","32","30","41","54","50",
		"53","40","34","14","42","04",
		"24","02","21","25","52","05",
		"15","51","43","10","35","20"
	};
	private int[][] table2 =
	{
		new int[]{ 3, 5, 1, 2, 4, 0, 2, 5, 1, 0, 3, 4 },
		new int[]{ 4, 0, 3, 1, 2, 5, 0, 3, 5, 1, 4, 2 },
		new int[]{ 0, 2, 5, 4, 1, 3, 3, 2, 4, 5, 1, 0 },
		new int[]{ 5, 4, 2, 0, 3, 1, 1, 4, 0, 3, 2, 5 },
		new int[]{ 2, 1, 0, 3, 5, 4, 4, 0, 3, 2, 5, 1 },
		new int[]{ 1, 3, 4, 5, 0, 2, 5, 1, 2, 4, 0, 3 }
	};
	public GreenHexabuttons(ColorfulButtonSeries m, KMAudio aud, AudioClip[] N, int MI, KMSelectable[] HB, MeshRenderer[] BM, TextMesh[] BT, Material[] LC, MeshRenderer[] LM, Light[] L, Transform T)
	{
		coloredHexabuttons = m;
		Audio = aud;
		notes = N;
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
		flag = false;
		moduleSolved = false;
		solution = new int[6];
		string noteOrder = "";
		string choices = "012345";
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color Generated: Green", moduleId);
		for(int aa = 0; aa < 6; aa++)
		{
			solution[aa] = choices[UnityEngine.Random.Range(0, choices.Length)] - '0';
			string noteChoices = "";
			for(int bb = 0; bb < 12; bb++)
			{
				if (table2[aa][bb] == solution[aa])
					noteChoices = noteChoices + "" + "0123456789AB"[bb];
			}
			noteOrder = noteOrder + "" + noteChoices[UnityEngine.Random.Range(0, noteChoices.Length)];
			choices = choices.Replace(solution[aa] + "", "");
		}
		choices = "012345";
		string buttonOrder = "";
		greenNotes = "------";
		buttonText[6].text = "";
		var alpha = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		for(int i = 0; i < 6; i++)
		{
			buttonOrder = buttonOrder + "" + choices[UnityEngine.Random.Range(0, choices.Length)];
			choices = choices.Replace(buttonOrder[i] + "", "");
			greenNotes = greenNotes.Substring(0, (buttonOrder[i] - '0')) + "" + noteOrder[i] + "" + greenNotes.Substring((buttonOrder[i] - '0') + 1);
			if(i % 2 == 1)
			{
				string temp = buttonOrder[i - 1] + "" + buttonOrder[i];
				for(int aa = 0; aa < table1.Length; aa++)
				{
					if(table1[aa].Equals(temp))
					{
						buttonText[6].text = buttonText[6].text + "" + alpha[aa];
						break;
					}
				}
			}
		}
		var num = UnityEngine.Random.Range(0, 6);
		var noteName = noteNames["0123456789AB".IndexOf(greenNotes[num])];
		if(noteName.Length == 1)
			buttonText[num].text = noteName;
		else
		{
			var spl = noteName.Split('/');
			buttonText[num].text = spl[UnityEngine.Random.Range(0, 2)];
		}
		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedGreen(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { pressedGreenRelease(i); };
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} button is playing {2}", moduleId, positions[i], noteNames["0123456789AB".IndexOf(greenNotes[i])]);
		}
		hexButtons[6].OnInteract = delegate { pressedGreenCenter(); return false; };
		hexButtons[6].OnInteractEnded = delegate { pressedGreenCenterRelease(); };
		submission = new int[6];
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Center Text: {1}", moduleId, buttonText[6].text);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Button Read Order: {1} {2} {3} {4} {5} {6}", moduleId, positions[buttonOrder[0] - '0'], positions[buttonOrder[1] - '0'], positions[buttonOrder[2] - '0'], positions[buttonOrder[3] - '0'], positions[buttonOrder[4] - '0'], positions[buttonOrder[5] - '0']);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Note Order: {1} {2} {3} {4} {5} {6}", moduleId, noteNames["0123456789AB".IndexOf(noteOrder[0])], noteNames["0123456789AB".IndexOf(noteOrder[1])], noteNames["0123456789AB".IndexOf(noteOrder[2])], noteNames["0123456789AB".IndexOf(noteOrder[3])], noteNames["0123456789AB".IndexOf(noteOrder[4])], noteNames["0123456789AB".IndexOf(noteOrder[5])]);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Solution: {1} {2} {3} {4} {5} {6}", moduleId, positions[solution[0]], positions[solution[1]], positions[solution[2]], positions[solution[3]], positions[solution[4]], positions[solution[5]]);
	}
	void pressedGreen(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlaySoundAtTransform(notes["0123456789AB".IndexOf(greenNotes[n])].name, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	void pressedGreenRelease(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	void pressedGreenSubmit(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlaySoundAtTransform(notes["0123456789AB".IndexOf(greenNotes[n])].name, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			submission[numButtonPresses] = n;
			numButtonPresses++;
			hexButtons[n].OnInteract = null;
			ledMesh[n].material = ledColors[1];
			if (numButtonPresses == 6)
			{
				Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User Submission: {1} {2} {3} {4} {5} {6}", moduleId, positions[submission[0]], positions[submission[1]], positions[submission[2]], positions[submission[3]], positions[submission[4]], positions[submission[5]]);
				flag = false;
				bool correct = true;
				for(int aa = 0; aa < 6; aa++)
				{
					if(submission[aa] != solution[aa])
					{
						correct = false;
						break;
					}
				}
				if(correct)
				{
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
	void pressedGreenCenter()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			if (flag)
			{
				flag = false;
				resetInput();
			}
			else
			{
				flag = true;
				numButtonPresses = 0;
				foreach (int i in buttonIndex)
				{
					hexButtons[i].OnInteract = delegate { pressedGreenSubmit(i); return false; };
					hexButtons[i].OnInteractEnded = null;
				}
				coloredHexabuttons.StartCoroutine(greenFlasher());
			}
		}
	}
	void pressedGreenCenterRelease()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	IEnumerator greenFlasher()
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
		foreach (int i in buttonIndex)
		{
			Vector3 pos = buttonMesh[i].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[i].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			hexButtons[i].OnInteract = delegate { pressedGreen(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { pressedGreenRelease(i); };
			ledMesh[i].material = ledColors[0];
		}
	}
}
