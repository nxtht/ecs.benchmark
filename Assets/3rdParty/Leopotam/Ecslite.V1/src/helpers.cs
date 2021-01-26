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
    static class EcsMath {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static int Pot (int v) {
            if (v < 2) {
                return 2;
            }
            var n = v - 1;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            return n + 1;
        }
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    sealed class EcsBitSet {
        const int ItemCapacity = sizeof (ulong) * 8;
        public readonly ulong[] RawData;
        public int Count;

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public EcsBitSet (int capacity = 0) {
            var newSize = capacity / ItemCapacity;
            if (capacity % ItemCapacity != 0) { newSize++; }
            RawData = new ulong[newSize];
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Set (int idx) {
            var chunk = idx / ItemCapacity;
            var oldV = RawData[chunk];
            var newV = oldV | (1UL << (idx % ItemCapacity));
            if (oldV != newV) {
                RawData[chunk] = newV;
                Count++;
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Unset (in int idx) {
            var chunk = idx / ItemCapacity;
            var oldV = RawData[chunk];
            var newV = oldV & ~(1UL << (idx % ItemCapacity));
            if (oldV != newV) {
                RawData[chunk] = newV;
                Count--;
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Get (int idx) {
            return (RawData[idx / ItemCapacity] & (1UL << (idx % ItemCapacity))) != 0;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Contains (EcsBitSet bs) {
            for (var i = 0; i < RawData.Length; i++) {
                ref var bsData = ref bs.RawData[i];
                if ((RawData[i] & bsData) != bsData) {
                    return false;
                }
            }
            return true;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Intersects (EcsBitSet bs) {
            for (var i = 0; i < RawData.Length; i++) {
                if ((RawData[i] & bs.RawData[i]) != 0) {
                    return true;
                }
            }
            return false;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Clear () {
            for (var i = 0; i < RawData.Length; i++) {
                RawData[i] = 0;
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Merge (EcsBitSet include) {
            for (var i = 0; i < RawData.Length; i++) {
                RawData[i] |= include.RawData[i];
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator () {
            return new Enumerator (this);
        }

        public ref struct Enumerator {
            readonly EcsBitSet _bs;
            readonly int _count;
            int _idx;
            int _returned;

            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            internal Enumerator (EcsBitSet bs) {
                _bs = bs;
                _count = bs.Count;
                _idx = -1;
                _returned = 0;
            }

            public int Current {
#if ENABLE_IL2CPP
            [Il2CppSetOption (Option.NullChecks, false)]
#endif
                [MethodImpl (MethodImplOptions.AggressiveInlining)]
                get {
                    while (true) {
                        _idx++;
                        if (_bs.Get (_idx)) {
                            _returned++;
                            return _idx;
                        }
                    }
                }
            }

            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            public bool MoveNext () {
                return _returned < _count;
            }
        }
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    public sealed class EcsSparseSet {
        // zero item not in use.
        public int[] Dense;
        int[] _sparse;
        int _denseCount;

        public EcsSparseSet (int denseCapacity, int sparseCapacity) {
            Dense = new int[denseCapacity > 0 ? denseCapacity : 16];
            _sparse = new int[sparseCapacity > 0 ? sparseCapacity : 16];
            _denseCount = 1;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public int GetCount () {
            return _denseCount - 1;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public int GetDenseIdx (in int sparseIdx) {
#if DEBUG
            if (sparseIdx >= _sparse.Length) { throw new Exception ($"invalid sparse idx {sparseIdx}: capacity {_sparse.Length}."); }
#endif
            var denseIdx = _sparse[sparseIdx];
            if (denseIdx > 0 && denseIdx < _denseCount) {
                return denseIdx;
            }
            return 0;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Set (int sparseIdx) {
#if DEBUG
            if (GetDenseIdx (sparseIdx) > 0) { throw new Exception ($"cant set sparse idx {sparseIdx}: already present."); }
#endif
            if (Dense.Length == _denseCount) {
                Array.Resize (ref Dense, _denseCount << 1);
            }
            _sparse[sparseIdx] = _denseCount;
            Dense[_denseCount] = sparseIdx;
            _denseCount++;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Unset (int sparseIdx) {
#if DEBUG
            if (GetDenseIdx (sparseIdx) <= 0) { throw new Exception ($"Cant unset sparse idx {sparseIdx}: not present."); }
#endif
            var denseIdx = _sparse[sparseIdx];
            _sparse[sparseIdx] = default;
            _denseCount--;
            if (denseIdx < _denseCount) {
                var lastSparseIdx = Dense[_denseCount];
                Dense[denseIdx] = lastSparseIdx;
                _sparse[lastSparseIdx] = denseIdx;
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void EnsureSparseCapacity (int capacity) {
            if (_sparse.Length < capacity) {
                Array.Resize (ref _sparse, EcsMath.Pot (capacity));
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void EnsureDenseCapacity (int capacity) {
            if (Dense.Length < capacity) {
                Array.Resize (ref Dense, EcsMath.Pot (capacity));
            }
        }
    }
}

#if ENABLE_IL2CPP
// Unity IL2CPP performance optimization attribute.
namespace Unity.IL2CPP.CompilerServices {
    enum Option {
        NullChecks = 1,
        ArrayBoundsChecks = 2
    }

    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    class Il2CppSetOptionAttribute : Attribute {
        public Option Option { get; private set; }
        public object Value { get; private set; }

        public Il2CppSetOptionAttribute (Option option, object value) { Option = option; Value = value; }
    }
}
#endif