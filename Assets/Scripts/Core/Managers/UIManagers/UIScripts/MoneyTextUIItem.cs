using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Core
{
    public class MoneyTextUIItem : UIItem
    {
        [SerializeField]
        private TextMeshProUGUI _moneyText;

        public void SetText(int money)
        {
            _moneyText.text = money.ToString();
        }
    }
}