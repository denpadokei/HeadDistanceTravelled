using Zenject;

namespace HeadDistanceTravelled.Installers
{
    public class HDTGameInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<HeadDistanceTravelledController>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
        }
    }
}
