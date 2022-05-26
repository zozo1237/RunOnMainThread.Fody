# RunOnMainThread.Fody
Run annotated methods on the Main Thread.
The [Fody](https://github.com/Fody/Home/) plugin you never realized you needed. 

This fork has been gutted, updated to work with [Fody](https://github.com/Fody/Home/) 6.6.2, and made to work with WPF only.
Beware: It's held together with ducktape.

# Installation

Installation:

- Install The Foddy Package to any project that wants to use RunOnMainThread.Fody
- Git clone this repostory
- Add both `RunOnMainThread.csproj` and `RunOnMainThread.Fody.csproj` to your solution
- Resolve the RunOnMainThread's refrence to PresentationFramework.dll and WindowsBase.dll
- Refrence RunOnMainThread from any project that wants to use RunOnMainThread.Fody
- Add to the project file (i.e. .csproj) of any project that wants to use RunOnMainThread.Fody:

```
<ItemGroup>
	<WeaverFiles Include="[PATH_TO_CLONED_REPOSITORY]\RunOnMainThread.Fody\obj\debug\netstandard2.0\RunOnMainThread.Fody.dll" />
</ItemGroup>
```

- Add `<RunOnMainThread />` to your `FodyWeavers.xml` for any project using RunOnMainThread.Fody:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Weavers>
  <RunOnMainThread />
</Weavers>
```

# Usage

The package will add a static `MainThreadDispatcher` class with a `void RunOnMainThread(Action action)` method. 
You can use it directly to access the main thread from anywhere in your app:

```csharp
private void ShowDialogUsingDispatcher()
{
    MainThreadDispatcher.RunOnMainThread(() =>
    {
        Console.WriteLine("Main thread!");
    });
}
```

The cool part about this library, though, is that you can instead annotate your method with the `RunOnMainThreadAttribute` and all that boilerplate will be added for you.
The method below is the same as the one in the static class example:

```csharp
[RunOnMainThread]
private void ShowDialogUsingWeaver()
{
    Console.WriteLine("Main thread!");
}
```

For more examples, check out the sample apps.
