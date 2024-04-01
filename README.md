# Nivaes Tool

Nivaes Async is a set of utilities for handling asynchronous functions.

## Features

### AsyncEnumerable

Proporciona metodos para acceder a una colecci�n as�ncrona.


### AsyncLazy

Provides support for Lazy initialization, allowing the creation of the object with an asynchronous call.

``` C#
using Nivaes;

async ValueTask<MyClass> InitMyObject()
{
    ....
    MyClass myObject = await myService.GetObject();
    return myObject;
}

async Task Main()
{
    ... 
    var lazyObject = new AsyncLazy<MyClass>(InitMyObject);
    var MyObject = await lazyObject.Value;
    ...
}
```

### AsyncTemporary

It amacen a variable, which is generated asynchronously, over a period of time.

## Packages

| NuGet Package | Latest Versions |
| --- | --- |
| [Nivaes.Async](https://www.nuget.org/packages/Nivaes.Async) | [![latest stable version](https://img.shields.io/nuget/v/Nivaes.Async.svg)](https://www.nuget.org/packages/Nivaes.Async) |


## Integration

![CI](https://github.com/Nivaes/Nivaes.Tools/workflows/CI/badge.svg)[![codecov](https://codecov.io/gh/Nivaes/Nivaes.Tools/graph/badge.svg?token=HIMJ4XQBFU)](https://codecov.io/gh/Nivaes/Nivaes.Tools)

![Build Release](https://github.com/Nivaes/Nivaes.Tools/workflows/Build%20Release/badge.svg)

![Publish Release](https://github.com/Nivaes/Nivaes.Tools/workflows/Publish%20Release/badge.svg)
