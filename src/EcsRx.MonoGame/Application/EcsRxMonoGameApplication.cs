﻿using System;
using System.Reactive.Linq;
using EcsRx.Infrastructure;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Ninject;
using EcsRx.MonoGame.Modules;
using EcsRx.MonoGame.Wrappers;
using EcsRx.Plugins.ReactiveSystems;
using EcsRx.Plugins.Views;
using EcsRx.Plugins.Views.Extensions;

namespace EcsRx.MonoGame.Application
{
    public abstract class EcsRxMonoGameApplication : EcsRxApplication, IDisposable
    {
        public override IDependencyContainer Container { get; } = new NinjectDependencyContainer();

        protected IEcsRxGame EcsRxGame { get; }
        protected IEcsRxContentManager EcsRxContentManager => EcsRxGame.EcsRxContentManager;
        protected IEcsRxGraphicsDeviceManager DeviceManager => EcsRxGame.EcsRxGraphicsDeviceManager;

        public EcsRxMonoGameApplication()
        {
            EcsRxGame = new EcsRxGame();
            EcsRxGame.GameLoading.FirstAsync().Subscribe(x => StartApplication());
            EcsRxGame.Run();
        }

        protected override void StartSystems()
        { this.StartAllBoundViewSystems(); }

        protected override void LoadPlugins()
        {
            RegisterPlugin(new ReactiveSystemsPlugin());
            RegisterPlugin(new ViewsPlugin());
        }

        protected override void LoadModules()
        {
            Container.LoadModule(new MonoGameModule(EcsRxGame));
            base.LoadModules();
        }
        
        public void Dispose()
        { EcsRxGame.Dispose(); }
    }
}