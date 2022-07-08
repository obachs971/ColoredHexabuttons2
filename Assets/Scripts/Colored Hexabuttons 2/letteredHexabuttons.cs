using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class letteredHexabuttons : MonoBehaviour
{
	public KMBombModule module;
	private static int moduleIdCounter = 1;
	public KMAudio Audio;
	private int moduleId;
	public KMSelectable[] hexButtons;
	public MeshRenderer[] buttonMesh;
	public TextMesh[] buttonText;
	public Material[] ledColors;
	public MeshRenderer[] ledMesh;
	private string[] voiceMessage;
	private int numButtonPresses;
	private bool moduleSolved;
	private int[] buttonCur;
	private int[] distances;
	private int prevCur = 0;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private int[][] maxChoice =
	{
		new int[]{ 12, 21, 23, 32 },
		new int[]{ 2, 11, 13, 20, 24, 31, 33, 42 },
		new int[]{ 1, 3, 10, 14, 30, 34, 41, 43 },
		new int[]{ 0, 4, 40, 44 }
	};
	private bool deafMode = false;
	void Awake()
	{
		moduleId = moduleIdCounter++;
		moduleSolved = false;
		numButtonPresses = 0;
		moduleSolved = false;
		//Generate 5 digit number
		string scramble = new string("12345".ToCharArray().Shuffle());
		string[] template = { "ABCDE", "FGHIJ", "KLMNO", "PQRST", "UVWXY" }, grid = { "", "", "", "", "" };
		voiceMessage = new string[5];
		for (int aa = 0; aa < 5; aa++)
		{
			voiceMessage[aa] = scramble[aa] + "";
			int col = scramble.IndexOf("12345"[aa]);
			for (int bb = 0; bb < 5; bb++)
				grid[bb] = grid[bb] + "" + template[scramble.IndexOf("12345"[bb])][col];
		}
		Debug.LogFormat("[Lettered Hexabuttons #{0}] Generated Number: {1}", moduleId, scramble);
		Debug.LogFormat("[Lettered Hexabuttons #{0}] {1}", moduleId, grid[0]);
		Debug.LogFormat("[Lettered Hexabuttons #{0}] {1}", moduleId, grid[1]);
		Debug.LogFormat("[Lettered Hexabuttons #{0}] {1}", moduleId, grid[2]);
		Debug.LogFormat("[Lettered Hexabuttons #{0}] {1}", moduleId, grid[3]);
		Debug.LogFormat("[Lettered Hexabuttons #{0}] {1}", moduleId, grid[4]);
		//Generate letters
		int max = UnityEngine.Random.Range(0, 4) + 5;
		string[] diffs;
		switch (max)
		{
			case 5:
				diffs = new string[] { "1111" };
				break;
			case 6:
				diffs = new string[] { "1111", "1112" };
				break;
			case 7:
				diffs = new string[] { "1111", "1112", "1122", "1113" };
				break;
			default:
				diffs = new string[] { "1111", "1112", "1122", "1222", "1113", "1123", "1114" };
				break;
		}
		string diff = new string(diffs[UnityEngine.Random.Range(0, diffs.Length)].ToCharArray().Shuffle());
		ArrayList choices = new ArrayList();
		for (int aa = max - 5; aa < 4; aa++)
		{
			for (int bb = 0; bb < maxChoice[aa].Length; bb++)
				choices.Add(maxChoice[aa][bb]);
		}
		int[] cursors = new int[6];
		cursors[0] = (int)choices[UnityEngine.Random.Range(0, choices.Count)];
		choices = findPossSpaces(cursors[0], max);
		cursors[1] = (int)choices[UnityEngine.Random.Range(0, choices.Count)];
		for (int aa = 2; aa < 6; aa++)
		{
			max = max - (diff[aa - 2] - '0');
			choices = findPossSpaces(cursors[aa - 1], max);
			cursors[aa] = (int)choices[UnityEngine.Random.Range(0, choices.Count)];
		}
		string pos = new string("012345".ToCharArray().Shuffle());
		buttonCur = new int[6];
		foreach (int i in buttonIndex)
		{
			buttonText[i].text = grid[cursors[pos[i] - '0'] / 10][cursors[pos[i] - '0'] % 10] + "";
			hexButtons[i].OnInteract = delegate { pressedButton(i, cursors[pos[i] - '0']); return false; };
			buttonCur[i] = cursors[pos[i] - '0'] + 0;
			Debug.LogFormat("[Lettered Hexabuttons #{0}] {1} Button's Letter: {2}", moduleId, positions[i], buttonText[i].text);
		}
		Debug.LogFormat("[Lettered Hexabuttons #{0}] Possible Solution: {1}{2}{3}{4}{5}{6}", moduleId, grid[cursors[5] / 10][cursors[5] % 10], grid[cursors[4] / 10][cursors[4] % 10], grid[cursors[3] / 10][cursors[3] % 10], grid[cursors[2] / 10][cursors[2] % 10], grid[cursors[1] / 10][cursors[1] % 10], grid[cursors[0] / 10][cursors[0] % 10]);
		hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
		hexButtons[7].OnInteract = delegate { deafMode = !(deafMode); return false; };
		distances = new int[5];
	}
	ArrayList findPossSpaces(int cur, int diff)
	{
		ArrayList poss = new ArrayList();
		for (int aa = 0; aa < 5; aa++)
		{
			for (int bb = 0; bb < 5; bb++)
			{
				int d = getDifference(cur / 10, aa) + getDifference(cur % 10, bb);
				if (d == diff)
					poss.Add((aa * 10) + bb);
			}
		}
		return poss;
	}
	int getDifference(int a, int b)
	{
		if (a > b)
			return (a - b);
		else
			return (b - a);
	}
	void pressedButton(int n, int cur)
	{
		Debug.LogFormat("[Lettered Hexabuttons #{0}] User pressed {1} which has a cursor of {2}, {3}", moduleId, positions[n], (cur / 10) + 1, (cur % 10) + 1);
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		Vector3 pos = buttonMesh[n].transform.localPosition;
		pos.y = 0.0126f;
		buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		hexButtons[n].OnInteract = null;
		ledMesh[n].material = ledColors[1];
		if (numButtonPresses >= 1)
			distances[numButtonPresses - 1] = getDifference(prevCur / 10, cur / 10) + getDifference(prevCur % 10, cur % 10);
		prevCur = cur + 0;
		numButtonPresses++;
		if (numButtonPresses == 6)
		{
			Debug.LogFormat("[Lettered Hexabuttons #{0}] Distances: {1} {2} {3} {4} {5}", moduleId, distances[0], distances[1], distances[2], distances[3], distances[4]);
			bool correct = true;
			for (int aa = 1; aa < distances.Length; aa++)
			{
				if (distances[aa] <= distances[aa - 1])
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
				buttonText[6].text = voiceMessage[aa] + "";
			yield return new WaitForSeconds(1.5f);
		}
		buttonText[6].text = "";
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
			hexButtons[i].OnInteract = delegate { pressedButton(i, buttonCur[i]); return false; };
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
