// ----------------------------------------------------------------------------
// The MIT License
// Lite Entity Component System framework https://github.com/Leopotam/ecslite
// Copyright (c) 2017-2021 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Leopotam.EcsLite {
    public struct EcsWorldConfig {
        public int EntitiesCount;
        public int ComponentsCount;
        public const int DefaultEntitiesCount = 512;
        public const int DefaultComponentsCount = 256;
    }

    struct EcsEntityData {
        public int Gen;
        public EcsBitSet Mask;
        public int[] Components;
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    static class EcsWorldPool {
        static object _allSyncObject;
        static EcsWorld[] _allWorlds;
        static int _allWorldsCount;
        static int[] _recycledAllWorlds;
        static int _recycledAllWorldsCount;

        static EcsWorldPool () {
            _allSyncObject = new object ();
            _allWorlds = new EcsWorld[64];
            _allWorldsCount = 1;
            _recycledAllWorlds = new int[64];
            _recycledAllWorldsCount = 0;
        }

        public static int Get (in EcsWorldConfig cfg) {
            lock (_allSyncObject) {
                int idx;
                if (_recycledAllWorldsCount > 0) {
                    idx = _recycledAllWorlds[--_recycledAllWorldsCount];
                } else {
                    if (_allWorlds.Length == _allWorldsCount) {
                        Array.Resize (ref _allWorlds, _allWorldsCount << 1);
                    }
                    idx = _allWorldsCount++;
                }
                var world = new EcsWorld (idx, cfg);
                _allWorlds[idx] = world;
                return idx;
            }
        }

        public static void Recycle (int worldId) {
            lock (_allSyncObject) {
                if (_recycledAllWorlds.Length == _recycledAllWorldsCount) {
                    Array.Resize (ref _recycledAllWorlds, _recycledAllWorldsCount << 1);
                }
                _recycledAllWorlds[_recycledAllWorldsCount++] = worldId;
                _allWorlds[worldId] = null;
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static EcsWorld ById (int id) {
            return id > 0 && id < _allWorldsCount ? _allWorlds[id] : null;
        }
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    public class EcsWorlds {
        readonly Dictionary<int, int> _worlds;
        readonly object _sharedData;

        public EcsWorlds (object sharedData = null) {
            _sharedData = sharedData;
            _worlds = new Dictionary<int, int> (16);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public T Shared<T> () where T : class {
            return _sharedData as T;
        }

        public EcsWorld Create (string name = "Default", in EcsWorldConfig cfg = default) {
            var hash = name.GetHashCode ();
#if DEBUG
            if (_worlds.TryGetValue (hash, out _)) { throw new Exception ($"\"{name}\" world already created."); }
#endif
            var worldId = EcsWorldPool.Get (cfg);
            _worlds[hash] = worldId;
            return EcsWorldPool.ById (worldId);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public EcsWorld Get (string name = "Default") {
            return _worlds.TryGetValue (name.GetHashCode (), out var idx) ? EcsWorldPool.ById (idx) : null;
        }

        public void Init () {
            foreach (var pair in _worlds) {
                EcsWorldPool.ById (pair.Value).Init ();
            }
        }

        public void Destroy () {
#if DEBUG
            var leaked = CheckLeaks ();
            if (leaked != null) { throw new Exception ($"\"{leaked}\" - before EcsWorlds.Destroy()."); }
#endif
            foreach (var pair in _worlds) {
                var world = EcsWorldPool.ById (pair.Value);
                if (world != null) {
                    world.Destroy ();
                    EcsWorldPool.Recycle (world.Index);
                }
            }
        }

#if DEBUG
        public string CheckLeaks () {
            foreach (var pair in _worlds) {
                var world = EcsWorldPool.ById (pair.Value);
                if (world.LeakedEntities.Count > 0) {
                    for (int i = 0, iMax = world.LeakedEntities.Count; i < iMax; i++) {
                        if (world.GetEntityDataById (world.LeakedEntities[i].Id).Mask.Count == 0) {
                            return "empty entity detected";
                        }
                    }
                    world.LeakedEntities.Clear ();
                }
                if (world.QueriesRequested > 0) {
                    return "opened queries detected";
                }
            }
            return null;
        }
#endif
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    public sealed class EcsWorld {
        public readonly int Index;
        bool _inited;
        bool _destroyed;

        // zero item not in use.
        internal EcsEntityData[] _entities;
        int _entitiesCount;
        int[] _recycledEntities;
        int _recycledEntitiesCount;
        EcsQuery[] _queries;
        int _queriesCount;
#if DEBUG
        internal readonly List<EcsEntity> LeakedEntities = new List<EcsEntity> (256);
        internal int QueriesRequested;
#endif

        // zero item not in use.
        internal EcsComponentPoolBase[] Pools;
        int _poolsCount;

        EcsWorldConfig _config;

        internal EcsWorld (int idx, in EcsWorldConfig cfg) {
            _config = cfg;
            Index = idx;
            _entities = Array.Empty<EcsEntityData> ();
            var capacity = cfg.ComponentsCount > 0 ? EcsMath.Pot (cfg.ComponentsCount + 1) : EcsWorldConfig.DefaultComponentsCount;
            Pools = new EcsComponentPoolBase[capacity];
            _poolsCount = 1;
            _queries = new EcsQuery[32];
            _queriesCount = 0;
        }

        internal void Init () {
#if DEBUG
            if (_inited) { throw new Exception ("Already initialized."); }
#endif
            var capacity = _config.EntitiesCount > 0 ? EcsMath.Pot (_config.EntitiesCount + 1) : EcsWorldConfig.DefaultEntitiesCount;
            EnsureEntitiesCapacity (capacity);
            _entitiesCount = 1;
            _recycledEntities = new int[_entities.Length];
            _recycledEntitiesCount = 0;

            _inited = true;
        }

        internal void Destroy () {
#if DEBUG
            if (!_inited) { throw new Exception ("Not initialized."); }
            if (_destroyed) { throw new Exception ("Already destroyed."); }
#endif
            EcsEntity entity;
            entity.World = this;
            for (var i = _entitiesCount - 1; i > 0; i--) {
                ref var entityData = ref _entities[i];
                if (entityData.Mask.Count > 0) {
                    entity.Id = i;
                    entity.Gen = entityData.Gen;
                    entity.Destroy ();
                }
            }
            _entitiesCount = 1;
            for (var i = 1; i < _poolsCount; i++) {
                Pools[i].UnregisterWorld (Index);
                Pools[i] = null;
            }
            _poolsCount = 1;
            _destroyed = true;
        }

        void EnsureEntitiesCapacity (int capacity) {
            if (_entities.Length < capacity) {
                var newCapacity = EcsMath.Pot (capacity);
                Array.Resize (ref _entities, newCapacity);
                for (int i = 1, iMax = _poolsCount; i < iMax; i++) {
                    Pools[i].Entities.EnsureSparseCapacity (newCapacity);
                }
            }
        }

        internal void RecycleEntity (int id) {
            ref var data = ref _entities[id];
            data.Mask.Count = -1;
            data.Gen++;
            if (data.Gen == 0) { data.Gen = 1; }
            if (_recycledEntities.Length == _recycledEntitiesCount) {
                Array.Resize (ref _recycledEntities, _recycledEntitiesCount << 1);
            }
            _recycledEntities[_recycledEntitiesCount++] = id;
        }

        internal void RecycleQuery (EcsQuery query) {
            if (_queries.Length == _queriesCount) {
                Array.Resize (ref _queries, _queriesCount << 1);
            }
            _queries[_queriesCount++] = query;
#if DEBUG
            QueriesRequested--;
#endif
        }

        internal bool IsEntityAlive (in EcsEntity entity) {
            if (entity.World != this || !IsAlive () || entity.Id <= 0 || entity.Id >= _entitiesCount) { return false; }
            ref var entityData = ref _entities[entity.Id];
            return entityData.Gen == entity.Gen && entityData.Mask.Count >= 0;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool IsAlive () {
            return _inited && !_destroyed;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        internal ref EcsEntityData GetEntityData (in EcsEntity entity) {
#if DEBUG
            if (entity.World != this) { throw new Exception ("invalid world."); }
            if (!_inited) { throw new Exception ("world not initialized."); }
            if (_destroyed) { throw new Exception ("world already destroyed."); }
            if (entity.Id <= 0 || entity.Id >= _entitiesCount) { throw new Exception ("invalid entity."); }
            if (entity.Gen != _entities[entity.Id].Gen) { throw new Exception ("entity was deleted."); }
#endif
            return ref _entities[entity.Id];
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        internal ref EcsEntityData GetEntityDataById (int entityId) {
#if DEBUG
            if (!_inited) { throw new Exception ("world not initialized."); }
            if (_destroyed) { throw new Exception ("world already destroyed."); }
            if (entityId <= 0 || entityId >= _entitiesCount) { throw new Exception ($"invalid entity: {entityId}."); }
#endif
            return ref _entities[entityId];
        }

        public bool IsInited () {
            return _inited;
        }

        public bool IsDestroyed () {
            return _destroyed;
        }

        public EcsEntity NewEntity () {
#if DEBUG
            if (!_inited) { throw new Exception ("Not initialized."); }
            if (_destroyed) { throw new Exception ("Already destroyed."); }
#endif
            EcsEntity entity;
            entity.World = this;
            if (_recycledEntitiesCount > 0) {
                var id = _recycledEntities[--_recycledEntitiesCount];
                ref var entityData = ref _entities[id];
                entityData.Mask.Count = 0;
                entity.Id = id;
                entity.Gen = entityData.Gen;
                return entity;
            }
            if (_entities.Length == _entitiesCount) {
                EnsureEntitiesCapacity (_entitiesCount << 1);
            }
            ref var newData = ref _entities[_entitiesCount];
            entity.Id = _entitiesCount++;
            entity.Gen = 1;
            newData.Gen = 1;
            newData.Mask = new EcsBitSet (_poolsCount);
            newData.Components = new int[_poolsCount];
#if DEBUG
            LeakedEntities.Add (entity);
            // foreach (var debugListener in DebugListeners) {
            //     debugListener.OnEntityCreated (entity);
            // }
#endif
            return entity;
        }

        public EcsQuery Query () {
#if DEBUG
            if (!_inited) { throw new Exception ("world not initialized."); }
            if (_destroyed) { throw new Exception ("world already destroyed."); }
            QueriesRequested++;
#endif
            if (_queriesCount > 0) {
                return _queries[--_queriesCount];
            }
            return new EcsQuery (this, _poolsCount);
        }

        public EcsWorld Register<T> () where T : struct {
#if DEBUG
            if (_inited) { throw new Exception ("world already initialized."); }
            if (_destroyed) { throw new Exception ("world already destroyed."); }
            if (EcsComponentType<T>.GetIdInWorld (Index) > 0) { throw new Exception ("type already registered."); }
#endif
            if (Pools.Length == _poolsCount) {
                Array.Resize (ref Pools, _poolsCount << 1);
            }
            EcsComponentType<T>.Register (Index, _poolsCount);
            var pool = new EcsComponentPool<T> (_config.EntitiesCount > 0 ? _config.EntitiesCount : EcsWorldConfig.DefaultEntitiesCount);
            Pools[_poolsCount++] = pool;
            return this;
        }
    }
}