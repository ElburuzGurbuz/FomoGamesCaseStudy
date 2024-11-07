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
            // Baþlangýçta UI'ý gizli baþlat
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