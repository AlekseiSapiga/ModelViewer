using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterChoise
{
    public static string _characterId { get; private set; }

    public static void LoadCharacterPreviewScene(string characterId)
    {
        _characterId = characterId;
        Renderer.Instance.Clear();
        SceneManager.LoadScene(1);
    }
    
}
