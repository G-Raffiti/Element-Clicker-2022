using System;
using System.Collections.Generic;
using _Extensions;
using _SaveSystem;
using _SaveSystem.Data;
using _SaveSystem.SerializableClasses;
using Buildings;
using Singletons;
using Trades;
using Trees;
using UnityEngine;
using Upgrades;

namespace Managers
{
     [Serializable]
     public struct BuildingTrees
     {
          public BuildingTree Tree;
          public BuildingSO Building;
     }

     [Serializable]
     public class BuildingManager : MonoBehaviour, IDataPersistence
     {
          [SerializeField] private BuildingBtn _building_PF;
          [SerializeField] private Transform _buttonsHolder;
          [SerializeField] [NonReorderable] private List<BuildingTrees> _buildings;
          [SerializeField] private Totem totem;
          private List<Balance.BuildingUnlock> _buildingToUnlock = new List<Balance.BuildingUnlock>();

          public List<BuildingBtn> BuildingsBtns;
          public static event Action eOnGameLoaded;

          private void Awake()
          {
               foreach (BuildingTrees buildingTrees in _buildings)
               {
                    GameObject building = Instantiate(_building_PF.gameObject, _buttonsHolder);
                    buildingTrees.Tree.gameObject.SetActive(true);
                    building.GetComponent<BuildingBtn>().Initialize(buildingTrees.Building, buildingTrees.Tree);
                    BuildingsBtns.Add(building.GetComponent<BuildingBtn>());
                    buildingTrees.Tree.gameObject.SetActive(false);
                    building.gameObject.SetActive(false);
               }

               foreach (Balance.BuildingUnlock bu in Balance.Instance.BuildingToUnlock)
               {
                    _buildingToUnlock.Add(bu);
               }
               
               totem.gameObject.SetActive(false);
          }

          public void OnUnlock(BuildingSO buildingSO)
          {
               foreach (BuildingBtn building in BuildingsBtns)
               {
                    if(building.Building.BuildingSO == buildingSO)
                         building.gameObject.SetActive(true);
               }
          }

          private void OnEnable()
          {
               SpecialUpgradeFactors.eOnBankValueChange += UnlockBuilding;
               UpgradeSOUnlock.eUnlockBuilding += OnUnlock;
               BuildingBtn.eOpenTree += OpenTree;
          }

          private void OpenTree(BuildingSO obj)
          {
               foreach (BuildingTrees building in _buildings)
               {
                    building.Tree.gameObject.SetActive(false);
               }

               GameObject tree = _buildings.Find(bt => bt.Building == obj).Tree.gameObject;
               tree.SetActive(true);
          }

          private void UnlockBuilding(EResource bankResource)
          {
               if(bankResource == EResource.Air && !totem.gameObject.activeSelf) 
                    totem.gameObject.SetActive(true);
               
               for (int i = 0; i < _buildingToUnlock.Count; i++)
               {
                    if (_buildingToUnlock[i].resource != bankResource) continue;
                    if (RunStats.Instance.Produced[_buildingToUnlock[i].resource] < _buildingToUnlock[i].value)
                         continue;
                    OnUnlock(_buildingToUnlock[i].building);
                    _buildingToUnlock.RemoveAt(i);
               }
               
               /*if(_buildingToUnlock.Count == 0 && totem.gameObject.activeSelf)
               {
                    SpecialUpgradeFactors.eOnBankValueChange -= UnlockBuilding;
               }*/
          }

          public void SaveData(GameData data)
          {
               data.Buildings = new List<BuildingSerializable>();
               foreach (BuildingBtn buildingBtn in BuildingsBtns)
               {
                    BuildingSerializable building = new (buildingBtn.Building, buildingBtn.gameObject.activeSelf);
                    data.Buildings.Add(building);
               }
          }

          public void LoadData(GameData data)
          {
               foreach (BuildingBtn buildingBtn in BuildingsBtns)
               {
                    buildingBtn.gameObject.SetActive(true);
                    BuildingSerializable building =
                         data.Buildings.Find(b => b.buildingSO == buildingBtn.Building.BuildingSO);
                    buildingBtn.Building.LoadData(building);
               }

               foreach (BuildingBtn buildingBtn in BuildingsBtns)
               {
                    BuildingSerializable building =
                         data.Buildings.Find(b => b.buildingSO == buildingBtn.Building.BuildingSO);
                    buildingBtn.gameObject.SetActive(building.IsActive);
               }
               
               SpecialUpgradeFactors.eOnBankValueChange += UnlockBuilding;
               eOnGameLoaded?.Invoke();
               
          }
     }
}