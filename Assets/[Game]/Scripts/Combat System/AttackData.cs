using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class AttackData : ScriptableObject
{
    
    [field: SerializeField] public string AnimName { get; private set; } //public string AnimName => animName;

}
