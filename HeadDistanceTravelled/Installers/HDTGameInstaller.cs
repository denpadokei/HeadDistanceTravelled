using HeadDistanceTravelled.Configuration;
using HeadDistanceTravelled.Views;
#if VER_1_18_0
using SiraUtil;
#endif
using Zenject;

namespace HeadDistanceTravelled.Installers
{
    public class HDTGameInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<HeadDistanceTravelledController>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
            if (PluginConfig.Instance.ShowDistanceOnHMD) {
                this.Container.BindInterfacesAndSelfTo<HMDDistanceFloatingScreen>().FromNewComponentAsViewController().AsCached().NonLazy();
            }
        }
    }
}
