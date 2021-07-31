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
	
	private int[] blackLights;
	private bool flag;
	private int[] pitchIndex;
	private int[] brightIndex;
	private int[] submission;
	private int[] solution;
	private int numButtonPresses;
	private bool moduleSolved;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private string[] pitchNames = { "LMH", "LHM", "MLH", "MHL", "HLM", "HML" };
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
		string alpha = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		string brights = "012345";
		blackLights = new int[6];
		brightIndex = new int[6];
		pitchIndex = new int[6];
		int[] read = new int[6];
		int[][] table =
		{
			new int[]{35, 33, 25, 21, 7, 1},
			new int[]{34, 26, 22, 8, 2, 12},
			new int[]{27, 23, 9, 3, 13, 17},
			new int[]{24, 10, 4, 14, 18, 28},
			new int[]{11, 5, 15, 19, 29, 31},
			new int[]{6, 16, 20, 30, 32, 36}
		};
		foreach (int i in buttonIndex)
		{
			blackLights[i] = UnityEngine.Random.Range(0, 6);
			pitchIndex[i] = UnityEngine.Random.Range(0, 6);
			hexButtons[i].OnInteract = delegate { pressedBlack(i); return false; };
			hexButtons[i].OnHighlight = delegate { ledMesh[blackLights[i]].material = ledColors[2]; };
			hexButtons[i].OnHighlightEnded = delegate { ledMesh[blackLights[i]].material = ledColors[0]; };
			brightIndex[i] = brights[UnityEngine.Random.Range(0, brights.Length)] - '0';
			brights = brights.Replace(brightIndex[i] + "", "");
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} button's brightness level: {2}", moduleId, positions[i], brightIndex[i]);
			read[i] = table[i][brightIndex[i]];
			Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} button's priority level: {2}", moduleId, positions[i], read[i]);
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
		numButtonPresses = 0;
		hexButtons[6].OnInteract = delegate { pressedBlackCenter(); return false; };
		hexButtons[6].OnInteractEnded = delegate { releasedBlackCenter(); };
		lights[6].color = Color.white;
		lights[6].intensity = 30;
		lights[6].range = 0.020f;
		string code = "";
		for(int aa = 36; aa > 0; aa--)
		{
			for(int bb = 0; bb < 6; bb++)
			{
				if(read[bb] == aa)
				{
					code = code + "" + alpha[pitchIndex[bb] + (blackLights[bb] * 6)];
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} button's lit up button: {2}", moduleId, positions[bb], positions[blackLights[bb]]);
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} button's pitches: {2}", moduleId, positions[bb], pitchNames[pitchIndex[bb]]);
					Debug.LogFormat("[Colored Hexabuttons 2 #{0}] {1} button's character: {2}", moduleId, positions[bb], code[code.Length - 1]);
					break;
				}
			}
		}
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Code: {1}", moduleId, code);
		string[][] table2 =
		{
					new string[]{"QB", "PX", "B9", "AJ", "J4", "SZ", "1T", "HK", "2A", "FQ", "UM", "9D", "75", "CN", "ZW", "RY", "W0", "LO", "K6", "8E", "GG", "O7", "VP", "N2", "5I", "EH", "XV", "6U", "MC", "I3", "Y1", "D8", "0R", "4F", "3S", "TL"},
					new string[]{"R3", "4V", "ME", "VX", "HM", "JA", "T8", "FT", "EL", "WU", "OB", "KW", "L5", "PK", "D4", "8Y", "Z0", "U6", "02", "7C", "YS", "57", "XD", "6R", "21", "3J", "CF", "SH", "GI", "AN", "IZ", "QG", "NQ", "BP", "1O", "99"},
					new string[]{"SM", "DB", "8Q", "OC", "CY", "9A", "PJ", "K7", "26", "6V", "ZS", "H2", "3O", "1I", "0P", "G0", "LD", "XL", "YG", "4W", "QT", "NK", "EE", "RR", "M8", "7Z", "FF", "VN", "J5", "AU", "TH", "IX", "W9", "U1", "B3", "54"},
					new string[]{"AF", "F8", "G2", "OZ", "SV", "50", "LM", "2B", "6Y", "9E", "7Q", "ZN", "RJ", "IU", "KA", "X1", "H4", "3I", "TS", "MG", "17", "VC", "BD", "DR", "NX", "UT", "4L", "PP", "C9", "W6", "Q5", "YW", "83", "JK", "0H", "EO"},
					new string[]{"8O", "6C", "A9", "UA", "WQ", "R0", "PL", "2E", "FG", "3X", "4J", "1H", "9F", "DD", "NY", "ZM", "X4", "L2", "VI", "J8", "BT", "CK", "IS", "01", "MW", "HR", "KV", "TP", "77", "GZ", "Y3", "ON", "QU", "56", "SB", "E5"},
					new string[]{"3V", "7O", "93", "DA", "JQ", "RD", "F7", "YY", "S0", "QH", "0M", "LN", "6I", "AC", "G8", "ZE", "5S", "TR", "C2", "NB", "KL", "E4", "VW", "B6", "HF", "IP", "MU", "15", "P1", "8Z", "OG", "UJ", "2T", "WX", "4K", "X9"},
					new string[]{"7V", "NN", "RS", "PO", "O0", "MB", "8F", "W5", "IT", "K2", "Z9", "YJ", "LL", "TA", "U4", "2R", "DP", "E1", "XZ", "GU", "03", "S6", "67", "18", "4H", "3G", "JI", "CD", "5M", "9C", "VK", "AX", "BW", "QE", "FY", "HQ"},
					new string[]{"QN", "AY", "OM", "FS", "MT", "N9", "G6", "HG", "R1", "VA", "YP", "CJ", "3L", "Z8", "5K", "SF", "EC", "KD", "LH", "UR", "TO", "2I", "90", "7X", "05", "12", "4Z", "IV", "DE", "W3", "8U", "JW", "P4", "6B", "B7", "XQ"},
					new string[]{"5O", "N3", "2Z", "RL", "X6", "UG", "HW", "GK", "D2", "BI", "WT", "ID", "F5", "8V", "C4", "YA", "MH", "S7", "4X", "E8", "3B", "7E", "QR", "VY", "OS", "PU", "0N", "AM", "ZQ", "9P", "J1", "T0", "1C", "LJ", "K9", "6F"},
					new string[]{"DW", "IM", "EQ", "N0", "XC", "W4", "VO", "UL", "8B", "ZY", "FH", "22", "47", "KK", "T6", "Q3", "91", "MJ", "0I", "5X", "HZ", "R8", "JS", "7N", "OE", "1V", "BR", "LA", "AP", "CG", "69", "G5", "YF", "3D", "PT", "SU"},
					new string[]{"S8", "TG", "5H", "7P", "66", "1U", "FR", "P9", "BS", "GD", "8K", "O2", "V4", "RA", "IO", "4Q", "DM", "WJ", "ZC", "9I", "AB", "LW", "EF", "JV", "U3", "NE", "Q1", "X7", "CX", "25", "K0", "MY", "HT", "YN", "0L", "3Z"},
					new string[]{"72", "KE", "AH", "0Y", "H0", "SS", "BM", "2N", "QZ", "1F", "3K", "XJ", "9V", "R7", "O9", "LT", "6Q", "CI", "ZD", "5P", "81", "IB", "UX", "PW", "FO", "4C", "ER", "DG", "G4", "NA", "V8", "WL", "T3", "YU", "J6", "M5"},
					new string[]{"1N", "0U", "98", "6P", "FX", "JE", "K4", "PS", "SJ", "E6", "OY", "GB", "NT", "4O", "X5", "80", "ML", "ZG", "D3", "YZ", "B2", "IF", "CQ", "2K", "H9", "5R", "3W", "RI", "U7", "L1", "WM", "QD", "AA", "VV", "TC", "7H"},
					new string[]{"D9", "RK", "1Y", "YC", "S2", "O1", "HS", "7W", "JD", "85", "L6", "ZZ", "40", "UE", "BU", "N7", "VR", "PA", "2F", "CB", "EG", "Q8", "6J", "34", "TQ", "GV", "IH", "53", "9N", "XT", "FL", "0X", "AI", "WO", "KP", "MM"},
					new string[]{"RW", "JZ", "0F", "V6", "OO", "YQ", "49", "8H", "51", "GR", "B5", "NU", "WB", "6X", "TM", "DN", "P2", "10", "XE", "C7", "A3", "3C", "FA", "HD", "94", "QI", "K8", "UY", "MP", "LK", "ET", "2S", "IL", "SG", "7J", "ZV"},
					new string[]{"42", "A6", "IE", "M9", "9K", "2M", "YD", "84", "0Z", "3P", "NR", "DY", "ES", "HV", "CO", "LU", "G7", "TB", "JG", "FW", "OX", "QL", "70", "VH", "B8", "R5", "K3", "W1", "PF", "ZI", "XA", "1J", "5Q", "6T", "SN", "UC"},
					new string[]{"7K", "OT", "6S", "W8", "1R", "9W", "C1", "MA", "I5", "DV", "SI", "QY", "H7", "LC", "X0", "RG", "5E", "VQ", "JX", "8N", "PZ", "E2", "N6", "UD", "GP", "F9", "AL", "0J", "23", "4M", "BO", "KU", "YB", "T4", "3F", "ZH"},
					new string[]{"BB", "1Q", "DJ", "45", "LP", "PY", "0A", "ST", "OW", "R9", "HU", "QX", "X3", "CZ", "6L", "UO", "97", "28", "30", "GS", "WN", "TI", "N1", "MK", "ED", "ZR", "5G", "JC", "FV", "74", "I6", "8M", "YE", "A2", "VF", "KH"},
					new string[]{"S3", "6G", "QC", "82", "EB", "19", "HE", "TF", "B0", "2D", "AV", "UQ", "FP", "96", "7U", "YM", "3A", "DT", "L7", "IY", "CS", "55", "RZ", "XW", "MO", "WI", "G1", "JH", "OK", "04", "PN", "VL", "ZX", "KJ", "4R", "N8"},
					new string[]{"HP", "FD", "NV", "1Z", "ZO", "PI", "0K", "87", "XM", "IW", "V0", "A4", "D6", "33", "QS", "BE", "TU", "SL", "M1", "29", "WC", "YR", "5Y", "9G", "LX", "7F", "RQ", "J2", "KT", "U8", "65", "CH", "GN", "OJ", "EA", "4B"},
					new string[]{"24", "CV", "PC", "AG", "VB", "MZ", "6D", "08", "TY", "H5", "9X", "3M", "5F", "76", "DU", "4P", "Z1", "BA", "GL", "1E", "U2", "WW", "S9", "I0", "8T", "NS", "XH", "FJ", "E3", "RO", "KN", "J7", "LQ", "QK", "OR", "YI"},
					new string[]{"P8", "43", "7R", "3E", "QF", "EW", "6A", "JJ", "KM", "SC", "RB", "A0", "5D", "NP", "OH", "T2", "V5", "2V", "9O", "14", "YT", "U9", "ZU", "C6", "FK", "MQ", "WZ", "HL", "B1", "XY", "07", "IN", "LG", "8I", "GX", "DS"},
					new string[]{"V2", "8J", "71", "DH", "FB", "CL", "AT", "BK", "HO", "Z6", "WD", "9R", "YV", "GW", "13", "UZ", "2X", "OI", "T5", "LY", "6E", "J0", "M7", "RC", "XS", "SP", "PM", "Q9", "N4", "EU", "4N", "5A", "0G", "38", "IQ", "KF"},
					new string[]{"AK", "MI", "Q6", "XP", "GE", "5W", "8S", "IA", "JN", "ZB", "1D", "2L", "CR", "SQ", "41", "9J", "HX", "NO", "DZ", "WH", "OV", "F2", "78", "KG", "P5", "63", "U0", "09", "TT", "BC", "LF", "EY", "V7", "RM", "Y4", "3U"},
					new string[]{"2C", "7D", "0O", "QQ", "MN", "4U", "F6", "3H", "9S", "61", "H8", "WV", "8A", "Y7", "I2", "NI", "TE", "SY", "UW", "EJ", "R4", "CT", "L3", "G9", "VZ", "KX", "O5", "ZK", "D0", "5L", "BG", "JP", "1M", "AR", "XF", "PB"},
					new string[]{"ZL", "CU", "48", "0E", "GC", "DF", "8D", "O4", "SK", "XG", "I7", "E9", "UP", "FI", "H1", "35", "AW", "5B", "6N", "RV", "2Q", "BJ", "W2", "QA", "NH", "M6", "V3", "L0", "1S", "KY", "JT", "9Z", "TX", "PR", "7M", "YO"},
					new string[]{"PQ", "Q4", "ND", "3R", "1W", "LI", "YK", "WF", "X8", "CM", "EV", "RE", "M2", "J3", "UB", "I1", "00", "5Z", "9T", "OU", "VG", "46", "HY", "79", "SX", "FN", "2P", "BH", "DL", "K5", "6O", "ZA", "AS", "T7", "8C", "GJ"},
					new string[]{"FU", "GF", "TK", "C8", "NJ", "KO", "Z7", "VE", "WP", "QM", "95", "0T", "UV", "MD", "5N", "HA", "1B", "YL", "S4", "A1", "LR", "X2", "IC", "BZ", "7G", "89", "RH", "OQ", "EX", "4S", "DI", "P3", "JY", "2W", "60", "36"},
					new string[]{"6H", "0S", "P7", "HN", "3Y", "7T", "KZ", "XB", "MX", "L4", "QV", "NM", "SW", "IR", "WE", "F1", "BF", "9U", "AD", "GQ", "U5", "T9", "O8", "Y0", "EI", "JL", "1G", "CC", "VJ", "R6", "52", "2O", "4A", "Z3", "DK", "8P"},
					new string[]{"9L", "T1", "JM", "F3", "N5", "O6", "7B", "AE", "UK", "4D", "62", "E7", "QP", "Y8", "5V", "8G", "2U", "RF", "KI", "MR", "CW", "P0", "ZT", "3N", "IJ", "B4", "DQ", "LZ", "HH", "XX", "0C", "V9", "GY", "1A", "SO", "WS"},
					new string[]{"GT", "3Q", "HJ", "5C", "DO", "Q2", "RP", "Y9", "4E", "1L", "6K", "JR", "WG", "A5", "V1", "Z4", "KS", "FZ", "C0", "SA", "UU", "MV", "86", "O3", "9M", "NF", "27", "BX", "TD", "EN", "7Y", "XI", "PH", "I8", "LB", "0W"},
					new string[]{"3T", "Q0", "KC", "2J", "9H", "L8", "6W", "D1", "PV", "CE", "RU", "XO", "FM", "BN", "A7", "I4", "MS", "4G", "59", "Y5", "SR", "0Q", "JB", "WY", "Z2", "8X", "OP", "7I", "TZ", "NL", "EK", "H3", "16", "UF", "VD", "GA"},
					new string[]{"06", "39", "1X", "IK", "D7", "WR", "NG", "XU", "FC", "SE", "JF", "M0", "2H", "Z5", "7A", "VM", "88", "Y2", "US", "RT", "5J", "G3", "CP", "OL", "TN", "K1", "QW", "64", "AO", "4I", "9Q", "HB", "EZ", "BY", "LV", "PD"},
					new string[]{"J9", "8R", "CA", "0V", "PE", "68", "S1", "Q7", "HC", "GO", "II", "1P", "ZF", "BL", "2G", "D5", "NW", "VU", "32", "OD", "F0", "KQ", "UH", "TJ", "Y6", "9B", "EM", "5T", "XN", "AZ", "LS", "4Y", "M4", "73", "WK", "RX"},
					new string[]{"OF", "7L", "C5", "E0", "UI", "0B", "5U", "SD", "GH", "92", "M3", "DC", "2Y", "VS", "TV", "XK", "H6", "JO", "WA", "QJ", "37", "RN", "LE", "I9", "F4", "A8", "8W", "BQ", "6M", "11", "ZP", "4T", "KR", "NZ", "YX", "PG"},
					new string[]{"IG", "P6", "44", "JU", "0D", "31", "9Y", "NC", "8L", "DX", "58", "S5", "ZJ", "20", "MF", "7S", "FE", "UN", "OA", "QO", "YH", "VT", "R2", "C3", "6Z", "TW", "GM", "EP", "L9", "W7", "XR", "BV", "HI", "KB", "AQ", "1K"}
			};
		solution = new int[12];
		submission = new int[12];
		for(int aa = 0; aa < 3; aa++)
		{
			string pair = code[aa * 2] + "" + code[(aa * 2) + 1];
			bool found = false;
			for(int bb = 0; bb < table2.Length; bb++)
			{
				for(int cc = 0; cc < table2[bb].Length; cc++)
				{
					if(table2[bb][cc].Equals(pair))
					{
						solution[aa * 4] = (bb / 6) + 1;
						solution[(aa * 4) + 1] = (bb % 6) + 1;
						solution[(aa * 4) + 2] = (cc / 6) + 1;
						solution[(aa * 4) + 3] = (cc % 6) + 1;
						found = true;
						break;
					}
				}
				if (found)
					break;
			}
		}
		Debug.LogFormat("[Colored Hexabuttons 2 #{0}] Solution: {1}{2}{3}{4} {5}{6}{7}{8} {9}{10}{11}{12}", moduleId, solution[0], solution[1], solution[2], solution[3], solution[4], solution[5], solution[6], solution[7], solution[8], solution[9], solution[10], solution[11]);
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
			submission[numButtonPresses] = p + 1;
			if (numButtonPresses % 2 == 0)
				ledMesh[numButtonPresses / 2].material = ledColors[3];
			else
				ledMesh[numButtonPresses / 2].material = ledColors[1];
			numButtonPresses++;
			if(numButtonPresses == 12)
			{
				Debug.LogFormat("[Colored Hexabuttons 2 #{0}] User Submission: {1}{2}{3}{4} {5}{6}{7}{8} {9}{10}{11}{12}", moduleId, submission[0], submission[1], submission[2], submission[3], submission[4], submission[5], submission[6], submission[7], submission[8], submission[9], submission[10], submission[11]);
				flag = false;
				bool correct = true;
				for(int aa = 0; aa < 12; aa++)
				{
					if(solution[aa] != submission[aa])
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
		Audio.PlaySoundAtTransform(pitches[pitchIndex[p]].name, transform);
		yield return new WaitForSeconds(2.0f);
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
			hexButtons[i].OnHighlight = delegate { ledMesh[blackLights[i]].material = ledColors[2]; };
			hexButtons[i].OnHighlightEnded = delegate { ledMesh[blackLights[i]].material = ledColors[0]; };
			ledMesh[i].material = ledColors[0];
		}
		numButtonPresses = 0;
	}
}
