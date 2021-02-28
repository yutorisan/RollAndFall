using RAF.Inventory;
using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        CoinInventory coinInventory = new CoinInventory();
        TimeInventory timeInventory = new TimeInventory();

        Container.Bind<IObservableCoinInventory>()
                 .To<CoinInventory>()
                 .FromInstance(coinInventory)
                 .AsCached();
        Container.Bind<IObservableTimeInventory>()
                 .To<TimeInventory>()
                 .FromInstance(timeInventory)
                 .AsCached();
        Container.Bind<ICoinInventoryAddable>()
                 .To<CoinInventory>()
                 .FromInstance(coinInventory)
                 .AsCached();
        Container.Bind<ITimeInventoryAddable>()
                 .To<TimeInventory>()
                 .FromInstance(timeInventory)
                 .AsCached();
        Container.Bind<ITimeInventorySpendable>()
                 .To<TimeInventory>()
                 .FromInstance(timeInventory)
                 .AsCached();
    }
}