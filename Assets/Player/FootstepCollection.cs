using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New footstep collection", menuName = "create new footstep collection")]
public class FootstepCollection : ScriptableObject
{
    public List<AudioClip> footstepSound = new List<AudioClip>(); 
}
