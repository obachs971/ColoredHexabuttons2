using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class musicalHexabuttons : MonoBehaviour
{
	public KMBombModule module;
	private static int moduleIdCounter = 1;
	public KMAudio Audio;
	public AudioClip[] notes;
	private int moduleId;
	public KMSelectable[] hexButtons;
	public MeshRenderer[] buttonMesh;
	public TextMesh[] buttonText;
	public Material[] ledColors;
	public MeshRenderer[] ledMesh;
	public Light centerFlash;
	private int[] solution;
	private int[] submission;
	private int numButtonPresses;
	private int[] greenNotes;
	private bool moduleSolved;
	private bool flag;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private string[] noteNames = { "C", "C♯/D♭", "D", "D♯/E♭", "E", "F", "F♯/G♭", "G", "G♯/A♭", "A", "A♯/B♭", "B" };
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
	void Awake()
	{
		moduleId = moduleIdCounter++;
		moduleSolved = false;
		flag = false;
		solution = new int[6];
		int[] noteOrder = new int[6];
		string choices = new string("012345".ToCharArray().Shuffle());
		for (int aa = 0; aa < 6; aa++)
		{
			solution[aa] = choices[aa] - '0';
			List<int> noteChoices = new List<int>();
			for (int bb = 0; bb < 12; bb++)
			{
				if (table2[aa][bb] == solution[aa])
					noteChoices.Add(bb);
			}
			noteOrder[aa] = noteChoices[UnityEngine.Random.Range(0, noteChoices.Count)];
		}
		string buttonOrder = new string("012345".ToCharArray().Shuffle());
		greenNotes = new int[6];
		buttonText[6].text = "";
		var alpha = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		for (int i = 0; i < 6; i+=2)
		{
			greenNotes[buttonOrder[i] - '0'] = noteOrder[i] + (12 * UnityEngine.Random.Range(0, 2));
			greenNotes[buttonOrder[i + 1] - '0'] = noteOrder[i + 1] + (12 * UnityEngine.Random.Range(0, 2));
			string temp = buttonOrder[i] + "" + buttonOrder[i + 1];
			for (int aa = 0; aa < table1.Length; aa++)
			{
				if (table1[aa].Equals(temp))
				{
					buttonText[6].text = buttonText[6].text + "" + alpha[aa];
					break;
				}
			}
		}
		var num = UnityEngine.Random.Range(0, 6);
		var noteName = noteNames[greenNotes[num] % 12];
		if (noteName.Length == 1)
			buttonText[num].text = noteName;
		else
		{
			var spl = noteName.Split('/');
			buttonText[num].text = spl[UnityEngine.Random.Range(0, 2)];
		}
		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { releasedButton(i); };
			Debug.LogFormat("[Musical Hexabuttons #{0}] {1} button is playing {2}", moduleId, positions[i], noteNames[greenNotes[i] % 12]);
		}
		hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
		hexButtons[6].OnInteractEnded = delegate { releasedCenter(); };
		submission = new int[6];
		Debug.LogFormat("[Musical Hexabuttons #{0}] Center Text: {1}", moduleId, buttonText[6].text);
		Debug.LogFormat("[Musical Hexabuttons #{0}] Button Read Order: {1} {2} {3} {4} {5} {6}", moduleId, positions[buttonOrder[0] - '0'], positions[buttonOrder[1] - '0'], positions[buttonOrder[2] - '0'], positions[buttonOrder[3] - '0'], positions[buttonOrder[4] - '0'], positions[buttonOrder[5] - '0']);
		Debug.LogFormat("[Musical Hexabuttons #{0}] Note Order: {1} {2} {3} {4} {5} {6}", moduleId, noteNames[noteOrder[0]], noteNames[noteOrder[1]], noteNames[noteOrder[2]], noteNames[noteOrder[3]], noteNames[noteOrder[4]], noteNames[noteOrder[5]]);
		Debug.LogFormat("[Musical Hexabuttons #{0}] Solution: {1} {2} {3} {4} {5} {6}", moduleId, positions[solution[0]], positions[solution[1]], positions[solution[2]], positions[solution[3]], positions[solution[4]], positions[solution[5]]);
	}
	void Start()
	{
		float scalar = transform.lossyScale.x;
		centerFlash.enabled = false;
		centerFlash.range *= scalar;
	}
	void pressedButton(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlaySoundAtTransform(notes[greenNotes[n]].name, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	void releasedButton(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	void submitButton(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlaySoundAtTransform(notes[greenNotes[n]].name, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			submission[numButtonPresses] = n;
			numButtonPresses++;
			hexButtons[n].OnInteract = null;
			ledMesh[n].material = ledColors[1];
			if (numButtonPresses == 6)
			{
				Debug.LogFormat("[Musical Hexabuttons #{0}] User Submission: {1} {2} {3} {4} {5} {6}", moduleId, positions[submission[0]], positions[submission[1]], positions[submission[2]], positions[submission[3]], positions[submission[4]], positions[submission[5]]);
				flag = false;
				bool correct = true;
				for (int aa = 0; aa < 6; aa++)
				{
					if (submission[aa] != solution[aa])
					{
						correct = false;
						break;
					}
				}
				if (correct)
				{
					moduleSolved = true;
					hexButtons[6].OnInteract = null;
					module.HandlePass();
				}
				else
				{
					module.HandleStrike();
					resetInput();
				}
			}
		}
	}
	void pressedCenter()
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
					hexButtons[i].OnInteract = delegate { submitButton(i); return false; };
					hexButtons[i].OnInteractEnded = null;
				}
				StartCoroutine(greenFlasher());
			}
		}
	}
	void releasedCenter()
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
			centerFlash.enabled = true;
			yield return new WaitForSeconds(1.0f);
			centerFlash.enabled = false;
			yield return new WaitForSeconds(1.0f);
		}
		centerFlash.enabled = false;
	}
	void resetInput()
	{
		foreach (int i in buttonIndex)
		{
			Vector3 pos = buttonMesh[i].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[i].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { releasedButton(i); };
			ledMesh[i].material = ledColors[0];
		}
	}
