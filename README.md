# ZemotoCommon

Common repository for my assorted projects. Contains helper classes, extension methods, and static utility methods that could be used by any of my projects and do not contain code specific to any one project. By having this code in a common place and using it as a submodule I can save time and reduce copied code, while also sharing any improvements or lessons learned between all my projects.

## Helper Classes

### ChildProcessWatcher

A helper class that takes advantage of the concept of Windows Jobs (https://docs.microsoft.com/en-us/windows/desktop/procthread/job-objects) to start processes that are guaranteed to be closed/ended when the process that started then ends, be it unexpectedly or intentionally. 

When this class is constructed (either by being used for the first time or calling the `Initialize()` method), it creates a job that is set to end any processes added to the job when the handle to the job is closed. Users of this class can then add started processes to this job via the `AddProcess()` method.

When the parent process ends, the `ChildProcessWatcher` static class is cleaned up and the job handle is subsequently closed, ending all processes that are added to the job. Keep in mind this ends those processes with a hard end process call and does not give those processes time to cleanup.

``` C#
void AddProcess( Process process )
```
Adds a process represented by a `System.Diagnostics.Process` objects to the main job handle.

### Process CPU Watcher

A helper class used to keep track of how much of the available cpu a given process is using. When constructed it is given a `Process` object and it will create a `PerformanceCounter` class that will be used to keep track of the process's cpu usage.

``` C#
int GetCpuUsage()
```
Returns an int representing the total percent of CPU power the process given in the constructor is using.

``` C#
static int GetTotalCpuUsage()
```
Returns an int representing the total CPU power percentage being used across all processes

## Extension Methods

``` C#
public static void StartAsChildProcess( this Process process )
```
Extension of `System.Diagnostics.Process` to start a process and add it to the `ChildProcessWatcher` job.

``` C#
public static void ForEach<T>( this IEnumerable<T> collection, Action<T> action )
```
Extension of `System.Collections.Generic.IEnumerable<T>` to add the `System.Collections.Generic.List<T>.ForEach()` functionality to every `IEnumerable<T>`.

``` C#
public static T GetAttribute<T>( this Enum enumValue ) where T : Attribute
public static T GetAttribute<T>( this ICustomAttributeProvider property ) where T : Attribute
```
Extension methods to allow for quickly grabbing the attributes off a given `Enum` or `ICustomAttributeProvider` without the bulky reflection calls. Mainly to improve readability. Keep in mind `System.Type` is a `ICustomAttributeProvider` and is probably going to be the main way this method is used. This method assumes you are only grabbing a single attribute.

## Utility Methods

``` C#
public static void SafeDeleteFile( string filePath )
```
Simple method that will only attempt to delete the file if it exists, and catch any exceptions thrown by the deletion. Basically a "attempt to delete but I don't care if it fails" sort of method.

## UI Utils

### ViewModelBase
Basic implementor of `INotifyPropertyChanged`.

``` C# 
protected bool SetProperty<T>( ref T property, T newValue, [CallerMemberName] string propertyName = null )
```
Utility method intended to be used by the setters of the properties within `ViewModelBase`. Will set the property if it has changed, and call `INotifyPropertyChanged.OnPropertyChanged` if it has. Returns a bool indicating the property has changed or not.

### RelayCommand and RelayCommand<T>
Simple implementors of `ICommand`, with `RelayCommand<T>` allowing for a parameter to be sent to the command.
  
### GetEnumValuesExtension
`MarkupExtension` that allows for WPF `ComboBox` cotrols to easily list all the possible values within an enum. Takes advantage of the `DescriptionAttribute` to allow for enums to be represented with clear names

### IntMinMaxBinding and DoubleMinMaxBinding
Subclasses of `Binding` that can be used instead of a `Binding` in WPF. These versions create a binding as normal but also add a validation rule with min and max values.

### EqualityToVisibilityConverter
Simple `IValueConverter` that returns `Visibility.Visible` if the given value is equal to the parameter, otherwise `Visibility.Collapsed`.
