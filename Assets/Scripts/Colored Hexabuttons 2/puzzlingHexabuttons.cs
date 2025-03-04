using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class puzzlingHexabuttons : MonoBehaviour
{
	public KMBombModule module;
	private int moduleId;
	private static int moduleIdCounter = 1;
	public KMAudio Audio;
	public KMSelectable[] hexButtons;
	public MeshRenderer[] buttonMesh;
	public TextMesh[] buttonText;
	public Material[] ledColors;
	public MeshRenderer[] ledMesh;
	private char[] submission;
	private char[] solution;
	private int numButtonPresses;
	private bool flag;
	private readonly int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private readonly string[] table = new string[6];
	private char current;
	void Awake()
	{
		moduleId = moduleIdCounter++;
		for (int i = 0; i < table.Length - 1; i++)
		{
			table[i] = new string("ABCDEF".ToCharArray().Shuffle());
			while (check(i, table[i]))
				table[i] = new string("ABCDEF".ToCharArray().Shuffle());
			Debug.LogFormat("[Puzzling Hexabuttons #{0}] {1}", moduleId, table[i]);
		}
		for(int i = 0; i < 6; i++)
		{
			string temp = "ABCDEF";
			for (int j = 0; j < 5; j++)
				temp = temp.Replace(table[j][i] + "", "");
			table[5] += temp;
		}
		Debug.LogFormat("[Puzzling Hexabuttons #{0}] {1}", moduleId, table[5]);

		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { releasedButton(i); };
		}
		hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
		hexButtons[6].OnInteractEnded = delegate { releasedCenter(); };
		buttonText[6].text = "ABCDEF"[UnityEngine.Random.Range(0, 6)] + "";
		submission = new char[6];
	}
	private bool check(int i, string str)
	{
		for(int cur = 0; cur < i; cur++)
		{
			for(int j = 0; j < 6; j++)
			{
				if (table[cur][j] == str[j])
					return true;
			}
		}
		return false;
	}
	void pressedButton(int n)
	{
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		Vector3 pos2 = buttonMesh[n].transform.localPosition;
		pos2.y = 0.0126f;
		buttonMesh[n].transform.localPosition = new Vector3(pos2.x, pos2.y, pos2.z);
		buttonText[6].text = table[n][buttonText[6].text[0] - 'A'] + "";
	}
	void releasedButton(int n)
	{
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos2 = buttonMesh[n].transform.localPosition;
		pos2.y = 0.0169f;
		buttonMesh[n].transform.localPosition = new Vector3(pos2.x, pos2.y, pos2.z);
	}
	void submitButton(int n)
	{
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		Vector3 pos = buttonMesh[n].transform.localPosition;
		pos.y = 0.0126f;
		buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		hexButtons[n].OnInteract = null;
		ledMesh[n].material = ledColors[1];
		current = submission[n] = table[n][current - 'A'];
		numButtonPresses++;
		if (numButtonPresses == 6)
		{
			Debug.LogFormat("[Puzzling Hexabuttons #{0}] User Submission: {1}", moduleId, new string(submission));
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
				hexButtons[6].OnInteract = null;
				hexButtons[6].OnInteractEnded = null;
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
		if (flag)
		{
			resetInput();
		}
		else
		{
			generatePuzzle();
		}
	}
	
	void releasedCenter()
	{
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
	}
	void generatePuzzle()
	{
		flag = true;
		numButtonPresses = 0;
		foreach (int i in buttonIndex)
		{
			Vector3 pos2 = buttonMesh[i].transform.localPosition;
			pos2.y = 0.0169f;
			buttonMesh[i].transform.localPosition = new Vector3(pos2.x, pos2.y, pos2.z);
			ledMesh[i].material = ledColors[0];
			hexButtons[i].OnInteract = delegate { submitButton(i); return false; };
			hexButtons[i].OnInteractEnded = null;
		}
		buttonText[6].text = "ABCDEF"[UnityEngine.Random.Range(0, 6)] + "";
		char temp = current = buttonText[6].text[0];
		string order = new string("012345".ToCharArray().Shuffle());
		solution = new char[6];
		foreach(char n in order)
		{
			temp = solution[n - '0'] = table[n - '0'][temp - 'A'];
			buttonText[n - '0'].text = temp + "";
		}
		Debug.LogFormat("[Puzzling Hexabuttons #{0}] Button Texts: {1}", moduleId, new string(solution));
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
			hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { releasedButton(i); };
			ledMesh[i].material = ledColors[0];
			buttonText[i].text = "";
		}
	}
#pragma warning disable 414
	private readonly string TwitchHelpMessage = @"!{0} press|p tl/1 tr/2 ml/3 mr/4 bl/5 br/6 c/7 presses the top-left, top-right, middle-left, middle-right, bottom-left, bottom-right, and center buttons in that order.";
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
						yield return new WaitForSeconds(0.2f);
						if (hexButtons[cursor].OnInteractEnded != null)
						{
							hexButtons[cursor].OnInteractEnded();
							yield return new WaitForSeconds(0.2f);
						}
					}
				}
			}
		}
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
