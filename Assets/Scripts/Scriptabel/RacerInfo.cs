using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RacerInfo", menuName = "RacerInfo")]
public class RacerInfo : ScriptableObject
{
    public string RacerName;
    public Color RacerColor;
    public int NumOfJumpsToMoveToNextObject;
    public Constances.RacerDifficulty RacerDifficulty;
}
