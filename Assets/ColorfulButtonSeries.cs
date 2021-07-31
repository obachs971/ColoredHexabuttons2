using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using KeepCoding;
public class ColorfulButtonSeries : MonoBehaviour
{
	public class ColoredHexabuttons2Settings
	{
		public bool red = true;
		public bool orange = true;
		public bool yellow = true;
		public bool green = true;
		public bool blue = true;
		public bool purple = true;
		public bool white = true;
		public bool gray = true;
		public bool black = true;
		public bool brown = true;
	}
	private static int[] missionPoss;
	private static bool first = true;
	private static int moduleIdCounter = 1;
	private int moduleId;

	private bool moduleSolved;
	public KMBombModule module;
	public KMAudio Audio;
	public KMBombInfo bomb;

	public Material[] buttonColors;
	public MeshRenderer[] buttonMesh;
	public KMSelectable[] hexButtons;
	public TextMesh[] buttonText;
	public MeshFilter[] buttonMF;
	public MeshFilter[] highlightMF;
	public Transform[] highlightTF;
	public Material[] ledColors;
	public Material[] brightness;
	public MeshRenderer[] ledMesh;
	public Light[] flashLights;
	public AudioClip[] alphabet;
	public AudioClip[] numbers;
	public AudioClip[] ciphers;
	public MeshFilter[] shapes;
	public AudioClip[] notes;
	public AudioClip[] morseSounds;
	public AudioClip[] pitches;
	private string TPOrder;
	private int TPScore;
	private bool TPSwitch;
	private int colorIndex;
	
