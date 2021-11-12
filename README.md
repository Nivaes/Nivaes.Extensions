# Nivaes Async

Nivaes Async is a set of utilities for handling asynchronous functions.

## AsyncEnumerable

## AsyncLazy

Provides support for lazy initialization, permitiendo la creación del objeto con una llamada asíncrona.

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

## AsyncTemporary

### Actions

![CI](https://github.com/Nivaes/Nivaes.Async/workflows/CI/badge.svg)

![Build Release](https://github.com/Nivaes/Nivaes.Async/workflows/Build%20Release/badge.svg)

![Publish Release](https://github.com/Nivaes/Nivaes.Async/workflows/Publish%20Release/badge.svg)
