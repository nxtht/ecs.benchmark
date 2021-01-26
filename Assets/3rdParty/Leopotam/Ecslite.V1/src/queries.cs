// ----------------------------------------------------------------------------
// The MIT License
// Lite Entity Component System framework https://github.com/Leopotam/ecslite
// Copyright (c) 2017-2021 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Leopotam.EcsLite {
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
    //[Il2CppSetOption (Option.DivideByZeroChecks, false)]
#endif
    public sealed class EcsQuery {
        readonly EcsWorld _world;
        readonly EcsBitSet _include;
        readonly EcsBitSet _exclude;
        EcsSparseSet _entities;
        int _entitiesCount;
        EcsEntity _it;

        internal EcsQuery (EcsWorld world, int capacity) {
            _world = world;
            _include = new EcsBitSet (capacity);
            _exclude = new EcsBitSet (capacity);
            _entities = null;
            _entitiesCount = int.MaxValue;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        void Recycle () {
            _it = default;
            _entities = null;
            _entitiesCount = int.MaxValue;
            _include.Clear ();
            _exclude.Clear ();
            _world.RecycleQuery (this);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public EcsQuery With<T> () where T : struct {
            var typeId = EcsComponentType<T>.GetIdInWorld (_world.Index);
#if DEBUG
            if (typeId <= 0) { throw new Exception ($"invalid query: \"{typeof (T).Name}\" component not registered in world."); }
            if (_include.Get (typeId) || _exclude.Get (typeId)) {
                throw new Exception ($"invalid query: \"{typeof (T).Name}\" constraint already used.");
            }
#endif
            _include.Set (typeId);
            var pool = _world.Pools[typeId];
            var poolCount = pool.Entities.GetCount ();
            if (poolCount < _entitiesCount) {
                _entities = pool.Entities;
                _entitiesCount = poolCount;
            }
            return this;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public EcsQuery Without<T> () where T : struct {
            var typeId = EcsComponentType<T>.GetIdInWorld (_world.Index);
#if DEBUG
            if (typeId <= 0) { throw new Exception ($"invalid query: \"{typeof (T).Name}\" component not registered in world."); }
            if (_include.Get (typeId) || _exclude.Get (typeId)) {
                throw new Exception ($"Invalid query: \"{typeof (T).Name}\" constraint already used.");
            }
#endif
            _exclude.Set (typeId);
            return this;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator () {
#if DEBUG
            if (_it.World != null) { throw new Exception ("query iterator already in use."); }
#endif
            _it.World = _world;
            _it.Gen = 0;
            _it.Id = 0;
            return new Enumerator (this);
        }

        public struct Enumerator : IDisposable {
            readonly EcsQuery _query;
            readonly EcsWorld _world;
            readonly EcsSparseSet _entities;
            int _idx;
            readonly int _count;

            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            internal Enumerator (EcsQuery query) {
                _query = query;
                _world = query._world;
                _entities = query._entities;
                _idx = 0;
                _count = _entities != null ? query._entitiesCount + 1 : 0;
            }

            public ref EcsEntity Current {
#if ENABLE_IL2CPP
            [Il2CppSetOption (Option.NullChecks, false)]
#endif
                [MethodImpl (MethodImplOptions.AggressiveInlining)]
                get { return ref _query._it; }
            }

#if ENABLE_IL2CPP
            [Il2CppSetOption (Option.NullChecks, false)]
#endif
            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            public void Dispose () {
                _query.Recycle ();
            }

#if ENABLE_IL2CPP
            [Il2CppSetOption (Option.NullChecks, false)]
            [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            public bool MoveNext () {
                _idx++;
                for (; _idx < _count; _idx++) {
                    var entityIdx = _entities.Dense[_idx];
                    // ref var entityData = ref _world.GetEntityDataById (entityIdx);
                    ref var entityData = ref _world._entities[entityIdx]; 
                    if (entityData.Mask.Contains (_query._include)
                        && (_query._exclude.Count == 0 || !entityData.Mask.Intersects (_query._exclude))) {
                        _query._it.Id = entityIdx;
                        _query._it.Gen = entityData.Gen;
                        return true;
                    }
                }
                return false;
            }
        }
    }
}