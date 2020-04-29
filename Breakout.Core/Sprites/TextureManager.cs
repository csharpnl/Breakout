using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Core.Sprites
{
    public class TextureManager
    {
        #region Singleton
        private static TextureManager _instance = null;

        public static TextureManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TextureManager();

                return _instance;
            }
        }
        #endregion

        private Dictionary<int, TextureMapper> _mapping;

        private TextureManager()
        {
            _mapping = new Dictionary<int, TextureMapper>();
        }

        public TextureMapper CreateMapper(int index, string path, ContentManager content)
        {
            var mapper = new TextureMapper();
            mapper.LoadContent(content, path);
            _mapping.Add(index, mapper);

            return mapper;
        }

        public TextureMapper this[int index]
        {
            get
            {
                return _mapping[index];
            }
        }

        public void UnloadContent()
        {
            foreach (var mapper in _mapping)
            {
                mapper.Value.UnloadContent();
            }
        }
    }
}
