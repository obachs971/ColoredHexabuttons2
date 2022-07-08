using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class magicalHexabuttons : MonoBehaviour
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
	public TextMesh text;
	private string[] voiceMessage;

	private bool moduleSolved;
	private int numButtonPresses;
	private string[] chemicals;
	private string[] potions;
	private string[] submission;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private string[] potionTable =
		{
			"R-", "B-", "M-", "K-",
			"R0", "B0", "M0", "K0",
			"R+", "B+", "M+", "K+",
			"G-", "C-", "Y-", "W-",
			"G0", "C0", "Y0", "W0",
			"G+", "C+", "Y+", "W+"
		};
	private bool deafMode = false;
	void Awake()
	{
		moduleId = moduleIdCounter++;
		moduleSolved = false;
		potions = new string[6];
		submission = new string[7];
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWX";
		buttonText[6].text = alpha[UnityEngine.Random.Range(0, 24)] + "";
		submission[0] = potionTable[alpha.IndexOf(buttonText[6].text)].ToUpperInvariant();
		string curr = submission[0].ToUpperInvariant();
		string[] sizes = { "BBB", "BBS", "BSB", "BSS", "SBB", "SBS", "SSB", "SSS" };
		string[] charges = { "---", "--+", "-+-", "-++", "+--", "+-+", "++-", "+++" };
		chemicals = new string[6];
		int[] possSolution = new int[6];
		string possSize = new string("01234567".ToCharArray().Shuffle()), possCharge = new string("01234567".ToCharArray().Shuffle()), possIndex = new string("012345".ToCharArray().Shuffle());
		for (int aa = 0; aa < 6; aa++)
		{
			chemicals[possIndex[aa] - '0'] = sizes[possSize[aa] - '0'][0] + "" + charges[possCharge[aa] - '0'][0] + "" + sizes[possSize[aa] - '0'][1] + "" + charges[possCharge[aa] - '0'][1] + "" + sizes[possSize[aa] - '0'][2] + "" + charges[possCharge[aa] - '0'][2];
			curr = getResult(curr, chemicals[possIndex[aa] - '0']);
			potions[aa] = curr.ToUpperInvariant();
			possSolution[aa] = possIndex[aa] - '0';
		}
		string[][] chemicalTable =
		{
			new string[]{ "S+S+S+", "S-S+B+", "S+B-S+", "B+S+S-", "B+B-S-", "B-S+B-", "S-B-B+", "B-B-B-" },
			new string[]{ "S-B+S+", "B+S-S+", "B+B+S-", "B+S-B-", "S-B+B-", "B-B-B+", "S-S-S-", "S+S+B+" },
			new string[]{ "B+B-S+", "B+S+B-", "S+B-B-", "B-B+B-", "S-S-S+", "S-S-B-", "S+B+S+", "B-S+S+" },
			new string[]{ "S+B+B-", "B+B-B-", "S-S+S-", "S-S-B+", "S-B-S-", "B+S+S+", "B-B+S+", "B+S-B+" },
			new string[]{ "S+S-B-", "S-B+S-", "B-S-S+", "B-B-S-", "B+S+B+", "S-B+B+", "B+B-B+", "S+S+S-" },
			new string[]{ "B-S+S-", "B-B-S+", "B-S-B-", "S+B+B+", "B-B+B+", "S+S-S+", "S+S+B-", "S+B-S-" },
			new string[]{ "B-S-B+", "S-B-B-", "B+B+B+", "S-S+S+", "S+S-B+", "S+B+S-", "B+S-S-", "B-B+S-" },
			new string[]{ "B+B+B-", "S+S-S-", "S-S+B-", "S-B-S+", "B-S-S-", "B+B+S+", "B-S+B+", "S+B-B+" }
		};
		voiceMessage = new string[6];
		string indexes = new string("012345".ToCharArray().Shuffle());
		for (int aa = 0; aa < 6; aa++)
		{
			for (int bb = 0; bb < potionTable.Length; bb++)
			{
				if (potionTable[bb].Equals(potions[indexes[aa] - '0']))
				{
					voiceMessage[aa] = alpha[bb] + "";
					break;
				}
			}
		}
		Debug.LogFormat("[Magical Hexabuttons #{0}] Center Text: {1}", moduleId, buttonText[6].text);
		Debug.LogFormat("[Magical Hexabuttons #{0}] Voice Message: {1}{2}{3}{4}{5}{6}", moduleId, voiceMessage[0], voiceMessage[1], voiceMessage[2], voiceMessage[3], voiceMessage[4], voiceMessage[5]);
		foreach (int i in buttonIndex)
		{
			buttonText[i].text = "";
			for (int aa = 0; aa < chemicalTable.Length; aa++)
			{
				for (int bb = 0; bb < chemicalTable[aa].Length; bb++)
				{
					if (chemicalTable[aa][bb].Equals(chemicals[i]))
					{
						buttonText[i].text = (aa + 1) + "" + (bb + 1);
						break;
					}
				}
				if (buttonText[i].text.Length > 0)
					break;
			}
			hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
			Debug.LogFormat("[Magical Hexabuttons #{0}] {1} Text: {2}", moduleId, positions[i], buttonText[i].text);
			Debug.LogFormat("[Magical Hexabuttons #{0}] {1} Chemical: {2}", moduleId, positions[i], chemicals[i]);
		}
		Debug.LogFormat("[Magical Hexabuttons #{0}] Possible Solution: {1} {2} {3} {4} {5} {6}", moduleId, positions[possSolution[0]], positions[possSolution[1]], positions[possSolution[2]], positions[possSolution[3]], positions[possSolution[4]], positions[possSolution[5]]);
		hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
		hexButtons[7].OnInteract = delegate { deafMode = !(deafMode); return false; };
		numButtonPresses = 0;
	}
	void pressedButton(int n)
	{
		Debug.LogFormat("[Magical Hexabuttons #{0}] User pressed {1} which is {2}", moduleId, positions[n], chemicals[n]);
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		Vector3 pos = buttonMesh[n].transform.localPosition;
		pos.y = 0.0126f;
		buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		ledMesh[n].material = ledColors[1];
		hexButtons[n].OnInteract = null;
		numButtonPresses++;
		submission[numButtonPresses] = getResult(submission[numButtonPresses - 1], chemicals[n]);
		Debug.LogFormat("[Magical Hexabuttons #{0}] Potion is now {1}", moduleId, submission[numButtonPresses]);
		if (numButtonPresses == 6)
		{
			bool[] flag = { false, false, false, false, false, false };
			for (int aa = 0; aa < 6; aa++)
			{
				for (int bb = 1; bb < 7; bb++)
				{
					if (potions[aa].Equals(submission[bb]) && !(flag[aa]))
					{
						flag[aa] = true;
						break;
					}
				}
			}
			if (flag[0] && flag[1] && flag[2] && flag[3] && flag[4] && flag[5])
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
	string getResult(string p, string c)
	{
		int num = "KBGCRMYW".IndexOf(p[0]);
		string[] RGB = { "", "" };
		for (int aa = 0; aa < 3; aa++)
		{
			RGB[0] = (num % 2) + "" + RGB[0];
			num = num / 2;
			if (c[aa * 2] == 'B')
				RGB[1] = RGB[1] + "1";
			else
				RGB[1] = RGB[1] + "0";
		}
		string result = "";
		for (int aa = 0; aa < 3; aa++)
		{
			if (RGB[0][aa] == RGB[1][aa])
				result = result + "0";
			else
				result = result + "1";
		}
		string charges = p[1] + "" + c[1] + "" + c[3] + "" + c[5];
		int charge = 0;
		for (int aa = 0; aa < 4; aa++)
		{
			if (charges[aa] == '+')
				charge++;
			else if (charges[aa] == '-')
				charge--;
		}
		switch (result)
		{
			case "100":
				result = "R";
				break;
			case "010":
				result = "G";
				break;
			case "001":
				result = "B";
				break;
			case "011":
				result = "C";
				break;
			case "101":
				result = "M";
				break;
			case "110":
				result = "Y";
				break;
			case "111":
				result = "W";
				break;
			case "000":
				result = "K";
				break;
		}
		if (charge > 0)
			return (result + "+");
		else if (charge < 0)
			return (result + "-");
		else
			return (result + "0");
	}
	void pressedCenter()
	{
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0126f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		StartCoroutine(playAudio());
		resetInput();
	}
	IEnumerator playAudio()
	{
		hexButtons[6].OnInteract = null;
		yield return new WaitForSeconds(0.5f);
		for (int aa = 0; aa < voiceMessage.Length; aa++)
		{
			Audio.PlaySoundAtTransform(voiceMessage[aa], transform);
			if (deafMode)
				text.text = voiceMessage[aa];
			yield return new WaitForSeconds(1.3f);
			text.text = "";
			yield return new WaitForSeconds(0.2f);
		}
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		if(!(moduleSolved))
			hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
	}
	void resetInput()
	{
		Debug.LogFormat("[Magical Hexabuttons #{0}] Resetting Potion", moduleId);
		numButtonPresses = 0;
		foreach (int i in buttonIndex)
		{
			Vector3 pos = buttonMesh[i].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[i].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			ledMesh[i].material = ledColors[0];
			hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
		}
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
						if (hexButtons[cursor].OnInteractEnded != null)
						{
							hexButtons[cursor].OnInteractEnded();
							yield return new WaitForSeconds(0.2f);

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
				case "SL":
					break;
				default:
					return false;
			}
		}
		return true;
	}
}
