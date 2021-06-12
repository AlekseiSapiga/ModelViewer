using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaterialSetter : MonoBehaviour
{
    private Material _originalMaterial = null;

    void Awake()
    {
        _originalMaterial = GetComponent<UnityEngine.Renderer>().material;
    }

    public void SetNewMaterial(Material mat)
    {
        if (_originalMaterial == null)
        {
            _originalMaterial = GetComponent<UnityEngine.Renderer>().material;
        }
        GetComponent<UnityEngine.Renderer>().material = mat;
    }

    public void RestoreOriginal()
    {
        if (_originalMaterial == null)
        {
            return;
        }
        GetComponent<UnityEngine.Renderer>().material = _originalMaterial;
    }

    void OnDestroy()
    {
        Destroy(_originalMaterial);
    }
}
