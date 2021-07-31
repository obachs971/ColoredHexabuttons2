using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteHexabuttons {
	private ColorfulButtonSeries coloredHexabuttons;
	private KMAudio Audio;
	private int moduleId;
	private KMSelectable[] hexButtons;
	private MeshRenderer[] buttonMesh;
	private Material[] buttonColors;
	private MeshRenderer[] ledMesh;
	private Transform transform;
	private string solution;
	private bool moduleSolved;
	private string colors;
	private string centerColor;
	private string colorState;
	private string colorFlash;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	public WhiteHexabuttons(ColorfulButtonSeries m, KMAudio aud, int MI, KMSelectable[] HB, MeshRenderer[] BM, Material[] BC, MeshRenderer[] LM, Transform T)
	{
		coloredHexabuttons = m;
		Audio = aud;
		moduleId = MI;
		hexButtons = HB;
		buttonMesh = BM;
		buttonColors = BC;
		ledMesh = LM;
		transform = T;
	}
	public void run()
	{
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Generated Color: White", moduleId);
		colors = ""; string temp = "ROYGBP";
		for (int aa = 0; aa < 6; aa++)
		{
			colors = colors + "" + temp[UnityEngine.Random.Range(0, temp.Length)];
			temp = temp.Replace(colors[aa] + "", "");
		}
		centerColor = "ROYGBP"[UnityEngine.Random.Range(0, 6)] + "";
		colorState = colors.Replace(centerColor, "-");
		generateSolution();
		colorFlash = solution.Replace("-", centerColor);
		foreach(int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedWhite(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { pressedWhiteRelease(i); };
			ledMesh[i].transform.localScale = new Vector3Int(0, 0, 0);
		}
		hexButtons[6].OnHighlight = delegate { buttonMesh[6].material = buttonColors["ROYGBP".IndexOf(centerColor)]; };
		hexButtons[6].OnHighlightEnded = delegate { buttonMesh[6].material = buttonColors[6]; };
		hexButtons[6].OnInteract = delegate { pressedWhiteCenter(); return false; };
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Absent Color: {1}", moduleId, centerColor);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Beginning State: {1}", moduleId, colorState);
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Finished State: {1}", moduleId, solution);
	}
	void generateSolution()
	{
		ArrayList cursors = new ArrayList();
		ArrayList prev = new ArrayList();
		cursors.Add(colorState.ToUpperInvariant());
		prev.Add(colorState.ToUpperInvariant());
		for(int aa = 0; aa < 6; aa++)
		{
			for(int bb = 0; bb < cursors.Count; bb++)
			{
				ArrayList poss = getAllMoves((string)cursors[bb]);
				for(int cc = 0; cc < poss.Count; cc++)
				{
					if(prev.Contains((string)poss[cc]))
					{
						poss.RemoveAt(cc);
						cc--;
					}
				}
				if(poss.Count > 0)
				{
					cursors[bb] = poss[0];
					prev.Add(poss[0]);
					for(int cc = 1; cc < poss.Count; cc++)
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
		switch(cur.IndexOf("-"))
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
	void pressedWhiteCenter()
	{
		if (!(moduleSolved))
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
			coloredHexabuttons.StartCoroutine(whiteFlasher());
		}
	}
	void pressedWhite(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[n].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	void pressedWhiteRelease(int n)
	{
		if (!(moduleSolved))
		{
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User pressed {1} which is the color {2}", moduleId, positions[n], colors[n]);
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
					else if(blank == 2)
						colorState = colorState[2] + "" + colorState[1] + "" + colorState[0] + "" + colorState.Substring(3);
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color state is now {1}", moduleId, colorState);
					break;
				case 1:
					if (blank == 0)
						colorState = colorState[1] + "" + colorState[0] + "" + colorState.Substring(2);
					else if (blank == 3)
						colorState = colorState[0] + "" + colorState[3] + "" + colorState[2] + "" + colorState[1] + "" + colorState.Substring(4);
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color state is now {1}", moduleId, colorState);
					break;
				case 2:
					if (blank == 0)
						colorState = colorState[2] + "" + colorState[1] + "" + colorState[0] + "" + colorState.Substring(3);
					else if (blank == 3)
						colorState = colorState.Substring(0, 2) + "" + colorState[3] + "" + colorState[2] + "" + colorState.Substring(4);
					else if (blank == 4)
						colorState = colorState.Substring(0, 2) + "" + colorState[4] + "" + colorState[3] + "" + colorState[2] + "" + colorState[5];
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color state is now {1}", moduleId, colorState);
					break;
				case 3:
					if (blank == 1)
						colorState = colorState[0] + "" + colorState[3] + "" + colorState[2] + "" + colorState[1] + "" + colorState.Substring(4);
					else if (blank == 2)
						colorState = colorState.Substring(0, 2) + "" + colorState[3] + "" + colorState[2] + "" + colorState.Substring(4);
					else if (blank == 5)
						colorState = colorState.Substring(0, 3) + "" + colorState[5] + "" + colorState[4] + "" + colorState[3];
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color state is now {1}", moduleId, colorState);
					break;
				case 4:
					if (blank == 2)
						colorState = colorState.Substring(0, 2) + "" + colorState[4] + "" + colorState[3] + "" + colorState[2] + "" + colorState[5];
					else if (blank == 5)
						colorState = colorState.Substring(0, 4) + "" + colorState[5] + "" + colorState[4];
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color state is now {1}", moduleId, colorState);
					break;
				case 5:
					if (blank == 3)
						colorState = colorState.Substring(0, 3) + "" + colorState[5] + "" + colorState[4] + "" + colorState[3];
					else if (blank == 4)
						colorState = colorState.Substring(0, 4) + "" + colorState[5] + "" + colorState[4];
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Color state is now {1}", moduleId, colorState);
					break;
				default:
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User submitted {1}", moduleId, colorState);
					if (solution.Equals(colorState))
					{
						moduleSolved = true;
						coloredHexabuttons.Solve();
					}
					else
					{
						colorState = colors.Replace(centerColor, "-");
						coloredHexabuttons.Strike();
					}
					break;
			}
		}
	}
	IEnumerator whiteFlasher()
	{
		yield return new WaitForSeconds(1.0f);
		for (int aa = 0; aa < 6; aa++)
		{
			int index = colors.IndexOf(colorFlash[aa]);
			buttonMesh[index].material = buttonColors["ROYGBP".IndexOf(colors[index])];
			yield return new WaitForSeconds(0.5f);
			buttonMesh[index].material = buttonColors[6];
			yield return new WaitForSeconds(0.5f);
		}
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		hexButtons[6].OnHighlight = delegate { buttonMesh[6].material = buttonColors["ROYGBP".IndexOf(centerColor)]; };
		hexButtons[6].OnHighlightEnded = delegate { buttonMesh[6].material = buttonColors[6]; };
		hexButtons[6].OnInteract = delegate { pressedWhiteCenter(); return false; };
		buttonMesh[6].material = buttonColors[6];
		foreach (int i in buttonIndex)
		{
			hexButtons[i].OnInteract = delegate { pressedWhite(i); return false; };
			hexButtons[i].OnInteractEnded = delegate { pressedWhiteRelease(i); };
		}
	}
}
