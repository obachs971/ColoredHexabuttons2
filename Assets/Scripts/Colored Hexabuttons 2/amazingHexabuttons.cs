using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class amazingHexabuttons : MonoBehaviour
{
	private string[][] yellowMazes =
	{
		new string[]{
			"-------------",
			"-0-1-2.3.4.5-",
			"-.-.---------",
			"-6.7-8.9-A.B-",
			"-----.---.---",
			"-C-D.E-F.G-H-",
			"-.---------.-",
			"-I.J-K-L.M.N-",
			"-.---.-------",
			"-O-P-Q.R-S.T-",
			"---.---.-.---",
			"-U.V.W-X-Y.Z-",
			"-------------"
			},
		new string[]{
			"-------------",
			"-0.1.2.3-4.5-",
			"---------.---",
			"-6-7.8-9.A-B-",
			"-.-.-------.-",
			"-C-D-E.F-G.H-",
			"-.-.-.-.-.---",
			"-I-J-K-L-M-N-",
			"-.---------.-",
			"-O-P.Q-R-S.T-",
			"---.---.---.-",
			"-U.V-W.X.Y-Z-",
			"-------------"
			},
			new string[]{
			"-------------",
			"-0-1.2.3-4.5-",
			"-.---.-----.-",
			"-6.7-8-9.A-B-",
			"---.-----.-.-",
			"-C-D-E-F.G-H-",
			"-.---.-------",
			"-I-J.K-L-M.N-",
			"-.-.---.---.-",
			"-O-P-Q.R.S-T-",
			"-.---------.-",
			"-U-V.W.X.Y-Z-",
			"-------------"
			},
			new string[]{
			"-------------",
			"-0.1.2-3.4-5-",
			"---.---.---.-",
			"-6-7-8.9-A-B-",
			"-.-------.-.-",
			"-C.D.E-F-G-H-",
			"-------.-.-.-",
			"-I-J.K.L-M-N-",
			"-.-------.---",
			"-O.P-Q.R-S-T-",
			"-.---.-----.-",
			"-U-V.W-X.Y.Z-",
			"-------------"
			},
			new string[]{
			"-------------",
			"-0.1.2-3-4.5-",
			"-----.-.-.-.-",
			"-6.7-8-9-A-B-",
			"-.-----.-----",
			"-C.D-E-F.G-H-",
			"-----.-----.-",
			"-I.J-K.L-M-N-",
			"---.---.-.-.-",
			"-O-P.Q-R-S-T-",
			"-.-------.-.-",
			"-U.V.W-X.Y-Z-",
			"-------------"
			},
			new string[]{
			"-------------",
			"-0-1.2.3.4-5-",
			"-.---------.-",
			"-6-7.8-9.A.B-",
			"-.---.-------",
			"-C.D-E.F-G.H-",
			"-----------.-",
			"-I.J-K.L.M-N-",
			"---.-.-----.-",
			"-O.P-Q-R.S-T-",
			"---------.---",
			"-U.V.W.X-Y.Z-",
			"-------------"
			},
			new string[]{
			"-------------",
			"-0.1-2-3.4.5-",
			"-.---.---.---",
			"-6-7.8.9-A-B-",
			"-.---------.-",
			"-C-D.E-F.G-H-",
			"---.---.---.-",
			"-I.J-K.L-M-N-",
			"---------.-.-",
			"-O.P-Q.R.S-T-",
			"-.-.---------",
			"-U-V-W.X.Y.Z-",
			"-------------"
			},
			new string[]{
			"-------------",
			"-0-1-2-3.4.5-",
			"-.-.-.-----.-",
			"-6-7-8.9.A-B-",
			"-.-.---------",
			"-C-D.E-F.G.H-",
			"-.-----.-----",
			"-I-J.K-L-M.N-",
			"---.-------.-",
			"-O.P-Q-R-S.T-",
			"-----.-.-----",
			"-U.V.W-X.Y.Z-",
			"-------------"
			},
			new string[]{
			"-------------",
			"-0-1.2-3.4.5-",
			"-.---.-.-----",
			"-6.7-8-9-A.B-",
			"-.---.-----.-",
			"-C-D-E-F.G-H-",
			"---.---.---.-",
			"-I-J.K-L-M-N-",
			"-.-.---.-.---",
			"-O-P-Q-R-S.T-",
			"-.---.-----.-",
			"-U.V-W.X.Y-Z-",
			"-------------"
			},
			new string[]{
			"-------------",
			"-0.1.2-3.4-5-",
			"-.-----.---.-",
			"-6-7-8-9-A.B-",
			"---.-.-.---.-",
			"-C.D-E-F-G-H-",
			"-.---.---.---",
			"-I-J.K-L.M-N-",
			"---------.-.-",
			"-O.P.Q-R-S-T-",
			"-.-----.---.-",
			"-U-V.W.X-Y.Z-",
			"-------------"
			}
	};
	public KMBombModule module;
	private static int moduleIdCounter = 1;
	public KMAudio Audio;
	private int moduleId;
	public KMSelectable[] hexButtons;
	public MeshRenderer[] buttonMesh;
	public MeshFilter[] buttonMF;
	public MeshFilter[] highlightMF;
	public Transform[] highlightTF;
	public TextMesh text;
	public MeshFilter[] shapes;
	private string[] voiceMessage;
	private bool moduleSolved;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR", "C" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private float[][] shapeSizes =
	{
		new float[]{0.046f, 0.00323f, 0.046f},//Circle
		new float[]{0.0173f, 0.0034f, 0.0173f},//Triangle
		new float[]{0.0341f, 0.0065f, 0.0341f},//Square
		new float[]{0.0198f, 0.0036f, 0.0198f},//Pentagon
		new float[]{0.017f, 0.007f, 0.015f},//Hexagon
		new float[]{0.217f, 0.063f, 0.217f},//Octagon
		new float[]{0.0196f, 0.003f, 0.0196f},//Heart
		new float[]{0.0197f, 0.0034f, 0.0197f},//Star
		new float[]{0.0204f, 0.0045f, 0.0204f},//Crescent
		new float[]{0.369f, 0.107f, 0.369f}//Cross
	};
	private float[] shapeHLSizes = { 1.045f, 1.08f, 1.06f, 1.06f, 1.04f, 1.05f, 1.06f, 1.12f, 1.05f, 1.05f };
	private float[] shapeHLPositions = { -0.5f, -0.5f, -0.24f, -0.5f, -0.24f, -0.025f, -0.5f, -0.5f, -0.35f, -0.016f };
	private string[] shapeNames = { "CIRCLE", "TRIANGLE", "SQUARE", "PENTAGON", "HEXAGON", "OCTAGON", "HEART", "STAR", "CRESCENT", "CROSS" };
	private string yellowShapes;
	private int[] yellowRC;
	private string[][] mazes;
	private int mazeToggle;
	private bool deafMode = false;
	void Awake()
	{
		moduleId = moduleIdCounter++;
		yellowShapes = new string("0123456789".ToCharArray().Shuffle()).Substring(0, 7);
		for (int i = 0; i < 7; i++)
		{
			buttonMF[i].mesh = shapes[yellowShapes[i] - '0'].sharedMesh;
			highlightMF[i].mesh = shapes[yellowShapes[i] - '0'].sharedMesh;
			highlightTF[i].transform.localScale = new Vector3(shapeHLSizes[yellowShapes[i] - '0'], 0.01f, shapeHLSizes[yellowShapes[i] - '0']);
			highlightTF[i].transform.localPosition = new Vector3(0f, shapeHLPositions[yellowShapes[i] - '0'], 0f);
			hexButtons[i].transform.localScale = new Vector3(shapeSizes[yellowShapes[i] - '0'][0], shapeSizes[yellowShapes[i] - '0'][1], shapeSizes[yellowShapes[i] - '0'][2]);
			Debug.LogFormat("[Amazing Hexabuttons #{0}] {1} button is a {2}", moduleId, positions[i], shapeNames[yellowShapes[i] - '0']);
		}
		//Index starts at 0: Circle, Triangle, Square, Pentagon, Hexagon, Octagon, Heart, Star, Crescent, Cross
		string[] priorityList =
		{
			"594326781",
			"469235807",
			"186907453",
			"047158692",
			"930862175",
			"703684219",
			"258071934",
			"812549360",
			"371490526",
			"625713048"
		};
		int accum = 0;
		string[] order = new string[6];
		string buttonPriority = "";
		mazes = new string[2][];
		mazeToggle = 0;
		for (int aa = 0; aa < 9; aa++)
		{
			if (yellowShapes.IndexOf(priorityList[yellowShapes[6] - '0'][aa]) >= 0)
			{
				int ind = yellowShapes.IndexOf(priorityList[yellowShapes[6] - '0'][aa]);
				order[ind] = new string[] { "M1", "M2", "UP", "RIGHT", "DOWN", "LEFT" }[accum];
				if (order[ind].Equals("M1"))
					mazes[0] = yellowMazes[priorityList[yellowShapes[6] - '0'][aa] - '0'];
				else if (order[ind].Equals("M2"))
					mazes[1] = yellowMazes[priorityList[yellowShapes[6] - '0'][aa] - '0'];
				buttonPriority = buttonPriority + "" + positions[ind] + " ";
				accum++;
			}
			if (accum == 6)
				break;
		}
		Debug.LogFormat("[Amazing Hexabuttons #{0}] Button Order: {1}", moduleId, buttonPriority);
		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedButton(i, order[i]); return false; };
			hexButtons[i].OnInteractEnded = delegate { releasedButton(i); };
		}
		hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
		hexButtons[7].OnInteract = delegate { deafMode = !(deafMode); return false; };
		voiceMessage = new string[2];
		yellowRC = new int[2];
		voiceMessage[0] = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"[UnityEngine.Random.Range(0, 36)] + "";
		var num = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(voiceMessage[0]);
		yellowRC[0] = ((num / 6) * 2) + 1;
		yellowRC[1] = ((num % 6) * 2) + 1;
		string[] tempMaze = new string[13];
		for (int aa = 0; aa < 13; aa++)
		{
			tempMaze[aa] = "";
			for (int bb = 0; bb < 13; bb++)
			{
				if (mazes[0][aa][bb] == mazes[1][aa][bb])
					tempMaze[aa] = tempMaze[aa] + "" + mazes[0][aa][bb];
				else
					tempMaze[aa] = tempMaze[aa] + ".";
			}
		}
		var possGoals = getPossGoals(yellowRC, tempMaze);
		voiceMessage[1] = possGoals[UnityEngine.Random.Range(0, possGoals.Length)] + "";
		Debug.LogFormat("[Amazing Hexabuttons #{0}] Start: {1}", moduleId, voiceMessage[0]);
		Debug.LogFormat("[Amazing Hexabuttons #{0}] Goal: {1}", moduleId, voiceMessage[1]);
	}
	string getPossGoals(int[] cur, string[] tempMaze)
	{
		string goals = tempMaze[cur[0]][cur[1]] + "";
		string prev = tempMaze[cur[0]][cur[1]] + "";
		for (int i = 0; i < 6; i++)
		{
			for (int aa = 0; aa < goals.Length; aa++)
			{
				for (int bb = 0; bb < tempMaze.Length; bb++)
				{
					if (tempMaze[bb].IndexOf(goals[aa]) >= 0)
					{
						int col = tempMaze[bb].IndexOf(goals[aa]);
						string possSpaces = "";
						if (tempMaze[bb - 1][col] == '.' && prev.IndexOf(tempMaze[bb - 2][col]) < 0)
							possSpaces = possSpaces + "" + tempMaze[bb - 2][col];
						if (tempMaze[bb][col + 1] == '.' && prev.IndexOf(tempMaze[bb][col + 2]) < 0)
							possSpaces = possSpaces + "" + tempMaze[bb][col + 2];
						if (tempMaze[bb + 1][col] == '.' && prev.IndexOf(tempMaze[bb + 2][col]) < 0)
							possSpaces = possSpaces + "" + tempMaze[bb + 2][col];
						if (tempMaze[bb][col - 1] == '.' && prev.IndexOf(tempMaze[bb][col - 2]) < 0)
							possSpaces = possSpaces + "" + tempMaze[bb][col - 2];
						if (possSpaces.Length > 0)
						{
							prev = prev + "" + possSpaces;
							goals = goals.Substring(0, aa) + "" + possSpaces[0] + "" + goals.Substring(aa + 1);
							for (int cc = 1; cc < possSpaces.Length; cc++)
							{
								aa++;
								goals = goals.Substring(0, aa) + "" + possSpaces[cc] + "" + goals.Substring(aa);
							}
						}
						else
						{
							goals = goals.Replace(goals[aa] + "", "");
							aa--;
						}
						break;
					}
				}
			}
		}
		return goals;
	}
	void pressedButton(int n, string order)
	{
		if (!(moduleSolved))
		{
			Debug.LogFormat("[Amazing Hexabuttons #{0}] User pressed {1}, which is {2}.", moduleId, positions[n], order);
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			switch (order)
			{
				case "M1":
					mazeToggle = 0;
					break;
				case "M2":
					mazeToggle = 1;
					break;
				case "UP":
					if (mazes[mazeToggle][yellowRC[0] - 1][yellowRC[1]] == '.')
						yellowRC[0] -= 2;
					else
					{
						Debug.LogFormat("[Amazing Hexabuttons #{0}] Strike! User tried to cross a wall!", moduleId, positions[n], order);
						module.HandleStrike();
						resetModule();
					}
					break;
				case "RIGHT":
					if (mazes[mazeToggle][yellowRC[0]][yellowRC[1] + 1] == '.')
						yellowRC[1] += 2;
					else
					{
						Debug.LogFormat("[Amazing Hexabuttons #{0}] Strike! User tried to cross a wall!", moduleId, positions[n], order);
						module.HandleStrike();
						resetModule();
					}
					break;
				case "DOWN":
					if (mazes[mazeToggle][yellowRC[0] + 1][yellowRC[1]] == '.')
						yellowRC[0] += 2;
					else
					{
						Debug.LogFormat("[Amazing Hexabuttons #{0}] Strike! User tried to cross a wall!", moduleId, positions[n], order);
						module.HandleStrike();
						resetModule();
					}
					break;
				default:
					if (mazes[mazeToggle][yellowRC[0]][yellowRC[1] - 1] == '.')
						yellowRC[1] -= 2;
					else
					{
						Debug.LogFormat("[Amazing Hexabuttons #{0}] Strike! User tried to cross a wall!", moduleId, positions[n], order);
						module.HandleStrike();
						resetModule();
					}
					break;
			}
			if (mazes[mazeToggle][yellowRC[0]][yellowRC[1]] == voiceMessage[1][0])
			{
				hexButtons[6].OnInteract = null;
				hexButtons[7].OnInteract = null;
				moduleSolved = true;
				module.HandlePass();
			}
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
	void pressedCenter()
	{
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0126f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		resetModule();
		StartCoroutine(playAudio());	
	}
	IEnumerator playAudio()
	{
		hexButtons[6].OnInteract = null;
		yield return new WaitForSeconds(0.5f);
		for (int aa = 0; aa < voiceMessage.Length; aa++)
		{
			Audio.PlaySoundAtTransform(voiceMessage[aa], transform);
			if (deafMode)
				text.text = voiceMessage[aa] + "";
			yield return new WaitForSeconds(1.5f);
		}
		text.text = "";
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		if (!(moduleSolved))
			hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
	}
	void resetModule()
	{
		var num = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(voiceMessage[0]);
		yellowRC[0] = ((num / 6) * 2) + 1;
		yellowRC[1] = ((num % 6) * 2) + 1;
		mazeToggle = 0;
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
