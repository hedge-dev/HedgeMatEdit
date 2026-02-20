using HedgeDev.Editor.Material.ViewModels.Base;
using J113D.UndoRedo.Collections;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System.Collections.ObjectModel;
using System.Linq;
using static J113D.UndoRedo.GlobalChangeTracker;
using SNTexture = SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData.Texture;

namespace HedgeDev.Editor.Material.ViewModels.Resource.Texture
{
    internal class TextureSetViewModel : ViewModelBase
    {
        private readonly Texset _data;
        private readonly TrackList<SNTexture> _dataList;
        private readonly TrackList<TextureViewModel> _textures;

        public ReadOnlyObservableCollection<TextureViewModel> Textures { get; }

        public TextureSetViewModel(Texset data)
        {
            _data = data;
            _dataList = new(data.Textures);

            ObservableCollection<TextureViewModel> textures = new(
                data.Textures.Select(x => new TextureViewModel(x, this)));

            _textures = new(textures);
            Textures = new(textures);
        }

        private void AddTexture(SNTexture texture)
        {
            BeginChangeGroup("TextureSetViewModel.AddTexture");
            TextureViewModel viewmodel = new(texture, this);

            _dataList.Add(texture);
            _textures.Add(viewmodel);
            EndChangeGroup();
        }

        public void AddNewTexture()
        {
            AddTexture(new()
            {
                Type = "diffuse"
            });
        }

        public void RemoveTexture(TextureViewModel texture)
        {
            BeginChangeGroup("TextureSetViewModel.RemoveTexture");
            _dataList.Remove(texture._data);
            _textures.Remove(texture);
            EndChangeGroup();
        }
    
        public void FromJsonImport(Texset textureSet)
        {
            BeginChangeGroup("TextureSetViewModel.FromJsonImport");

            foreach (TextureViewModel texture in Textures.ToArray())
            {
                RemoveTexture(texture);
            }

            foreach (SNTexture texture in textureSet.Textures)
            {
                AddTexture(texture);
            }

            EndChangeGroup();
        }
    }
}
