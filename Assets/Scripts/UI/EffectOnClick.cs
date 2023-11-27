using Buildings;
using Trades;
using UnityEngine;

namespace UI
{
    public class EffectOnClick : MonoBehaviour
    {
        [SerializeField] private Coin _coin_PF;
        
        private void OnEnable()
        {
            MainBtn.eOnClicked += SendCoin;
        }

        private void SendCoin(EResource resource, string value)
        {
            Coin coin = Instantiate(_coin_PF, transform);
            coin.SetValue(resource, value);
        }
    }
}