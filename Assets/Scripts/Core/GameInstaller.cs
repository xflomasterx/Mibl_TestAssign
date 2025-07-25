using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private PlayerController playerController;
    [SerializeField] int BulletPoolSize = 20;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<UIManager>().FromComponentInHierarchy().AsSingle();
#if UNITY_ANDROID || UNITY_IOS 
        Container.Bind<IInputService>().To<OverlayInputService>().AsSingle();
#else
        Container.Bind<IInputService>().To<KeyboardInputService>().AsSingle();
#endif
        Container.Bind<ILayoutGenerator>().To<LinearLayoutGenerator>().AsSingle();
        Container.Bind<ILevelActorSpawner>().To<LevelActorSpawner>().AsSingle();
        Container.Bind<ILevelRenderer>().To<LevelRenderer>().FromComponentInHierarchy().AsSingle();       
        Container.Bind<ILevelCompletionController>().To<LevelCompletionController>().AsSingle() ;
        Container.Bind<IConfigProvider>().To<ConfigProvider>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerController>().FromComponentInNewPrefab(playerController).AsSingle();
        Container.BindMemoryPool<Bullet, Bullet.Pool>()
            .WithInitialSize(BulletPoolSize)
            .FromComponentInNewPrefab(bulletPrefab)
            .UnderTransformGroup("Bullets");
    }
}