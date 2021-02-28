using RAF;
using RAF.Camera;
using RAF.Player;
using UnityEngine;
using Zenject;

public class RAFInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ITimeOperator>()
                 .To<TimeManager>()
                 .FromComponentInHierarchy()
                 .AsCached();
        Container.Bind<ISubCameraFollower>()
                 .To<SubCameraFollower>()
                 .FromComponentInHierarchy()
                 .AsCached();
        Container.Bind<IObservableGetItem>()
                 .To<ItemCollisionDetector>()
                 .FromComponentInHierarchy()
                 .AsCached();
    }
}