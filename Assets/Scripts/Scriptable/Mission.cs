using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Gameplay/Mission")]
public class Mission : ScriptableObject
{
    public string missionName;
    public List<WavesPerMission> totalWaves = new List<WavesPerMission>();
    public int missionReward;
}

[Serializable]
public class WavesPerMission
{
    public List<PlayerCharacterPure> enemyToSpawn = new List<PlayerCharacterPure>();
}