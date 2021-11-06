using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHexabuttons{
	private ColorfulButtonSeries coloredHexabuttons;
	private KMAudio Audio;
	private AudioClip[] pitches;
	private int moduleId;
	private KMSelectable[] hexButtons;
	private MeshRenderer[] buttonMesh;
	private Material resetColor;
	private TextMesh[] buttonText;
	private Material[] ledColors;
	private MeshRenderer[] ledMesh;
	private Material[] brightness;
	private Light[] lights;
	private Transform transform;
	
	private string[] blackLights;
	private bool flag;
	private int[][] pitchIndex;
	private int[] brightIndex;
	private string solution;
	private bool moduleSolved;
	private string[] input;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private string[] flagSubmissions;
	private string submission;
	private int flagToggle;
	private string mode;
	private int type;
	private string[][] flagChart =
	{
		new string[]{ "N", "NE", "E", "SE" },
		new string[]{ "S", "SW", "W", "NW" }
	};
	public BlackHexabuttons(ColorfulButtonSeries m, KMAudio aud, AudioClip[] P, int MI, KMSelectable[] HB, MeshRenderer[] BM, TextMesh[] BT, Material[] LC, MeshRenderer[] LM, Material[] B, Light[] L, Transform T, Material RC)
	{
		coloredHexabuttons = m;
		Audio = aud;
		pitches = P;
		moduleId = MI;
		hexButtons = HB;
		buttonMesh = BM;
		buttonText = BT;
		ledColors = LC;
		ledMesh = LM;
		brightness = B;
		lights = L;
		transform = T;
		resetColor = RC;
	}
	public void run()
	{
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color Generated: Black", moduleId);
		int[][] priorityTable =
		{
			new int[]{35,33,25,21,7,1},
			new int[]{34,26,22,8,2,12},
			new int[]{27,23,9,3,13,17},
			new int[]{24,10,4,14,18,28},
			new int[]{11,5,15,19,29,31},
			new int[]{6,16,20,30,32,36}
		};
		string[][][] pitchTable =
		{
			new string[][]
			{
				new string[]{ "01", "10", "11", "00" },
				new string[]{ "10", "11", "00", "01" },
				new string[]{ "11", "00", "01", "10" }
			},
			new string[][]
			{
				new string[]{ "11", "00", "01", "10" },
				new string[]{ "01", "10", "11", "00" },
				new string[]{ "10", "11", "00", "01" }
			},
			new string[][]
			{
				new string[]{ "10", "11", "00", "01" },
				new string[]{ "11", "00", "01", "10" },
				new string[]{ "01", "10", "11", "00" }
			}
		};
		string[] braille =
		{
			"100000","110000","100100","100110","100010","110100",
			"110110","110010","010100","010110","101000","111000",
			"101100","101110","101010","111100","111110","111010",
			"011100","011110","101001","111001","010111","101101",
			"101111","101011","010000","011000","010010","010011",
			"010001","011010","011011","011001","001010","001011"
		};
		int[] priorityLevel = new int[6];
		brightIndex = new int[6];
		pitchIndex = new int[6][];
		string code = "";
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
		string brightChoice = "012345";
		blackLights = new string[6];
		foreach(int i in buttonIndex)
		{
			brightIndex[i] = brightChoice[UnityEngine.Random.Range(0, brightChoice.Length)] - '0';
			brightChoice = brightChoice.Replace(brightIndex[i] + "", "");
			priorityLevel[i] = priorityTable[i][brightIndex[i]];
			pitchIndex[i] = new int[3];
			string choices = "012";
			for(int aa = 0; aa < 3; aa++)
			{
				int pitch = choices[UnityEngine.Random.Range(0, choices.Length)] - '0';
				pitchIndex[i][aa] = pitch;
				choices = choices.Replace(pitch + "", "");
			}
			code = code + "" + alpha[UnityEngine.Random.Range(0, alpha.Length)];
			string brailleBin = braille[alpha.IndexOf(code[i])];
			string temp = "";
			string[] col = { "00", "01", "10", "11" };
			for(int aa = 0; aa < 3; aa++)
				temp = temp + col[Array.IndexOf(pitchTable[pitchIndex[i][aa]][aa], brailleBin.Substring(aa * 2, 2))];
			blackLights[i] = "";
			for (int aa = 0; aa < temp.Length; aa++)
			{
				if (temp[aa] == '1')
					blackLights[i] = blackLights[i] + "" + (aa + 1);
			}
			hexButtons[i].OnInteract = delegate { pressedBlack(i); return false; };
			hexButtons[i].OnInteractEnded = null;
			hexButtons[i].OnHighlight = delegate 
			{
				foreach (char bl in blackLights[i])
					ledMesh["142536".IndexOf(bl)].material = ledColors[2];
			};
			hexButtons[i].OnHighlightEnded = delegate
			{
				foreach (char bl in blackLights[i])
					ledMesh["142536".IndexOf(bl)].material = ledColors[0];
			};
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} Brightness: {2}", moduleId, positions[i], brightIndex[i]);
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} Priority Level: {2}", moduleId, positions[i], priorityLevel[i]);
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} Character: {2}", moduleId, positions[i], code[i]);
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} Pitches: {2}{3}{4}", moduleId, positions[i], "LMH"[pitchIndex[i][0]], "LMH"[pitchIndex[i][1]], "LMH"[pitchIndex[i][2]]);
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} Braille: {2}", moduleId, positions[i], blackLights[i]);
		}
		hexButtons[6].OnHighlight = delegate {
			buttonMesh[0].material = brightness[brightIndex[0]];
			buttonMesh[1].material = brightness[brightIndex[1]];
			buttonMesh[2].material = brightness[brightIndex[2]];
			buttonMesh[3].material = brightness[brightIndex[3]];
			buttonMesh[4].material = brightness[brightIndex[4]];
			buttonMesh[5].material = brightness[brightIndex[5]];
			buttonMesh[6].material = brightness[0];
		};
		hexButtons[6].OnHighlightEnded = delegate {
			buttonMesh[0].material = resetColor;
			buttonMesh[1].material = resetColor;
			buttonMesh[2].material = resetColor;
			buttonMesh[3].material = resetColor;
			buttonMesh[4].material = resetColor;
			buttonMesh[5].material = resetColor;
			buttonMesh[6].material = resetColor;
		};
		hexButtons[6].OnInteract = delegate { pressedBlackCenter(); return false; };
		hexButtons[6].OnInteractEnded = delegate { releasedBlackCenter(); };
		solution = "";
		input = new string[6];
		for(int aa = 36; aa >= 0; aa--)
		{
			if (Array.IndexOf(priorityLevel, aa) >= 0)
			{
				int cur = Array.IndexOf(priorityLevel, aa);
				input[cur] = new string[]{"A", "B", "C", "D", "1", "2"}[solution.Length];
				solution = solution + "" + code[cur];
				Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} Type: {2}", moduleId, positions[cur], input[cur]);
			}
		}
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Solution: {1}", moduleId, solution);
		flagSubmissions = new string[2];
		submission = "";
		flagToggle = 0;
		mode = "NULL";
		type = 0;
		lights[6].color = Color.white;
}
	void pressedBlack(int p)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[p].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[p].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			hexButtons[p].OnInteract = null;
			coloredHexabuttons.StartCoroutine(PlayAudio(p));
		}
	}
	void pressedBlackSubmit(int p)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[p].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[p].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	void releasedBlackSubmit(int p)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
			Vector3 pos = buttonMesh[p].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[p].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} was pressed which is {2}", moduleId, positions[p], input[p]);
			if ("12".IndexOf(input[p]) >= 0)
				type = "12".IndexOf(input[p]);
			else
			{
				flagSubmissions[flagToggle] = flagChart[type]["ABCD".IndexOf(input[p])];
				flagToggle++;
				if (flagToggle == 2)
				{
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Entered flag sequence: {1} {2}", moduleId, flagSubmissions[0], flagSubmissions[1]);
					flagToggle = 0;
					switch (mode)
					{
						case "NULL":
							switch (flagSubmissions[0] + " " + flagSubmissions[1])
							{
								case "N NE":
									mode = "NUMBER";
									break;
								case "N E":
									mode = "LETTER";
									break;
								default:
									submission = submission + "?";
									break;
							}
							break;
						case "NUMBER":
							switch (flagSubmissions[0] + " " + flagSubmissions[1])
							{
								case "SW S":
									submission = submission + "1";
									break;
								case "W S":
									submission = submission + "2";
									break;
								case "NW S":
									submission = submission + "3";
									break;
								case "N S":
									submission = submission + "4";
									break;
								case "S NE":
									submission = submission + "5";
									break;
								case "S E":
									submission = submission + "6";
									break;
								case "S SE":
									submission = submission + "7";
									break;
								case "W SW":
									submission = submission + "8";
									break;
								case "SW NW":
									submission = submission + "9";
									break;
								case "SW N":
									submission = submission + "0";
									break;
								case "N E":
									mode = "LETTER";
									break;
								default:
									submission = submission + "?";
									break;
							}
							break;
						default:
							switch (flagSubmissions[0] + " " + flagSubmissions[1])
							{
								case "SW S":
									submission = submission + "A";
									break;
								case "W S":
									submission = submission + "B";
									break;
								case "NW S":
									submission = submission + "C";
									break;
								case "N S":
									submission = submission + "D";
									break;
								case "S NE":
									submission = submission + "E";
									break;
								case "S E":
									submission = submission + "F";
									break;
								case "S SE":
									submission = submission + "G";
									break;
								case "W SW":
									submission = submission + "H";
									break;
								case "SW NW":
									submission = submission + "I";
									break;
								case "N E":
									submission = submission + "J";
									break;
								case "SW N":
									submission = submission + "K";
									break;
								case "SW NE":
									submission = submission + "L";
									break;
								case "SW E":
									submission = submission + "M";
									break;
								case "SW SE":
									submission = submission + "N";
									break;
								case "W NW":
									submission = submission + "O";
									break;
								case "W N":
									submission = submission + "P";
									break;
								case "W NE":
									submission = submission + "Q";
									break;
								case "W E":
									submission = submission + "R";
									break;
								case "W SE":
									submission = submission + "S";
									break;
								case "NW N":
									submission = submission + "T";
									break;
								case "NW NE":
									submission = submission + "U";
									break;
								case "N SE":
									submission = submission + "V";
									break;
								case "NE E":
									submission = submission + "W";
									break;
								case "NE SE":
									submission = submission + "X";
									break;
								case "NW W":
									submission = submission + "Y";
									break;
								case "SE E":
									submission = submission + "Z";
									break;
								case "N NE":
									mode = "NUMBER";
									break;
								default:
									submission = submission + "?";
									break;
							}
							break;
					}
					if (submission.Length > 0)
						ledMesh[submission.Length - 1].material = ledColors[1];
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Entered so far: {1}", moduleId, submission);
					if (submission.Length == 6)
					{
						Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User Submission: {1}", moduleId, submission);
						flag = false;
						if (solution.Equals(submission))
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
		}
	}
	void pressedBlackCenter()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	void releasedBlackCenter()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			if (flag)
			{
				resetInput();
			}
			else
			{
				hexButtons[6].OnHighlight = null;
				hexButtons[6].OnHighlightEnded = null;
				coloredHexabuttons.StartCoroutine(blackFlasher());
				foreach (int i in buttonIndex)
				{
					hexButtons[i].OnInteract = delegate { pressedBlackSubmit(i); return false; };
					hexButtons[i].OnInteractEnded = delegate { releasedBlackSubmit(i); };
					hexButtons[i].OnHighlight = null;
					hexButtons[i].OnHighlightEnded = null;
					buttonMesh[i].material = resetColor;
					ledMesh[i].material = ledColors[0];
				}
				buttonMesh[6].material = resetColor;
			}
			flag = !(flag);
		}
	}
	IEnumerator blackFlasher()
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
	IEnumerator PlayAudio(int p)
	{
		yield return new WaitForSeconds(0.5f);
		foreach(int pi in pitchIndex[p])
		{
			Audio.PlaySoundAtTransform(pitches[pi].name, transform);
			yield return new WaitForSeconds(0.45f);
		}
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos = buttonMesh[p].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[p].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		if(!(flag))
			hexButtons[p].OnInteract = delegate { pressedBlack(p); return false; };
	}
	void resetInput()
	{
		hexButtons[6].OnHighlight = delegate {
			buttonMesh[0].material = brightness[brightIndex[0]];
			buttonMesh[1].material = brightness[brightIndex[1]];
			buttonMesh[2].material = brightness[brightIndex[2]];
			buttonMesh[3].material = brightness[brightIndex[3]];
			buttonMesh[4].material = brightness[brightIndex[4]];
			buttonMesh[5].material = brightness[brightIndex[5]];
			buttonMesh[6].material = brightness[0];
		};
		hexButtons[6].OnHighlightEnded = delegate {
			buttonMesh[0].material = resetColor;
			buttonMesh[1].material = resetColor;
			buttonMesh[2].material = resetColor;
			buttonMesh[3].material = resetColor;
			buttonMesh[4].material = resetColor;
			buttonMesh[5].material = resetColor;
			buttonMesh[6].material = resetColor;
		};
		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedBlack(i); return false; };
			hexButtons[i].OnInteractEnded = null;
			hexButtons[i].OnHighlight = delegate
			{
				foreach (char bl in blackLights[i])
					ledMesh["142536".IndexOf(bl)].material = ledColors[2];
			};
			hexButtons[i].OnHighlightEnded = delegate
			{
				foreach (char bl in blackLights[i])
					ledMesh["142536".IndexOf(bl)].material = ledColors[0];
			};
			ledMesh[i].material = ledColors[0];
		}
		flagSubmissions = new string[2];
		submission = "";
		flagToggle = 0;
		mode = "NULL";
		type = 0;
	}
}
