using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboData",menuName = "ComboData")]
public class ComboData : ScriptableObject
{
    public List<AttackCombo> combo;
}
