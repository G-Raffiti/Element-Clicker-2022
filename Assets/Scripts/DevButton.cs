using System;
using Trades;
using UnityEngine;
using UnityEngine.EventSystems;

public class DevButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Resource.SResource SResource;
    private Resource Resource;

    private void Start()
    {
        Resource = new Resource(SResource);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Bank.Produce(Resource);
    }
}