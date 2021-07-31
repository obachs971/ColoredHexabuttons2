using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowHexabuttons{

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
			"-C-D.E-F.G-H-",
			"-.-----.---.-",
			"-I-J.K-L-M.N-",
			"---.---.---.-",
			"-O.P-Q-R-S-T-",
			"-----.---.---",
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
	private ColorfulButtonSeries coloredHexabuttons;
	private KMAudio Audio;
	private int moduleId;
	private KMSelectable[] hexButtons;
	private MeshRenderer[] buttonMesh;
	private MeshFilter[] buttonMF;
	private MeshFilter[] highlightMF;
	private Transform[] highlightTF;
	private MeshRenderer[] ledMesh;
	private Transform transform;
	private MeshFilter[] shapes;
	private string[] voiceMessage;
	private char cursor;
	private bool moduleSolved;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR", "C"};
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
	private float[] shapeHLSizes = {1.045f, 1.08f, 1.06f, 1.06f, 1.04f, 1.05f, 1.06f, 1.12f, 1.05f, 1.05f};
	private float[] shapeHLPositions = {-0.5f, -0.5f, -0.24f, -0.5f, -0.24f, -0.025f, -0.5f, -0.5f, -0.35f, -0.016f};
	private string[] shapeNames = {"CIRCLE", "TRIANGLE", "SQUARE", "PENTAGON", "HEXAGON", "OCTAGON", "HEART", "STAR", "CRESCENT", "CROSS"};
	private string yellowShapes;
	private int[] yellowRC;
	private string[][] mazes;
	private int mazeToggle;
	public YellowHexabuttons(ColorfulButtonSeries m, KMAudio aud, int MI, KMSelectable[] HB, MeshRenderer[] BM, MeshFilter[] BMF, MeshFilter[] HMF, Transform[] HTF, MeshRenderer[] LM, Transform T, MeshFilter[] S)
	{
		coloredHexabuttons = m;
		Audio = aud;
		moduleId = MI;
		hexButtons = HB;
		buttonMesh = BM;
		buttonMF = BMF;
		highlightMF = HMF;
		highlightTF = HTF;
		ledMesh = LM;
		transform = T;
		shapes = S;
	}
	public void run()
	{
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color Generated: Yellow", moduleId);
		string possible = "0123456789";
		yellowShapes = "";
		for (int i = 0; i < 7; i++)
		{
			if (i < 6)
				ledMesh[i].transform.localScale = new Vector3(0f, 0f, 0f);
			yellowShapes = yellowShapes + "" + possible[UnityEngine.Random.Range(0, possible.Length)];
			buttonMF[i].mesh = shapes[yellowShapes[i] - '0'].sharedMesh;
			highlightMF[i].mesh = shapes[yellowShapes[i] - '0'].sharedMesh;
			highlightTF[i].transform.localScale = new Vector3(shapeHLSizes[yellowShapes[i] - '0'], 0.01f, shapeHLSizes[yellowShapes[i] - '0']);
			highlightTF[i].transform.localPosition = new Vector3(0f, shapeHLPositions[yellowShapes[i] - '0'], 0f);
			hexButtons[i].transform.localScale = new Vector3(shapeSizes[yellowShapes[i] - '0'][0], shapeSizes[yellowShapes[i] - '0'][1], shapeSizes[yellowShapes[i] - '0'][2]);
			possible = possible.Replace(yellowShapes[i] + "", "");
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} button is a {2}", moduleId, positions[i], shapeNames[yellowShapes[i] - '0']);
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
				order[ind] = new string[] {"M1", "M2", "UP", "RIGHT", "DOWN", "LEFT"}[accum];
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
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Button Order: {1}", moduleId, buttonPriority);
		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedYellow(i, order[i]); return false; };
			hexButtons[i].OnInteractEnded = delegate { pressedYellowRelease(i); };
		}
		hexButtons[6].OnInteract = delegate { pressedYellowCenter(); return false; };
		voiceMessage = new string[2];
		yellowRC = new int[2];
		voiceMessage[0] = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"[UnityEngine.Random.Range(0, 36)] + "";
		var num = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(voiceMessage[0]);
		yellowRC[0] = ((num / 6) * 2) + 1;
		yellowRC[1] = ((num % 6) * 2) + 1;
		string[] tempMaze = new string[13];
		for(int aa = 0; aa < 13; aa++)
		{
			tempMaze[aa] = "";
			for(int bb = 0; bb < 13; bb++)
			{
				if (mazes[0][aa][bb] == mazes[1][aa][bb])
					tempMaze[aa] = tempMaze[aa] + "" + mazes[0][aa][bb];
				else
					tempMaze[aa] = tempMaze[aa] + ".";
			}
		}
		var possGoals = getPossGoals(yellowRC, tempMaze);
		voiceMessage[1] = possGoals[UnityEngine.Random.Range(0, possGoals.Length)] + "";
		cursor = voiceMessage[0][0];
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Start: {1}", moduleId, voiceMessage[0]);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Goal: {1}", moduleId, voiceMessage[1]);
	}
	string getPossGoals(int[] cur, string[] tempMaze)
	{
		string goals = tempMaze[cur[0]][cur[1]] + "";
		string prev = tempMaze[cur[0]][cur[1]] + "";
		for(int i = 0; i < 6; i++)
		{
			for(int aa = 0; aa < goals.Length; aa++)
			{
				for(int bb = 0; bb < tempMaze.Length; bb++)
				{
					if(tempMaze[bb].IndexOf(goals[aa]) >= 0)
					{
						int col = tempMaze[bb].IndexOf(goals[aa]);
						string possSpaces = "";
						if(tempMaze[bb - 1][col] == '.' && prev.IndexOf(tempMaze[bb - 2][col]) < 0)
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
							for(int cc = 1; cc < possSpaces.Length; cc++)
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
	void pressedYellowCenter()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			resetModule();
			coloredHexabuttons.StartCoroutine(playAudio());
		}
	}
	IEnumerator playAudio()
	{
		hexButtons[6].OnInteract = null;
		yield return new WaitForSeconds(0.5f);
		for (int aa = 0; aa < voiceMessage.Length; aa++)
		{
			Audio.PlaySoundAtTransform(voiceMessage[aa], transform);
			yield return new WaitForSeconds(1.5f);
		}
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		hexButtons[6].OnInteract = delegate { pressedYellowCenter(); return false; };
	}
	void pressedYellow(int n, string order)
	{
		if (!(moduleSolved))
		{
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User pressed {1}, which is {2}.", moduleId, positions[n], order);
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			switch(order)
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
						Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Strike! User tried to cross a wall!", moduleId, positions[n], order);
						coloredHexabuttons.Strike();
						resetModule();
					}
					break;
				case "RIGHT":
					if (mazes[mazeToggle][yellowRC[0]][yellowRC[1] + 1] == '.')
						yellowRC[1] += 2;
					else
					{
						Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Strike! User tried to cross a wall!", moduleId, positions[n], order);
						coloredHexabuttons.Strike();
						resetModule();
					}
					break;
				case "DOWN":
					if (mazes[mazeToggle][yellowRC[0] + 1][yellowRC[1]] == '.')
						yellowRC[0] += 2;
					else
					{
						Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Strike! User tried to cross a wall!", moduleId, positions[n], order);
						coloredHexabuttons.Strike();
						resetModule();
					}
					break;
				default:
					if (mazes[mazeToggle][yellowRC[0]][yellowRC[1] - 1] == '.')
						yellowRC[1] -= 2;
					else
					{
						Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Strike! User tried to cross a wall!", moduleId, positions[n], order);
						coloredHexabuttons.Strike();
						resetModule();
					}
					break;
			}
			if (mazes[mazeToggle][yellowRC[0]][yellowRC[1]] == voiceMessage[1][0])
			{
				moduleSolved = true;
				coloredHexabuttons.Solve();
			}
		}
	}
	void pressedYellowRelease(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	void resetModule()
	{
		var num = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(voiceMessage[0]);
		yellowRC[0] = ((num / 6) * 2) + 1;
		yellowRC[1] = ((num % 6) * 2) + 1;
		mazeToggle = 0;
	}
}
