# Samples for Plugin.Maui.MarkdownView
Small collection of samples to demonstrate the usage of the `Plugin.Maui.MarkdownView` plugin.

## XAML Hot-Reload for multiple platforms

Multiple platforms can be started simultaneously with the following principle. 
- The main project with a SingleProject structure can start one specific platform version of an app. Logically, a platform is set up here for which no dedicated project is available.
- Platform dedicated projects in the solution folder 'MultiStartupProjects' can start a platform-specific version of an app within this SingleProject structure.


#### Setup running multiple projects
in short:
1. Build all desired platforms and Debug targets on the Main project with SingleProject structure, and build the specific version on the dedicated projects
1. Configure Startup Projects
1. Start debug


in detail:

1. Build `Plugin.Maui.MarkdownView.Sample` for desired platforms (iOS, Android, Windows, etc)
    1. build every platform with the `Debug target` to your a working `Device` of `Simulator`
	1. build the platform you want to run on the SingleProject structure last
1. Build `Plugin.Maui.MarkdownView.Sample.iOS`
    1. build this project with the same `Debug target` as used at the SingleProject build
1. Configure startup projects by
    1. Look & start feature `Configure Startup Projects...`
	1. Set all desired project and their matching platform to `Start` action
	1. Set all desired project `Debug target` to your `Simulator` or `Device`
	1. Hit the `OK` button
1. Hit the `Start / Debug` button
