using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public LetterDataSO data;

    public SpriteRenderer spriteRenderer;

    public bool isMainLetter;

    private void Awake()
    {
       // Collider2D c = gameObject.GetComponent<PolygonCollider2D>();
        //c.enabled = false;
        //c.enabled = true;

    }
    void Start()
    {

        //if (data != null && spriteRenderer != null)
        //{
        //    spriteRenderer.sprite = data.sprite;
        //}
    }

    void Update()
    {
        
    }

    public void Init(LetterDataSO data, Sprite sprite, bool isMainLetter)
    {
        this.data = data;
        this.isMainLetter = isMainLetter;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;

            gameObject.AddComponent<PolygonCollider2D>();
        }
    }
}
