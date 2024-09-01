using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

    // [SerializeField]
    // private Material OnHoverActiveMaterial;

    // [SerializeField]
    // private Material OnSelectedActiveMaterial;

    // [SerializeField]
    // private Material OnIdleMaterial;

    [SerializeField]
    public UnityEvent<GameObject> OnSelectionChanged;

    private MeshRenderer meshRenderer;

    private Transform originalAnchor;

    // private TextMeshPro statusText;
    private Vector3 initialScale;
    private Vector3 baseScale;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // statusText = GetComponentInChildren<TextMeshPro>();
        originalAnchor = transform.parent;
        initialScale = transform.localScale;
        Debug.LogWarning($"Initial scale: {initialScale}");

    }
    
    void Update()
    { 
        if (IsHovered)
        {
            OnObjectHover?.Invoke(gameObject);
            // meshRenderer.material = OnHoverActiveMaterial;
            // statusText.text = $"<color=\"yellow\">HOVERED</color>";
        }
        if (IsSelected)
        {
            OnObjectSelected?.Invoke(gameObject);
            // meshRenderer.material = OnSelectedActiveMaterial;
            // statusText.text = $"<color=\"yellow\">SELECTED</color>";
        }
        // if (!IsHovered && !IsSelected)
        // {
        //     meshRenderer.material = OnIdleMaterial;
        //     statusText.text = $"<color=\"yellow\">IDLE</color>";
        // }
    }

    public void Hover(bool state)
    {
        IsHovered = state;
    }

    public void Select(bool state, Transform anchor = null) 
    {
        IsSelected = state;
        OnSelectionChanged?.Invoke(gameObject);
        
        if (anchor) {
            transform.SetParent(anchor);
        }
        if (!IsSelected) {
            transform.SetParent(originalAnchor);
        }
    }

    public void Resize(float ratio) 
    {
        transform.localScale = baseScale * ratio;
    }

    public void SetBaseScale() 
    {
        baseScale = transform.localScale;
    }

}
