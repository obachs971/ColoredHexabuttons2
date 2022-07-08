using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class colorfulHexabuttons : MonoBehaviour
{
	public KMBombModule module;
	private static int moduleIdCounter = 1;
	public KMAudio Audio;
	private int moduleId;
	public KMSelectable[] hexButtons;
	public MeshRenderer[] buttonMesh;
	public TextMesh[] buttonText;
	public Material[] buttonColors;
	private string solution;
	private string colors;
	private string centerColor;
	private string colorState;
	private string colorFlash;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private bool colorMode = false;
	void Awake()
	{
		moduleId = moduleIdCounter++;
		colors = new string("ROYGBP".ToCharArray().Shuffle());
		for (int aa = 0; aa < 6; aa++)
			buttonText[aa].color = "OY".Contains(colors[aa] + "") ? Color.black : Color.white;
		centerColor = "ROYGBP"[UnityEngine.Random.Range(0, 6)] + "";
		buttonText[6].color = "OY".Contains(centerColor) ? Color.black : Color.white;
		colorState = colors.Replace(centerColor, "-");
		generateSolution();
		colorFlash = solution.Replace("-", centerColor);
		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedWhite(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { pressedWhiteRelease(i); };
		}
		hexButtons[6].OnHighlight = delegate { buttonMesh[6].material = buttonColors["ROYGBP".IndexOf(centerColor)]; if (colorMode) buttonText[6].text = centerColor; };
		hexButtons[6].OnHighlightEnded = delegate { buttonMesh[6].material = buttonColors[6]; buttonText[6].text = ""; };
		hexButtons[6].OnInteract = delegate { pressedWhiteCenter(); return false; };
		hexButtons[7].OnInteract = delegate { colorMode = !(colorMode); return false; };
		Debug.LogFormat("[Colorful Hexabuttons #{0}] Absent Color: {1}", moduleId, centerColor);
		Debug.LogFormat("[Colorful Hexabuttons #{0}] Beginning State: {1}", moduleId, colorState);
		Debug.LogFormat("[Colorful Hexabuttons #{0}] Finished State: {1}", moduleId, solution);
	}
	void generateSolution()
	{
		ArrayList cursors = new ArrayList();
		ArrayList prev = new ArrayList();
		cursors.Add(colorState.ToUpperInvariant());
		prev.Add(colorState.ToUpperInvariant());
		for (int aa = 0; aa < 6; aa++)
		{
			for (int bb = 0; bb < cursors.Count; bb++)
			{
				ArrayList poss = getAllMoves((string)cursors[bb]);
				for (int cc = 0; cc < poss.Count; cc++)
				{
					if (prev.Contains((string)poss[cc]))
					{
						poss.RemoveAt(cc);
						cc--;
					}
				}
				if (poss.Count > 0)
				{
					cursors[bb] = poss[0];
					prev.Add(poss[0]);
					for (int cc = 1; cc < poss.Count; cc++)
					{
						bb++;
						cursors.Insert(bb, poss[cc]);
						prev.Add(poss[cc]);
					}
				}
				else
				{
					cursors.RemoveAt(bb);
					bb--;
				}
			}
		}
		solution = (string)cursors[UnityEngine.Random.Range(0, cursors.Count)];
	}
	ArrayList getAllMoves(string cur)
	{
		ArrayList moves = new ArrayList();
		switch (cur.IndexOf("-"))
		{
			case 0:
				moves.Add(cur[1] + "" + cur[0] + "" + cur.Substring(2));
				moves.Add(cur[2] + "" + cur[1] + "" + cur[0] + "" + cur.Substring(3));
				break;
			case 1:
				moves.Add(cur[1] + "" + cur[0] + "" + cur.Substring(2));
				moves.Add(cur[0] + "" + cur[3] + "" + cur[2] + "" + cur[1] + "" + cur.Substring(4));
				break;
			case 2:
				moves.Add(cur[2] + "" + cur[1] + "" + cur[0] + "" + cur.Substring(3));
				moves.Add(cur.Substring(0, 2) + "" + cur[3] + "" + cur[2] + "" + cur.Substring(4));
				moves.Add(cur.Substring(0, 2) + "" + cur[4] + "" + cur[3] + "" + cur[2] + "" + cur[5]);
				break;
			case 3:
				moves.Add(cur[0] + "" + cur[3] + "" + cur[2] + "" + cur[1] + "" + cur.Substring(4));
				moves.Add(cur.Substring(0, 2) + "" + cur[3] + "" + cur[2] + "" + cur.Substring(4));
				moves.Add(cur.Substring(0, 3) + "" + cur[5] + "" + cur[4] + "" + cur[3]);
				break;
			case 4:
				moves.Add(cur.Substring(0, 2) + "" + cur[4] + "" + cur[3] + "" + cur[2] + "" + cur[5]);
				moves.Add(cur.Substring(0, 4) + "" + cur[5] + "" + cur[4]);
				break;
			case 5:
				moves.Add(cur.Substring(0, 3) + "" + cur[5] + "" + cur[4] + "" + cur[3]);
				moves.Add(cur.Substring(0, 4) + "" + cur[5] + "" + cur[4]);
				break;
		}
		return moves;
	}
	void pressedWhite(int n)
	{
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		Vector3 pos = buttonMesh[n].transform.localPosition;
		pos.y = 0.0126f;
		buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
	}
	void pressedWhiteRelease(int n)
	{
		Debug.LogFormat("[Colorful Hexabuttons #{0}] User pressed {1} which is the color {2}", moduleId, positions[n], colors[n]);
		Vector3 pos = buttonMesh[n].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		int blank = colorState.IndexOf("-");
		switch (colorState.IndexOf(colors[n]))
		{
			case 0:
				if (blank == 1)
					colorState = colorState[1] + "" + colorState[0] + "" + colorState.Substring(2);
				else if (blank == 2)
					colorState = colorState[2] + "" + colorState[1] + "" + colorState[0] + "" + colorState.Substring(3);
				Debug.LogFormat("[Colorful Hexabuttons #{0}] Color state is now {1}", moduleId, colorState);
				break;
			case 1:
				if (blank == 0)
					colorState = colorState[1] + "" + colorState[0] + "" + colorState.Substring(2);
				else if (blank == 3)
					colorState = colorState[0] + "" + colorState[3] + "" + colorState[2] + "" + colorState[1] + "" + colorState.Substring(4);
				Debug.LogFormat("[Colorful Hexabuttons #{0}] Color state is now {1}", moduleId, colorState);
				break;
			case 2:
				if (blank == 0)
					colorState = colorState[2] + "" + colorState[1] + "" + colorState[0] + "" + colorState.Substring(3);
				else if (blank == 3)
					colorState = colorState.Substring(0, 2) + "" + colorState[3] + "" + colorState[2] + "" + colorState.Substring(4);
				else if (blank == 4)
					colorState = colorState.Substring(0, 2) + "" + colorState[4] + "" + colorState[3] + "" + colorState[2] + "" + colorState[5];
				Debug.LogFormat("[Colorful Hexabuttons #{0}] Color state is now {1}", moduleId, colorState);
				break;
			case 3:
				if (blank == 1)
					colorState = colorState[0] + "" + colorState[3] + "" + colorState[2] + "" + colorState[1] + "" + colorState.Substring(4);
				else if (blank == 2)
					colorState = colorState.Substring(0, 2) + "" + colorState[3] + "" + colorState[2] + "" + colorState.Substring(4);
				else if (blank == 5)
					colorState = colorState.Substring(0, 3) + "" + colorState[5] + "" + colorState[4] + "" + colorState[3];
				Debug.LogFormat("[Colorful Hexabuttons #{0}] Color state is now {1}", moduleId, colorState);
				break;
			case 4:
				if (blank == 2)
					colorState = colorState.Substring(0, 2) + "" + colorState[4] + "" + colorState[3] + "" + colorState[2] + "" + colorState[5];
				else if (blank == 5)
					colorState = colorState.Substring(0, 4) + "" + colorState[5] + "" + colorState[4];
				Debug.LogFormat("[Colorful Hexabuttons #{0}] Color state is now {1}", moduleId, colorState);
				break;
			case 5:
				if (blank == 3)
					colorState = colorState.Substring(0, 3) + "" + colorState[5] + "" + colorState[4] + "" + colorState[3];
				else if (blank == 4)
					colorState = colorState.Substring(0, 4) + "" + colorState[5] + "" + colorState[4];
				Debug.LogFormat("[Colorful Hexabuttons #{0}] Color state is now {1}", moduleId, colorState);
				break;
			default:
				Debug.LogFormat("[Colorful Hexabuttons #{0}] User submitted {1}", moduleId, colorState);
				if (solution.Equals(colorState))
				{
					foreach (int i in buttonIndex)
					{
						hexButtons[i].OnInteract = null;
						hexButtons[i].OnInteractEnded = null;
					}
					hexButtons[6].OnInteract = null;
					hexButtons[6].OnHighlight = null;
					hexButtons[6].OnHighlightEnded = null;
					hexButtons[7].OnInteract = null;
					module.HandlePass();
				}
				else
				{
					colorState = colors.Replace(centerColor, "-");
					module.HandleStrike();
				}
				break;
		}
	}
	void pressedWhiteCenter()
	{
		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = null;
			hexButtons[i].OnInteractEnded = null;
		}
		colorState = colors.Replace(centerColor, "-");
		hexButtons[6].OnHighlight = null;
		hexButtons[6].OnHighlightEnded = null;
		buttonMesh[6].material = buttonColors["ROYGBP".IndexOf(centerColor)];
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0126f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		hexButtons[6].OnInteract = null;
		StartCoroutine(whiteFlasher());
	}
	IEnumerator whiteFlasher()
	{
		yield return new WaitForSeconds(1.0f);
		for (int aa = 0; aa < 6; aa++)
		{
			int index = colors.IndexOf(colorFlash[aa]);
			buttonMesh[index].material = buttonColors["ROYGBP".IndexOf(colors[index])];
			if (colorMode)
				buttonText[index].text = colors[index] + "";
			yield return new WaitForSeconds(0.8f);
			buttonMesh[index].material = buttonColors[6];
			buttonText[index].text = "";
			yield return new WaitForSeconds(0.2f);
		}
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		hexButtons[6].OnHighlight = delegate { buttonMesh[6].material = buttonColors["ROYGBP".IndexOf(centerColor)]; if (colorMode) buttonText[6].text = centerColor; };
		hexButtons[6].OnHighlightEnded = delegate { buttonMesh[6].material = buttonColors[6]; buttonText[6].text = ""; };
		hexButtons[6].OnInteract = delegate { pressedWhiteCenter(); return false; };
		buttonMesh[6].material = buttonColors[6];
		buttonText[6].text = "";
		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedWhite(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { pressedWhiteRelease(i); };
		}
	}
#pragma warning disable 414
	private readonly string TwitchHelpMessage = @"!{0} press|p tl/1 tr/2 ml/3 mr/4 bl/5 br/6 c/7 sl presses the top-left, top-right, middle-left, middle-right, bottom-left, bottom-right, center, and the status light in that order. !{0} hover|h tl/1 tr/2 ml/3 mr/4 bl/5 br/6 c/7 will hover the buttons in the same fashion.";
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
		else if ((Regex.IsMatch(param[0], @"^\s*HOVER\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(param[0], @"^\s*H\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) && param.Length > 1)
		{
			if (isPos2(param))
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
					if (hexButtons[cursor].OnHighlight != null)
					{
						hexButtons[cursor].OnHighlight();
						yield return new WaitForSeconds(1.0f);
						hexButtons[cursor].OnHighlightEnded();
						yield return new WaitForSeconds(0.3f);
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
	private bool isPos2(string[] param)
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
