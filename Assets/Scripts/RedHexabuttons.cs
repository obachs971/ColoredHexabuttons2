using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RedHexabuttons
{
	private ColorfulButtonSeries coloredHexabuttons;
	private KMAudio Audio;
	private int moduleId;
	private KMSelectable[] hexButtons;
	private MeshRenderer[] buttonMesh;
	private Material[] ledColors;
	private MeshRenderer[] ledMesh;
	private Transform transform;
	private string[] voiceMessage;
	private int[] solution;
	private int[] submission;
	private int numButtonPresses;
	private bool moduleSolved;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private Vector3[] ledPos =
	{
		new Vector3(-1.131f, 0.43f, 0f),
		new Vector3(0f, 0.43f, 1.131f),
		new Vector3(1.131f, 0.43f, 0f),
		new Vector3(0f, 0.43f, -1.131f)
	};
	private string[] table1 =
	{
		"LUDRUR",
		"ULRDDL",
		"DRLURU",
		"RDULLD"
	};
	private int[][] table2 =
	{
		new int[]{ 2, 4, 1, 0, 3, 5 },
		new int[]{ 0, 2, 5, 3, 1, 4 },
		new int[]{ 4, 3, 0, 1, 5, 2 },
		new int[]{ 3, 0, 4, 5, 2, 1 },
		new int[]{ 5, 1, 3, 2, 4, 0 },
		new int[]{ 1, 5, 2, 4, 0, 3 }
	};
	public RedHexabuttons(ColorfulButtonSeries m, KMAudio aud, int MI, KMSelectable[] HB, MeshRenderer[] BM, Material[] LC, MeshRenderer[] LM, Transform T)
	{
		coloredHexabuttons = m;
		Audio = aud;
		moduleId = MI;
		hexButtons = HB;
		buttonMesh = BM;
		ledColors = LC;
		ledMesh = LM;
		transform = T;
	}
	public void run()
	{
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color Generated: Red", moduleId);
		moduleSolved = false;
		voiceMessage = new string[2];
		voiceMessage[0] = "123456"[UnityEngine.Random.Range(0, 6)] + "";
		voiceMessage[1] = "ABCDEF"[UnityEngine.Random.Range(0, 6)] + "";
		hexButtons[6].OnInteract = delegate { pressedRedCenter(); return false; };
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Generated Voice Message {1}{2}", moduleId, voiceMessage[0], voiceMessage[1]);
		string dir = "";
		foreach(int i in buttonIndex)
		{
			int numRot = UnityEngine.Random.Range(0, 4);
			ledMesh[i].transform.localPosition = ledPos[numRot];
			dir = dir + "" + table1[numRot][i];
			hexButtons[i].OnInteract = delegate { pressedRed(i); return false; };
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} button's LED is pointing {2}", moduleId, positions[i], new string[] {"UP", "RIGHT", "DOWN", "LEFT"}[numRot]);
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} button's direction is {2}", moduleId, positions[i], new string[] { "UP", "RIGHT", "DOWN", "LEFT" }["URDL".IndexOf(dir[i])]);
		}
		numButtonPresses = 0;
		solution = new int[6];
		submission = new int[6];
		int[] cursor = {"123456".IndexOf(voiceMessage[0]), "ABCDEF".IndexOf(voiceMessage[1])};
		solution[0] = table2[cursor[0]][cursor[1]];
		for(int aa = 1; aa < 6; aa++)
		{
			while(hasPressed(solution, table2[cursor[0]][cursor[1]], aa))
			{
				switch(dir[solution[aa - 1]])
				{
					case 'U':
						cursor[0] = mod(cursor[0] - 1, 6);
						break;
					case 'R':
						cursor[1] = mod(cursor[1] + 1, 6);
						break;
					case 'D':
						cursor[0] = mod(cursor[0] + 1, 6);
						break;
					case 'L':
						cursor[1] = mod(cursor[1] - 1, 6);
						break;
				}
			}
			solution[aa] = table2[cursor[0]][cursor[1]];
		}
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Solution: {1} {2} {3} {4} {5} {6}", moduleId, positions[solution[0]], positions[solution[1]], positions[solution[2]], positions[solution[3]], positions[solution[4]], positions[solution[5]]);
	}
	bool hasPressed(int[] s, int p, int i)
	{
		for(int aa = 0; aa < i; aa++)
		{
			if (s[aa] == p)
				return true;
		}
		return false;
	}
	int mod(int n, int m)
	{
		while (n < 0)
			n += m;
		return (n % m);
	}
	void pressedRed(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			hexButtons[n].OnInteract = null;
			ledMesh[n].material = ledColors[1];
			submission[numButtonPresses] = n;
			numButtonPresses++;
			if(numButtonPresses == 6)
			{
				Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User Submission: {1} {2} {3} {4} {5} {6}", moduleId, positions[submission[0]], positions[submission[1]], positions[submission[2]], positions[submission[3]], positions[submission[4]], positions[submission[5]]);
				bool correct = true;
				for(int aa = 0; aa < 6; aa++)
				{
					if(submission[aa] != solution[aa])
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
	void pressedRedCenter()
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
		hexButtons[6].OnInteract = delegate { pressedRedCenter(); return false; };
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
			hexButtons[i].OnInteract = delegate { pressedRed(buttonIndex[i]); return false; };
	}
}
