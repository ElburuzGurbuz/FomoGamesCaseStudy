using TMPro;
using UnityEngine;
using Core;

namespace InGame
{
    public class MoveLimitUIItem : UIItem
    {
        [SerializeField]
        private TextMeshProUGUI _moveLimitText;

        public override void InitializeUIItems()
        {
            base.InitializeUIItems();
            // Ba�lang��ta UI'� gizli ba�lat
            CloseItem();
        }

        public void UpdateText(int remainingMoves)
        {
            if (_moveLimitText != null)
            {
                _moveLimitText.text = ""+remainingMoves;
            }
        }
    }
}