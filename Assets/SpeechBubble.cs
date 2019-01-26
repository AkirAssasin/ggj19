using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour {

    public static List<SpeechBubble> pool = new List<SpeechBubble>();
    public bool inPool;

    public static SpeechBubble GetFromPool (GameObject _prefab) {

        SpeechBubble result;

        if (pool.Count > 0) {
            result = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
        } else {
            result = Instantiate(_prefab).GetComponent<SpeechBubble>();
        }

        return result;

    }

    [Header("Settings")]

    public Vector3 spritePivotOffset;
    public Vector2 spriteSizeDiference; // sprite size = textMeshSize + sprite size difference

    Transform followTransform;
    Vector3 anchorPosition;

    // ---

    SpriteRenderer spriteRenderer;
    TextMeshPro textMesh;
    RectTransform textRect;
    new Transform transform;

    public void Initialize (Transform _followTransform, Vector3 _anchorPosition, Vector2 _textMeshSize) {

        inPool = false;

        // ---

        if (transform == null) transform = GetComponent<Transform>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (textMesh == null) textMesh = GetComponentInChildren<TextMeshPro>();
        if (textRect == null) textRect = textMesh.rectTransform;

        // ---

        spriteRenderer.enabled = true;
        textMesh.enabled = true;

        followTransform = _followTransform;

        anchorPosition = _anchorPosition + spritePivotOffset;

        transform.position = followTransform.position + anchorPosition;

        SetTextMeshSize(_textMeshSize);

    }

    public void SetTextMeshSize (Vector2 _textMeshSize) {

        textRect.sizeDelta = _textMeshSize;
        spriteRenderer.size = new Vector2(_textMeshSize.x + spriteSizeDiference.x,_textMeshSize.y + spriteSizeDiference.y);

    }

    public void SetText (string _text) {

        textMesh.text = _text;

    }

    void Update () {

        if (inPool) return;
        // float dt = Time.deltaTime;

        transform.position = followTransform.position + anchorPosition;

    }

    public void Pool () {

        if (inPool) return;

        spriteRenderer.enabled = false;

        inPool = true;
        pool.Add(this);

    }

}
