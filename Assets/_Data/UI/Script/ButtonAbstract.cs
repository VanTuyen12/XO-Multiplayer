using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ButtonAbstract : MyMonoBehaviour
{
    [SerializeField]protected Button _button;
    protected override void Start()
    {
        base.Start();
        AddOnClickEvent();
    }

    protected virtual void AddOnClickEvent()
    {
        if (_button == null) return;
        _button.onClick.AddListener(OnClick);
    }

    protected abstract void OnClick();
    

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadButton();
    }

    protected virtual void LoadButton()
    {
        if (_button != null) return;
        _button = GetComponent<Button>();
        Debug.Log(transform.name + " :LoadButton ", gameObject);
    }
    
}
