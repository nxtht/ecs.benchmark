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
    public interface IEcsAutoReset<T> where T : struct {
        void AutoReset (ref T c);
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    abstract class EcsComponentPoolBase {
        public readonly EcsSparseSet Entities;

        public EcsComponentPoolBase (int entitiesCapacity) {
            Entities = new EcsSparseSet (entitiesCapacity, entitiesCapacity);
        }

        public abstract void UnregisterWorld (int worldId);

        public abstract void Recycle (int id);
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    static class EcsComponentType<T> where T : struct {
        // ReSharper disable StaticMemberInGenericType
        public static readonly Type Type;
        public static readonly bool IsAutoReset;
        static int[] _worlds;
        static object _syncObj;
        // ReSharper restore StaticMemberInGenericType

        static EcsComponentType () {
            Type = typeof (T);
            IsAutoReset = typeof (IEcsAutoReset<T>).IsAssignableFrom (Type);
            _worlds = new int[64];
            _syncObj = new object ();
#if DEBUG
            if (!IsAutoReset && Type.GetInterface ("IEcsAutoReset`1") != null) {
                throw new Exception ($"IEcsAutoReset should have <{typeof (T).Name}> constraint for component \"{typeof (T).Name}\".");
            }
#endif
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static int GetIdInWorld (int worldId) {
            // ReSharper disable InconsistentlySynchronizedField
            // no lock - race condition should not break anything, previous array
            // contains same data except new registration.
#if DEBUG
            if (worldId <= 0 || worldId >= _worlds.Length) { return 0; }
#endif
            return _worlds[worldId];
            // ReSharper restore InconsistentlySynchronizedField
        }

        public static void Register (int worldId, int typeId) {
            lock (_syncObj) {
                if (worldId >= _worlds.Length) {
                    Array.Resize (ref _worlds, EcsMath.Pot (worldId));
                }
                _worlds[worldId] = typeId;
            }
        }

        public static void Unregister (int worldId) {
            lock (_syncObj) {
                _worlds[worldId] = 0;
            }
        }
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    sealed class EcsComponentPool<T> : EcsComponentPoolBase where T : struct {
        // zero item not in use.
        public T[] Items;
        int _itemsCount;
        int[] _recycledItems;
        int _recycledItemsCount;

        readonly AutoResetHandler _autoReset;
#if ENABLE_IL2CPP && !UNITY_EDITOR
        T _autoresetFakeInstance;
#endif

        delegate void AutoResetHandler (ref T component);

        public EcsComponentPool (int entitiesCapacity) : base (entitiesCapacity) {
            Items = new T[512];
            _itemsCount = 1;
            _recycledItems = new int[512];
            _recycledItemsCount = 0;
            if (EcsComponentType<T>.IsAutoReset) {
                var autoResetMethod = typeof (T).GetMethod (nameof (IEcsAutoReset<T>.AutoReset));
#if DEBUG

                if (autoResetMethod == null) {
                    throw new Exception (
                        $"IEcsAutoReset<{typeof (T).Name}> explicit implementation not supported, use implicit instead.");
                }
#endif
                _autoReset = (AutoResetHandler) Delegate.CreateDelegate (
                    typeof (AutoResetHandler),
#if ENABLE_IL2CPP && !UNITY_EDITOR
                    _autoresetFakeInstance,
#else
                    null,
#endif
                    autoResetMethod);
            }
        }

        public int Get () {
            if (_recycledItemsCount > 0) {
                return _recycledItems[--_recycledItemsCount];
            }
            if (Items.Length == _itemsCount) {
                Array.Resize (ref Items, _itemsCount << 1);
            }
            // reset brand new instance if custom AutoReset was registered.
            _autoReset?.Invoke (ref Items[_itemsCount]);
            return _itemsCount++;
        }

        public override void Recycle (int id) {
            if (_autoReset != null) {
                _autoReset (ref Items[id]);
            } else {
                Items[id] = default;
            }
            if (_recycledItems.Length == _recycledItemsCount) {
                Array.Resize (ref _recycledItems, _recycledItemsCount << 1);
            }
            _recycledItems[_recycledItemsCount++] = id;
        }

        public override void UnregisterWorld (int worldId) {
            EcsComponentType<T>.Unregister (worldId);
        }
    }
}