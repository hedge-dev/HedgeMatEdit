using HedgeDev.Editor.Material.Attributes;
using J113D.UndoRedo;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System.Globalization;
using System.Numerics;
using static J113D.UndoRedo.GlobalChangeTracker;

namespace HedgeDev.Editor.Material.ViewModels.Resource.Parameter
{
    internal class FloatParameterViewModel : ParameterViewModel<Vector4>
    {
        private string _x, _y, _z, _w;

        [FloatTextValidation]
        public string X
        {
            get => _x;
            set => UpdateVectorValue(value, _x, nameof(_x), nameof(X), 0);
        }

        [FloatTextValidation]
        public string Y
        {
            get => _y;
            set => UpdateVectorValue(value, _y, nameof(_y), nameof(Y), 1);
        }

        [FloatTextValidation]
        public string Z
        {
            get => _z;
            set => UpdateVectorValue(value, _z, nameof(_z), nameof(Z), 2);
        }

        [FloatTextValidation]
        public string W
        {
            get => _w;
            set => UpdateVectorValue(value, _w, nameof(_w), nameof(W), 3);
        }

        public FloatParameterViewModel(MaterialParameter<Vector4> data, ParametersViewModel<Vector4> parent, string name)
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

            BeginChangeGroup("FloatParameterViewModel." + propertyName);

            TrackFieldChange(this, fieldName, value);
            this.AddChangeGroupInvokePropertyChanged(propertyName);

            try
            {
                float floatValue = FloatTextValidationAttribute.Validate(value);

                if(_data.Value[index] != floatValue)
                {
                    Vector4 newVector = _data.Value;
                    newVector[index] = floatValue;

                    TrackPropertyChange(_data, nameof(MaterialParameter<Vector4>.Value), newVector);
                }
            }
            finally
            {
                EndChangeGroup();
            }
        }
    }
}
