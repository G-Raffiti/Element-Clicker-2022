using System;
using System.Collections.Generic;
using System.Linq;
using _Extensions;
using BigNumbers;
using Buildings;
using Trades;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Clover
{
    public enum ECloverType
    {
        Gold, GoldMax, ElementLowest, ElementAll, ElementMaxRandom, FreeBuilding, ProductionMulti,
    }
    
    public class Clover : MonoBehaviour, IPointerClickHandler
    {
        private float lifeTime = 30f;
        private float life;
        private bool isDestroyed;
        private bool isUsed;
        private bool initialized;
        private Animator anim;
        [SerializeField] private List<BuildingSO> buildings;
        private ECloverType type;
        public static event Action<BuildingSO> eLuckyLevel;
        public static event Action<float, float> eLuckyProductionMultiForTime;
        public static event Action<string> eOnLuckyClick;

        public void Initialize(PopUpUI popUpUI)
        {
            type = (ECloverType)Random.Range(0, Enum.GetValues(typeof(ECloverType)).Length);
            life = Time.time + lifeTime;
            anim = GetComponent<Animator>();
            eOnLuckyClick += popUpUI.PopUpMessage;
            initialized = true;
        }

        private void Update()
        {
            if (!initialized) return;
            if (Time.time >= life && !isDestroyed)
            {
                isDestroyed = true;
                Destroy(gameObject, 3f);
                anim.SetTrigger("Ends");
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(isUsed) return;
            isUsed = true;
            OnClick();
            life = 0;
        }

        private void OnClick()
        {
            string message = "";
            switch (type)
            {
                case ECloverType.Gold:
                    Gold(out message);
                    break;
                case ECloverType.GoldMax:
                    MaxGold(out message);
                    break;
                case ECloverType.ElementLowest:
                    LowestElement(out message);
                    break;
                case ECloverType.ElementAll:
                    AllElements(out message);
                    break;
                case ECloverType.ElementMaxRandom:
                    MaxRandomElement(out message);
                    break;
                case ECloverType.FreeBuilding:
                    BuildingSO building = buildings.GetRandom();
                    eLuckyLevel?.Invoke(building);
                    message = $"Lucky You!\n your {building.BuildingName} has Leveled Up for free";
                    break;
                case ECloverType.ProductionMulti:
                    eLuckyProductionMultiForTime?.Invoke(7,60);
                    message = $"Lucky You!\n your all your productions are Boosted for 1 minute";
                    break;
                default:
                    Gold(out message);
                    break;
            }
            
            eOnLuckyClick?.Invoke(message);
        }

        private static void MaxRandomElement(out string message)
        {
            List<EResource> elements = new List<EResource>()
                { EResource.Earth, EResource.Fire, EResource.Nature, EResource.Water };
            EResource randomEle = elements.GetRandom();
            Bank.UpgradeMax(randomEle, true);
            message =
                $"Lucky You, the Maximum {randomEle.ToString()} you can have is now {Bank.MaxStock[randomEle]}";
        }

        private void MaxGold(out string message)
        {
            Bank.UpgradeMax(EResource.Gold, true);
            message =
                $"Lucky You, the Maximum {EResource.Gold.ToString()} you can have is now {Bank.MaxStock[EResource.Gold]}";
        }

        private static void Gold(out string message)
        {
            Resource toProduce = new Resource();
            toProduce[EResource.Gold] = new bn(Bank.MaxStock[EResource.Gold]/2);
            Bank.Produce(toProduce);
            message = $"Lucky you, You Gain {toProduce.ToString()} !";
        }

        private static void LowestElement(out string message)
        {
            Resource toProduce = new Resource();
            Resource Elements = new Resource();
            Elements[EResource.Earth] = Bank.Stock[EResource.Earth];
            Elements[EResource.Fire] = Bank.Stock[EResource.Fire];
            Elements[EResource.Nature] = Bank.Stock[EResource.Nature];
            Elements[EResource.Water] = Bank.Stock[EResource.Water];

            EResource lowest = EResource.Gold;
            bn value = -1;
            foreach (EResource resource in Elements.Keys.Where(resource => Elements[resource] != 0))
            {
                if (value < Elements[resource])
                {
                    value = Elements[resource];
                    lowest = resource;
                }
            }

            toProduce[lowest] = new bn(Bank.MaxStock[lowest]);
            Bank.Produce(toProduce);
            message = $"Lucky you, You Gain {toProduce.ToString()} !";
        }

        private void AllElements(out string message)
        {
            Resource toProduce = new Resource();
            toProduce[EResource.Earth] = new bn(Bank.MaxStock[EResource.Earth]/2);
            toProduce[EResource.Fire] = new bn(Bank.MaxStock[EResource.Fire]/2);
            toProduce[EResource.Nature] = new bn(Bank.MaxStock[EResource.Nature]/2);
            toProduce[EResource.Water] = new bn(Bank.MaxStock[EResource.Water]/2);
            Bank.Produce(toProduce);
            message = $"Lucky you, You Gain {toProduce.ToString()} !";
        }
    }
}