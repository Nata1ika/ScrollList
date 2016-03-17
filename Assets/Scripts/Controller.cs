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
		//_scroll.AddRandomElements(3, false);
		//_scroll.AddRandomElements(20, true);
		//_scroll.GoToElement(30);
		StartCoroutine(_scroll.AddRandomElements(1000, 99, 50)); //добавить 1000 элементов, 99 из них будут выше выбранного, по 50 элеметнов за фрейм
	}

	public void OnClickClearButton()
	{
		_scroll.Clear();
	}

	[SerializeField] ScrollController _scroll;
}
