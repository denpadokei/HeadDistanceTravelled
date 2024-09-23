using HeadDistanceTravelled.Databases;
using HeadDistanceTravelled.Models;
using Zenject;

namespace HeadDistanceTravelled.Installers
{
    internal class HDTAppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<HDTDatabase>().AsSingle();
            this.Container.BindInterfacesAndSelfTo<ManualMeasurementController>().AsSingle();
        }
    }
}
