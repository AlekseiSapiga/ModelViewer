using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterChoise
{
    public static string _characterId  = "72c050e2-054a-41a9-817f-ba53b9335589"; //{ get; set; }

    public static void LoadCharacterPreviewScene(string characterId)
    {
        _characterId = characterId;
        Renderer.Instance.Clear();
        SceneManager.LoadScene(1);
    }
    
}
