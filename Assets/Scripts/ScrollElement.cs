using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollElement : MonoBehaviour 
{
	public void Set(Grid grid)
	{
		_textId.text = grid.id.ToString();
		_textName.text = grid.name;
		_textColor.text = grid.color;
	}

	[SerializeField] Text	_textId;
	[SerializeField] Text	_textName;
	[SerializeField] Text	_textColor;
}
