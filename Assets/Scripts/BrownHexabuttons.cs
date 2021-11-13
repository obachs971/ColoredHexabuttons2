using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownHexabuttons {

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
	public BrownHexabuttons(ColorfulButtonSeries m, KMAudio aud, int MI, KMSelectable[] HB, MeshRenderer[] BM, TextMesh[] BT, Material[] LC, MeshRenderer[] LM, Transform T)
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
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color Generated: Brown", moduleId);
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
		string possSize = "01234567", possCharge = "01234567", possIndex = "012345";
		for(int aa = 0; aa < 6; aa++)
		{
			int I = possIndex[UnityEngine.Random.Range(0, possIndex.Length)] - '0';
			int SI = possSize[UnityEngine.Random.Range(0, possSize.Length)] - '0';
			int CI = possCharge[UnityEngine.Random.Range(0, possCharge.Length)] - '0';
			possIndex = possIndex.Replace(I + "", "");
			possSize = possSize.Replace(SI + "", "");
			possCharge = possCharge.Replace(CI + "", "");
			chemicals[I] = sizes[SI][0] + "" + charges[CI][0] + "" + sizes[SI][1] + "" + charges[CI][1] + "" + sizes[SI][2] + "" + charges[CI][2];
			curr = getResult(curr, chemicals[I]);
			potions[aa] = curr.ToUpperInvariant();
			possSolution[aa] = I;
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
		possIndex = "012345";
		for (int aa = 0; aa < 6; aa++)
		{
			int I = possIndex[UnityEngine.Random.Range(0, possIndex.Length)] - '0';
			possIndex = possIndex.Replace(I + "", "");
			for (int bb = 0; bb < potionTable.Length; bb++)
			{
				if(potionTable[bb].Equals(potions[I]))
				{
					voiceMessage[aa] = alpha[bb] + "";
					break;
				}
			}
		}
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Center Text: {1}", moduleId, buttonText[6].text);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Voice Message: {1}{2}{3}{4}{5}{6}", moduleId, voiceMessage[0], voiceMessage[1], voiceMessage[2], voiceMessage[3], voiceMessage[4], voiceMessage[5]);
		foreach (int i in buttonIndex)
		{
			buttonText[i].text = "";
			for(int aa = 0; aa < chemicalTable.Length; aa++)
			{
				for(int bb = 0; bb < chemicalTable[aa].Length; bb++)
				{
					if(chemicalTable[aa][bb].Equals(chemicals[i]))
					{
						buttonText[i].text = (aa + 1) + "" + (bb + 1);
						break;
					}
				}
				if (buttonText[i].text.Length > 0)
					break;
			}
			hexButtons[i].OnInteract = delegate { pressedBrown(i); return false; };
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} Text: {2}", moduleId, positions[i], buttonText[i].text);
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} Chemical: {2}", moduleId, positions[i], chemicals[i]);
		}
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Possible Solution: {1} {2} {3} {4} {5} {6}", moduleId, positions[possSolution[0]], positions[possSolution[1]], positions[possSolution[2]], positions[possSolution[3]], positions[possSolution[4]], positions[possSolution[5]]);
		hexButtons[6].OnInteract = delegate { pressedBrownCenter(); return false; };
		numButtonPresses = 0;
	}
	void pressedBrown(int n)
	{
		if (!(moduleSolved))
		{
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User pressed {1} which is {2}", moduleId, positions[n], chemicals[n]);
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			ledMesh[n].material = ledColors[1];
			hexButtons[n].OnInteract = null;
			numButtonPresses++;
			submission[numButtonPresses] = getResult(submission[numButtonPresses - 1], chemicals[n]);
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Potion is now {1}", moduleId, submission[numButtonPresses]);
			if (numButtonPresses == 6)
			{
				bool[] flag = { false, false, false, false, false, false };
				for(int aa = 0; aa < 6; aa++)
				{
					for(int bb = 1; bb < 7; bb++)
					{
						if(potions[aa].Equals(submission[bb]) && !(flag[aa]))
						{
							flag[aa] = true;
							break;
						}
					}
				}
				if(flag[0] && flag[1] && flag[2] && flag[3] && flag[4] && flag[5])
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
	string getResult(string p, string c)
	{
		int num = "KBGCRMYW".IndexOf(p[0]);
		string[] RGB = { "", "" };
		for(int aa = 0; aa < 3; aa++)
		{
			RGB[0] = (num % 2) + "" + RGB[0];
			num = num / 2;
			if (c[aa * 2] == 'B')
				RGB[1] = RGB[1] + "1";
			else
				RGB[1] = RGB[1] + "0";
		}
		string result = "";
		for(int aa = 0; aa < 3; aa++)
		{
			if (RGB[0][aa] == RGB[1][aa])
				result = result + "0";
			else
				result = result + "1";
		}
		string charges = p[1] + "" + c[1] + "" + c[3] + "" + c[5];
		int charge = 0;
		for(int aa = 0; aa < 4; aa++)
		{
			if (charges[aa] == '+')
				charge++;
			else if (charges[aa] == '-')
				charge--;
		}
		switch(result)
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
	void pressedBrownCenter()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			coloredHexabuttons.StartCoroutine(playAudio());
			resetInput();
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
		hexButtons[6].OnInteract = delegate { pressedBrownCenter(); return false; };
	}
	void resetInput()
	{
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Resetting Potion", moduleId);
		numButtonPresses = 0;
		foreach (int i in buttonIndex)
		{
			Vector3 pos = buttonMesh[i].transform.localPosition;
			pos.y = 0.0169f;
			buttonMesh[i].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			ledMesh[i].material = ledColors[0];
			hexButtons[i].OnInteract = delegate { pressedBrown(i); return false; };
		}
	}
}
