using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour 
{
	void Start()
	{
		OnClickCreateButton();
	}
	
	public void OnClickCreateButton()
	{
		_scroll.AddRandomElements(100);
		_scroll.GoToElement(98);
	}

	public void OnClickClearButton()
	{
		_scroll.Clear();
	}

	[SerializeField] ScrollController _scroll;
}
