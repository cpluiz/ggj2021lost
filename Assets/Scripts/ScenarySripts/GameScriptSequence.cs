using System;
using UnityEngine;

namespace ScenarySripts{
    [CreateAssetMenu(fileName = "SequenceConfig", menuName = "GameIsLost/ScriptSequence", order = 0)]
    public class GameScriptSequence : ScriptableObject{
        public GameSegment[] segment;
    }
}

public enum SegmentType{
    ambience,
    animation,
    audio,
    dialog
}

[System.Serializable]
public class GameSegment{
    public SegmentType type;
    public AudioClip audio;
    public string textId;
    public AmbienceEffectType ambienceEffect;
}