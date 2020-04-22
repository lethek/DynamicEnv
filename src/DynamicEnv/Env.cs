using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;


namespace DynamicEnv
{

    public class Env : DynamicObject
    {
        public static readonly dynamic Process = new Env(EnvironmentVariableTarget.Process);
        public static readonly dynamic User = new Env(EnvironmentVariableTarget.User);
        public static readonly dynamic Machine = new Env(EnvironmentVariableTarget.Machine);


        public Env() : this(EnvironmentVariableTarget.Process) { }


        public Env(EnvironmentVariableTarget target)
            => _target = target;


        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetEnvVar(binder.Name);
            return true;
        }


        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetEnvVar(binder.Name, value);
            return true;
        }


        [ExcludeFromCodeCoverage]
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            DelEnvVar(binder.Name);
            return true;
        }


        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = GetEnvVar(GetEnvKeyFromIndexes(indexes));
            return true;
        }


        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            SetEnvVar(GetEnvKeyFromIndexes(indexes), value);
            return true;
        }


        [ExcludeFromCodeCoverage]
        public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
        {
            DelEnvVar(GetEnvKeyFromIndexes(indexes));
            return true;
        }


        public override IEnumerable<string> GetDynamicMemberNames()
            => Environment.GetEnvironmentVariables(_target).Keys.Cast<string>();


        private string GetEnvVar(string name)
            => Environment.GetEnvironmentVariable(name, _target);


        private void SetEnvVar(string name, object value)
        {
            if (value != null && value.GetType() != typeof(string)) {
                throw new ArgumentException("Value must be of type String", nameof(value));
            }

            Environment.SetEnvironmentVariable(name, (string)value, _target);
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

}
