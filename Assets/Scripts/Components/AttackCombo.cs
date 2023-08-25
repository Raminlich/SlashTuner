using System;
using System.Numerics;
using UnityEngine;

[Serializable]
public class AttackCombo
{
    public Vector2Int headFrames;
    public Vector2Int bodyFrames;
    public int frameOffset;
    public int damageFrame;
    public int totalFrames { get { return bodyFrames.y; } private set { } }
}