#pragma warning disable 414
	private readonly string TwitchHelpMessage = @"!{0} press|p tl/1 tr/2 ml/3 mr/4 bl/5 br/6 c/7 sl presses the top-left, top-right, middle-left, middle-right, bottom-left, bottom-right, and center buttons in that order.";
#pragma warning restore 414
	IEnumerator ProcessTwitchCommand(string command)
	{
		string[] param = command.ToUpper().Split(' ');
		if ((Regex.IsMatch(param[0], @"^\s*PRESS\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(param[0], @"^\s*P\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) && param.Length > 1)
		{
			if (isPos(param))
			{
				yield return null;
				for (int i = 1; i < param.Length; i++)
				{
					int cursor = -1;
					switch (param[i])
					{
						case "TL":
						case "1":
							cursor = 0;
							break;
						case "TR":
						case "2":
							cursor = 1;
							break;
						case "ML":
						case "3":
							cursor = 2;
							break;
						case "MR":
						case "4":
							cursor = 3;
							break;
						case "BL":
						case "5":
							cursor = 4;
							break;
						case "BR":
						case "6":
							cursor = 5;
							break;
						case "C":
						case "7":
							cursor = 6;
							break;
					}
					if (hexButtons[cursor].OnInteract != null)
					{
						hexButtons[cursor].OnInteract();
						yield return new WaitForSeconds(0.3f);
						if (hexButtons[cursor].OnInteractEnded != null)
						{
							hexButtons[cursor].OnInteractEnded();
							yield return new WaitForSeconds(0.3f);

						}
					}
				}
			}
			else
				yield return "sendtochat An error occured because the user inputted something wrong.";
		}
		else
			yield return "sendtochat An error occured because the user inputted something wrong.";
	}
	private bool isPos(string[] param)
	{
		for (int aa = 1; aa < param.Length; aa++)
		{
			switch (param[aa])
			{
				case "TL":
				case "TR":
				case "ML":
				case "MR":
				case "BL":
				case "BR":
				case "C":
				case "1":
				case "2":
				case "3":
				case "4":
				case "5":
				case "6":
				case "7":
					break;
				default:
					return false;
			}
		}
		return true;
	}
}
