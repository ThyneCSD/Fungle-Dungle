using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Add all 52 card prefabs here")]
    public List<GameObject> deck = new List<GameObject>();
    public List<GameObject> handCards = new List<GameObject>();

    private void Start()
    {
        ShuffleDeck();
        DrawAndSpawnCard(cardSpawnPoint.position);
        DrawAndSpawnCard(cardSpawnPoint.position);

    }

    public Transform cardSpawnPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawAndSpawnCard(cardSpawnPoint.position);
        }
            Debug.Log("Hand value: " + CalculateHandValue());
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIndex = Random.Range(i, deck.Count);

            GameObject temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }

        Debug.Log("Deck shuffled!");
    }

    public GameObject DrawCard()
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("Deck is empty!");
            return null;
        }

        GameObject drawnCard = deck[0];
        deck.RemoveAt(0);

        CardData cardData = drawnCard.GetComponent<CardData>();

        if (cardData != null)
        {
            Debug.Log(
                "Drew: " +
                cardData.cardName +
                " | Value: " +
                cardData.cardValue
            );
        }

        return drawnCard;
    }

    public int DrawCardValue()
    {
        GameObject card = DrawCard();

        if (card == null)
            return 0;

        CardData data = card.GetComponent<CardData>();

        if (data != null)
            return data.cardValue;

        return 0;
    }
    public GameObject DrawAndSpawnCard(Vector3 spawnPosition)
    {
        GameObject cardPrefab = DrawCard();

        if (cardPrefab == null)
            return null;

        Vector3 offset = new Vector3(handCards.Count * 2f, 0, 0);

        GameObject spawnedCard = Instantiate(
            cardPrefab,
            spawnPosition + offset,
            Quaternion.identity
        );

        handCards.Add(spawnedCard);

        return spawnedCard;
    }

    public int CalculateHandValue()
    {
        int total = 0;

        foreach (GameObject card in handCards)
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