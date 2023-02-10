using UnityEngine;
using UnityEngine.EventSystems;

class OWP_DraggableUI : MonoBehaviour, IDragHandler
{
	/*
	* Basic MonoBehaivour to be attached to a UI to add drag captabilities 
	* Made by Niako the Duck, but use it if you want
	*/
	public void OnDrag(PointerEventData data)
	{
		this.transform.position += new Vector3(Mouse.delta.x, -Mouse.delta.y, 0);
	}
}