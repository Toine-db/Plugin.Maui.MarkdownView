![nuget.png](https://raw.githubusercontent.com/Toine-db/Plugin.Maui.MarkdownView/main/nuget.png)
# Plugin.Maui.MarkdownView

`Plugin.Maui.MarkdownView` provides the ability to do this amazing thing in your .NET MAUI application.

## Install Plugin

[![NuGet](https://img.shields.io/nuget/v/Plugin.Maui.MarkdownView.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.Maui.MarkdownView/)

Available on [NuGet](http://www.nuget.org/packages/Plugin.Maui.MarkdownView).

Install with the dotnet CLI: `dotnet add package Plugin.Maui.MarkdownView`, or through the NuGet Package Manager in Visual Studio.

### Supported Platforms

| Platform | Minimum Version Supported |
|----------|---------------------------|
| iOS      | 11+                       |
| Android  | 5.0 (API 21)              |

## API Usage

`Plugin.Maui.MarkdownView` provides the `Feature` class that has a single property `Property` that you can get or set.

You can either use it as a static class, e.g.: `Feature.Default.Property = 1` or with dependency injection: `builder.Services.AddSingleton<IFeature>(Feature.Default);`

### Permissions

Before you can start using Feature, you will need to request the proper permissions on each platform.

#### iOS

No permissions are needed for iOS.

#### Android

No permissions are needed for Android.

### Dependency Injection

You will first need to register the `Feature` with the `MauiAppBuilder` following the same pattern that the .NET MAUI Essentials libraries follow.

```csharp
builder.Services.AddSingleton(Feature.Default);
```

You can then enable your classes to depend on `IFeature` as per the following example.

```csharp
public class FeatureViewModel
{
    readonly IFeature feature;

    public FeatureViewModel(IFeature feature)
    {
        this.feature = feature;
    }

    public void StartFeature()
    {
        feature.ReadingChanged += (sender, reading) =>
        {
            Console.WriteLine(reading.Thing);
        };

        feature.Start();
    }
}
```

### Straight usage

Alternatively if you want to skip using the dependency injection approach you can use the `Feature.Default` property.

```csharp
public class FeatureViewModel
{
    public void StartFeature()
    {
        feature.ReadingChanged += (sender, reading) =>
        {
            Console.WriteLine(feature.Thing);
        };

        Feature.Default.Start();
    }
}
```

### Feature

Once you have created a `Feature` you can interact with it in the following ways:

#### Events

##### `ReadingChanged`

Occurs when feature reading changes.

#### Properties

##### `IsSupported`

Gets a value indicating whether reading the feature is supported on this device.

##### `IsMonitoring`

Gets a value indicating whether the feature is actively being monitored.

#### Methods

##### `Start()`

Start monitoring for changes to the feature.

##### `Stop()`

Stop monitoring for changes to the feature.

# Acknowledgements

This project could not have came to be without these projects and people, thank you! <3
