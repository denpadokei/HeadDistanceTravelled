using HeadDistanceTravelled.Views;
using Zenject;

namespace HeadDistanceTravelled.Installers
{
    public class HDTMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<HDTFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<HeadDistanceTravelledMainViewController>().FromNewComponentAsViewController().AsSingle();
            this.Container.BindInterfacesAndSelfTo<HDTSettingView>().FromNewComponentAsViewController().AsSingle().NonLazy();
        }
    }
}
