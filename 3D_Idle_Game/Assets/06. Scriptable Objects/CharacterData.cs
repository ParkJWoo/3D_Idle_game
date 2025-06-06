using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName ="Scriptable Object/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public int maxHP;
    public int attackPower;
}
