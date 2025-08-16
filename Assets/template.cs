using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class template : MonoBehaviour
{
    //public private KMAudio Audio;

    public KMSelectable[] buttons;
    public KMSelectable screen;
    public MeshRenderer screenMesh;
    public Texture[] cellTextures;

    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    private bool stage = false;
    private int[] coordinate = new int[2] { 4, 4 };
    private int[] initCoordinate;
    private int pressed = 0;
    private int currentLowestDist = 0;


    private int[] distances = new int[72] { 3, 1, 2, 2, 3, 1, 4, 2, 3, 1, 3, 1, 4, 2, 4, 2, 2, 3, 1, 3, 1, 2, 4, 2, 3, 3, 1, 1, 4, 2, 2, 2, 4, 1, 1, 2, 2, 2, 3, 3, 3, 1, 3, 1, 4, 2, 4, 2, 2, 3, 1, 3, 1, 2, 4, 2, 2, 1, 3, 2, 1, 3, 2, 4, 3, 1, 2, 2, 3, 1, 4, 2 };

    private int[] grid = new int[81]{6, 0, 1, 5, 3, 8, 4, 7, 2,
                                     8, 4, 3, 7, 2, 6, 0, 5, 1,
                                     7, 2, 5, 0, 4, 1, 8, 3, 6,
                                     5, 1, 0, 4, 8, 3, 6, 2, 7,
                                     3, 6, 7, 2, 9, 5, 1, 4, 8,
                                     4, 8, 2, 6, 1, 7, 5, 0, 3,
                                     0, 3, 4, 8, 6, 2, 7, 1, 5,
                                     2, 7, 6, 1, 5, 0, 3, 8, 4,
                                     1, 5, 8, 3, 7, 4, 2, 6, 0};

    private int[] offsets = new int[4];
    private List<int>[] coprimes = new List<int>[6]
    {
        new List<int>{3,5},
        new List<int>{2,4,5,7},
        new List<int>{3,7},
        new List<int>{2,3,6,7},
        new List<int>{5,7},
        new List<int>{3,4,5,6}
    };

    int mod(int a, int b)
    {
        while (a >= b) a -= b;
        while (a < 0) a += b;
        return a;
    }

    void generateOffsets()
    {
        int num = Rnd.Range(0, 5);
        offsets[0] = num + 2;
        offsets[1] = coprimes[num].PickRandom();
        num = Rnd.Range(0, 5);
        offsets[2] = num + 2;
        offsets[3] = coprimes[num].PickRandom();

        Debug.LogFormat("[Termet #{0}] Offsets are: ◄{1}, ▲{2}, ▼{3}, ►{4}", ModuleId, offsets[0], offsets[2], offsets[3], offsets[1]);
    }
    int lowestdist()
    {
        int[] diff = new int[2] { mod(coordinate[0] - 4, 9), mod(coordinate[1] - 4, 9) };
        int ansx = 0;
        int ansy = 0;
        
        if (diff[0]!=0)
            switch (offsets[0] * 10 + offsets[1])
            {
                case 23: { ansx = distances[0 * 8 + diff[0] - 1]; break; }
                case 25: { ansx = distances[1 * 8 + diff[0] - 1]; break; }
                case 34: { ansx = distances[2 * 8 + diff[0] - 1]; break; }
                case 35: { ansx = distances[3 * 8 + diff[0] - 1]; break; }
                case 37: { ansx = distances[4 * 8 + diff[0] - 1]; break; }
                case 47: { ansx = distances[5 * 8 + diff[0] - 1]; break; }
                case 56: { ansx = distances[6 * 8 + diff[0] - 1]; break; }
                case 57: { ansx = distances[7 * 8 + diff[0] - 1]; break; }
                case 67: { ansx = distances[8 * 8 + diff[0] - 1]; break; }

                case 32: { ansx = distances[1 * 8 - diff[0]]; break; }
                case 52: { ansx = distances[2 * 8 - diff[0]]; break; }
                case 43: { ansx = distances[3 * 8 - diff[0]]; break; }
                case 53: { ansx = distances[4 * 8 - diff[0]]; break; }
                case 73: { ansx = distances[5 * 8 - diff[0]]; break; }
                case 74: { ansx = distances[6 * 8 - diff[0]]; break; }
                case 65: { ansx = distances[7 * 8 - diff[0]]; break; }
                case 75: { ansx = distances[8 * 8 - diff[0]]; break; }
                case 76: { ansx = distances[9 * 8 - diff[0]]; break; }
            }
        if (diff[1] != 0)
            switch (offsets[2] * 10 + offsets[3])
            {
                case 23: { ansy= distances[0 * 8 + diff[1] - 1]; break; }
                case 25: { ansy= distances[1 * 8 + diff[1] - 1]; break; }
                case 34: { ansy= distances[2 * 8 + diff[1] - 1]; break; }
                case 35: { ansy= distances[3 * 8 + diff[1] - 1]; break; }
                case 37: { ansy= distances[4 * 8 + diff[1] - 1]; break; }
                case 47: { ansy= distances[5 * 8 + diff[1] - 1]; break; }
                case 56: { ansy= distances[6 * 8 + diff[1] - 1]; break; }
                case 57: { ansy= distances[7 * 8 + diff[1] - 1]; break; }
                case 67: { ansy= distances[8 * 8 + diff[1] - 1]; break; }

                case 32: { ansy= distances[1 * 8 - diff[1]]; break; }
                case 52: { ansy= distances[2 * 8 - diff[1]]; break; }
                case 43: { ansy= distances[3 * 8 - diff[1]]; break; }
                case 53: { ansy= distances[4 * 8 - diff[1]]; break; }
                case 73: { ansy= distances[5 * 8 - diff[1]]; break; }
                case 74: { ansy= distances[6 * 8 - diff[1]]; break; }
                case 65: { ansy= distances[7 * 8 - diff[1]]; break; }
                case 75: { ansy= distances[8 * 8 - diff[1]]; break; }
                case 76: { ansy= distances[9 * 8 - diff[1]]; break; }
            }
        Debug.LogFormat("[Termet #{0}] Difference: ({1},{2}). Cases: {3} for X, {4} for Y. Lowest distances are {5} for X and {6} for Y.", ModuleId, mod(coordinate[0] - 4, 9), mod(coordinate[1] - 4, 9), offsets[0] * 10 + offsets[1], offsets[2] * 10 + offsets[3],ansx,ansy);
        return ansx+ansy;

    }

    void press(int n)
    {
        if (ModuleSolved) return;
        coordinate[n/2] = mod(coordinate[n/2] + (n%2==0? -1:1) * offsets[n], 9);
        if (!stage) screenMesh.material.mainTexture = cellTextures[grid[coordinate[1] * 9 + coordinate[0]]];
        else {
            pressed++;
            if (pressed > currentLowestDist) pressScreen();
        }
        
        Debug.LogFormat("[Termet #{0}] You pressed {1}", ModuleId, n<2?(n==0?"LEFT":"RIGHT"): (n == 2 ? "UP" : "DOWN"));
        Debug.LogFormat("[Termet #{0}] New coorinates: ({1}, {2})", ModuleId, coordinate[0], coordinate[1]);
        if (coordinate[0]==4 && coordinate[1] == 4)
        {
            if (stage)
            {
                GetComponent<KMBombModule>().HandlePass();
                ModuleSolved = true;
                screenMesh.material.mainTexture = cellTextures[grid[4 * 9 + 4]];
            }
            else GetComponent<KMBombModule>().HandleStrike();
        }
    }

    void pressScreen()
    {
        if (ModuleSolved) return;
        if (stage)
        {
            if(pressed!=0) GetComponent<KMBombModule>().HandleStrike();
            generateCoords();
            generateOffsets();
            screenMesh.material.mainTexture = cellTextures[grid[coordinate[1] * 9 + coordinate[0]]];
            stage = false;
        }
        else
        {
            stage = true;
            pressed = 0;
            screenMesh.material.mainTexture = null;
            coordinate[0] = initCoordinate[0];
            coordinate[1] = initCoordinate[1];
            currentLowestDist = lowestdist();
            Debug.LogFormat("[Termet #{0}] Lowest distance is {1}. Good luck.", ModuleId, currentLowestDist);
        }
    }

    void generateCoords()
    {
        coordinate[0] = Rnd.Range(0, 8);
        coordinate[1] = Rnd.Range(0, 8);
        if (coordinate[0] > 4) coordinate[0]++;
        if (coordinate[1] > 4) coordinate[1]++;
        initCoordinate = new int[2] { coordinate[0], coordinate[1] };
        Debug.LogFormat("[Termet #{0}] New generated coorinates: ({1}, {2})", ModuleId,coordinate[0],coordinate[1]);
    }

    void Awake()
    {
        ModuleId = ModuleIdCounter++;
        buttons[0].OnInteract += delegate () { press(0); return false; };
        buttons[1].OnInteract += delegate () { press(1); return false; };
        buttons[2].OnInteract += delegate () { press(2); return false; };
        buttons[3].OnInteract += delegate () { press(3); return false; };
        screen.OnInteract += delegate () { pressScreen(); return false; };
        generateCoords();
        generateOffsets();
        screenMesh.material.mainTexture = cellTextures[grid[coordinate[1] * 9 + coordinate[0]]];
    }

    void Start()
    {

    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string Command)
    {
        if (!Command.RegexMatch("[ULRDSulrds]+"))
        {
            yield return "sendtochaterror Сommand is not valid.";
        }
        var commandArgs = Command.ToUpperInvariant();
        foreach (char i in Command)
        {
            switch (i)
            {
                case 'U': { press(0); break; }
                case 'D': { press(1); break; }
                case 'L': { press(2); break; }
                case 'R': { press(3); break; }
                case 'S': { pressScreen(); break; }
            }
        }
        yield return null;
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        coordinate[0] = 4;
        coordinate[1] = 4;
        GetComponent<KMBombModule>().HandlePass();
        ModuleSolved = true;
        screenMesh.material.mainTexture = cellTextures[grid[4 * 9 + 4]];
        yield return null;
    }
}
