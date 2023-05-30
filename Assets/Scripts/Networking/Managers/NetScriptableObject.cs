using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            //  This if statement guarantees that there is no other object with the type T.
            //  This allows the programmer to have control over the amount of networking instances
            //  that there are in the game.
            if (_instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();

                if (results.Length == 0)
                {
                    Debug.LogError("NetScriptableObject -> Instance -> results length is 0 for type " + typeof(T).ToString() + ".");
                    return null;
                }

                if (results.Length > 1)
                {
                    Debug.LogError("NetScriptableObject -> Instance -> results length is greater than 1 for type " + typeof(T).ToString() + ".");
                    return null;
                }

                _instance = results[0];
            }

            return _instance;
        }
    }
}
