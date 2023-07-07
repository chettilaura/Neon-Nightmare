using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonsBigger : MonoBehaviour
{
    public void PointerEnter()
    {
        transform.localScale = new Vector3(1.3f, 1.3f, 1f);
    }

    public void PointerExit()
    {
        transform.localScale = Vector3.one;
    }

}
