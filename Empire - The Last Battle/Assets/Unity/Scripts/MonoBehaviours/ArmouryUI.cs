using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class PurchasableItem : ScriptableObject
{
	public int cost;
}

public class ArmouryUI : MonoBehaviour
{
	public List<PurchasableUnit> purchasableUnits;
	public List<PurchasableCard> purchasableCards;
	public List<PurchasableCastlePiece> purchasableCastlePieces;

	public List<GameObject> armouryUISections;

	public delegate void PurchasedItemCallback(PurchasableItem purchasedItem);
	public event PurchasedItemCallback OnPurchasedItem = delegate { };
	public event Action<bool> OnShowToggled = delegate { };
	public event Action<bool> OnDisableToggled = delegate { };

	Player player;
	int previousCurrency;

	const string k_UnitsSection = "UnitsSection";
	const string k_CardsSection = "CardsSection";
	const string k_CastleSection = "CastleSection";

	public void Initialise(Player battleBeard, Player stormShaper)
	{
		armouryUISections = GameObject.FindGameObjectsWithTag ("ArmouryUISection").ToList();
	}

	public IEnumerable<PurchasableUnit> AvailableUnits(Player player)
	{
		return purchasableUnits.Where(x => x.purchaseLevel <= player.CastleProgress && x.cost <= player.Currency.getPoints()).ToList();
	}

	public IEnumerable<PurchasableCard> AvailableCards(Player player)
	{
		return purchasableCards.Where(x => x.cost <= player.Currency.getPoints()).ToList();
	}

	public IEnumerable<PurchasableCastlePiece> AvailableCastlePieces(Player player)
	{
		//return early if no purchasable castle pieces
		if (purchasableCastlePieces.Count == 0) {
			Debug.Log ("No purchasable castle peices at all");
			return Enumerable.Empty<PurchasableCastlePiece> ();
		}

		return purchasableCastlePieces.Where (x => player.LostImmortalKillCount >= x.purchaseLevel && x.b_AlreadyPurchased).ToList();
	}

	public void CurrencyChangedUpdate(int val, Player player)
	{
		bool disable = false;

		if (val <= previousCurrency)
			disable = true;

		var units = AvailableUnits (player).ToList ();
		ToggleUIImages (purchasableUnits, k_UnitsSection, disable);

		var cards = AvailableCards (player).ToList ();
		ToggleUIImages (purchasableCards, k_CardsSection, disable);

		var castlePieces = AvailableCastlePieces (player).ToList ();
		ToggleUIImages (purchasableCastlePieces, k_CastleSection, disable);

		previousCurrency = val;
	}

	void UpdateItems(Player player)
	{
		ToggleUIImages <PurchasableUnit>(AvailableUnits (player).ToList(), k_UnitsSection);
		ToggleUIImages <PurchasableCard>(AvailableCards (player).ToList(), k_CardsSection);
		ToggleUIImages <PurchasableCastlePiece>(AvailableCastlePieces (player).ToList(), k_CastleSection);
	}

	void ToggleUIImages<T>(IList<T> purchasableItems, string sectionName, bool disableOn = true) where T : PurchasableItem
	{
		var section = GameObject.Find (sectionName);

		if (section != null) 
		{
			if (disableOn)
				GameObject.Find(sectionName).GetComponentsInChildren<Image>(true).Where(x => !purchasableItems.Any(z => x.name.Contains(z.name.ToString())))
					.ToList().ForEach(x => { x.color = Color.grey; x.GetComponent<Button>().interactable = false; });
			else
				GameObject.Find(sectionName).GetComponentsInChildren<Image>(true).Where(x => purchasableItems.Any(z => x.name.Contains(z.name.ToString())))
					.ToList().ForEach(x => { x.color = Color.white; x.GetComponent<Button>().interactable = true; });
		}	
	}

	public void ToggleShow(bool toggledOn, Player player)
	{
		gameObject.SetActive(toggledOn);
		UpdateItems (player);

		OnShowToggled(toggledOn);
	}

	public void ToggleDisable(bool disableOn)
	{
		OnDisableToggled (disableOn);
	}

	public void BuyUnit(PurchasableUnit purchasableUnit)
	{
		OnPurchasedItem(purchasableUnit);

		if (Debug.isDebugBuild)
			Debug.Log("Unit " + purchasableUnit.name + " bought");
	}

	public void BuyCard(PurchasableCard purchasableCard)
	{
		OnPurchasedItem(purchasableCard);

		if (Debug.isDebugBuild)
			Debug.Log("Card " + purchasableCard.name + " bought");
	}

	public void BuyCastlePiece(PurchasableCastlePiece purchasableCastlePiece)
	{
		OnPurchasedItem(purchasableCastlePiece);

		if (Debug.isDebugBuild)
			Debug.Log("Castle Piece " + purchasableCastlePiece.name + " bought");
	}
}