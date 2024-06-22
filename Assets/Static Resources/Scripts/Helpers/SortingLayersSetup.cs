using UnityEngine;

public class SortingLayersSetup : MonoBehaviour
{
    [SerializeField] private int _sortingLayerToSet;
    
    [ContextMenu("Setup Sorting Layers")]
    public void SetupLayers()
    {
        var sprites = transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in sprites)
        {
            sprite.sortingOrder = _sortingLayerToSet;
        }
    }
}
