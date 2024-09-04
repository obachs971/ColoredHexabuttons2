using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class simpleHexabuttons : MonoBehaviour
{
	public KMBombModule module;
	private static int moduleIdCounter = 1;
	public KMAudio Audio;
	private int moduleId;
	public KMSelectable[] hexButtons;
	public MeshRenderer[] buttonMesh;
	public Material[] ledColors;
	public MeshRenderer[] ledMesh;
	public TextMesh centerText;
	private bool moduleSolved;
	private string[] voiceMessage;
	private int[] solution;
	private int[] submission;
	private int numButtonPresses;
	private readonly string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private readonly int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private readonly Vector3[] ledPos =
	{
		new Vector3(-1.131f, 0.43f, 0f),
		new Vector3(0f, 0.43f, 1.131f),
		new Vector3(1.131f, 0.43f, 0f),
		new Vector3(0f, 0.43f, -1.131f)
	};
	private readonly string[] table1 =
	{
		"LUDRUR",
		"ULRDDL",
		"DRLURU",
		"RDULLD"
	};
	private readonly int[][] table2 =
	{
		new int[]{ 2, 4, 1, 0, 3, 5 },
		new int[]{ 0, 2, 5, 3, 1, 4 },
		new int[]{ 4, 3, 0, 1, 5, 2 },
		new int[]{ 3, 0, 4, 5, 2, 1 },
		new int[]{ 5, 1, 3, 2, 4, 0 },
		new int[]{ 1, 5, 2, 4, 0, 3 }
	};
	private bool deafMode = false;
	void Awake()
	{
		moduleId = moduleIdCounter++;
		moduleSolved = false;
		voiceMessage = new string[2];
		voiceMessage[0] = "123456"[UnityEngine.Random.Range(0, 6)] + "";
		voiceMessage[1] = "ABCDEF"[UnityEngine.Random.Range(0, 6)] + "";
		hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
		hexButtons[7].OnInteract = delegate { deafMode = !(deafMode); return false; };
		Debug.LogFormat("[Simple Hexabuttons #{0}] Generated Voice Message {1}{2}", moduleId, voiceMessage[0], voiceMessage[1]);
		string dir = "";
		foreach (int i in buttonIndex)
		{
			int numRot = UnityEngine.Random.Range(0, 4);
			ledMesh[i].transform.localPosition = ledPos[numRot];
			dir = dir + "" + table1[numRot][i];
			hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
			Debug.LogFormat("[Simple Hexabuttons #{0}] {1} button's LED is pointing {2}", moduleId, positions[i], new string[] { "UP", "RIGHT", "DOWN", "LEFT" }[numRot]);
			Debug.LogFormat("[Simple Hexabuttons #{0}] {1} button's direction is {2}", moduleId, positions[i], new string[] { "UP", "RIGHT", "DOWN", "LEFT" }["URDL".IndexOf(dir[i])]);
		}
		numButtonPresses = 0;
		solution = new int[6];
		submission = new int[6];
		int[] cursor = { "123456".IndexOf(voiceMessage[0]), "ABCDEF".IndexOf(voiceMessage[1]) };
		solution[0] = table2[cursor[0]][cursor[1]];
		for (int aa = 1; aa < 6; aa++)
		{
			while (hasPressed(solution, table2[cursor[0]][cursor[1]], aa))
			{
				switch (dir[solution[aa - 1]])
				{
					case 'U':
						cursor[0] = mod(cursor[0] - 1, 6);
						break;
					case 'R':
						cursor[1] = mod(cursor[1] + 1, 6);
						break;
					case 'D':
						cursor[0] = mod(cursor[0] + 1, 6);
						break;
					case 'L':
						cursor[1] = mod(cursor[1] - 1, 6);
						break;
				}
			}
			solution[aa] = table2[cursor[0]][cursor[1]];
		}
		Debug.LogFormat("[Simple Hexabuttons #{0}] Solution: {1} {2} {3} {4} {5} {6}", moduleId, positions[solution[0]], positions[solution[1]], positions[solution[2]], positions[solution[3]], positions[solution[4]], positions[solution[5]]);

	}
	void pressedButton(int n)
	{
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		Vector3 pos = buttonMesh[n].transform.localPosition;
		pos.y = 0.0126f;
		buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		hexButtons[n].OnInteract = null;
		ledMesh[n].material = ledColors[1];
		submission[numButtonPresses] = n;
		numButtonPresses++;
		if (numButtonPresses == 6)
		{
			Debug.LogFormat("[Simple Hexabuttons #{0}] User Submission: {1} {2} {3} {4} {5} {6}", moduleId, positions[submission[0]], positions[submission[1]], positions[submission[2]], positions[submission[3]], positions[submission[4]], positions[submission[5]]);
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
				hexButtons[7].OnInteract = null;
				module.HandlePass();
			}
			else
			{
				module.HandleStrike();
				resetInput();
			}
		}
	}
	void pressedCenter()
	{
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0126f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		resetInput();
		StartCoroutine(playAudio());
	}
	bool hasPressed(int[] s, int p, int i)
	{
		for (int aa = 0; aa < i; aa++)
		{
			if (s[aa] == p)
				return true;
		}
		return false;
	}
	IEnumerator playAudio()
	{
		hexButtons[6].OnInteract = null;
		yield return new WaitForSeconds(0.5f);
		for (int aa = 0; aa < voiceMessage.Length; aa++)
		{
			Audio.PlaySoundAtTransform(voiceMessage[aa], transform);
			if (deafMode)
				centerText.text = voiceMessage[aa] + "";
			yield return new WaitForSeconds(1.5f);
		}
		centerText.text = "";
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		if (!(moduleSolved))
			hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
	}
	void resetInput()
	{
		numButtonPresses = 0;
		for (int aa = 0; aa < 6; aa++)
		{
			Vector3 pos2 = buttonMesh[aa].transform.localPosition;
			pos2.y = 0.0169f;
			buttonMesh[aa].transform.localPosition = new Vector3(pos2.x, pos2.y, pos2.z);
			ledMesh[aa].material = ledColors[0];
		}
		foreach (int i in buttonIndex)
			hexButtons[i].OnInteract = delegate { pressedButton(buttonIndex[i]); return false; };
	}
	int mod(int n, int m)
	{
		while (n < 0)
			n += m;
		return (n % m);
	}
#pragma warning disable 414
	private readonly string TwitchHelpMessage = @"!{0} press|p tl/1 tr/2 ml/3 mr/4 bl/5 br/6 c/7 sl presses the top-left, top-right, middle-left, middle-right, bottom-left, bottom-right, center, and the status light in that order.";
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
                    int cursor;
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
						default:
							cursor = 7;
							break;
					}
					if (hexButtons[cursor].OnInteract != null)
					{
						hexButtons[cursor].OnInteract();
						yield return new WaitForSeconds(0.2f);
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
				case "SL":
					break;
				default:
					return false;
			}
		}
		return true;
	}
}
