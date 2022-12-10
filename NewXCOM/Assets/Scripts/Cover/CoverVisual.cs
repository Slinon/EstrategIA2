using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverVisual : MonoBehaviour
{

    [SerializeField] private Sprite CoverSprite;

    private SpriteRenderer[] spriteRendererArray;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRendererArray = new SpriteRenderer[]
        {
            transform.Find("Escudo (1)").GetComponent<SpriteRenderer>(),
            transform.Find("Escudo (2)").GetComponent<SpriteRenderer>(),
            transform.Find("Escudo (3)").GetComponent<SpriteRenderer>(),
            transform.Find("Escudo (4)").GetComponent<SpriteRenderer>(),
        };
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = MouseWorld.GetPosition();
        GridPosition mousegridPosition = LevelGrid.Instance.GetGridPosition(mouseWorldPosition);
        //Debug.Log("La posicion global del raton es " + mouseWorldPosition + " y en el grid es " + mousegridPosition);
        if(LevelGrid.Instance.IsValidGridPosition(mousegridPosition))
        {
            Vector3 snappedWorldPosition = LevelGrid.Instance.GetWorldPosition(mousegridPosition);
            //Debug.Log("El snappedWorld es " + snappedWorldPosition);
            CoverType coverType = LevelGrid.Instance.GetCoverTypeAtPosition(snappedWorldPosition);
            //Debug.Log("El coverType es " + coverType);
            switch (coverType) 
            {
                case CoverType.None:
                    foreach (SpriteRenderer spriteRenderer in spriteRendererArray) {
                        spriteRenderer.enabled = false;
                        
                    }
                    break;
                case CoverType.Covered:
                    foreach (SpriteRenderer spriteRenderer in spriteRendererArray) {
                        spriteRenderer.enabled = true;
                        spriteRenderer.sprite = CoverSprite;
                    }
                    break;
            }
        }
    }
}
