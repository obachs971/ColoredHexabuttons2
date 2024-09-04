using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class transmittingHexabuttons : MonoBehaviour
{
    public KMBombModule module;
    private static int moduleIdCounter = 1;
    private int moduleId;
    public KMAudio Audio;
    public AudioClip[] pitches;
    public KMSelectable[] hexButtons;
    public MeshRenderer[] buttonMesh;
    public Material resetColor;
    public Material[] ledColors;
    public MeshRenderer[] ledMesh;
    public Material[] brightness;
    public Light centerLight;
    public Transform[] whiteDots;

    private string[] blackLights;
    private bool flag;
    private int[][] pitchIndex;
    private int[] brightIndex;
    private string solution;
    private string[] input;
    private readonly string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
    private readonly int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
    private string[] flagSubmissions;
    private string submission;
    private int flagToggle;
    private string mode;
    private int type;
    private readonly string[][] flagChart =
    {
        new string[]{ "N", "NE", "E", "SE" },
        new string[]{ "S", "SW", "W", "NW" }
    };
    private static readonly int[][] priorityTable =
        {
            new int[]{35,33,25,21,7,1},
            new int[]{34,26,22,8,2,12},
            new int[]{27,23,9,3,13,17},
            new int[]{24,10,4,14,18,28},
            new int[]{11,5,15,19,29,31},
            new int[]{6,16,20,30,32,36}
        };
    private static readonly string[][][] pitchTable =
    {
            new string[][]
            {
                new string[]{ "01", "10", "11", "00" },
                new string[]{ "10", "11", "00", "01" },
                new string[]{ "11", "00", "01", "10" }
            },
            new string[][]
            {
                new string[]{ "11", "00", "01", "10" },
                new string[]{ "01", "10", "11", "00" },
                new string[]{ "10", "11", "00", "01" }
            },
            new string[][]
            {
                new string[]{ "10", "11", "00", "01" },
                new string[]{ "11", "00", "01", "10" },
                new string[]{ "01", "10", "11", "00" }
            }
        };
    private static readonly string[] braille =
    {
            "100000","110000","100100","100110","100010","110100",
            "110110","110010","010100","010110","101000","111000",
            "101100","101110","101010","111100","111110","111010",
            "011100","011110","101001","111001","010111","101101",
            "101111","101011","010000","011000","010010","010011",
            "010001","011010","011011","011001","001010","001011"
        };
    private static readonly float[] dotPosition = { 0.7f, 0.1f, -0.5f };
    private bool deafMode = false;
    void Awake()
    {
        moduleId = moduleIdCounter++;
        int[] priorityLevel = new int[6];
        brightIndex = new int[6];
        pitchIndex = new int[6][];
        string code = "";
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        string brightChoice = "012345";
        blackLights = new string[6];
        foreach (int i in buttonIndex)
        {
            brightIndex[i] = brightChoice[UnityEngine.Random.Range(0, brightChoice.Length)] - '0';
            brightChoice = brightChoice.Replace(brightIndex[i] + "", "");
            priorityLevel[i] = priorityTable[i][brightIndex[i]];
            pitchIndex[i] = new int[3];
            string pitch = new string("012".ToCharArray().Shuffle());
            for (int aa = 0; aa < 3; aa++)
                pitchIndex[i][aa] = (pitch[aa] - '0');
            code = code + "" + alpha[UnityEngine.Random.Range(0, alpha.Length)];
            string brailleBin = braille[alpha.IndexOf(code[i])];
            string temp = "";
            string[] col = { "00", "01", "10", "11" };
            for (int aa = 0; aa < 3; aa++)
                temp += col[Array.IndexOf(pitchTable[pitchIndex[i][aa]][aa], brailleBin.Substring(aa * 2, 2))];
            blackLights[i] = "";
            for (int aa = 0; aa < temp.Length; aa++)
            {
                if (temp[aa] == '1')
                    blackLights[i] = blackLights[i] + "" + (aa + 1);
            }

            hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
            hexButtons[i].OnInteractEnded = null;
            hexButtons[i].OnHighlight = delegate
            {
                foreach (char bl in blackLights[i])
                    ledMesh["142536".IndexOf(bl)].material = ledColors[2];
            };
            hexButtons[i].OnHighlightEnded = delegate
            {
                foreach (char bl in blackLights[i])
                    ledMesh["142536".IndexOf(bl)].material = ledColors[0];
            };
            Debug.LogFormat("[Transmitting Hexabuttons #{0}] {1} Brightness: {2}", moduleId, positions[i], brightIndex[i]);
            Debug.LogFormat("[Transmitting Hexabuttons #{0}] {1} Priority Level: {2}", moduleId, positions[i], priorityLevel[i]);
            Debug.LogFormat("[Transmitting Hexabuttons #{0}] {1} Character: {2}", moduleId, positions[i], code[i]);
            Debug.LogFormat("[Transmitting Hexabuttons #{0}] {1} Pitches: {2}{3}{4}", moduleId, positions[i], "LMH"[pitchIndex[i][0]], "LMH"[pitchIndex[i][1]], "LMH"[pitchIndex[i][2]]);
            Debug.LogFormat("[Transmitting Hexabuttons #{0}] {1} Braille: {2}", moduleId, positions[i], blackLights[i]);
        }
        hexButtons[6].OnHighlight = delegate
        {
            buttonMesh[0].material = brightness[brightIndex[0]];
            buttonMesh[1].material = brightness[brightIndex[1]];
            buttonMesh[2].material = brightness[brightIndex[2]];
            buttonMesh[3].material = brightness[brightIndex[3]];
            buttonMesh[4].material = brightness[brightIndex[4]];
            buttonMesh[5].material = brightness[brightIndex[5]];
            buttonMesh[6].material = brightness[0];
        };
        hexButtons[6].OnHighlightEnded = delegate
        {
            buttonMesh[0].material = resetColor;
            buttonMesh[1].material = resetColor;
            buttonMesh[2].material = resetColor;
            buttonMesh[3].material = resetColor;
            buttonMesh[4].material = resetColor;
            buttonMesh[5].material = resetColor;
            buttonMesh[6].material = resetColor;
        };
        hexButtons[6].OnInteract = delegate { pressedCenter(); return false; };
        hexButtons[6].OnInteractEnded = delegate { releasedCenter(); };
        hexButtons[7].OnInteract = delegate { deafMode = !(deafMode); return false; };
        solution = "";
        input = new string[6];
        for (int aa = 36; aa >= 0; aa--)
        {
            if (Array.IndexOf(priorityLevel, aa) >= 0)
            {
                int cur = Array.IndexOf(priorityLevel, aa);
                input[cur] = new string[] { "A", "B", "C", "D", "1", "2" }[solution.Length];
                solution = solution + "" + code[cur];
                Debug.LogFormat("[Transmitting Hexabuttons #{0}] {1} Type: {2}", moduleId, positions[cur], input[cur]);
            }
        }
        Debug.LogFormat("[Transmitting Hexabuttons #{0}] Solution: {1}", moduleId, solution);
        flagSubmissions = new string[2];
        submission = "";
        flagToggle = 0;
        mode = "NULL";
        type = 0;
        centerLight.color = Color.white;
    }
    void Start()
    {
        float scalar = transform.lossyScale.x;
        centerLight.enabled = false;
        centerLight.range *= scalar;
    }
    void pressedButton(int p)
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        Vector3 pos = buttonMesh[p].transform.localPosition;
        pos.y = 0.0126f;
        buttonMesh[p].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
        hexButtons[p].OnInteract = null;
        StartCoroutine(PlayAudio(p));
    }
    void submitButton(int p)
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        Vector3 pos = buttonMesh[p].transform.localPosition;
        pos.y = 0.0126f;
        buttonMesh[p].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
    }
    void submitButtonRelease(int p)
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
        Vector3 pos = buttonMesh[p].transform.localPosition;
        pos.y = 0.0169f;
        buttonMesh[p].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
        Debug.LogFormat("[Transmitting Hexabuttons #{0}] {1} was pressed which is {2}", moduleId, positions[p], input[p]);
        if ("12".IndexOf(input[p]) >= 0)
            type = "12".IndexOf(input[p]);
        else
        {
            flagSubmissions[flagToggle] = flagChart[type]["ABCD".IndexOf(input[p])];
            flagToggle++;
            if (flagToggle == 2)
            {
                Debug.LogFormat("[Transmitting Hexabuttons #{0}] Entered flag sequence: {1} {2}", moduleId, flagSubmissions[0], flagSubmissions[1]);
                flagToggle = 0;
                switch (mode)
                {
                    case "NULL":
                        switch (flagSubmissions[0] + " " + flagSubmissions[1])
                        {
                            case "N NE":
                                mode = "NUMBER";
                                break;
                            case "N E":
                                mode = "LETTER";
                                break;
                            default:
                                submission += "?";
                                break;
                        }
                        break;
                    case "NUMBER":
                        switch (flagSubmissions[0] + " " + flagSubmissions[1])
                        {
                            case "SW S":
                                submission += "1";
                                break;
                            case "W S":
                                submission += "2";
                                break;
                            case "NW S":
                                submission += "3";
                                break;
                            case "N S":
                                submission += "4";
                                break;
                            case "S NE":
                                submission += "5";
                                break;
                            case "S E":
                                submission += "6";
                                break;
                            case "S SE":
                                submission += "7";
                                break;
                            case "W SW":
                                submission += "8";
                                break;
                            case "SW NW":
                                submission += "9";
                                break;
                            case "SW N":
                                submission += "0";
                                break;
                            case "N E":
                                mode = "LETTER";
                                break;
                            default:
                                submission += "?";
                                break;
                        }
                        break;
                    default:
                        switch (flagSubmissions[0] + " " + flagSubmissions[1])
                        {
                            case "SW S":
                                submission += "A";
                                break;
                            case "W S":
                                submission += "B";
                                break;
                            case "NW S":
                                submission += "C";
                                break;
                            case "N S":
                                submission += "D";
                                break;
                            case "S NE":
                                submission += "E";
                                break;
                            case "S E":
                                submission += "F";
                                break;
                            case "S SE":
                                submission += "G";
                                break;
                            case "W SW":
                                submission += "H";
                                break;
                            case "SW NW":
                                submission += "I";
                                break;
                            case "N E":
                                submission += "J";
                                break;
                            case "SW N":
                                submission += "K";
                                break;
                            case "SW NE":
                                submission += "L";
                                break;
                            case "SW E":
                                submission += "M";
                                break;
                            case "SW SE":
                                submission += "N";
                                break;
                            case "W NW":
                                submission += "O";
                                break;
                            case "W N":
                                submission += "P";
                                break;
                            case "W NE":
                                submission += "Q";
                                break;
                            case "W E":
                                submission += "R";
                                break;
                            case "W SE":
                                submission += "S";
                                break;
                            case "NW N":
                                submission += "T";
                                break;
                            case "NW NE":
                                submission += "U";
                                break;
                            case "N SE":
                                submission += "V";
                                break;
                            case "NE E":
                                submission += "W";
                                break;
                            case "NE SE":
                                submission += "X";
                                break;
                            case "NW E":
                                submission += "Y";
                                break;
                            case "SE E":
                                submission += "Z";
                                break;
                            case "N NE":
                                mode = "NUMBER";
                                break;
                            default:
                                submission += "?";
                                break;
                        }
                        break;
                }
                if (submission.Length > 0)
                    ledMesh[submission.Length - 1].material = ledColors[1];
                Debug.LogFormat("[Transmitting Hexabuttons #{0}] Entered so far: {1}", moduleId, submission);
                if (submission.Length == 6)
                {
                    Debug.LogFormat("[Transmitting Hexabuttons #{0}] User Submission: {1}", moduleId, submission);
                    flag = false;
                    if (solution.Equals(submission))
                    {
                        foreach (int i in buttonIndex)
                        {
                            hexButtons[i].OnInteract = null;
                            hexButtons[i].OnInteractEnded = null;
                        }
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
        }
    }
    void pressedCenter()
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        Vector3 pos = buttonMesh[6].transform.localPosition;
        pos.y = 0.0126f;
        buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
    }
    void releasedCenter()
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
            StartCoroutine(blackFlasher());
            foreach (int i in buttonIndex)
            {
                hexButtons[i].OnInteract = delegate { submitButton(i); return false; };
                hexButtons[i].OnInteractEnded = delegate { submitButtonRelease(i); };
                hexButtons[i].OnHighlight = null;
                hexButtons[i].OnHighlightEnded = null;
                buttonMesh[i].material = resetColor;
                ledMesh[i].material = ledColors[0];
            }
            buttonMesh[6].material = resetColor;
        }
        flag = !(flag);
    }
    IEnumerator blackFlasher()
    {
        yield return new WaitForSeconds(1.0f);
        while (flag)
        {
            centerLight.enabled = true;
            yield return new WaitForSeconds(1.0f);
            centerLight.enabled = false;
            yield return new WaitForSeconds(1.0f);
        }
        centerLight.enabled = false;
    }
    IEnumerator PlayAudio(int p)
    {
        yield return new WaitForSeconds(0.5f);
        if (deafMode)
            whiteDots[p].localPosition = new Vector3(0f, 0.5f, 0f);
        foreach (int pi in pitchIndex[p])
        {
            Audio.PlaySoundAtTransform(pitches[pi].name, transform);
            whiteDots[p].localPosition = new Vector3(dotPosition[pi], whiteDots[p].localPosition.y, 0f);
            yield return new WaitForSeconds(0.45f);
        }
        whiteDots[p].localPosition = new Vector3(0f, 0f, 0f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
        Vector3 pos = buttonMesh[p].transform.localPosition;
        pos.y = 0.0169f;
        buttonMesh[p].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
        if (!(flag))
            hexButtons[p].OnInteract = delegate { pressedButton(p); return false; };
    }
    void resetInput()
    {
        hexButtons[6].OnHighlight = delegate
        {
            buttonMesh[0].material = brightness[brightIndex[0]];
            buttonMesh[1].material = brightness[brightIndex[1]];
            buttonMesh[2].material = brightness[brightIndex[2]];
            buttonMesh[3].material = brightness[brightIndex[3]];
            buttonMesh[4].material = brightness[brightIndex[4]];
            buttonMesh[5].material = brightness[brightIndex[5]];
            buttonMesh[6].material = brightness[0];
        };
        hexButtons[6].OnHighlightEnded = delegate
        {
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
            hexButtons[i].OnInteract = delegate { pressedButton(i); return false; };
            hexButtons[i].OnInteractEnded = null;
            hexButtons[i].OnHighlight = delegate
            {
                foreach (char bl in blackLights[i])
                    ledMesh["142536".IndexOf(bl)].material = ledColors[2];
            };
            hexButtons[i].OnHighlightEnded = delegate
            {
                foreach (char bl in blackLights[i])
                    ledMesh["142536".IndexOf(bl)].material = ledColors[0];
            };
            ledMesh[i].material = ledColors[0];
        }
        flagSubmissions = new string[2];
        submission = "";
        flagToggle = 0;
        mode = "NULL";
        type = 0;
    }
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press|p tl/1 tr/2 ml/3 mr/4 bl/5 br/6 c/7 sl presses the top-left, top-right, middle-left, middle-right, bottom-left, bottom-right, center, and the status light in that order. !{0} hover|h tl/1 tr/2 ml/3 mr/4 bl/5 br/6 c/7 will hover the buttons in the same fashion.";
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
                    int cursor;
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
        }
        else if ((Regex.IsMatch(param[0], @"^\s*HOVER\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(param[0], @"^\s*H\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) && param.Length > 1)
        {
            if (isPos2(param))
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
                    }
                    if (hexButtons[cursor].OnHighlight != null)
                    {
                        hexButtons[cursor].OnHighlight();
                        yield return new WaitForSeconds(2.25f);
                        hexButtons[cursor].OnHighlightEnded();
                        yield return new WaitForSeconds(0.25f);
                    }
                }
            }
        }
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
    private bool isPos2(string[] param)
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
                    break;
                default:
                    return false;
            }
        }
        return true;
    }
}
