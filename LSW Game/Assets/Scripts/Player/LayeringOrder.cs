using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeringOrder : MonoBehaviour
{
    [SerializeField][Tooltip("the starting order layer")] 
    private int layerOffset;

    [SerializeField][Tooltip("the amount layers get changed every interval")] 
    private int layeringScale;

    [SerializeField][Tooltip("the amount of time the ordering layer changes in units")] 
    private float positionInterval;

    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
       spriteRenderer.sortingOrder = Mathf.FloorToInt(transform.position.y / positionInterval) * -layeringScale + layerOffset;
    }
}
