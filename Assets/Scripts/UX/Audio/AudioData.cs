using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

//https://stackoverflow.com/questions/59459321/unity-manage-audiomanager-on-all-scenes
[CreateAssetMenu]
public class AudioData : ScriptableObject
{
    public List<Sound> sounds = new List<Sound>();
}
