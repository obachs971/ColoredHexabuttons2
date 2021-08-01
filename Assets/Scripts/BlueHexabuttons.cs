using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueHexabuttons {

	private ColorfulButtonSeries coloredHexabuttons;
	private KMAudio Audio;
	private int moduleId;
	private KMSelectable[] hexButtons;
	private MeshRenderer[] buttonMesh;
	private TextMesh[] buttonText;
	private Material[] ledColors;
	private MeshRenderer[] ledMesh;
	private Transform transform;
	private int[] solution;
	private int[] submission;
	private string[] blueSwaps;
	private int[] blueButtonValues;
	private string blueButtonText;
	private string blueCenterText;
	private bool flag;
	private string TPOrder;
	private int numButtonPresses;
	private bool moduleSolved;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	public BlueHexabuttons(ColorfulButtonSeries m, KMAudio aud, int MI, KMSelectable[] HB, MeshRenderer[] BM, TextMesh[] BT, Material[] LC, MeshRenderer[] LM, Transform T)
	{
		coloredHexabuttons = m;
		Audio = aud;
		moduleId = MI;
		hexButtons = HB;
		buttonMesh = BM;
		buttonText = BT;
		ledColors = LC;
		ledMesh = LM;
		transform = T;
	}
	public void run()
	{
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color Generated: Blue", moduleId);
		numButtonPresses = 0;
		flag = true;
		moduleSolved = false;
		string order = "ρΞδονφσΓΠλςκγ£ιχΛθΩ∞εξηΨζ$μαπωβ¢ΣυΔτ";
		string choices = "ρΞδονφσΓΠλςκγ£ιχΛθΩ∞εξηΨζ$μαπωβ¢ΣυΔτ";
		blueButtonValues = new int[6];
		blueButtonText = "";
		var tempVals = new int[6];
		for (int aa = 0; aa < 6; aa++)
		{
			blueButtonText = blueButtonText + "" + choices[UnityEngine.Random.Range(0, choices.Length)] + "";
			blueButtonValues[aa] = order.IndexOf(blueButtonText[aa]) + 1;
			tempVals[aa] = blueButtonValues[aa] + 0;
			choices = choices.Replace(blueButtonText[aa] + "", "");
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} button's value: {2}", moduleId, positions[aa], blueButtonValues[aa]);
			buttonText[aa].text = blueButtonText[aa] + "";
		}
		int initial = UnityEngine.Random.Range(0, 1000);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Center Number: {1}", moduleId, initial);
		blueCenterText = initial + "";
		blueSwaps = new string[6];
		choices = "111223";
		int N = initial + 0;
		for(int aa = 0; aa < 6; aa++)
		{
			int cur = UnityEngine.Random.Range(0, choices.Length);
			blueSwaps[aa] = "";
			string swapChoices = "123456";
			for (int bb = 0; bb < (choices[cur] - '0'); bb++)
			{
				char s1 = swapChoices[UnityEngine.Random.Range(0, swapChoices.Length)];
				swapChoices = swapChoices.Replace(s1 + "", "");
				char s2 = swapChoices[UnityEngine.Random.Range(0, swapChoices.Length)];
				swapChoices = swapChoices.Replace(s2 + "", "");
				if((s1 - '0') < (s2 - '0'))
					blueSwaps[aa] = blueSwaps[aa] + "" + s1 + "" + s2;
				else
					blueSwaps[aa] = blueSwaps[aa] + "" + s2 + "" + s1;
			}
			choices = choices.Substring(0, cur) + "" + choices.Substring(cur + 1);
			int newVal = getNewValue(N, blueSwaps[aa], tempVals);
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1}({2}, [{3}, {4}, {5}, {6}, {7}, {8}]) => {9}", moduleId, blueSwaps[aa], N, tempVals[0], tempVals[1], tempVals[2], tempVals[3], tempVals[4], tempVals[5], newVal);
			N = newVal + 0;
			for(int bb = 0; bb < blueSwaps[aa].Length; bb+=2)
			{
				newVal = tempVals["123456".IndexOf(blueSwaps[aa][bb])] + 0;
				tempVals["123456".IndexOf(blueSwaps[aa][bb])] = tempVals["123456".IndexOf(blueSwaps[aa][bb + 1])] + 0;
				tempVals["123456".IndexOf(blueSwaps[aa][bb + 1])] = newVal + 0;
			}
		}
		Array.Sort(tempVals);
		string digitString = (((initial / 100) + (N / 100)) % 10) + "" + ((((initial % 100) / 10) + ((N % 100) / 10)) % 10) + "" + (((initial % 10) + (N % 10)) % 10);
		digitString = digitString + "" + abs((initial / 100), (N / 100)) + "" + abs(((initial % 100) / 10), ((N % 100) / 10)) + "" + abs((initial % 10), (N % 10));
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Digit String: {1}", moduleId, digitString);
		solution = new int[6];
		submission = new int[6];
		int items = 0;
		for(int aa = 0; aa < 10; aa++)
		{
			if(digitString.Contains(aa + ""))
			{
				for(int bb = 0; bb < 6; bb++)
				{
					if((digitString[bb] - '0') == aa)
					{
						solution[bb] = tempVals[items];
						items++;
					}
				}
			}
			if (items >= 6)
				break;
		}
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Solution: {1}, {2}, {3}, {4}, {5}, {6}", moduleId, solution[0], solution[1], solution[2], solution[3], solution[4], solution[5]);
		buttonText[6].text = blueCenterText;
		hexButtons[6].OnInteract = delegate { pressedBlueCenter(); return false; };
		hexButtons[6].OnInteractEnded = delegate { pressedBlueCenterRelease(); };
	}
	int getNewValue(int N, string swaps, int[] vals)
	{
		if (swaps.Length == 2)
			return swap1(N, swaps, vals);
		else if (swaps.Length == 4)
			return swap2(N, swaps, vals);
		else
			return swap3(N, swaps, vals);
	}
	int swap1(int N, string swaps, int[] vals)
	{
		int newVal;
		int X = vals["123456".IndexOf(swaps[0])] + vals["123456".IndexOf(swaps[1])];
		int Y = abs(vals["123456".IndexOf(swaps[0])], vals["123456".IndexOf(swaps[1])]);
		switch (swaps)
		{
			case "12":
				newVal = 2 * abs(N, X);
				break;
			case "13":
				newVal = N + Y;
				break;
			case "14":
				newVal = abs(N, Y);
				break;
			case "15":
				newVal = N + (2 * X);
				break;
			case "16":
				newVal = 2 * (N + Y);
				break;
			case "23":
				newVal = abs(N, X);
				break;
			case "24":
				newVal = abs(N, 2 * Y);
				break;
			case "25":
				newVal = N + (2 * Y);
				break;
			case "26":
				newVal = (2 * N) + Y;
				break;
			case "34":
				newVal = N + X;
				break;
			case "35":
				newVal = 2 * (N + X);
				break;
			case "36":
				newVal = abs(2 * N, Y);
				break;
			case "45":
				newVal = abs(2 * N, X);
				break;
			case "46":
				newVal = abs(N, 2 * X);
				break;
			default:
				newVal = (2 * N) + X;
				break;
		}
		return (newVal % 1000);
	}
	int swap2(int N, string swaps, int[] vals)
	{
		string temp = "123456";
		for (int aa = 0; aa < swaps.Length; aa++)
			temp = temp.Replace(swaps[aa] + "", "");
		int newVal;
		switch (temp)
		{
			case "12":
				newVal = abs(swap1(N + 0, swaps.Substring(0, 2), vals), swap1(N + 0, swaps.Substring(2), vals)) / 2;
				break;
			case "13":
				newVal = 2 * Math.Max(swap1(N + 0, swaps.Substring(0, 2), vals), swap1(N + 0, swaps.Substring(2), vals));
				break;
			case "14":
				newVal = abs(swap1(N + 0, swaps.Substring(0, 2), vals), swap1(N + 0, swaps.Substring(2), vals));
				break;
			case "15":
				newVal = Math.Max(swap1(N + 0, swaps.Substring(0, 2), vals), swap1(N + 0, swaps.Substring(2), vals));
				break;
			case "16":
				newVal = 2 * abs(swap1(N + 0, swaps.Substring(0, 2), vals), swap1(N + 0, swaps.Substring(2), vals));
				break;
			case "23":
				newVal = swap1(N + 0, swaps.Substring(0, 2), vals) + swap1(N + 0, swaps.Substring(2), vals);
				break;
			case "24":
				newVal = 2 * Math.Min(swap1(N + 0, swaps.Substring(0, 2), vals), swap1(N + 0, swaps.Substring(2), vals));
				break;
			case "25":
				newVal = abs(swap1(999 - N, swaps.Substring(0, 2), vals), swap1(999 - N, swaps.Substring(2), vals));
				break;
			case "26":
				newVal = Math.Max(swap1(N + 0, swaps.Substring(0, 2), vals), swap1(N + 0, swaps.Substring(2), vals)) / 2;
				break;
			case "34":
				newVal = swap1(999 - N, swaps.Substring(0, 2), vals) + swap1(999 - N, swaps.Substring(2), vals);
				break;
			case "35":
				newVal = Math.Min(swap1(N + 0, swaps.Substring(0, 2), vals), swap1(N + 0, swaps.Substring(2), vals)) / 2;
				break;
			case "36":
				newVal = 2 * (swap1(N + 0, swaps.Substring(0, 2), vals) + swap1(N + 0, swaps.Substring(2), vals));
				break;
			case "45":
				newVal = (swap1(N + 0, swaps.Substring(0, 2), vals) + swap1(N + 0, swaps.Substring(2), vals)) / 2;
				break;
			case "46":
				newVal = 999 - abs(swap1(N + 0, swaps.Substring(0, 2), vals), swap1(N + 0, swaps.Substring(2), vals));
				break;
			default:
				newVal = Math.Min(swap1(N + 0, swaps.Substring(0, 2), vals), swap1(N + 0, swaps.Substring(2), vals));
				break;
		}
		return (newVal % 1000);
	}
	int swap3(int N, string swaps, int[] vals)
	{
		string oper = "P";
		string[][] list =
		{
			new string[]{ "12", "34", "56" },
			new string[]{ "12", "35", "46" },
			new string[]{ "13", "25", "46" },
			new string[]{ "13", "26", "45" },
			new string[]{ "14", "23", "56" },
			new string[]{ "15", "24", "36" },
			new string[]{ "16", "23", "45" },
			new string[]{ "16", "25", "34" }
		};
		for(int aa = 0; aa < list.Length; aa++)
		{
			if(swaps.Contains(list[aa][0]) && swaps.Contains(list[aa][1]) && swaps.Contains(list[aa][2]))
			{
				oper = "O";
				break;
			}
		}
		if (oper.Equals("O"))
			return Math.Max(swap1(N + 0, swaps.Substring(0, 2), vals), Math.Max(swap1(N + 0, swaps.Substring(2, 2), vals), swap1(N + 0, swaps.Substring(4), vals)));
		else
			return Math.Min(swap1(N + 0, swaps.Substring(0, 2), vals), Math.Min(swap1(N + 0, swaps.Substring(2, 2), vals), swap1(N + 0, swaps.Substring(4), vals)));
	}
	int abs(int a, int b)
	{
		if (a > b)
			return (a - b);
		else
			return (b - a);
	}
	void pressedBlueCenter()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	void pressedBlueCenterRelease()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			if(!(flag) && numButtonPresses > 0)
				resetInput();
			else if (flag)
			{
				flag = false;
				coloredHexabuttons.StartCoroutine(movements());
			}
			else
			{
				flag = true;
				hexButtons[0].transform.localPosition = new Vector3(-0.025f, 0.0169f, 0.034f);
				hexButtons[1].transform.localPosition = new Vector3(0.025f, 0.0169f, 0.034f);
				hexButtons[2].transform.localPosition = new Vector3(-0.05f, 0.0169f, -0.008f);
				hexButtons[3].transform.localPosition = new Vector3(0.05f, 0.0169f, -0.008f);
				hexButtons[4].transform.localPosition = new Vector3(-0.025f, 0.0169f, -0.05f);
				hexButtons[5].transform.localPosition = new Vector3(0.025f, 0.0169f, -0.05f);
				foreach (int i in buttonIndex)
				{
					buttonText[i].color = Color.white;
					buttonText[i].text = blueButtonText[i] + "";
					hexButtons[i].OnInteract = null;
					ledMesh[i].material = ledColors[0];
				}
				buttonText[6].text = blueCenterText;
				numButtonPresses = 0;
				coloredHexabuttons.setOrder("0123456");
			}
		}
	}
	void pressedBlue(int n, int p)
	{
		if (!(moduleSolved))
		{
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User pressed {1}", moduleId, positions[TPOrder.IndexOf((n + ""))], p);
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			hexButtons[n].OnInteract = null;
			ledMesh[n].material = ledColors[1];
			submission[numButtonPresses] = p;
			numButtonPresses++;
			if (numButtonPresses == 6)
			{
				Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User Submission: {1} {2} {3} {4} {5} {6}", moduleId, submission[0], submission[1], submission[2], submission[3], submission[4], submission[5]);
				bool correct = true;
				for(int aa = 0; aa < 6; aa++)
				{
					if (submission[aa] != solution[aa])
					{
						correct = false;
						break;
					}
				}
				if(correct)
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

	IEnumerator movements()
	{
		hexButtons[6].OnInteract = null;
		hexButtons[6].OnInteractEnded = null;
		for (int aa = 0; aa < 100; aa++)
		{
			Vector3 pos = hexButtons[6].transform.localPosition;
			pos.y -= 0.0001f;
			hexButtons[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			for (int bb = 0; bb < 6; bb++)
			{
				Color color = buttonText[bb].color;
				color.r -= 0.01f;
				color.g -= 0.01f;
				buttonText[bb].color = color;
			}
			yield return new WaitForSeconds(0.02f);
		}
		for (int bb = 0; bb < 7; bb++)
			buttonText[bb].text = "";
		yield return new WaitForSeconds(1.0f);
		TPOrder = "012345";
		for (int i = 0; i < blueSwaps.Length; i++)
		{
			float[][] diff = new float[blueSwaps[i].Length / 2][];
			string totalSwaps = "";
			for (int j = 0; j < blueSwaps[i].Length; j+=2)
			{
				string swaps = TPOrder["123456".IndexOf(blueSwaps[i][j])] + "" + TPOrder["123456".IndexOf(blueSwaps[i][j + 1])];
				totalSwaps = totalSwaps + "" + swaps;
				diff[j / 2] = new float[4];
				diff[j / 2][0] = (buttonMesh["012345".IndexOf(swaps[1])].transform.localPosition.x - buttonMesh["012345".IndexOf(swaps[0])].transform.localPosition.x) / 100f;
				diff[j / 2][1] = (buttonMesh["012345".IndexOf(swaps[1])].transform.localPosition.z - buttonMesh["012345".IndexOf(swaps[0])].transform.localPosition.z) / 100f;
				diff[j / 2][2] = (buttonMesh["012345".IndexOf(swaps[0])].transform.localPosition.x - buttonMesh["012345".IndexOf(swaps[1])].transform.localPosition.x) / 100f;
				diff[j / 2][3] = (buttonMesh["012345".IndexOf(swaps[0])].transform.localPosition.z - buttonMesh["012345".IndexOf(swaps[1])].transform.localPosition.z) / 100f;
				TPOrder = TPOrder.Replace(totalSwaps[j], '*');
				TPOrder = TPOrder.Replace(totalSwaps[j + 1], totalSwaps[j]);
				TPOrder = TPOrder.Replace('*', totalSwaps[j + 1]);
			}
			for (int zz = 0; zz < 100; zz++)
			{
				for (int aa = 0; aa < blueSwaps[i].Length; aa+=2)
				{
					Vector3 pos1 = buttonMesh["012345".IndexOf(totalSwaps[aa])].transform.localPosition;
					pos1.x += diff[aa / 2][0];
					pos1.z += diff[aa / 2][1];
					buttonMesh["012345".IndexOf(totalSwaps[aa])].transform.localPosition = new Vector3(pos1.x, pos1.y, pos1.z);
					Vector3 pos2 = buttonMesh["012345".IndexOf(totalSwaps[aa + 1])].transform.localPosition;
					pos2.x += diff[aa / 2][2];
					pos2.z += diff[aa / 2][3];
					buttonMesh["012345".IndexOf(totalSwaps[aa + 1])].transform.localPosition = new Vector3(pos2.x, pos2.y, pos2.z);
				}
				yield return new WaitForSeconds(0.01f);
			}
			float t = blueSwaps[i].Length / 2;
			yield return new WaitForSeconds(t);
		}
		foreach (int i in buttonIndex)
			hexButtons[i].OnInteract = delegate { pressedBlue(i, blueButtonValues[i]); return false; };
		for (int aa = 0; aa < 100; aa++)
		{
			Vector3 pos = hexButtons[6].transform.localPosition;
			pos.y += 0.0001f;
			hexButtons[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			yield return new WaitForSeconds(0.02f);
		}
		hexButtons[6].OnInteract = delegate { pressedBlueCenter(); return false; };
		hexButtons[6].OnInteractEnded = delegate { pressedBlueCenterRelease(); };
		TPOrder = TPOrder + "6";
		coloredHexabuttons.setOrder(TPOrder);
	}
	void resetInput()
	{
		numButtonPresses = 0;
		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedBlue(i, blueButtonValues[i]); return false; };
			Vector3 pos = buttonMesh[i].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[i].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			ledMesh[i].material = ledColors[0];
		}
	}
}
