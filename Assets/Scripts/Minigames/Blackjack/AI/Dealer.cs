using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public DeckManager deckManager;
    public Transform dealerSpawnPoint;

    public List<GameObject> dealerHand = new List<GameObject>();

    void Start()
    {
        DrawDealerCard();
        DrawDealerCard();
    }

    public void DrawDealerCard()
    {
        GameObject cardPrefab = deckManager.DrawCard();

        if (cardPrefab == null)
            return;

        Vector3 offset = new Vector3(dealerHand.Count * 2f, 0, 0);

        GameObject spawnedCard = Instantiate(
            cardPrefab,
            dealerSpawnPoint.position + offset,
            Quaternion.identity
        );

        dealerHand.Add(spawnedCard);
    }

    public int CalculateDealerValue()
    {
        int total = 0;

        foreach (GameObject card in dealerHand)
        {
            CardData data = card.GetComponent<CardData>();

            if (data != null)
            {
                total += data.cardValue;
            }
        }

        return total;
    }
}