using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class logicalHexabuttons : MonoBehaviour
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
	private int[] solution;
	private int[] submission;
	private int numButtonPresses;
	private int[] values;
	private ArrayList clues = new ArrayList();
	private int cursor;
	private bool play;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private bool deafMode = false;
	void Awake()
	{
		moduleId = moduleIdCounter++;
		play = true;
		//Generating Solution
		int[][] table =
		{
			new int[]{ 1, 7, 21, 25, 33, 35 },
			new int[]{ 12, 2, 8, 22, 26, 34 },
			new int[]{ 17, 13, 3, 9, 23, 27 },
			new int[]{ 28, 18, 14, 4, 10, 24 },
			new int[]{ 31, 29, 19, 15, 5, 11 },
			new int[]{ 36, 32, 30, 20, 16, 6 }
		};
		string[] pairChoices = { "ABCDEF", "123456" };
		string[] pairs = new string[6];
		values = new int[6];
		for (int aa = 0; aa < 6; aa++)
		{
			pairs[aa] = pairChoices[0][UnityEngine.Random.Range(0, pairChoices[0].Length)] + "" + pairChoices[1][UnityEngine.Random.Range(0, pairChoices[1].Length)];
			pairChoices[0] = pairChoices[0].Replace(pairs[aa][0] + "", "");
			pairChoices[1] = pairChoices[1].Replace(pairs[aa][1] + "", "");
			values[aa] = table["123456".IndexOf(pairs[aa][1])]["ABCDEF".IndexOf(pairs[aa][0])];
			Debug.LogFormat("[Logical Hexabuttons #{0}] {1} button's pair: {2}", moduleId, positions[aa], pairs[aa]);
		}
		solution = new int[6];
		submission = new int[6];
		int items = 0;
		for (int aa = 1; aa < 37; aa++)
		{
			for (int bb = 0; bb < 6; bb++)
			{
				if (values[bb] == aa)
				{
					solution[items] = bb;
					items++;
					break;
				}
			}
			if (items == 6)
				break;
		}
		Debug.LogFormat("[Logical Hexabuttons #{0}] Solution: {1} {2} {3} {4} {5} {6}", moduleId, positions[solution[0]], positions[solution[1]], positions[solution[2]], positions[solution[3]], positions[solution[4]], positions[solution[5]]);
		//Generating Clues
		getClues(pairs);
		for (int aa = 0; aa < clues.Count; aa++)
			Debug.LogFormat("[Logical Hexabuttons #{0}] Clue #{1}: {2}", moduleId, aa, (string)clues[aa]);
		cursor = clues.Count - 1;
		buttonText[6].color = Color.white;
		numButtonPresses = 0;
		foreach (int i in buttonIndex)
			hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
		hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
		hexButtons[6].OnInteractEnded = delegate { releasedCenter(); };
		hexButtons[7].OnInteract = delegate { deafMode = !(deafMode); return false; };
	}
	void getClues(string[] pairs)
	{
		//Setting up so that each section is a row, and each number is the column it took, with the position it's in is the row
		clues = new ArrayList();
		int[][] rowSol = new int[3][];
		for (int aa = 0; aa < 3; aa++)
			rowSol[aa] = new int[6];
		for (int aa = 0; aa < 6; aa++)
			rowSol[0]["123456".IndexOf(pairs[aa][1])] = aa + 0;
		for (int aa = 0; aa < 6; aa++)
			rowSol[1]["123456".IndexOf(pairs[aa][1])] = "ABCDEF".IndexOf(pairs[aa][0]);
		for (int aa = 0; aa < 6; aa++)
			rowSol[2]["ABCDEF".IndexOf(pairs[aa][0])] = aa + 0;
		//Debug.LogFormat("[Logical Hexabuttons #{0}] Row Solution #1: {1}{2}{3}{4}{5}{6}", moduleId, rowSol[0][0], rowSol[0][1], rowSol[0][2], rowSol[0][3], rowSol[0][4], rowSol[0][5]);
		//Debug.LogFormat("[Logical Hexabuttons #{0}] Row Solution #2: {1}{2}{3}{4}{5}{6}", moduleId, rowSol[1][0], rowSol[1][1], rowSol[1][2], rowSol[1][3], rowSol[1][4], rowSol[1][5]);
		//Debug.LogFormat("[Logical Hexabuttons #{0}] Row Solution #3: {1}{2}{3}{4}{5}{6}", moduleId, rowSol[2][0], rowSol[2][1], rowSol[2][2], rowSol[2][3], rowSol[2][4], rowSol[2][5]);
		char[][][] grid =
		{
			new char[][]
			{
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' }
			},
			new char[][]
			{
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' }
			},
			new char[][]
			{
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' },
				new char[]{ 'W', 'W', 'W', 'W', 'W', 'W' }
			}
		};
		bool flag = true;
		while (flag)
		{
			//This here will give all possible positions that can be used to generate a clue.
			//It will always be in this format: White Count, Section of the grid, If it's a Row or Col position, Index of the Row/Col
			ArrayList poss = getPossPos(grid);
			string pos = (string)poss[UnityEngine.Random.Range(0, poss.Count)];
			//Depending on the white count will generate the clue.
			//My idea is to limit the number of variables as much as I can for the user.
			//Less variables means less communication and more solving
			string[] messages =
			{
				"QZ5",  "NOR",  "EW2",  "BU7",
				"C48",  "VX9",  "DLY",  "HJM",
				"KS3",  "PT0",  "AI1",  "FG6"
			};
			//Each 1st index of the message refers to the section of the grid.
			//Each 2nd index of the message will always be in the order of Row, Col.
			string message;
			switch (pos[0])
			{
				case '6':
				case '5':   //A white count of 5 or 6 will give us a positive clue
					int[][] messageIndex65 = { new int[] { 4, 0 }, new int[] { 6, 10 }, new int[] { 8, 2 } };
					message = messages[messageIndex65[pos[1] - '0']["RC".IndexOf(pos[2])]];
					break;
				case '4':   //A white count of 4 will give a 50/50 chance of being a positive or negative clue
					int[][] messageIndex4;
					if (UnityEngine.Random.Range(0, 2) == 0)
						messageIndex4 = new int[][] { new int[] { 4, 0 }, new int[] { 6, 10 }, new int[] { 8, 2 } };
					else
						messageIndex4 = new int[][] { new int[] { 5, 1 }, new int[] { 7, 11 }, new int[] { 9, 3 } };
					message = messages[messageIndex4[pos[1] - '0']["RC".IndexOf(pos[2])]];
					break;
				default:    //A white count of 2 or 3 will give us a negative clue
					int[][] messageIndex32 = { new int[] { 5, 1 }, new int[] { 7, 11 }, new int[] { 9, 3 } };
					message = messages[messageIndex32[pos[1] - '0']["RC".IndexOf(pos[2])]];
					break;
			}
			//This here adds the numbers for the clue
			//If it is a positive clue, we add the numbers that have been crossed out, including the solution for that position
			//If it is a negative clue, we add the numbers that have not been crossed out, excluding the solution for that position
			string numbers = "-012345".IndexOf(pos[3]) + "";
			switch (message)
			{
				case "C48":
				case "DLY":
				case "KS3":
					for (int aa = 0; aa < 6; aa++)
					{
						if (grid[pos[1] - '0'][pos[3] - '0'][aa] != 'W' || rowSol[pos[1] - '0'][pos[3] - '0'] == aa)
							numbers = numbers + "" + (aa + 1);
					}
					break;
				case "VX9":
				case "HJM":
				case "PT0":
					for (int aa = 0; aa < 6; aa++)
					{
						if (rowSol[pos[1] - '0'][pos[3] - '0'] != aa && grid[pos[1] - '0'][pos[3] - '0'][aa] == 'W')
							numbers = numbers + "" + (aa + 1);
					}
					break;
				case "QZ5":
				case "AI1":
				case "EW2":
					int num = findValue(rowSol[pos[1] - '0'], pos[3] - '0');
					for (int aa = 0; aa < 6; aa++)
					{
						if (grid[pos[1] - '0'][aa][pos[3] - '0'] != 'W' || num == aa)
							numbers = numbers + "" + (aa + 1);
					}
					break;
				case "NOR":
				case "FG6":
				case "BU7":
					for (int aa = 0; aa < 6; aa++)
					{
						int ignore = findValue(rowSol[pos[1] - '0'], pos[3] - '0');
						if (ignore != aa && grid[pos[1] - '0'][aa][pos[3] - '0'] == 'W')
							numbers = numbers + "" + (aa + 1);
					}
					break;
			}
			clues.Add(message[UnityEngine.Random.Range(0, 3)] + "" + numbers);
			//Finally, we fill in the grid based on the clue so that the next generated clue can benefit from it
			//Basically its a solver for this module
			if (pos[2] == 'R')
				grid = fillGrid(grid, pos[1] - '0', pos[3] - '0', rowSol[pos[1] - '0'][pos[3] - '0']);
			else
				grid = fillGrid(grid, pos[1] - '0', findValue(rowSol[pos[1] - '0'], pos[3] - '0'), pos[3] - '0');
			flag = check(grid);
		}
		clues.Shuffle();
	}
	char[][][] fillGrid(char[][][] grid, int section, int row, int col)
	{
		if (section == 0)
		{
			for (int aa = 0; aa < 6; aa++)
			{
				if (aa != row)
				{
					grid[section][aa][col] = 'R';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[1][aa][bb] == 'G')
						{
							grid[2][bb][col] = 'R';
							break;
						}
					}
				}
				else
				{
					grid[section][aa][col] = 'G';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[1][aa][bb] == 'R')
							grid[2][bb][col] = 'R';
					}
				}
			}
			for (int aa = 0; aa < 6; aa++)
			{
				if (aa != col)
				{
					grid[section][row][aa] = 'R';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[2][bb][aa] == 'G')
						{
							grid[1][row][bb] = 'R';
							break;
						}
					}
				}
				else
				{
					grid[section][row][aa] = 'G';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[2][bb][aa] == 'R')
							grid[1][row][bb] = 'R';
					}
				}
			}
		}
		else if (section == 1)
		{
			for (int aa = 0; aa < 6; aa++)
			{
				if (aa != row)
				{
					grid[section][aa][col] = 'R';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[0][aa][bb] == 'G')
						{
							grid[2][col][bb] = 'R';
							break;
						}
					}
				}
				else
				{
					grid[section][aa][col] = 'G';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[0][aa][bb] == 'R')
							grid[2][col][bb] = 'R';
					}
				}
			}
			for (int aa = 0; aa < 6; aa++)
			{
				if (aa != col)
				{
					grid[section][row][aa] = 'R';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[2][aa][bb] == 'G')
						{
							grid[0][row][bb] = 'R';
							break;
						}
					}
				}
				else
				{
					grid[section][row][aa] = 'G';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[2][aa][bb] == 'R')
							grid[0][row][bb] = 'R';
					}
				}
			}
		}
		else
		{
			for (int aa = 0; aa < 6; aa++)
			{
				if (aa != row)
				{
					grid[section][aa][col] = 'R';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[1][bb][aa] == 'G')
						{
							grid[0][bb][col] = 'R';
							break;
						}
					}
				}
				else
				{
					grid[section][aa][col] = 'G';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[1][bb][aa] == 'R')
							grid[0][bb][col] = 'R';
					}
				}
			}
			for (int aa = 0; aa < 6; aa++)
			{
				if (aa != col)
				{
					grid[section][row][aa] = 'R';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[0][bb][aa] == 'G')
						{
							grid[1][bb][row] = 'R';
							break;
						}
					}
				}
				else
				{
					grid[section][row][aa] = 'G';
					for (int bb = 0; bb < 6; bb++)
					{
						if (grid[0][bb][aa] == 'R')
							grid[1][bb][row] = 'R';
					}
				}
			}
		}
		Debug.LogFormat("[Logical Hexabuttons #{0}] Clue: {1}", moduleId, clues[clues.Count - 1]);
		for (int aa = 0; aa < 3; aa++)
		{
			for (int bb = 0; bb < 6; bb++)
				Debug.LogFormat("[Logical Hexabuttons #{0}] {1}{2}{3}{4}{5}{6}", moduleId, grid[aa][bb][0], grid[aa][bb][1], grid[aa][bb][2], grid[aa][bb][3], grid[aa][bb][4], grid[aa][bb][5]);
			Debug.LogFormat("[Logical Hexabuttons #{0}] ---------------", moduleId);
		}
		for (int aa = 0; aa < 3; aa++)
		{
			for (int bb = 0; bb < 6; bb++)
			{
				string temp = grid[aa][bb][0] + "" + grid[aa][bb][1] + "" + grid[aa][bb][2] + "" + grid[aa][bb][3] + "" + grid[aa][bb][4] + "" + grid[aa][bb][5];
				if (getWhiteCount(temp) == 1)
					grid = fillGrid(grid, aa, bb, temp.IndexOf("W"));
				temp = grid[aa][0][bb] + "" + grid[aa][1][bb] + "" + grid[aa][2][bb] + "" + grid[aa][3][bb] + "" + grid[aa][4][bb] + "" + grid[aa][5][bb];
				if (getWhiteCount(temp) == 1)
					grid = fillGrid(grid, aa, temp.IndexOf("W"), bb);
			}
		}
		return grid;
	}
	int getWhiteCount(string s)
	{
		int sum = 0;
		for (int aa = 0; aa < 6; aa++)
		{
			if (s[aa] == 'W')
				sum++;
		}
		return sum;
	}
	ArrayList getPossPos(char[][][] grid)
	{
		ArrayList possPos = new ArrayList();
		for (int aa = 0; aa < grid.Length; aa++)
		{
			for (int bb = 0; bb < 6; bb++)
			{
				string temp = grid[aa][bb][0] + "" + grid[aa][bb][1] + "" + grid[aa][bb][2] + "" + grid[aa][bb][3] + "" + grid[aa][bb][4] + "" + grid[aa][bb][5];
				int count = getWhiteCount(temp);
				if (count > 0)
					possPos.Add(count + "" + aa + "R" + bb);
				temp = grid[aa][0][bb] + "" + grid[aa][1][bb] + "" + grid[aa][2][bb] + "" + grid[aa][3][bb] + "" + grid[aa][4][bb] + "" + grid[aa][5][bb];
				count = getWhiteCount(temp);
				if (count > 0)
					possPos.Add(count + "" + aa + "C" + bb);
			}
		}
		return possPos;
	}
	int findValue(int[] rowSol, int val)
	{
		for (int aa = 0; aa < 6; aa++)
		{
			if (rowSol[aa] == val)
				return aa;
		}
		return -1;
	}
	bool check(char[][][] grid)
	{
		for (int aa = 0; aa < grid.Length; aa++)
		{
			for (int bb = 0; bb < grid[aa].Length; bb++)
			{
				for (int cc = 0; cc < grid[aa][bb].Length; cc++)
				{
					if (grid[aa][bb][cc] == 'W')
						return true;
				}
			}
		}
		return false;
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
			Debug.LogFormat("[Logical Hexabuttons #{0}] User Submission: {1} {2} {3} {4} {5} {6}", moduleId, values[submission[0]], values[submission[1]], values[submission[2]], values[submission[3]], values[submission[4]], values[submission[5]]);

			bool correct = true;
			for (int aa = 0; aa < 6; aa++)
			{
				if (solution[aa] != submission[aa])
				{
					correct = false;
					break;
				}
			}
			if (correct)
			{
				buttonText[6].text = "";
				hexButtons[6].OnInteract = null;
				hexButtons[6].OnInteractEnded = null;
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
	}
	void releasedCenter()
	{
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		cursor = (cursor + 1) % clues.Count;
		buttonText[6].text = cursor + "";
		StartCoroutine(playAudio((string)clues[cursor]));
	}
	IEnumerator playAudio(string voiceMessage)
	{
		if (play)
		{
			play = false;
			yield return new WaitForSeconds(1f);
			for (int aa = 0; aa < voiceMessage.Length; aa++)
			{
				Audio.PlaySoundAtTransform(voiceMessage[aa] + "", transform);
				if (deafMode)
				{
					for (int i = 0; i < 6; i++)
						buttonText[i].text = voiceMessage[aa] + "";
				}
				yield return new WaitForSeconds(0.8f);
				for (int i = 0; i < 6; i++)
					buttonText[i].text = "";
				yield return new WaitForSeconds(0.2f);
			}
			play = true;
		}
	}
	void resetInput()
	{
		numButtonPresses = 0;
		foreach (int i in buttonIndex)
		{
			Vector3 pos = buttonMesh[i].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[i].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
			ledMesh[i].material = ledColors[0];
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
