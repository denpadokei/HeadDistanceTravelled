using HeadDistanceTravelled.Databases;
using HeadDistanceTravelled.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
