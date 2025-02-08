![nuget.png](https://raw.githubusercontent.com/Toine-db/Plugin.Maui.MarkdownView/main/nuget.png)
# Plugin.Maui.MarkdownView

:movie_camera: Quick-peek videos [here](#videos)

`Plugin.Maui.MarkdownView` provides the ability to create your UI based on Markdown files.

- Easy to use
- Highly customizable
- :fire: Hot Reload support :fire:
- IgnoreSafeArea support
- Massively scalable
- Perfect for local and remote use cases
- Fully works with default MAUI UI rendering
- No hacking or other fragile mechanisms

## Install Plugin
 
[![NuGet](https://img.shields.io/nuget/v/Plugin.Maui.MarkdownView.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.Maui.MarkdownView/)

Available on [NuGet](http://www.nuget.org/packages/Plugin.Maui.MarkdownView).

Install with the dotnet CLI: `dotnet add package Plugin.Maui.MarkdownView`, or through some NuGet Package Manager in your editor.

### Supported Platforms

This plugin works completely with the default MAUI UI rendering, so Platform supported versions should be the same as the MAUI supported versions.

Available on [microsoft.com > supported-platforms](https://learn.microsoft.com/en-us/dotnet/maui/supported-platforms).


### Proven system
This system has been part of my private project for a long time and has proven to be a great way to easily scale-up apps, like working with views that contain lots of static content or when supporting many different languages. As an example, the app in question has approx 2 million downloads and is available in 11 languages ​​using different alphabets and writing directions.

With the rise of MAUI I thought this was a great time to make it public, because it's just too good to keep it to myself :grin:. And with the help of the community, we can make it even better.

## Usage

`Plugin.Maui.MarkdownView` provides the `MarkdownView` class as a MAUI View.

The MarkdownView has one required property for the Markdown text, this can be set using it's `Content` field in XAML or the `MarkdownText` property in the code behind. When using the Content field in XAML it automaticly supports :fire: UI Hot-Reload :fire:.

XAML
```xaml
<ScrollView>
    <mdv:MarkdownView xml:space="preserve">
# My first MarkdownView

Some new Paragraph, that is separated by a blank line.
    </mdv:MarkdownView>
</ScrollView>
```

or C#
```csharp
await using var stream = await FileSystem.OpenAppPackageFileAsync("MyMarkdown.md");
using var reader = new StreamReader(stream);
MyMarkdownView.MarkdownText = await reader.ReadToEndAsync();
```

Check out the Pages in the [Plugin.Maui.MarkdownView.Sample](https://github.com/Toine-db/Plugin.Maui.MarkdownView/tree/main/samples) project for detailed examples.
- MarkdownInXamlPage : how to use the `Content` field in XAML
- MarkdownFromRemotePage : how to load a markdown file from a remote server
- MarkdownFullExamplePage : full blown example how to style and customize

### Quick start

Two steps to quickly get started with MarkdownView and get the most of its power right away:
  1. copy-paste the styles `MarkdownStyles.xaml` into your project
  2. copy-paste the complete `<mdv:MarkdownView>` from the MarkdownInXamlPage to where ever you want to use the control

Now you can change the Markdown content in the `<mdv:MarkdownView>` and start styling it with the `MarkdownStyles.xaml` file or customize your own IMauiViewSupplier.

### Permissions

No permissions required :tada:

### Dependency Injection

No dependency injection required :confetti_ball:

### Feature

Once you have created a `MarkdownView` you can interact with its properties and specialy by using `ViewSupplier` that handles the view creation. 

Most interesting components to use:

#### Properties

##### `MarkdownText`

Sets a value for markdown text that needs to be parsed to views. 

This property is can be set in XAML using the `Content` field. When using XAML use `xml:space="preserve"` to keep line-breaks and spacing working during Hot-Reload.

##### `IsLoadingMarkdown`

Gets a value indicating whether control is parsing markdown text to views.

##### `ViewSupplier`

Sets a value for IMauiViewSupplier that creates the views. Here you have three options:
* _MauiBasicViewSupplier_ : very basic view generator
* _MauiFormattedTextViewSupplier_ : view generator that also supports placeholders and substring styling like italic, bold, etc (inherits MauiBasicViewSupplier)
* _Your custom IMauiViewSupplier_ : your own custom view creator, its recommended to inherit from MauiFormattedTextViewSupplier and start overriding methods

##### `MauiBasicViewSupplier.Styles`

When using MauiViewSupplier as IViewSupplier (this is default), the supplier uses this property to look for optional styles.

##### `MauiBasicViewSupplier.BasePathForRelativeUrlConversion`

When using MauiViewSupplier as IViewSupplier (this is default), the supplier tries to convert links to the correct path using this a property as base path.

##### `MauiBasicViewSupplier.PrefixesToIgnoreForRelativeUrlConversion`

When using MauiViewSupplier as IViewSupplier (this is default), The provider tries to convert links to the correct path, except for values ​​with a prefix from this property.

##### `MauiFormattedTextViewSupplier.FormattedTextStyles`

When using MauiFormattedTextViewSupplier as IViewSupplier, the supplier uses this property to look for optional styles for formatted text.

##### `MauiFormattedTextViewSupplier.OnHyperlinkTappedFallback`

When using MauiFormattedTextViewSupplier as IViewSupplier, the supplier uses this property as last fallback when hyperlink can not open external url or find heading to navigate to.

## Customizability

This plugin has been created with only structured object-oriented code, no hidden reflection or injection mechanisms.

Almost all methods are virtual and ready to be overridden, sorry to the devs who are principally against this. 

Use this power wisely, customize it as you wish, you may break this system but the freedom it gives you to make it your own is endless :smirk:.

### Use UI Hot-Reload on multiple platforms simultaneously
For UI Development purposes a dedicated dummy sample iOS project has been created to be used in a ‘multi startup projects’ scenario. This can be used to simultaneously use UI Hot-Reload on multiple platforms at the same time, that is iOS + a second desired platform.

Because this scenario requires everything to be build for all desired platforms its recommended to manually build everything before starting a debug session with UI Hot-Reload.
1.	Set ‘Plugin.Maui.MarkdownView.Sample’ as startup project
2.	Set target to some iOS simulator
3.	Build
4.	Set target to some Android emulator or Windows device
5.	Build
6.	Set ‘Plugin.Maui.MarkdownView.Sample.iOS’ as startup project
7.	Set target to some iOS simulator
8.	Build
9.	Start ‘Configure startup projects’
    1. pick ‘Multiple startup projects’
    2.	set MarkdownView to action ‘None’
    3.	set MarkdownView.Sample to action ‘Start’ and target to an Android emulator or Windows device
    4.	set MarkdownView.Sample.iOS to action ‘Start’ and target to an iOS simulator
10.	Start debug session

When you loose Hot-Reload for one of the platforms, do the following:
1. Temporarily set ‘Plugin.Maui.MarkdownView.Sample’ as startup project and shortly start a debug session. 
2. Set the ‘Plugin.Maui.MarkdownView.Sample’ to previous target 
3. Start ‘Configure startup projects’ and set the Multiple startup projects settings as before

## Side Notes
- Italic and Bold/Strong do not work on iOS with most custom fonts like OpenSans. Reset the Font with `<Setter Property="FontFamily" Value="" />` will force to use the platform designated Font. So check if your font is supported on iOS for using italic and bold if you wan to use it.
  - Consider using the standard platform specific fonts for normal text, and perhaps custom fonts for headers. OS regulated fonts provide more flexibility and are often chosen by the platform builders for (mostly) good reasons.  


## Roadmap

- [x] Support original Markdown syntax
- [ ] Support more extended markdown syntax (like tables)
  - _requires update [MarkdownParser](https://github.com/Toine-db/MarkdownParser/tree/main)_ dependency

# Acknowledgements

This project could not have came to be without these projects and people, thank you! <3 :clap:
- You ;-) 

# Videos

MarkdownView with design example:

https://github.com/user-attachments/assets/9508a1e3-dbdb-446f-8081-4a48efa84a32

MarkdownView Quick-Peek:
[![Watch the video](https://raw.githubusercontent.com/Toine-db/Plugin.Maui.MarkdownView/main/demo/maui-markdownview-quick-peek.png)](https://youtu.be/ZvUlcuIAdV8)
