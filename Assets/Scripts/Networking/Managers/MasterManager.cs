using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "NetScriptables/MasterManager")]

public class MasterManager : NetScriptableObject<MasterManager>
{

    [SerializeField]
    private GameSettings _gameSettings;

    public static GameSettings GameSettings { get { return Instance._gameSettings; } }
    
}
