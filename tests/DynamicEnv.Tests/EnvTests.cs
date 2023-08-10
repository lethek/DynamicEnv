namespace DynamicEnv;

public class EnvTests
{
    [Fact]
    public void Env_StaticInstances_AreNotNull()
    {
        Assert.NotNull(Env.Process);
        Assert.NotNull(Env.User);
        Assert.NotNull(Env.Machine);
    }


    [Fact]
    public void Env_GetMissingVariable_ReturnsNull()
    {
        dynamic env = new Env();
        Assert.Null(env.TEST_MISSING_VAR);
        Assert.Null(Env.Process.TEST_MISSING_VAR);
        Assert.Null(Env.User.TEST_MISSING_VAR);
        Assert.Null(Env.Machine.TEST_MISSING_VAR);
    }


    [Fact]
    public void Env_GetMissingVariableByIndex_ReturnsNull()
    {
        dynamic env = new Env();
        Assert.Null(env["TEST_MISSING_BYINDEX"]);
        Assert.Null(Env.Process["TEST_MISSING_BYINDEX"]);
        Assert.Null(Env.User["TEST_MISSING_BYINDEX"]);
        Assert.Null(Env.Machine["TEST_MISSING_BYINDEX"]);
    }


    [Fact]
    public void Env_SetVariableWithString_NoErrors()
    {
        dynamic env = new Env();
        env.TEST_STRING = "testing";
    }


    [Fact]
    public void Env_SetVariableWithStringByIndex_NoErrors()
    {
        dynamic env = new Env();
        env["TEST_STRING_BYINDEX"] = "testing";
    }


    [Fact]
    public void Env_SetAndGetVariable_ReturnsSameValue()
    {
        dynamic env = new Env();
        var value = Guid.NewGuid().ToString();
        env.TEST_SETGET = value;
        Assert.StrictEqual(value, env.TEST_SETGET);
    }


    [Fact]
    public void Env_SetAndGetVariableByIndex_ReturnsSameValue()
    {
        dynamic env = new Env();
        var value = Guid.NewGuid().ToString();
        env["TEST_SETGET_BYINDEX"] = value;
        Assert.StrictEqual(value, env["TEST_SETGET_BYINDEX"]);
    }


    [Fact]
    public void Env_SetInvalidType_ThrowsArgumentException()
    {
        dynamic env = new Env();
        var obj = new Object();
        Assert.Throws<ArgumentException>("value", () => env.TEST_INVALIDTYPE = obj);
    }


    [Fact]
    public void Env_SetInvalidTypeByIndex_ThrowsArgumentException()
    {
        dynamic env = new Env();
        var obj = new Object();
        Assert.Throws<ArgumentException>("value", () => env["TEST_INVALIDTYPE_BYINDEX"] = obj);
    }


    [Fact]
    public void Env_GetDynamicMemberNames_ReturnsEnumerable()
    {
        dynamic env = new Env();
        Assert.IsAssignableFrom<IEnumerable<string>>(env.GetDynamicMemberNames());
        Assert.IsAssignableFrom<IEnumerable<string>>(Env.Process.GetDynamicMemberNames());
        Assert.IsAssignableFrom<IEnumerable<string>>(Env.User.GetDynamicMemberNames());
        Assert.IsAssignableFrom<IEnumerable<string>>(Env.Machine.GetDynamicMemberNames());

        IEnumerable<string> varNames = env.GetDynamicMemberNames();
        foreach (var key in varNames) {
            string value = env[key];
            Assert.NotNull(value);
        }
    }


    [Fact]
    public void Env_GetByInvalidIndex_ThrowsIndexOutOfRangeException()
    {
        dynamic env = new Env();
            
        Assert.Throws<IndexOutOfRangeException>(() => env[0]);
        Assert.Throws<IndexOutOfRangeException>(() => Env.Process[0]);
        Assert.Throws<IndexOutOfRangeException>(() => Env.User[0]);
        Assert.Throws<IndexOutOfRangeException>(() => Env.Machine[0]);

        Assert.Throws<IndexOutOfRangeException>(() => env["TEST", "1"]);
        Assert.Throws<IndexOutOfRangeException>(() => Env.Process["TEST", "1"]);
        Assert.Throws<IndexOutOfRangeException>(() => Env.User["TEST", "1"]);
        Assert.Throws<IndexOutOfRangeException>(() => Env.Machine["TEST", "1"]);

        Console.WriteLine((string)env.ThisVarDoesntExist);
        Console.WriteLine($"{env.ThisVarDoesntExist}");
    }
}