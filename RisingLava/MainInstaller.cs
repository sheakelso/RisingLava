using System;
using System.Collections.Generic;
using System.Text;
using Zenject;
using ComputerInterface.Interfaces;

namespace RisingLava
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IQueueInfo>().To<LavaQueue>().AsSingle();
        }
    }
}