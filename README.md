
# DynamicEnv

.NET Standard library that exposes environment variable interactions through a Dynamic object.

# Getting Started

Install [`Smmx.DynamicEnv`](https://www.nuget.org/packages/Smmx.DynamicEnv) from NuGet. E.g. from Visual Studio's Package Manager Console:

```PowerShell
Install-Package Smmx.DynamicEnv
```

You can then use the `Env` class to access environment variables succinctly via a dynamic object. You can use either dynamic member syntax or indexes (similar to a string dictionary, although this class does not yet implement any dictionary interface):

```C#
using Smmx.DynamicEnv;

...

//First you need an instance of Env
dynamic env = new Env();

//Or explicitly specify the environment target (the default when none is specified, as above, is Process)
env = new Env(EnvironmentVariableTarget.Process)

//Or you can use one of the pre-initialized static instances:
env = Env.Machine; //Targets machine-level environment variables (Windows systems only)
env = Env.User;    //Targets user-level environment variables (Windows systems only)
env = Env.Process; //Targets process-level environment variables (this is the default)

//Read an environment variable; note, their names may be case sensitive on some platforms (Linux/Mac):
string path;
path = env.PATH;
path = env["PATH"];

//If the environment variable doesn't exist, then it'll return a null:
string empty;
empty = env.ThisVarDoesntExist;
empty = env["ThisVarDoesntExist"];
Assert.True(empty == null);

//You can also set environment variables like this:
env.DataRoot = @"C:\Data";
env["DataRoot"] = @"C:\Data";

//Clearing/deleting an environment variable is typically done by setting as null:
env.DataRoot = null;
env["DataRoot"] = null;

//Enumerate environment variable names:
IEnumerable<string> varNames = env.GetDynamicMemberNames();
foreach (string key in varNames) {
    string val = env[key];
    Console.WriteLine($"{key}: {val}");
}

//An invalid value type (currently anything other than String) throws an ArgumentException:
env.Test = 454;

//An invalid index types (currently anthing other than String) throws an IndexOutOfRangeException:
env[12] = "this won't work!";

//And finally here's a gotcha that causes a RuntimeBinderException because .NET can't determine whether the null result is a string or a char[] parameter to the Console.WriteLine(...) method:
Console.WriteLine(env.ThisVarDoesntExist);

//You can work around this by casting to string:
Console.WriteLine($"{env.ThisVarDoesntExist}");
Console.WriteLine((string)env.ThisVarDoesntExist);
```
