using UnityEngine;
using UnityEngine.EventSystems;

public class HoverMenu : MonoBehaviour, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
