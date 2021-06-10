using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUIAnimationSwitch
{
    void OnAnimationSelected(AnimationTypes type);
}

public class UIAnimationSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class RadiobuttonBind
    {
        public AnimationTypes _type;
        public Toggle _toggle;
    }

    public RadiobuttonBind[] _radioButtons;
    private IUIAnimationSwitch _callback;

    public void Init(IUIAnimationSwitch callback)
    {
        _callback = callback;
    }

    void Start()
    {
        foreach(var rb in _radioButtons)
        {
            if (rb._toggle)
            {
                rb._toggle.onValueChanged.AddListener(delegate {ToggleValueChanged(rb._toggle, rb._type);});
                ToggleValueChanged(rb._toggle, rb._type);                
            }
        }
    }

    void ToggleValueChanged(Toggle change, AnimationTypes type)
    {        
        if (change.isOn && _callback != null)
        {
            _callback.OnAnimationSelected(type);
        }
    }

}
