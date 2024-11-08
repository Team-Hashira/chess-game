using Crogen.PowerfulInput;
using UnityEngine;

public class WorldObjectSelector : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _whatIsSelectable;

	public ISelectableObject curSelectedObject;

	private void Awake()
	{
		_inputReader.OnLeftMouseClickEvent += OnMouseClick;
		_inputReader.OnMouseMoveEvent += OnMouseMove;
	}

	private void OnDestroy()
	{
		_inputReader.OnLeftMouseClickEvent -= OnMouseClick;
		_inputReader.OnMouseMoveEvent -= OnMouseMove;
	}

	private void OnMouseClick(bool mouseClick)
	{
		if (curSelectedObject != null)
			curSelectedObject.OnClick();
	}

	private void OnMouseMove(Vector3 mousePos)
	{
		ISelectableObject obj = GetSelectableObject();
		if (curSelectedObject != null && obj != curSelectedObject)
		{
			curSelectedObject?.OnSelectExit();
			curSelectedObject = null;
		}
		else if (curSelectedObject == null && obj != null)
		{
			curSelectedObject = obj;
			curSelectedObject.OnSelectEnter();
		}
	}

	private void Update()
	{
		if (curSelectedObject != null)
		{
			curSelectedObject.OnSelect();
		}
	}

	private ISelectableObject GetSelectableObject()
	{
		Vector2 position = Camera.main.ScreenToWorldPoint(_inputReader.MousePosition);
		Debug.DrawRay(position, Vector3.forward * 1000);
		RaycastHit2D hitInfo = Physics2D.Raycast(position, Vector3.back, 1000, _whatIsSelectable);
		return hitInfo.collider?.GetComponent<ISelectableObject>();
	}
}
