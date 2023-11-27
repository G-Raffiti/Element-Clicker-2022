using System;
using Trades;

namespace _Extensions
{
    public static class StringExtension
    {
        public static string String(this EResource resource)
        {
            switch (resource)
            {
                case EResource.Gold:
                    return "<sprite name=gold>";
                case EResource.Nature:
                    return "<sprite name=nature>";
                case EResource.Water:
                    return "<sprite name=water>";
                case EResource.Fire:
                    return "<sprite name=fire>";
                case EResource.Earth:
                    return "<sprite name=earth>";
                case EResource.Air:
                    return "<sprite name=air>";
                case EResource.Time:
                    return "<sprite name=time>";
                case EResource.Rage:
                    return "<sprite name=rage>";
                case EResource.Tech:
                    return "<sprite name=tech>";
                case EResource.Ether:
                    return "<sprite name=ether>";
                case EResource.Mana:
                    return "<sprite name=mana>";
                case EResource.Click:
                    return "Click";
                default:
                    return "";
            }
        }
    }
}