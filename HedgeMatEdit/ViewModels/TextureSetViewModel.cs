using J113D.UndoRedo.Collections;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System.Collections.ObjectModel;
using System.Linq;
using static J113D.UndoRedo.GlobalChangeTracker;

namespace HedgeDev.Editor.Material.ViewModels
{
    internal class TextureSetViewModel : ViewModelBase
    {
        private readonly Texset _data;
        private readonly TrackList<Texture> _dataList;
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

        public void AddNewTexture()
        {
            BeginChangeGroup("TextureSetViewModel.AddNewTexture");

            Texture texture = new()
            {
                Type = "diffuse"
            };

            TextureViewModel viewmodel = new(texture, this);

            _dataList.Add(texture);
            _textures.Add(viewmodel);

            EndChangeGroup();
        }

        public void RemoveTexture(TextureViewModel texture)
        {
            BeginChangeGroup("TextureSetViewModel.RemoveTexture");
            _dataList.Remove(texture._data);
            _textures.Remove(texture);
            EndChangeGroup();
        }
    }
}
