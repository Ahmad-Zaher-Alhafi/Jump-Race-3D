using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constances
{
    //Tags
    public static string PathJumpObjectTag = "PathJumpObject";
    public static string WinObjectTag = "WinObject";


    //Layers Numbers
    public static int JumpObjectLayerNum = 3;
    public static int PlayerLayerNum = 6;
    public static int JumpObjectBaseLayerNum = 7;
    public static int CenterPointLayerNum = 8;
    public static int RacerLayerNum = 9;

    //Animator Parameters
    public static string HasToJumpParameter = "hasToJump";
    public static string AnimationNumParameter = "animationNum";

    //PlayerPrefs Keys
    public static string levelNumKey = "LevelNum";

    //Enums
    public enum StateTxtWords
    {
        Prefect,
        LongJump,
        Good,
        Killer,
    }

    public enum RacerDifficulty
    {
        Pro,
        Hard,
        Normal,
        Easy,
        Noob
    }

    public enum AnimationsTypes
    {
       Jump,
       Fall,
       Win,
       Warmingup,
    }
}
