using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Singletons
{
    public class BiomeEpisodeMapping
    {
        private Dictionary<BiomeType, int> _biomeMappings;
        private static BiomeEpisodeMapping _instance;
        private BiomeEpisodeMapping()
        {
            GenerateMapping();
        }

        public static BiomeEpisodeMapping Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BiomeEpisodeMapping();
                }
                return _instance;
            }
        }

        public Dictionary<BiomeType, int> Mapping
        {
            get
            {
                if (_biomeMappings == null)
                {
                    GenerateMapping();
                }
                return _biomeMappings;
            }
        }

        private void GenerateMapping()
        {
            _biomeMappings = new Dictionary<BiomeType, int>();
            _biomeMappings.Add(BiomeType.KEEN4_CAVE, 4);
            _biomeMappings.Add(BiomeType.KEEN4_GREEN, 4);
            _biomeMappings.Add(BiomeType.KEEN4_MIRAGE, 4);
            _biomeMappings.Add(BiomeType.KEEN4_PYRAMID, 4);
            _biomeMappings.Add(BiomeType.KEEN5_BLACK, 5);
            _biomeMappings.Add(BiomeType.KEEN5_RED, 5);
            _biomeMappings.Add(BiomeType.KEEN5_GREEN, 5);
            _biomeMappings.Add(BiomeType.KEEN6_FOREST, 6);
            _biomeMappings.Add(BiomeType.KEEN6_INDUSTRIAL, 6);
            _biomeMappings.Add(BiomeType.KEEN6_DOME, 6);
            _biomeMappings.Add(BiomeType.KEEN6_FINAL, 6);
        }
    }
}
