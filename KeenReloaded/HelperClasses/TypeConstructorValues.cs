using KeenReloaded.Framework.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.HelperClasses
{
    public class TypeConstructorValues
    {
        private readonly Type _type;
        private readonly object[] _constructorParamNames;
        private object[] _values;

        public TypeConstructorValues(Type type, object[] constructorParamNames = null, object[] defaultValues = null)
        {
            _type = type;
            _constructorParamNames = constructorParamNames;
            _values = defaultValues;
        }

        public Type Type
        {
            get
            {
                return _type;
            }
        }

        public object[] ConstructorParamNames
        {
            get
            {
                return _constructorParamNames;
            }
        }

        public object[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }

        public object ConstructObject()
        {
            try
            {
                return Activator.CreateInstance(_type, _values);
            }
            catch (Exception ex)
            {
                if (_type.Name == typeof(NeuralStunnerAmmo).Name)
                {
                    _values[2] = Convert.ToInt32(_values[2]);
                }
                Type[] types = _values.Select(v => v.GetType()).ToArray();
                return _type.GetConstructor(types).Invoke(_values);
            }
        }
    }
}
