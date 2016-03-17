using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour 
{
	public class Element
	{
		public GameObject		obj;
		public Grid				grid;

		public Element(GameObject setObj, Grid setGrid)
		{
			obj = setObj;
			grid = setGrid;
		}
	}

	public void AddElement(Grid grid)
	{
		if (_scroll == null)
		{
			_scroll = new List<Element>();
		}

		GameObject obj =  GameObject.Instantiate(_prefabElement);
		if (obj != null)
		{
			obj.transform.SetParent(_content);
			obj.GetComponent<ScrollElement>().Set(grid);
		}

		_scroll.Add(new Element(obj, grid));
	}

	public void AddRandomElement()
	{
		Grid grid = new Grid();
		AddElement(grid);
	}

	public void AddRandomElements(int count)
	{
		if (count > 0)
		{
			for (int i=0; i<count; i++)
			{
				AddRandomElement();
			}
		}
	}

	public void Clear()
	{
		if (_scroll != null && _scroll.Count > 0)
		{
			for (int i=0; i<_scroll.Count; i++)
			{
				GameObject.Destroy(_scroll[i].obj);
			}
			_scroll.Clear();
		}
	}

	public void GoToElement(int index)
	{
		if (_scroll != null && index < _scroll.Count)
		{
			RectTransform rect = _content.gameObject.GetComponent<RectTransform>();
			if (rect != null)
			{
				//рассчитаем максимальную высоту, на которую можно поднять скролл. последний элемент скролла не может быть выше окончания скролла
				RectTransform parentRect = rect.parent.gameObject.GetComponent<RectTransform>();
				VerticalLayoutGroup layoutGroup = _content.gameObject.GetComponent<VerticalLayoutGroup>();
				LayoutElement layout = _prefabElement.GetComponent<LayoutElement>();

				if (parentRect != null && layoutGroup != null && layout != null)
				{
					float elementSize = layout.minHeight;
					float maxHeight = layoutGroup.padding.top + layoutGroup.padding.bottom + layoutGroup.spacing * (_scroll.Count - 1) + elementSize * _scroll.Count - parentRect.rect.height;

					float height = Mathf.Min(index * (elementSize + layoutGroup.spacing), maxHeight);//верхний элемент не может опуститься ниже
					if (height < 0f)
					{
						height = 0f;
					}

					rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, height);
				}
			}		
		}
	}

	[SerializeField] Transform	_content;
	[SerializeField] GameObject _prefabElement;

	List<Element>				_scroll;
}
