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

	public void AddElement(Grid grid, bool top) //grid - данные для строки, top - место добавления строки
	{
		if (_scroll == null)
		{
			_scroll = new List<Element>();
		}

		GameObject obj = CreateElement(grid);
		if (top)
		{
			_scroll.Insert(0, new Element(obj, grid));
			obj.transform.SetAsFirstSibling(); //двигаем элемент
			GoToElement(1, true); //двигаем скролл на 1 элемент вверх
		}
		else
		{
			_scroll.Add(new Element(obj, grid));
		}
	}

	public void AddRandomElements(int count, bool top) //добавить count случайных элементов вверху (top == true) или внизу (false) скролла
	{
		if (count > 0)
		{
			for (int i=0; i<count; i++)
			{
				Grid grid = new Grid();
				AddElement(grid, top);
			}
		}
	}

	public IEnumerator AddRandomElements(int count, int index, int elementForFrame) //общее количество элементов, элемент на котором остановится скоролл, количество элементов за кадр
	{
		_coroutineProcess = true;
		//добавляем по порядку элементы, исключая первые
		for (int i = (count-index) / elementForFrame + ((count-index) % elementForFrame > 0 ? 1 : 0) - 1; i >= 0; i--)
		{
			if (_coroutineProcess)
			{
				AddRandomElements((i == 0 && (count-index) % elementForFrame > 0) ? ((count-index) % elementForFrame) : elementForFrame, false);
			}
			yield return null;
		}
		//добавляем элементы вверх
		if (index > 0)
		{
			for (int i = index / elementForFrame + (index % elementForFrame > 0 ? 1 : 0) - 1; i >= 0; i--)
			{
				if (_coroutineProcess)
				{
					AddRandomElements((i == 0 && index % elementForFrame > 0) ? (index % elementForFrame) : elementForFrame, true);
				}
				yield return null;
			}
		}
		_coroutineProcess = false;
	}

	public void Clear()
	{
		if (_coroutineProcess)
		{
			_coroutineProcess = false;
		}

		if (_scroll != null && _scroll.Count > 0)
		{
			for (int i=0; i<_scroll.Count; i++)
			{
				GameObject.Destroy(_scroll[i].obj);
			}
			_scroll.Clear();
		}
	}

	public void GoToElement(int index) //движение к заданному индкексу
	{
		if (_scroll != null && index < _scroll.Count)
		{
			RectTransform rect = _content.gameObject.GetComponent<RectTransform>();
			if (rect != null)
			{
				float height = Mathf.Min(GetMaxHeight(), index * _elementSizeWithSpace);
				if (height < 0f)
				{
					height = 0f;
				}				
				rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, height);
			}		
		}
	}

	public void GoToElement(int index, bool top) //движение вверх/вниз на количество элементов
	{
		if (_scroll != null && index < _scroll.Count)
		{
			RectTransform rect = _content.gameObject.GetComponent<RectTransform>();
			if (rect != null)
			{
				float height = GetMaxHeight();

				float delta = (top ? 1f : (-1f)) * index * _elementSizeWithSpace;
				height = Mathf.Min(height, rect.anchoredPosition.y + delta);
				if (height < 0f)
				{
					height = 0f;
				}				
				rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, height);
			}
		}
	}

	GameObject CreateElement(Grid grid)
	{
		GameObject obj =  GameObject.Instantiate(_prefabElement);
		if (obj != null)
		{
			obj.transform.SetParent(_content);
			obj.GetComponent<ScrollElement>().Set(grid);
		}
		return obj;
	}

	float GetMaxHeight() //максимальная высота на которую можно поднять скролл. последний элемент может находиться не выше нижней границы (при уловии достаточного количества элементов))
	{
		if (_scroll == null || _scroll.Count == 0)
		{
			return 0;
		}

		if (_constParamForMaxHeight < 0 || _elementSizeWithSpace < 0)//не посчитан постоянный параметр
		{
			RectTransform rect = _content.gameObject.GetComponent<RectTransform>();
			RectTransform parentRect = rect.parent.gameObject.GetComponent<RectTransform>();
			VerticalLayoutGroup layoutGroup = _content.gameObject.GetComponent<VerticalLayoutGroup>();
			LayoutElement layout = _prefabElement.GetComponent<LayoutElement>();

			if (rect == null || parentRect == null || layoutGroup == null || layout == null)
			{
				return 0f;
			}

			_elementSizeWithSpace = layout.minHeight + layoutGroup.spacing;
			_constParamForMaxHeight = layoutGroup.padding.top + layoutGroup.padding.bottom - layoutGroup.spacing - parentRect.rect.height;
		}

		return _elementSizeWithSpace * _scroll.Count + _constParamForMaxHeight;
	}

	[SerializeField] Transform	_content;
	[SerializeField] GameObject _prefabElement;

	List<Element>				_scroll;

	float						_constParamForMaxHeight = -1f; //постоянные параменты для подсчета maxHeight
	float						_elementSizeWithSpace = -1f; //высота елемента с промежутком между элементами
	bool						_coroutineProcess = false;

}
