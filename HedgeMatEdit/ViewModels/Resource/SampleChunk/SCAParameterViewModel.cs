using HedgeDev.Editor.Material.Attributes;
using HedgeDev.Editor.Material.ViewModels.Base;
using J113D.Avalonia.Utilities.Enum;
using SharpNeedle.Framework.HedgehogEngine.Mirage;
using System;
using System.Globalization;
using System.Linq;
using static J113D.UndoRedo.GlobalChangeTracker;

namespace HedgeDev.Editor.Material.ViewModels.Resource.SampleChunk
{
    internal class SCAParameterViewModel : ViewModelBase
    {
        internal readonly SampleChunkNode _data;
        private readonly SCAParametersViewModel _parent;

        private SCAParameterType _displayType;
        private string _uintValue, _intValue, _floatValue;
        private bool _boolValue;

        public static EnumDescription[] DisplayTypes = EnumUtils.ToDescriptions<SCAParameterType>().ToArray();

        public string Name
        {
            get => _data.Name;
            set
            {
                if(_data.Name == value)
                {
                    return;
                }

                BeginChangeGroup("SCAParameterViewModel.Name");
                TrackPropertyChange(_data, nameof(SampleChunkNode.Name), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(Name));
                EndChangeGroup();
            }
        }

        public SCAParameterType DisplayType
        {
            get => _displayType;
            set
            {
                if(_displayType == value)
                {
                    return;
                }

                BeginChangeGroup("SCAParameterViewModel.DisplayType");
                TrackFieldChange(this, nameof(_displayType), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(DisplayType));
                this.AddChangeGroupInvokePropertyChanged(nameof(ShowUInt));
                this.AddChangeGroupInvokePropertyChanged(nameof(ShowInt));
                this.AddChangeGroupInvokePropertyChanged(nameof(ShowFloat));
                this.AddChangeGroupInvokePropertyChanged(nameof(ShowBool));
                EndChangeGroup();
            }
        }


        [UIntTextValidation]
        public string UIntValue
        {
            get => _uintValue;
            set => UpdateNumber(
                value, 
                _uintValue, 
                nameof(_uintValue), 
                nameof(UIntValue), 
                _data.Value, 
                UIntTextValidationAttribute.Validate, 
                nameof(SampleChunkNode.Value),
                SCAParameterType.UnsignedInteger
            );
        }

        [IntTextValidation]
        public string IntValue
        {
            get => _intValue;
            set => UpdateNumber(
                value,
                _intValue,
                nameof(_intValue),
                nameof(IntValue),
                _data.SignedValue,
                IntTextValidationAttribute.Validate,
                nameof(SampleChunkNode.SignedValue),
                SCAParameterType.SignedInteger
            );
        }

        [FloatTextValidation]
        public string FloatValue
        {
            get => _floatValue;
            set => UpdateNumber(
                value,
                _floatValue,
                nameof(_floatValue),
                nameof(FloatValue),
                _data.FloatValue,
                FloatTextValidationAttribute.Validate,
                nameof(SampleChunkNode.FloatValue),
                SCAParameterType.Float
            );
        }

        public bool BoolValue
        {
            get => _boolValue;
            set
            {
                if(value == _boolValue)
                {
                    return;
                }

                BeginChangeGroup("SCAParameterViewModel.BoolValue");

                TrackFieldChange(this, nameof(_boolValue), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(BoolValue));

                TrackPropertyChange(_data, nameof(SampleChunkNode.Value), value ? 1u : 0u);
                UpdateOtherValues(SCAParameterType.Boolean);
                
                EndChangeGroup();
            }
        }


        public bool ShowUInt => DisplayType == SCAParameterType.UnsignedInteger;
        public bool ShowInt => DisplayType == SCAParameterType.SignedInteger;
        public bool ShowFloat => DisplayType == SCAParameterType.Float;
        public bool ShowBool => DisplayType == SCAParameterType.Boolean;

        public SCAParameterViewModel(SampleChunkNode data, SCAParametersViewModel parent)
        {
            _data = data;
            _parent = parent;

            _displayType = SCAParameterType.UnsignedInteger;
            _uintValue = data.Value.ToString(CultureInfo.InvariantCulture);
            _intValue = data.SignedValue.ToString(CultureInfo.InvariantCulture);
            _floatValue = data.FloatValue.ToString("0.#####", CultureInfo.InvariantCulture);
            _boolValue = data.Value != 0;
        }


        private void UpdateNumber<T>(
            string newText, 
            string oldText, 
            string fieldName, 
            string propertyName, 
            T oldValue, 
            Func<string, T> validate, 
            string dataPropertyName,
            SCAParameterType type) where T: unmanaged
        {
            if(newText == oldText)
            {
                return;
            }

            BeginChangeGroup("SCAParameterViewModel." + propertyName);

            TrackFieldChange(this, fieldName, newText);
            this.AddChangeGroupInvokePropertyChanged(propertyName);

            try
            {
                T newValue = validate(newText);

                if(!newValue.Equals(oldValue))
                {
                    TrackPropertyChange(_data, dataPropertyName, newValue);
                    UpdateOtherValues(type);
                }
            }
            finally
            {
                EndChangeGroup();
            }
        }


        private void UpdateOtherValues(SCAParameterType type)
        {
            if(type != SCAParameterType.UnsignedInteger)
            {
                string uintValue = _data.Value.ToString(CultureInfo.InvariantCulture);
                TrackFieldChange(this, nameof(_uintValue), uintValue);
                this.AddChangeGroupInvokePropertyChanged(nameof(UIntValue));
            }

            if(type != SCAParameterType.SignedInteger)
            {
                string intValue = _data.SignedValue.ToString(CultureInfo.InvariantCulture);
                TrackFieldChange(this, nameof(_intValue), intValue);
                this.AddChangeGroupInvokePropertyChanged(nameof(IntValue));
            }

            if(type != SCAParameterType.Float)
            {
                string floatValue = _data.FloatValue.ToString("0.#####", CultureInfo.InvariantCulture);
                TrackFieldChange(this, nameof(_floatValue), floatValue);
                this.AddChangeGroupInvokePropertyChanged(nameof(FloatValue));
            }

            if(type != SCAParameterType.Boolean)
            {
                bool boolValue = _data.Value != 0;
                TrackFieldChange(this, nameof(_boolValue), boolValue);
                this.AddChangeGroupInvokePropertyChanged(nameof(BoolValue));
            }
        }

        public void Remove()
        {
            _parent.RemoveParameter(this);
        }
    }
}
