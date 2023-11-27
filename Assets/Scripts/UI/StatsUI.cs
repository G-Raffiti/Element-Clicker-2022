using Singletons;
using TMPro;
using Trades;
using UnityEngine;
using Upgrades;

namespace UI
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        private Resource chanceOnClick = new Resource();

        private void OnEnable()
        {
            UpgradeSOChanceOnClick.eChanceValue += UpdateChance;
        }

        private void Awake()
        {
            chanceOnClick[EResource.Gold] = 5;
            chanceOnClick[EResource.Fire] = 2;
            chanceOnClick[EResource.Nature] = 2;
            chanceOnClick[EResource.Water] = 2;
            chanceOnClick[EResource.Earth] = 2;
        }

        private void UpdateChance(EResource arg1, int arg2)
        {
            chanceOnClick[arg1] += arg2;
        }

        private void Update()
        {
            string str = "";
        
            str += "Run Started the:" + RunStats.Instance.StartRun.ToString("G");
            str += "\n\nProduced since the beginning of the run:\n" + RunStats.Instance.Produced.ToString();
            str += "\n\nChance to find elments on clicks:\n" + chanceOnClick.ToString();
            _text.text = str;
        }
    }
}
