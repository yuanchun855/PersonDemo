using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

[MessagePackObject(keyAsPropertyName:true)]
public class PlayerData
{
    [Key(0)]
    public int Level { get; set; }

    
    public PlayerData()
    {
        
    }
    
    [SerializationConstructor]
    public PlayerData(int level)
    {
        Level = level;
    }
}
