using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoContentWidth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        float elementSize = gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x;
        float sizeContent = elementSize * transform.childCount - gridLayoutGroup.spacing.x;
        rectTransform.sizeDelta = new Vector2(sizeContent, rectTransform.sizeDelta.y);
    }
}
