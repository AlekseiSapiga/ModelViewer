using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMaterialSetter : MonoBehaviour
{
    private Material _currentMaterial = null;

    public void UpdateMaterial()
    {
        var renderers = GetComponentsInChildren<UnityEngine.Renderer>();
        foreach (var rend in renderers)
        {
            var matSetter = rend.gameObject.GetComponent<MaterialSetter>();
            if (matSetter == null)
            {
                matSetter = rend.gameObject.AddComponent<MaterialSetter>();                
            }
            if (_currentMaterial != null)
            {
                matSetter.SetNewMaterial(_currentMaterial);
            }
            else
            {
                matSetter.RestoreOriginal();
            }
        }
    }

    public void SetMaterial(Material mat)
    {
        _currentMaterial = mat;
        UpdateMaterial();
    }

    public void RestoreOriginal()
    {
        _currentMaterial = null;
        UpdateMaterial();
    }
}
