using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleGridSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public int x;
    public int y;
    public RawImage backGround;
    public RawImage currentImage;
    private LayoutGrid parent;
    [HideInInspector] public bool isDisable = false;
    [HideInInspector] public ChessPieceType type;
    [HideInInspector] public ChessPieceTeam team;

    void Start()
    {
        this.currentImage.texture = backGround.texture;
        parent = GetComponentInParent<LayoutGrid>();
        type = ChessPieceType.None;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop image at: " + x.ToString() + ',' + y.ToString());
        GameObject dropped = eventData.pointerDrag;
        var dragable = dropped.GetComponent<Dragable>();
        // dragable.parentAfterDrag = transform;
        setItem(dragable);
    }

    public void setItem(Dragable item)
    {
        Debug.Log("Set item " + x.ToString() + ',' + y.ToString());
        if (!isDisable)
        {
            if (item.type == ChessPieceType.Hero)
            {
                parent.removeHero();
                parent.heroSlot = new (x, y);
            }
            type = item.type;
            currentImage.texture = item.image.texture;
            parent.syncSide(x, y, item);
        }
    }

    public void setItemForce(Dragable item)
    {
        Debug.Log("Set force item " + x.ToString() + ',' + y.ToString());
        type = item.type;
        currentImage.texture = item.image.texture;
    }

    public void resetItem()
    {
        if (type != ChessPieceType.None)
        {
            type = ChessPieceType.None;
            currentImage.texture = backGround.texture;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(name + " clicked");
        if(type == ChessPieceType.Hero)
            parent.removeHero();
        else
            resetItem();

        parent.resetOpposite(x, y);
    }
}
