﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverworldUI : MonoBehaviour
{
    public delegate void BoardAction(TileData tile);
    public event BoardAction OnCommanderMove = delegate { };
	public event BoardAction OnCommanderForceMove = delegate { };
	public event BoardAction OnCommanderGrounded = delegate { };

    public delegate void CardAction(CardData card);
    public event CardAction OnPlayerUseCard = delegate { };

    public event System.Action OnPause = delegate { };
    public event System.Action OnUnPause = delegate { };

    public bool _TileHover;
	public ResourceUI _ResourceUI;
	public CommanderUI _CommanderUI;
	CommanderUI _battlebeardCommanderUI;
	CommanderUI _stormshaperCommanderUI;
    public CameraMovement _CameraMovement;
    public BoardUI _BoardUI;
	public ArmyUI _ArmyUI;
    public GameObject _PauseScreen;
    public CardDisplayUI _CardDisplayUI;
    public HandUI _HandUI;

    bool _paused;
    public bool _Paused
    {
        get { return _paused; }
        set
        {
            if (value)
            {
                _PauseScreen.SetActive(true);
                Disable();
				Hide ();
            }
            else
            {
                _PauseScreen.SetActive(false);
                Enable();
				Show ();
            }

            _paused = value;
        }
    }

    // Use this for initialization
    public void Initialise(Player battlebeard, Player stormshaper)
    {
        _BoardUI.Init();

		_battlebeardCommanderUI = battlebeard.GetComponent<CommanderUI>();
		_stormshaperCommanderUI = stormshaper.GetComponent<CommanderUI>();
		_battlebeardCommanderUI.Initialise();    
		_stormshaperCommanderUI.Initialise();     
		//snap players to start position    
		_battlebeardCommanderUI.UpdateToPlayerPosition();    
		_stormshaperCommanderUI.UpdateToPlayerPosition();    
		_ArmyUI.Initialise(battlebeard, stormshaper);  
        _CardDisplayUI.Init();

        //add event listeners
        Enable();
    }

    public void Disable()
    {
        //remove event listeners
        _battlebeardCommanderUI.OnCommanderMoved -= _CommanderUI_OnCommanderMoved;
		_battlebeardCommanderUI.OnCommanderForceMoved -= _CommanderUI_OnCommanderForceMoved;
		_battlebeardCommanderUI.OnStartDrag -= _CommanderUI_OnStartDrag;
		_battlebeardCommanderUI.OnCommanderDrop -= _CommanderUI_OnCommanderDrop;
		_battlebeardCommanderUI.OnCommanderGrounded -= _CommanderUI_Grounded;
		_battlebeardCommanderUI.OnDropCommander -= _CommanderUI_OnDropCommander;
		_stormshaperCommanderUI.OnCommanderMoved -= _CommanderUI_OnCommanderMoved;
		_stormshaperCommanderUI.OnCommanderForceMoved -= _CommanderUI_OnCommanderForceMoved;
		_stormshaperCommanderUI.OnStartDrag -= _CommanderUI_OnStartDrag;
		_stormshaperCommanderUI.OnCommanderDrop -= _CommanderUI_OnCommanderDrop;
		_stormshaperCommanderUI.OnCommanderGrounded -= _CommanderUI_Grounded;
		_stormshaperCommanderUI.OnDropCommander -= _CommanderUI_OnDropCommander;
        _CardDisplayUI.OnCardUse -= _CardDisplayUI_OnCardUse;
		_CardDisplayUI.Hide();
        _HandUI._Enabled = false;

        //disable components
        _battlebeardCommanderUI._Paused = true;
		_stormshaperCommanderUI._Paused = true;

		_BoardUI.PlayerPrompt_DefaultTiles ();
        _CameraMovement.DisableCameraMovement();
		_ArmyUI.Disable ();
    }

	public void Hide() {
		_ArmyUI.Hide();
		_HandUI.Hide();
		//show the card ui if there is a selected card
		if (_HandUI.m_SelectedCardUI != null)
			_CardDisplayUI.Show();
	}

	public void Show() {
		_ArmyUI.Show();
		_HandUI.Show();
	}

	public void ShowUnitSelectionUI(UnitSelection flags) {
		_ArmyUI.Show();
		_ArmyUI.MakeSelectable(flags);	
		Disable();
	}

	public void HideUnitSelectionUI() {
		_ArmyUI.MakeUnselectable();
		Enable();
	}


    public void Enable()
    {
        //add event listeners
        _battlebeardCommanderUI.OnCommanderMoved += _CommanderUI_OnCommanderMoved;
		_battlebeardCommanderUI.OnCommanderForceMoved += _CommanderUI_OnCommanderForceMoved;
		_battlebeardCommanderUI.OnStartDrag += _CommanderUI_OnStartDrag;
		_battlebeardCommanderUI.OnCommanderDrop += _CommanderUI_OnCommanderDrop;
		_battlebeardCommanderUI.OnCommanderGrounded += _CommanderUI_Grounded;
		_battlebeardCommanderUI.OnDropCommander += _CommanderUI_OnDropCommander;

		_stormshaperCommanderUI.OnCommanderMoved += _CommanderUI_OnCommanderMoved;
		_stormshaperCommanderUI.OnCommanderForceMoved += _CommanderUI_OnCommanderForceMoved;
		_stormshaperCommanderUI.OnStartDrag += _CommanderUI_OnStartDrag;
		_stormshaperCommanderUI.OnCommanderDrop += _CommanderUI_OnCommanderDrop;
		_stormshaperCommanderUI.OnCommanderGrounded += _CommanderUI_Grounded;
		_stormshaperCommanderUI.OnDropCommander += _CommanderUI_OnDropCommander;

		_CardDisplayUI.OnCardUse += _CardDisplayUI_OnCardUse;

		_HandUI._Enabled = true;

        //enable components
		_stormshaperCommanderUI._Paused = false;
		_battlebeardCommanderUI._Paused = false;
        _CameraMovement.EnableCameraMovement();
		_ArmyUI.Enable ();
    }

	public void SetPlayer(Player p) {
		_CommanderUI.DisablePlayerMovement ();
		_CommanderUI = p.Type == PlayerType.Battlebeard ? _battlebeardCommanderUI : _stormshaperCommanderUI;
		_CommanderUI.DisplayInfo();
		SwitchFocus (_CommanderUI);
	}

	public void _CardDisplayUI_OnCardUse(CardData cardData)
    {
        Debug.Log("CardData: "+cardData.name);
        OnPlayerUseCard(cardData);
        _CardDisplayUI.Hide();
    }

    void _CommanderUI_OnDropCommander(TileData tile)
    {
        _BoardUI.PlayerPrompt_DefaultTiles();
    }

	void _CommanderUI_Grounded(TileData tile)
	{
		//if camera is not moving to a new position then enable it 
		if (!_CameraMovement.IsLerping ())
			_CameraMovement.EnableCameraMovement ();
		OnCommanderGrounded(tile);
	}

	void _CommanderUI_OnCommanderDrop(Vector3 vec)
	{
		_CameraMovement.EnableCameraMovement(vec);
	}

    void _CommanderUI_OnStartDrag()
    {
        _CameraMovement.DisableCameraMovement();
        _BoardUI.PlayerPrompt_MovableTiles(_CommanderUI.GetReachableTiles());
    }

    void _CommanderUI_OnCommanderMoved(TileData tile)
    {
		OnCommanderMove(tile);
    }

	void _CommanderUI_OnCommanderForceMoved(TileData tile) {
		OnCommanderForceMove(tile);
	}

    public void AllowPlayerMovement(HashSet<TileData> reachableTiles)
    {
        _CommanderUI.AllowPlayerMovement(reachableTiles);
    }

    public void DisablePlayerMovement()
    {
        _CommanderUI.DisablePlayerMovement();
    }

    public void UpdateCommanderPosition()
    {
        _CommanderUI.UpdateToPlayerPosition();
    }

    public void PauseScreenClickHandler()
    {
        OnUnPause();
    }

	public void SwitchFocus(CommanderUI u){
		_CameraMovement.MoveToNewTarget (u._Player.transform, u.getPosition ());
		_ArmyUI.SwitchPlayer (u._Player.Type);
		_ResourceUI.UpdateResources(u._Player.Currency.getPoints());
		_ResourceUI.UpdatePlayerImage(u._Player);
	}

	public void AddPlayerCard(PlayerType pType, CardData cData)
	{
		//update the hand ui here
		if (_CommanderUI._Player.Type == pType) 
		{
			_CommanderUI.DisplayInfo();
		}
	}

	public void RemovePlayerCard(PlayerType pType, CardData cData)
	{
		//update the hand ui here
		if (_CommanderUI._Player.Type == pType) 
		{
			_CommanderUI.DisplayInfo();
		}
	}

	public void MoveCommander(TileData tile){
		_CommanderUI.MoveCommander(tile);
	}

	public void ForceMoveCommander(TileData tile) {
		_CommanderUI.ForceMoveCommander(tile);
	}

    void Update()
    {
        //check for pause switch by key 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			if (_Paused) {
				OnUnPause();
			}
			else {
				OnPause();
			}
        }
    }
}
