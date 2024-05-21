using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectedItem : MonoBehaviour
{
    public void UpdateSelectedItem(Transform b)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform b1 = transform.GetChild(i).GetComponent<Transform>();
            if(b1 == null)
                continue;

            GameObject selectedIcon = FindChildrenOfName(b1);
            if(selectedIcon == null)
                continue;
            
            SetActiveSelectedRam(selectedIcon, b == b1);
        }
    }

    GameObject FindChildrenOfName(Transform b1)
    {
        GameObject selectedIcon = null;
        for (int j = 0; j < b1.transform.childCount; j++)
        {
            if (b1.transform.GetChild(j).name == "Selected")
            {
                selectedIcon = b1.transform.GetChild(j).gameObject;
                break;
            }
            selectedIcon = FindChildrenOfName(b1.transform.GetChild(j));
            if (selectedIcon != null)
                return selectedIcon;
        }
        return selectedIcon;
    }
    public void SetActiveSelectedRam(GameObject g,bool activity)
    {
        g.SetActive(activity);
    }
}
