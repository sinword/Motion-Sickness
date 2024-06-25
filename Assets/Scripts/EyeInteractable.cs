using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class EyeInteractable : MonoBehaviour
{
    [field: SerializeField]
    public bool IsHovered { get; private set; }
    
    [field: SerializeField]
    public bool IsSelected{ get; private set; }

    [SerializeField]
    private UnityEvent<GameObject> OnObjectHover;
    
    [SerializeField]
    private UnityEvent<GameObject> OnObjectSelected;

    [SerializeField]
    private Material OnHoverActiveMaterial;

    [SerializeField]
    private Material OnSelectedActiveMaterial;

    [SerializeField]
    private Material OnIdleMaterial;

    private MeshRenderer meshRenderer;

    private Transform originalAnchor;

    private TextMeshPro statusText;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        statusText = GetComponentInChildren<TextMeshPro>();
        originalAnchor = transform.parent;
    }

    public void Hover(bool state)
    {
        IsHovered = state;
    }

    public void Select(bool state, Transform anchor = null) {
        IsSelected = state;
        if (anchor) {
            transform.SetParent(anchor);
        }
        if (!IsSelected) {
            transform.SetParent(originalAnchor);
        }
    }

    void Update()
    { 
        if (IsHovered)
        {
            meshRenderer.material = OnHoverActiveMaterial;
            OnObjectHover?.Invoke(gameObject);
            statusText.text = $"<color=\"yellow\">HOVERED</color>";
        }
        if (IsSelected)
        {
            OnObjectSelected?.Invoke(gameObject);
            meshRenderer.material = OnSelectedActiveMaterial;
            statusText.text = $"<color=\"yellow\">SELECTED</color>";
        }
        if (!IsHovered && !IsSelected)
        {
            meshRenderer.material = OnIdleMaterial;
            statusText.text = $"<color=\"yellow\">IDLE</color>";
        }
    }
}
