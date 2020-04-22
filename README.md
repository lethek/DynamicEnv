
# DynamicEnv

![.NET Core Build](https://github.com/lethek/DynamicEnv/workflows/build/badge.svg)
[![NuGet Stats](https://img.shields.io/nuget/v/DynamicEnv.svg)](https://www.nuget.org/packages/DynamicEnv)
[![GitHub license](https://img.shields.io/github/license/lethek/DynamicEnv)](https://github.com/lethek/DynamicEnv/blob/master/LICENSE)

.NET Standard 2.0 library that exposes environment variable interactions through a Dynamic object.

# Getting Started

Install [`DynamicEnv`](https://www.nuget.org/packages/DynamicEnv) from NuGet. E.g. from Visual Studio's Package Manager Console:

```PowerShell
Install-Package DynamicEnv
```

## Usage

You can then use the `Env` class to access environment variables succinctly via a `dynamic` object. You can use either dynamic member syntax or indexes (similar to a string dictionary, although this class does not currently implement any dictionary interface).

First you need an instance of `Env`:

```C#
using DynamicEnv;

//...

dynamic env = new Env();

//Or explicitly specify the environment target (the default when none is
//specified, like the line above, is EnvironmentVariableTarget.Process)
env = new Env(EnvironmentVariableTarget.Process)

//Or you can use one of the pre-initialized static instances:
env = Env.Machine; //Targets machine-level environment variables (Windows systems only)
env = Env.User;    //Targets user-level environment variables (Windows systems only)
env = Env.Process; //Targets process-level environment variables (this is the default)
```

Read an environment variable; note, their names may be case sensitive on some platforms (Linux/Mac):

```C#
string path;
path = env.PATH;
path = env["PATH"];
```

If the environment variable doesn't exist, then it'll return a `null`:

```C#
string empty;
empty = env.ThisVarDoesntExist;
empty = env["ThisVarDoesntExist"];
Assert.True(empty == null);
```

You can also set environment variables like this:

```C#
env.DataRoot = @"C:\Data";
env["DataRoot"] = @"C:\Data";
```

Deleting/clearing an environment variable is typically done by setting as `null`:

```C#
env.DataRoot = null;
env["DataRoot"] = null;
```

Enumerate environment variable names:

```C#
IEnumerable<string> varNames = env.GetDynamicMemberNames();
foreach (string name in varNames) {
    string val = env[name];
    Console.WriteLine($"{name}: {val}");
}
```

## Errors

An unsupported value type (currently anything other than `String`) throws an `ArgumentException`:

```C#
env.Test = 454;
```

An unsupported index type (currently anthing other than `String`) throws an `IndexOutOfRangeException`:

```C#
env[12] = "this won't work!";
```

And finally here's a gotcha that causes a `RuntimeBinderException` because .NET can't determine whether the `null` result is a `string` or a `char[]` parameter to the `Console.WriteLine(...)` method:

```C#
Console.WriteLine(env.ThisVarDoesntExist);
```

You can work around this by casting to `string`:

```C#
Console.WriteLine($"{env.ThisVarDoesntExist}");
Console.WriteLine((string)env.ThisVarDoesntExist);
```
