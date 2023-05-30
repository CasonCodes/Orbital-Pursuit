using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using Photon.Pun;

public class PlayerListing: MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    //Publicly accessible, however, only to be set within the script.
    public Player Player {get; private set;}

    //  Reversing the string since text mesh reverses the text automatically.
    //  Code taken from: https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
    public string Reverse(string text)
    {
        char[] cArray = text.ToCharArray();
        string reverse = string.Empty;
        for (int i = cArray.Length - 1; i > -1; i--)
        {
            reverse += cArray[i];
        }
        return reverse;
    }

    // Setting the RoomListing object's info by name and maximum amount of players.
    public void SetPlayerInfo(Player player)
    {
        Player = player;

        _text.text = player.NickName;
        
        _text.text = Reverse(_text.text);

        Debug.Log(_text.text);
    }
    
}
