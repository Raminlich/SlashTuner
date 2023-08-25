using System;
using System.Numerics;
using UnityEngine;

[Serializable]
public class AttackCombo
{
    public int headFrames;
    public int bodyFrames;
    public int frameOffset;
    public int damageFrame;
    public int damageAmount;
    public int totalFrames { get { return bodyFrames + headFrames; } private set { } }
}
