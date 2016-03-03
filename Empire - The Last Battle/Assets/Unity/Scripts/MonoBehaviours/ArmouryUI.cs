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
	public List<PurchasableUnit> purchasbleUnits;
	public List<PurchasableCard> purchasbleCards;
	public List<PurchasableCastlePiece> purchasableCastlePieces;

	[SerializeField]
	GameObject m_UIUnitSection;
	[SerializeField]
	GameObject m_UICardsSection;
	[SerializeField]
	GameObject m_UICastleSection;

	public delegate void PurchasedItemCallback(PurchasableItem purchasedItem);
	public event PurchasedItemCallback OnPurchasedItem = delegate { };
	public event Action<bool> OnArmouryToggled = delegate { };

	public IEnumerable<PurchasableUnit> AvailableUnits(Player player)
	{
		//Return only units that player can buy
		return purchasbleUnits.Where(x => x.purchaseLevel <= player.CastleProgress && x.cost <= player.Currency.getPoints());
	}

	public IEnumerable<PurchasableCard> AvailableCards(Player player)
	{
		return purchasbleCards.Where(x => x.cost <= player.Currency.getPoints());
	}

	public IEnumerable<PurchasableCastlePiece> AvailableCastlePieces(Player player)
	{
		//return early if no purchasable castle pieces
		if (purchasableCastlePieces.Count == 0)
		{
			Debug.LogError("No purchasable castle peices at all");
			return Enumerable.Empty<PurchasableCastlePiece>();
		}

		//Castle pieces unlock after lost immortals die
		List<PurchasableCastlePiece> toReturn = new List<PurchasableCastlePiece>();
		foreach (var castlePiece in purchasableCastlePieces)
		{
			if (player.LostImmortalKillCount >= castlePiece.purchaseLevel && castlePiece.b_AlreadyPurchased)
				toReturn.Add(castlePiece);
		}

		return toReturn;
	}

	public void Setup(Player player)
	{
		ToggleArmoury(true);

		m_UIUnitSection.GetComponentsInChildren<Image>(true).Where(x => !AvailableUnits(player).Any(z => x.name.Contains(z.name.ToString())))
											.ToList().ForEach(x => { x.color = Color.grey; x.GetComponent<Button>().interactable = false; });

		m_UICardsSection.GetComponentsInChildren<Image>(true).Where(x => !AvailableCards(player).Any(z => x.name.Contains(z.name.ToString())))
									.ToList().ForEach(x => { x.color = Color.grey; x.GetComponent<Button>().interactable = false; });

		m_UICastleSection.GetComponentsInChildren<Image>(true).Where(x => !AvailableCastlePieces(player).Any(z => x.name.Contains(z.name.ToString())))
									.ToList().ForEach(x => { x.color = Color.grey; x.GetComponent<Button>().interactable = false; });
	}

	public void ToggleArmoury(bool toggledOn)
	{
		transform.Find("Panel").gameObject.SetActive(toggledOn);

		OnArmouryToggled(toggledOn);
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