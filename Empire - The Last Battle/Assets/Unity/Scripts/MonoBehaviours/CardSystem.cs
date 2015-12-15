using System;
using System.Collections.Generic;
using UnityEngine;

public enum Cards
{
	Healing_Card_1 = 1,
	Healing_Card_2 = 2,
	Resource_Card_100 = 100,
	Resource_Card_200 = 200,
	Resource_Card_300 = 300,
	Resource_Card_400 = 400,
	Resource_Card_500 = 500,
	Battle_Card_1 = 1,
	Tactic_Card_1 = 1,
	Tactic_Card_2 = 2,
	Tactic_Card_3 = 3,
	Alliance_Card_1 = 1,
	Scout_Card_1 = 1,
	Priority_Card_1 = 1,
	Upgrade_Card_1 = 1,
	Upgrade_Card_2 = 2
}

public class CardSystem : MonoBehaviour
{
	public Dictionary<int, GameObject> cardsLinker;

	public delegate void CardCallback(int amount, Cards card, Player player);
	public event CardCallback OnEffectApplied = delegate { };
	public event CardCallback OnHealingCardUsed = delegate { };

	public void ApplyEffect(Cards card, Player player) {
		switch (card) {
			case Cards.Healing_Card_1:
			case Cards.Healing_Card_2:
				RegisterCardHeal(card, player);
				break;
			case Cards.Resource_Card_100:
			case Cards.Resource_Card_200:
			case Cards.Resource_Card_300:
			case Cards.Resource_Card_400:
			case Cards.Resource_Card_500:
				UseResourceCard((int)card, player);
				break;
			case Cards.Battle_Card_1:
				UseBattleCard((int)card, player);
				OnEffectApplied(card, player);
				break;
			case Cards.Tactic_Card_1:
			case Cards.Tactic_Card_2:
			case Cards.Tactic_Card_3:
				UseTacticCard((int)card, player);
				OnEffectApplied(card, player);
				break;
			case Cards.Alliance_Card_1:
				UseAllianceCard((int)card, player);
				OnEffectApplied(card, player);
				break;
			case Cards.Scout_Card_1:
				UseScoutCard((int)card, player);
				OnEffectApplied(card, player);
				break;
			case Cards.Priority_Card_1:
				UsePriorityCard((int)card, player);
				OnEffectApplied(card, player);
				break;
			case Cards.Upgrade_Card_1:
			case Cards.Upgrade_Card_2:
				UseUpgradeCard((int)card, player);
				OnEffectApplied(card, player);
				break;
			default:
				break;
		}
	}

	private void RegisterCardHeal(Cards card, Player player) {
		OnHealingCardUsed ((int)card, card, player);
	}

	public void UseHealingCard(Player player, List<Unit> unitsToHeal) {
		foreach (var unit in unitsToHeal) {
			unit.Heal();
		}

	}

	private void UseResourceCard(int amount, Player player) {
		player.Currency.addPoints(amount);
	}

	private void UseBattleCard(int amount, Player player) {
		throw new NotImplementedException();
	}

	private void UseTacticCard(int amount, Player player) {
		throw new NotImplementedException();
	}

	private void UseAllianceCard(int amount, Player player) {
		throw new NotImplementedException();
	}

	private void UseScoutCard(int amount, Player player) {
		throw new NotImplementedException();
	}

	private void UsePriorityCard(int amount, Player player) {
		throw new NotImplementedException();
	}

	private void UseUpgradeCard(int amount, Player player) {
		throw new NotImplementedException();
	}
}