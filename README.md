# ecs.benchmark

A set of benchmarks for ecs frameworks of [Leopotam](https://github.com/Leopotam). These benchmarks are not precise tools but allow to compare general perfomance of the frameworks in different scenarios.
Initially the project was aimed to evaluate this [issue](https://github.com/Leopotam/ecs/issues/37) with the OneComponent benchmark.
## Usage
Clone > Build > Run

Starting scene - \Assets\Benchmark\BenchmarkSelectorScene.unity

## Benchmarks
### OneComponent
A plane iteration over single component type.
#### Options: 
- `Components count`

### BouncyShooter
Scenario with creation and deletion of entities.
#### Options:
- `Actor Types Count`
- `Actors Count`
- `Bullet LifeTime`
- `Rate of Fire`

Each `Actor` emits `Bullets` according to the `Rate Of Fire`. `Bullets` are destroyed after their `Bullet LifeTime` is expired. `Actor Types Count` allows to evaluate the influence of the "filters" count.

| Some tests|
|-----|
| ![results](https://github.com/nxtht/images/blob/master/ecs.benchmark/Ecs.Benchmark_results_01.png) |
