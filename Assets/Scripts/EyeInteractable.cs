using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class EyeInteractable : MonoBehaviour
{
    [SerializeField]
    private bool is2DTask = false;
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

    // Variables to store initial rotation and Z position
    private Quaternion initialRotation;
    private float lockedZPosition;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // statusText = GetComponentInChildren<TextMeshPro>();
        originalAnchor = transform.parent;
        initialScale = transform.localScale;
        initialRotation = transform.rotation;
        lockedZPosition = 0.6f;
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

            // Lock rotation
            transform.rotation = initialRotation;
            if (is2DTask)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, lockedZPosition);
            }
            
            // meshRenderer.material = OnSelectedActiveMaterial;
            // statusText.text = $"<color=\"yellow\">SELECTED</color>";
        }   
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
            Debug.LogWarning($"Parented to {anchor.name}");
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
