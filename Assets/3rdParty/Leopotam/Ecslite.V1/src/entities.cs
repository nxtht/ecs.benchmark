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
    public struct EcsEntity {
        internal int Id;
        internal int Gen;
        internal EcsWorld World;
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    public static class EcsEntityExtensions {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static (int, int, EcsWorld) UnpackPointer (in this EcsEntity entity) {
            return (entity.Id, entity.Gen, entity.World);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool IsAlive (in this EcsEntity entity) {
            if (!IsWorldAlive (entity)) { return false; }
            return entity.World.IsEntityAlive (entity);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool IsWorldAlive (in this EcsEntity entity) {
            return entity.World != null && entity.World.IsAlive ();
        }

        public static void Destroy (in this EcsEntity entity) {
            var owner = entity.World;
            var id = entity.Id;

            ref var entityData = ref owner.GetEntityData (entity);
            // remove components first.
            if (entityData.Mask.Count > 0) {
                foreach (var typeIdx in entityData.Mask) {
                    ref var componentIdx = ref entityData.Components[typeIdx];
                    var pool = owner.Pools[typeIdx];
                    pool.Recycle (componentIdx);
                    componentIdx = 0;
                    pool.Entities.Unset (entity.Id);
                }
                entityData.Mask.Clear ();
#if DEBUG
                // for (var ii = 0; ii < savedEntity._owner.DebugListeners.Count; ii++) {
                //     owner.DebugListeners[ii].OnComponentListChanged (savedEntity);
                // }
#endif
            }
            owner.RecycleEntity (id);
#if DEBUG
            // for (var ii = 0; ii < savedEntity._owner.DebugListeners.Count; ii++) {
            //     owner.DebugListeners[ii].OnEntityDestroyed (savedEntity);
            // }
#endif
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void Del<T> (in this EcsEntity entity) where T : struct {
            var world = entity.World;
            ref var entityData = ref world.GetEntityData (entity);
            int typeId = EcsComponentType<T>.GetIdInWorld (world.Index);
            if (typeId <= 0) { return; }
            var pool = world.Pools[typeId];
            var itemId = entityData.Components[typeId];
            if (itemId <= 0) { return; }
            pool.Recycle (itemId);
            entityData.Mask.Unset (typeId);
            entityData.Components[typeId] = 0;
            pool.Entities.Unset (entity.Id);
#if DEBUG
            // for (var ii = 0; ii < savedEntity._owner.DebugListeners.Count; ii++) {
            //     owner.DebugListeners[ii].OnComponentListChanged (savedEntity);
            // }
#endif
            if (entityData.Mask.Count == 0) {
                world.RecycleEntity (entity.Id);
#if DEBUG
                // for (var ii = 0; ii < savedEntity._owner.DebugListeners.Count; ii++) {
                //     entity._owner.DebugListeners[ii].OnEntityDestroyed (savedEntity);
                // }
#endif
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static ref T Get<T> (in this EcsEntity entity) where T : struct {
            var world = entity.World;
            var typeId = EcsComponentType<T>.GetIdInWorld (world.Index);
#if DEBUG
            if (typeId <= 0) { throw new Exception ($"\"{typeof (T).Name}\" component not registered in world."); }
#endif
            ref var entityData = ref world.GetEntityData (entity);
            var itemIdx = entityData.Components[typeId];
            var pool = (EcsComponentPool<T>) world.Pools[typeId];
            if (itemIdx <= 0) {
                // create new instance.
                itemIdx = pool.Get ();
                entityData.Components[typeId] = itemIdx;
                entityData.Mask.Set (typeId);
                pool.Entities.Set (entity.Id);
            }
            return ref pool.Items[itemIdx];
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static ref T GetOnly<T> (in this EcsEntity entity) where T : struct {
            var world = entity.World;
            int typeId = EcsComponentType<T>.GetIdInWorld (world.Index);
#if DEBUG
            if (typeId <= 0) { throw new Exception ($"\"{typeof (T).Name}\" component not registered in world."); }
#endif
            ref var entityData = ref world.GetEntityData (entity);
#if DEBUG
            if (entityData.Components[typeId] <= 0) { throw new Exception ("component not exist."); }
#endif
            return ref ((EcsComponentPool<T>) world.Pools[typeId]).Items[entityData.Components[typeId]];
        }
    }
}