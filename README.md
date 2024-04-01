# Nivaes Extensions

Extensions funcionality.

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
| [Nivaes.Extensions](https://www.nuget.org/packages/Nivaes.Extensions) | [![latest stable version](https://img.shields.io/nuget/v/Nivaes.Extensions.svg)](https://www.nuget.org/packages/Nivaes.Extensions) |


## Integration

![CI](https://github.com/Nivaes/Nivaes.Extensions/workflows/CI/badge.svg) [![codecov](https://codecov.io/gh/Nivaes/Nivaes.Extensions/graph/badge.svg?token=HIMJ4XQBFU)](https://codecov.io/gh/Nivaes/Nivaes.Tools)

![Build Release](https://github.com/Nivaes/Nivaes.Extensions/workflows/Build%20Release/badge.svg)

![Publish Release](https://github.com/Nivaes/Nivaes.Extensions/workflows/Publish%20Release/badge.svg)
