using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;


namespace DynamicEnv;

/// <summary>
/// Represents a dynamic environment variable object which can be accessed or updated in a code-friendly manner.
/// </summary>
public class Env : DynamicObject
{

    /// <summary>
    /// Gets or sets the <see cref="Env"/> instance associated with the current process.
    /// </summary>
    public static readonly dynamic Process = new Env(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Gets or sets the <see cref="Env"/> instance associated with the current user.
    /// </summary>
    public static readonly dynamic User = new Env(EnvironmentVariableTarget.User);

    /// <summary>
    /// Gets or sets the <see cref="Env"/> instance associated with the local machine.
    /// </summary>
    public static readonly dynamic Machine = new Env(EnvironmentVariableTarget.Machine);


    /// <summary>
    /// Initializes a new instance of the <see cref="Env"/> class with the environment block associated with the current process.
    /// </summary>
    public Env() : this(EnvironmentVariableTarget.Process) { }


    /// <summary>
    /// Initializes a new instance of the <see cref="Env"/> class with specified target.
    /// </summary>
    /// <param name="target">The target environment variable scope.</param>
    public Env(EnvironmentVariableTarget target)
        => _target = target;


    /// <summary>
    /// Tries to get the value of an environment variable by a key.
    /// </summary>
    /// <param name="binder">The dynamic get member binder.</param>
    /// <param name="result">The result value.</param>
    /// <returns>Always returns <see langword="true"/>. If an environment variable associated with key is not found, the <paramref name="result"/> will be <see langword="null"/>.</returns>
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        result = GetEnvVar(binder.Name);
        return true;
    }


    /// <summary>
    /// Tries to set the value of an environment variable by a key.
    /// </summary>
    /// <param name="binder">The dynamic set member binder.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>Always returns <see langword="true"/>.</returns>
    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        SetEnvVar(binder.Name, value);
        return true;
    }


    /// <summary>
    /// Tries to delete the value of an environment variable by a key. This is achieved by setting the value to <see langword="null"/>.
    /// </summary>
    /// <param name="binder">The dynamic delete member binder.</param>
    /// <returns>Always returns <see langword="true"/>.</returns>
    [ExcludeFromCodeCoverage]
    public override bool TryDeleteMember(DeleteMemberBinder binder)
    {
        DelEnvVar(binder.Name);
        return true;
    }


    /// <summary>
    /// Tries to get a value of an environment variable by an index array of strings.
    /// </summary>
    /// <param name="binder">The dynamic get index binder.</param>
    /// <param name="indexes">The index array of strings. </param>
    /// <param name="result">The result value.</param>
    /// <returns>Always returns <see langword="true"/>.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when no environment variable with the specified index is found.</exception>
    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
    {
        result = GetEnvVar(GetEnvKeyFromIndexes(indexes));
        return true;
    }


    /// <summary>
    /// Tries to set a value to an environment variable by an index array of strings.
    /// </summary>
    /// <param name="binder">The dynamic set index binder.</param>
    /// <param name="indexes">The index array of strings. </param>
    /// <param name="value">The value to set.</param>
    /// <returns>Always returns <see langword="true"/>.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when no environment variable with the specified index is found.</exception>
    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object? value)
    {
        SetEnvVar(GetEnvKeyFromIndexes(indexes), value);
        return true;
    }


    /// <summary>
    /// Tries to delete a value of an environment variable by an index array of strings.
    /// </summary>
    /// <param name="binder">The dynamic delete index binder.</param>
    /// <param name="indexes">The index array of strings. </param>
    /// <returns>Always returns <see langword="true"/>.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when no environment variable with the specified index is found.</exception>
    [ExcludeFromCodeCoverage]
    public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
    {
        DelEnvVar(GetEnvKeyFromIndexes(indexes));
        return true;
    }


    /// <summary>
    /// Returns the list of the environment variable keys.
    /// </summary>
    /// <returns>A list of the environment variable keys.</returns>
    public override IEnumerable<string> GetDynamicMemberNames()
        => Environment.GetEnvironmentVariables(_target).Keys.Cast<string>();


    private string? GetEnvVar(string name)
        => Environment.GetEnvironmentVariable(name, _target);


    private void SetEnvVar(string name, object? value)
    {
        if (value != null && value.GetType() != typeof(string)) {
            throw new ArgumentException("Value must be of type String", nameof(value));
        }

        Environment.SetEnvironmentVariable(name, (string?)value, _target);
    }


    [ExcludeFromCodeCoverage]
    private void DelEnvVar(string name)
        => Environment.SetEnvironmentVariable(name, null);


    private string GetEnvKeyFromIndexes(object[] indexes)
    {
        try {
            return indexes.OfType<string>().Single();
        } catch {
            throw new IndexOutOfRangeException("Index must be of type String");
        }
    }


    private readonly EnvironmentVariableTarget _target;
}