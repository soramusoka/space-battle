﻿using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class Ship : BaseBehaviour<Ship>
{
	private Vector3 _endPosition;
	private float _moveSpeed;

	private Boolean IsSelected { get { return Current == this; }}
	public Rect WindowGui { get; set; }
	public ShipStruct Struct { get; set; }

	public Boolean isActionDo;

	public Player Player { get { return Player.Current; } }
	public Place Place { get; set; }

	public void Start()
	{
		_moveSpeed = 0.01f;

		WindowGui = new Rect(5, 80, 150, 400);
		_endPosition = Vector3.zero;

		isActionDo = false;
	}

	public void Update()
	{
		_moveShip();
	}

	public void OnMouseDown()
	{
		if (Struct.Action == ShipAction.Stay)
		{
			if (Current == this)
			{
				Debug.Log("Z");
			}

			Current = IsSelected ? null : this;

			if (!IsSelected)
			{
				DeselectShip();
			}
		}
	}

	public void OnGUI()
	{
		var offset = 5;
		var widthLabel = 150;
		var heightLabel = 20;

		if (IsSelected)
		{
			GUI.Label(new Rect(offset, 150, widthLabel, heightLabel), "*" + Current.Struct.Type + "Ship*");
			GUI.Label(new Rect(offset, 170, widthLabel, heightLabel), "Health: " + Current.Struct.Health);
			GUI.Label(new Rect(offset, 190, widthLabel, heightLabel), "Power: " + Current.Struct.Power);
			GUI.Label(new Rect(offset, 210, widthLabel, heightLabel), "Action: " + Current.Struct.Action);
		}
	}

	private void DeselectShip()
	{
		Player.SetAction(PlayerAction.Wait);
		Current = null;
	}

	public void SetDestination(Vector3 destination)
	{
		Current._endPosition = destination;
	}

	public void Move(Place place)
	{
		Place = place;

		place.IsFree = false;

		transform.position = place.Position;
		active = true;
	}

	public Boolean IsShipMove()
	{
		return Struct.Action == ShipAction.Move;
	}

	public Boolean IsShipStay()
	{
		return Struct.Action == ShipAction.Stay;
	}

	public Boolean IsShipAttack()
	{
		return Struct.Action == ShipAction.Attack;
	}

	private void _moveShip()
	{
		if (_endPosition != Vector3.zero)
		{
			Struct.Action = ShipAction.Move;

			Position = Vector3.Lerp(Position, _endPosition, Time.time * _moveSpeed);

			if (Position == _endPosition)
			{
				_endPosition = Vector3.zero;
				Player.ResetAction();
				
				Struct.Action = ShipAction.Stay;

				isActionDo = true;
			}
		}
	}
}

public class ShipStruct
{
	public int Id { get; set; }

	public ShipType Type { get; set; }
	public ShipAction Action { get; set; }
	public ShipState State { get; set; }

	public int Health;
	public int Power { get; set; }

	public void Start()
	{
		Id = 0;

		Type = ShipType.Unknown;
		Action = ShipAction.Unknown;
		State = ShipState.Unknown;

		Power = 0;
		Health = 0;
	}
}

public enum ShipType
{
	Unknown = 0,
	Small,
	Medium,
	Big
}

public enum ShipAction
{
	Unknown = 0,
	Move,
	Stay,
	Attack
}

public enum ShipState
{
	Unknown = 0,
	Alive,
	Wounded,
	Dead
}