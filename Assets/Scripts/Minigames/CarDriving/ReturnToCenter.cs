using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReturnToCenter : MonoBehaviour, IPointerUpHandler
{
    public Slider slider;

    public void OnPointerUp(PointerEventData eventData)
    {
        slider.value = 0;
    }
}
