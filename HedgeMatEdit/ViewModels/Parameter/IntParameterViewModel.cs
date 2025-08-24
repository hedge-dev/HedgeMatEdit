using HedgeDev.Editor.Material.Attributes;
using J113D.UndoRedo;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using SharpNeedle.Structs;
using System.Globalization;
using System.Numerics;
using static J113D.UndoRedo.GlobalChangeTracker;

namespace HedgeDev.Editor.Material.ViewModels.Parameter
{
    internal class IntParameterViewModel : ParameterViewModel<Vector4Int>
    {
        private string _x, _y, _z, _w;

        [IntTextValidation]
        public string X
        {
            get => _x;
            set => UpdateVectorValue(value, _x, nameof(_x), nameof(X), 0);
        }

        [IntTextValidation]
        public string Y
        {
            get => _y;
            set => UpdateVectorValue(value, _y, nameof(_y), nameof(Y), 1);
        }

        [IntTextValidation]
        public string Z
        {
            get => _z;
            set => UpdateVectorValue(value, _z, nameof(_z), nameof(Z), 2);
        }

        [IntTextValidation]
        public string W
        {
            get => _w;
            set => UpdateVectorValue(value, _w, nameof(_w), nameof(W), 3);
        }

        public IntParameterViewModel(MaterialParameter<Vector4Int> data, ParametersViewModel<Vector4Int> parent, string name)
            : base(data, parent, name)
        {
            _x = data.Value.X.ToString(CultureInfo.InvariantCulture);
            _y = data.Value.Y.ToString(CultureInfo.InvariantCulture);
            _z = data.Value.Z.ToString(CultureInfo.InvariantCulture);
            _w = data.Value.W.ToString(CultureInfo.InvariantCulture);
        }

        private void UpdateVectorValue(string value, string fieldValue, string fieldName, string propertyName, int index)
        {
            if(value == fieldValue)
            {
                return;
            }

            BeginChangeGroup("IntParameterViewModel." + propertyName);

            TrackFieldChange(this, fieldName, value);
            this.AddChangeGroupInvokePropertyChanged(propertyName);

            try
            {
                int intValue = IntTextValidationAttribute.Validate(value);

                Vector4Int vector = _data.Value;
                bool updated;

                switch(index)
                {
                    case 0:
                        updated = vector.X != intValue;
                        vector.X = intValue;
                        break;
                    case 1:
                        updated = vector.Y != intValue;
                        vector.Y = intValue;
                        break;
                    case 2:
                        updated = vector.Z != intValue;
                        vector.Z = intValue;
                        break;
                    default:
                        updated = vector.W != intValue;
                        vector.W = intValue;
                        break;
                }

                if(updated)
                {
                    TrackPropertyChange(_data, nameof(MaterialParameter<Vector4>.Value), vector);
                }
            }
            finally
            {
                EndChangeGroup();
            }
        }
    }
}
