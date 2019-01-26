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

    public Vector3 textMeshPosition;

    public float animationDuration;
    public AnimationCurve animationCurve;

    Transform followTransform;
    Vector3 anchorPosition;

    IEnumerator animationCoroutine;
    bool isPooling;

    // ---

    SpriteRenderer spriteRenderer;
    TextMeshPro textMesh;
    RectTransform textRect;
    new Transform transform;

    public void Initialize (Transform _followTransform, Vector3 _anchorPosition, Vector2 _textMeshSize, bool _flip) {

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
        SetTextMeshSize(_textMeshSize);

        spriteRenderer.flipX = _flip;

        if (_flip) {

            anchorPosition = _anchorPosition - spritePivotOffset;
            textRect.localPosition = textMeshPosition - new Vector3(spriteRenderer.size.x,0,0);

        } else {

            anchorPosition = _anchorPosition + spritePivotOffset;
            textRect.localPosition = textMeshPosition;

        }

        transform.position = followTransform.position + anchorPosition;

        StartInitialAnimation();

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

        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        spriteRenderer.enabled = false;
        textMesh.enabled = false;

        inPool = true;
        pool.Add(this);

    }

    public void StartPoolingAnimation () {

        if (isPooling) return;
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);

        animationCoroutine = Animate(transform.eulerAngles.z,-45f,transform.localScale,Vector3.zero,true);
        StartCoroutine(animationCoroutine);

    }

    public void StartInitialAnimation () {

        if (animationCoroutine != null) StopCoroutine(animationCoroutine);

        animationCoroutine = Animate(-90f,0f,Vector3.zero,Vector3.one,false);
        StartCoroutine(animationCoroutine);

    }

    IEnumerator Animate (float _startAngle, float _endAngle, Vector3 _startSize, Vector3 _endSize, bool _poolAfterEnd) {

        float t = 0;

        if (_poolAfterEnd) {
            isPooling = true;
        }

        while (t < animationDuration) {

            t += Time.deltaTime;
            float at = animationCurve.Evaluate(t / animationDuration);
            transform.eulerAngles = new Vector3(0,0,Mathf.LerpUnclamped(_startAngle,_endAngle,at));

            float lsx = Mathf.LerpUnclamped(_startSize.x,_endSize.x,at);
            float lsy = Mathf.LerpUnclamped(_startSize.y,_endSize.y,at);

            if (lsx < 0) lsx = 0;
            if (lsy < 0) lsy = 0;

            transform.localScale = new Vector3(lsx,lsy,1);

            yield return null;

        }

        if (_poolAfterEnd) {
            isPooling = false;
            Pool();
        } else {
            transform.eulerAngles = new Vector3(0,0,_endAngle);
            transform.localScale = _endSize;
        }

        animationCoroutine = null;

    }

}
