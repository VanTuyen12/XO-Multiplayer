using TMPro;
using UnityEngine;

public abstract class AbstractText : MyMonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI textUi;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTextUi();
    }

    protected virtual void LoadTextUi()
    {
        if (textUi != null) return;
        textUi = transform.GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + ": LoadTextUi",gameObject);
    }
}
