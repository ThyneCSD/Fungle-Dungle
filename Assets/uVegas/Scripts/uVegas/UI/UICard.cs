using UnityEngine;
using UnityEngine.UI;
using uVegas.Core.Cards;

namespace uVegas.UI
{
    public class UICard : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image baseImage;
        [SerializeField] private Image rankImage;
        [SerializeField] private Image suitImage;


    [Header("Theme")]
        [SerializeField] private CardTheme defaultTheme;

        private Card currentCard;
        private CardTheme currentTheme;

        public void Init(Card card)
        {
            currentCard = card;

            if (currentTheme == null)
                currentTheme = defaultTheme;

            Refresh();
        }

        public void Init(Card card, CardTheme theme)
        {
            currentCard = card;
            currentTheme = theme != null ? theme : defaultTheme;

            Refresh();
        }

        public void SetTheme(CardTheme theme)
        {
            currentTheme = theme != null ? theme : defaultTheme;
            Refresh();
        }

        public void Refresh()
        {
            if (currentTheme == null)
            {
                Debug.LogError(
                    $"No CardTheme assigned on {gameObject.name}. Assign one in the Inspector."
                );
                return;
            }

            baseImage.sprite = currentTheme.baseImage;
            baseImage.color = currentTheme.frontColor;

            switch (currentCard.suit)
            {
                case Suit.Hidden:
                    suitImage.gameObject.SetActive(false);

                    rankImage.sprite = currentTheme.backImage;
                    rankImage.color = currentTheme.backColor;
                    return;

                case Suit.Joker:
                    suitImage.gameObject.SetActive(false);

                    rankImage.sprite = currentTheme.jokerImage;
                    rankImage.color = currentTheme.jokerColor;
                    return;

                default:
                    suitImage.gameObject.SetActive(true);

                    RankEntry? rankEntry = currentTheme.GetRank(currentCard.rank);
                    if (rankEntry.HasValue)
                    {
                        rankImage.sprite = rankEntry.Value.image;
                        rankImage.color = currentTheme.rankColor;
                    }

                    SuitEntry? suitEntry = currentTheme.GetSuit(currentCard.suit);
                    if (suitEntry.HasValue)
                    {
                        suitImage.sprite = suitEntry.Value.image;
                        suitImage.color = suitEntry.Value.color;
                    }
                    break;
            }
        }

        public void Reveal(Card card)
        {
            currentCard = card;
            Refresh();
        }
    }


}
