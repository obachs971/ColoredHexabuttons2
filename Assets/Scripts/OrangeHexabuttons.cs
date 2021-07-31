using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeHexabuttons {
	private ColorfulButtonSeries coloredHexabuttons;
	private KMAudio Audio;
	private int moduleId;
	private KMSelectable[] hexButtons;
	private MeshRenderer[] buttonMesh;
	private TextMesh[] buttonText;
	private Material[] ledColors;
	private MeshRenderer[] ledMesh;
	private Transform transform;
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
	public OrangeHexabuttons(ColorfulButtonSeries m, KMAudio a, int mi, KMSelectable[] hb, MeshRenderer[] bm, TextMesh[] bt, Material[] lc, MeshRenderer[] lm, Transform t)
	{
		coloredHexabuttons = m;
		Audio = a;
		moduleId = mi;
		hexButtons = hb;
		buttonMesh = bm;
		buttonText = bt;
		ledColors = lc;
		ledMesh = lm;
		transform = t;
	}
	public void run()
	{
		numButtonPresses = 0;
		moduleSolved = false;
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color Generated: Orange", moduleId);
		//Generate 5 digit number
		string poss = "12345", scramble = "";
		string[] template = {"ABCDE","FGHIJ","KLMNO","PQRST","UVWXY"}, grid = { "", "", "", "", "" };
		voiceMessage = new string[5];
		for (int aa = 0; aa < 5; aa++)
		{
			scramble = scramble + "" + poss[UnityEngine.Random.Range(0, poss.Length)];
			poss = poss.Replace(scramble[aa] + "", "");
			voiceMessage[aa] = scramble[aa] + "";
		}
		for(int aa = 0; aa < 5; aa++)
		{
			int col = scramble.IndexOf("12345"[aa]);
			for (int bb = 0; bb < 5; bb++)
				grid[bb] = grid[bb] + "" + template[scramble.IndexOf("12345"[bb])][col];
		}
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Generated Number: {1}", moduleId, scramble);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1}", moduleId, grid[0]);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1}", moduleId, grid[1]);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1}", moduleId, grid[2]);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1}", moduleId, grid[3]);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1}", moduleId, grid[4]);
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
		string diff = diffs[UnityEngine.Random.Range(0, diffs.Length)];
		ArrayList choices = new ArrayList();
		for(int aa = max - 5; aa < 4; aa++)
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
			int diffNum = UnityEngine.Random.Range(0, diff.Length);
			max = max - (diff[diffNum] - '0');
			choices = findPossSpaces(cursors[aa - 1], max);
			cursors[aa] = (int)choices[UnityEngine.Random.Range(0, choices.Count)];
			diff = diff.Substring(0, diffNum) + "" + diff.Substring(diffNum + 1);
		}
		string pos = "012345";
		buttonCur = new int[6];
		foreach(int i in buttonIndex)
		{
			int tempPos = pos[UnityEngine.Random.Range(0, pos.Length)] - '0';
			buttonText[i].text = grid[cursors[tempPos] / 10][cursors[tempPos] % 10] + "";
			buttonText[i].color = Color.black;
			hexButtons[i].OnInteract = delegate { pressedOrange(i, cursors[tempPos], buttonText[i].text); return false; };
			buttonCur[i] = cursors[tempPos] + 0;
			pos = pos.Replace(tempPos + "", "");
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} Button's Letter: {2}", moduleId, positions[i], buttonText[i].text);
		}
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Possible Solution: {1}{2}{3}{4}{5}{6}", moduleId, grid[cursors[5] / 10][cursors[5] % 10], grid[cursors[4] / 10][cursors[4] % 10], grid[cursors[3] / 10][cursors[3] % 10], grid[cursors[2] / 10][cursors[2] % 10], grid[cursors[1] / 10][cursors[1] % 10], grid[cursors[0] / 10][cursors[0] % 10]);
		hexButtons[6].OnInteract = delegate { pressedOrangeCenter(); return false; };
		distances = new int[5];
	}
	ArrayList findPossSpaces(int cur, int diff)
	{
		ArrayList poss = new ArrayList();
		for(int aa = 0; aa < 5; aa++)
		{
			for(int bb = 0; bb < 5; bb++)
			{
				int d = getDifference(cur / 10, aa) + getDifference(cur % 10, bb);
				if (d == diff)
					poss.Add((aa * 10) + bb);
			}
		}
		return poss;
	}
	void pressedOrange(int n, int cur, string let)
	{
		if (!(moduleSolved))
		{
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User pressed {1} which has a cursor of {2}, {3}", moduleId, positions[n], (cur / 10) + 1, (cur % 10) + 1);
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
			if(numButtonPresses == 6)
			{
				Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Distances: {1} {2} {3} {4} {5}", moduleId, distances[0], distances[1], distances[2], distances[3], distances[4]);
				bool correct = true;
				for(int aa = 1; aa < distances.Length; aa++)
				{
					if(distances[aa] <= distances[aa - 1])
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
	void pressedOrangeCenter()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			resetInput();
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
		hexButtons[6].OnInteract = delegate { pressedOrangeCenter(); return false; };
	}
	int getDifference(int a, int b)
	{
		if (a > b)
			return (a - b);
		else
			return (b - a);
	}
	void resetInput()
	{
		numButtonPresses = 0;
		for (int aa = 0; aa < 6; aa++)
		{
			Vector3 pos = buttonMesh[aa].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[aa].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			ledMesh[aa].material = ledColors[0];
		}
		foreach (int i in buttonIndex)
			hexButtons[i].OnInteract = delegate { pressedOrange(i, buttonCur[i], buttonText[i].text); return false; };
	}
}
