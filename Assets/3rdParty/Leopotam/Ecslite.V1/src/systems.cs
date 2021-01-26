// ----------------------------------------------------------------------------
// The MIT License
// Lite Entity Component System framework https://github.com/Leopotam/ecslite
// Copyright (c) 2017-2021 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Leopotam.EcsLite {
    public interface IEcsSystem { }

    public interface IEcsPreInitSystem : IEcsSystem {
        void PreInit (EcsWorlds worlds);
    }

    public interface IEcsInitSystem : IEcsSystem {
        void Init (EcsWorlds worlds);
    }

    public interface IEcsRunSystem : IEcsSystem {
        void Run (EcsWorlds worlds);
    }

    public interface IEcsDestroySystem : IEcsSystem {
        void Destroy (EcsWorlds worlds);
    }

    public interface IEcsPostDestroySystem : IEcsSystem {
        void AfterDestroy (EcsWorlds worlds);
    }

    public sealed class EcsSystems {
        readonly List<IEcsSystem> _allSystems;
        readonly List<IEcsRunSystem> _runSystems;

        public EcsSystems () {
            _allSystems = new List<IEcsSystem> (64);
            _runSystems = new List<IEcsRunSystem> (64);
        }

        public EcsSystems Add (IEcsSystem system) {
            _allSystems.Add (system);
            if (system is IEcsRunSystem runSystem) {
                _runSystems.Add (runSystem);
            }
            return this;
        }

        public void Init (EcsWorlds worlds) {
            // pre init.
            foreach (var system in _allSystems) {
                if (system is IEcsPreInitSystem preInitSystem) {
                    preInitSystem.PreInit (worlds);
#if DEBUG
                    var leaked = worlds.CheckLeaks ();
                    if (leaked != null) { throw new System.Exception ($"\"{leaked}\" in {preInitSystem.GetType ().Name}.PreInit()."); }
#endif
                }
            }
            // init.
            foreach (var system in _allSystems) {
                if (system is IEcsInitSystem initSystem) {
                    initSystem.Init (worlds);
#if DEBUG
                    var leaked = worlds.CheckLeaks ();
                    if (leaked != null) { throw new System.Exception ($"\"{leaked}\" in {initSystem.GetType ().Name}.Init()."); }
#endif
                }
            }
        }

        public void Run (EcsWorlds worlds) {
            foreach (var runSystem in _runSystems) {
                runSystem.Run (worlds);
#if DEBUG
                var leaked = worlds.CheckLeaks ();
                if (leaked != null) { throw new System.Exception ($"\"{leaked}\" in {runSystem.GetType ().Name}.Run()."); }
#endif
            }
        }

        public void Destroy (EcsWorlds worlds) {
            // destroy.
            foreach (var system in _allSystems) {
                if (system is IEcsDestroySystem destroySystem) {
                    destroySystem.Destroy (worlds);
#if DEBUG
                    var leaked = worlds.CheckLeaks ();
                    if (leaked != null) { throw new System.Exception ($"\"{leaked}\" in {destroySystem.GetType ().Name}.Destroy()."); }
#endif
                }
            }
            // post destroy.
            foreach (var system in _allSystems) {
                if (system is IEcsPostDestroySystem postDestroySystem) {
                    postDestroySystem.AfterDestroy (worlds);
#if DEBUG
                    var leaked = worlds.CheckLeaks ();
                    if (leaked != null) { throw new System.Exception ($"\"{leaked}\" in {postDestroySystem.GetType ().Name}.PreInit()."); }
#endif
                }
            }
        }
    }
}