	void Awake()
	{
		//All of this needs to stay in the Awake Method.
		if(first)
		{
			first = false;
			missionPoss = MissionSettings();
		}
		TPOrder = "0123456";
		TPSwitch = false;
		moduleId = moduleIdCounter++;
		moduleSolved = false;
		string colorChoices;
		
		if (missionPoss != null && check(missionPoss))
		{
			colorChoices = "";
			for (int aa = 0; aa < missionPoss.Length; aa++)
			{
				for (int bb = 0; bb < missionPoss[aa]; bb++)
					colorChoices = colorChoices + "" + aa;
			}
			//colorChoices.Call();
			colorIndex = colorChoices[UnityEngine.Random.Range(0, colorChoices.Length)] - '0';
			--missionPoss[colorIndex];
		}
		else
		{
			ModConfig<ColoredHexabuttons2Settings> modConfig = new ModConfig<ColoredHexabuttons2Settings>("ColoredHexabuttons2Settings");
			colorChoices = FindColors(modConfig);
			colorIndex = colorChoices[UnityEngine.Random.Range(0, colorChoices.Length)] - '0';
		}
		//colorIndex = 5;
		switch (colorIndex)
		{
			case 0:
				TPScore = 6;
				RedHexabuttons red = new RedHexabuttons(this, Audio, moduleId, hexButtons, buttonMesh, ledColors, ledMesh, transform);
				red.run();
				break;
			case 1:
				TPScore = 10;
				OrangeHexabuttons orange = new OrangeHexabuttons(this, Audio, moduleId, hexButtons, buttonMesh, buttonText, ledColors, ledMesh, transform);
				orange.run();
				break;
			case 2:
				TPScore = 12;
				YellowHexabuttons yellow = new YellowHexabuttons(this, Audio, moduleId, hexButtons, buttonMesh, buttonMF, highlightMF, highlightTF, ledMesh, transform, shapes);
				yellow.run();
				break;
			case 3:
				TPScore = 12;
				GreenHexabuttons green = new GreenHexabuttons(this, Audio, notes, moduleId, hexButtons, buttonMesh, buttonText, ledColors, ledMesh, flashLights, transform);
				green.run();
				break;
			case 4:
				TPScore = 28;
				BlueHexabuttons blue = new BlueHexabuttons(this, Audio, moduleId, hexButtons, buttonMesh, buttonText, ledColors, ledMesh, transform);
				blue.run();
				break;
			case 5:
				TPScore = 16;
				PurpleHexabuttons purple = new PurpleHexabuttons(this, Audio, moduleId, hexButtons, buttonMesh, buttonText, ledColors, ledMesh, transform);
				purple.run();
				break;
			case 6:
				TPScore = 12;
				WhiteHexabuttons white = new WhiteHexabuttons(this, Audio, moduleId, hexButtons, buttonMesh, buttonColors, ledMesh, transform);
				white.run();
				break;
			case 7:
				TPScore = 12;
				GrayHexabuttons gray = new GrayHexabuttons(this, Audio, moduleId, hexButtons, buttonMesh, buttonText, ledColors, ledMesh, flashLights, transform);
				gray.run();
				break;
			case 8:
				TPScore = 10;
				BlackHexabuttons black = new BlackHexabuttons(this, Audio, pitches, moduleId, hexButtons, buttonMesh, buttonText, ledColors, ledMesh, brightness, flashLights, transform, buttonColors[8]);
				black.run();
				break;
			case 9:
				TPScore = 18;
				BrownHexabuttons brown = new BrownHexabuttons(this, Audio, moduleId, hexButtons, buttonMesh, buttonText, ledColors, ledMesh, transform);
				brown.run();
				break;
		}
	}
	void Start()
	{
		first = true;
		float scalar = transform.lossyScale.x;
		for (int aa = 0; aa < 7; aa++)
		{
			buttonMesh[aa].material = buttonColors[colorIndex];
			flashLights[aa].enabled = false;
			flashLights[aa].range *= scalar;
		}
	}
	string FindColors(ModConfig<ColoredHexabuttons2Settings> modConfig)
	{
		var settings = modConfig.Read();
		if (settings != null)
		{
			string colors = "";
			if (settings.red)
				colors = colors + "0";
			if (settings.orange)
				colors = colors + "1";
			if (settings.yellow)
				colors = colors + "2";
			if (settings.green)
				colors = colors + "3";
			if (settings.blue)
				colors = colors + "4";
			if (settings.purple)
				colors = colors + "5";
			if (settings.white)
				colors = colors + "6";
			if (settings.gray)
				colors = colors + "7";
			if (settings.black)
				colors = colors + "8";
			if (settings.brown)
				colors = colors + "9";
			if (colors.Length == 0)
				return "0123456789";
			else
				return colors;
		}
		else return "0123456789";
	}
	static int[] MissionSettings()
	{
		string description = Application.isEditor ? null : Game.Mission.Description;

		if (description == null)
			return null;
		
		Regex regex = new Regex(@"\[Colored Hexabuttons 2\] (\d+,){9}\d+");

		var match = regex.Match(description);
		
		if (!match.Success)
			return null;

		return match.Value.Replace("[Colored Hexabuttons 2] ", "").Split(',').ToNumbers(min: 0, max: 255, minLength: 10, maxLength: 10);
	}
	private bool check(int[] values)
	{
		for(int aa = 0; aa < values.Length; aa++)
		{
			if (values[aa] > 0)
				return true;
		}
		return false;
	}
	public void Solve()
	{
		moduleSolved = true;
		if (!(TPSwitch))
			module.HandlePass();
	}
	public void Strike()
	{
		module.HandleStrike();
	}
	public void setOrder(string o)
	{
		TPOrder = o.ToUpperInvariant();
	}
	private bool isPos(string[] param)
	{
		for(int aa = 1; aa < param.Length; aa++)
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
#pragma warning disable 414
	private readonly string TwitchHelpMessage = @"!{0} press|p tl/1 tr/2 ml/3 mr/4 bl/5 br/6 c/7 presses the top-left, top-right, middle-left, middle-right, bottom-left, bottom-right and center buttons in that order. !{0} hover|h tl/1 tr/2 ml/3 mr/4 bl/5 br/6 c/7 will hover the buttons in the same fashion.";
#pragma warning restore 414
	IEnumerator ProcessTwitchCommand(string command)
	{
		string[] param = command.ToUpper().Split(' ');
		if ((Regex.IsMatch(param[0], @"^\s*HOVER\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(param[0], @"^\s*H\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) && param.Length > 1)
		{
			if (isPos(param))
			{
				TPSwitch = true;
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
					if (hexButtons[TPOrder[cursor] - '0'].OnHighlight != null)
					{
						hexButtons[TPOrder[cursor] - '0'].OnHighlight();
						yield return new WaitForSeconds(0.5f);
						hexButtons[TPOrder[cursor] - '0'].OnHighlightEnded();
						yield return new WaitForSeconds(0.5f);
					}
				}
				TPSwitch = false;
			}
			else
				yield return "sendtochat An error occured because the user inputted something wrong.";
		}
		else if ((Regex.IsMatch(param[0], @"^\s*PRESS\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(param[0], @"^\s*P\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) && param.Length > 1)
		{
			if (isPos(param))
			{
				TPSwitch = true;
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
					if (hexButtons[TPOrder[cursor] - '0'].OnInteract != null)
					{
						hexButtons[TPOrder[cursor] - '0'].OnInteract();
						yield return new WaitForSeconds(0.2f);
						if (hexButtons[TPOrder[cursor] - '0'].OnInteractEnded != null)
						{
							hexButtons[TPOrder[cursor] - '0'].OnInteractEnded();
							yield return new WaitForSeconds(0.5f);
						}
					}
				}
				TPSwitch = false;
				if (moduleSolved)
				{
					yield return "awardpointsonsolve " + TPScore;
					Solve();
				}
			}
			else
				yield return "sendtochat An error occured because the user inputted something wrong.";
		}
		else
			yield return "sendtochat An error occured because the user inputted something wrong.";
	}
}
