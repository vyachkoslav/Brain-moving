using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isDown { get; private set; }
    public UnityEvent onClick;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
    }
}
