using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enemies;

namespace KeenReloaded.Framework.Singletons
{
    public class EnemyEpisodeMapping
    {
        private Dictionary<string, int> _typeToEpisodeMapping;
        private static EnemyEpisodeMapping _instance;

        private EnemyEpisodeMapping()
        {
            GenerateMapping();
        }

        public static EnemyEpisodeMapping Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EnemyEpisodeMapping();
                }
                return _instance;
            }
        }

        public Dictionary<string, int> Mapping
        {
            get
            {
                if (_typeToEpisodeMapping == null)
                {
                    GenerateMapping();
                }
                return _typeToEpisodeMapping;
            }
        }

        private void GenerateMapping()
        {
            //keen 4 enemies
            _typeToEpisodeMapping = new Dictionary<string, int>();
            _typeToEpisodeMapping.Add(typeof(PoisonSlug).Name, 4);
            _typeToEpisodeMapping.Add(typeof(Lick).Name, 4);
            _typeToEpisodeMapping.Add(typeof(Bounder).Name, 4);
            _typeToEpisodeMapping.Add(typeof(Wormmouth).Name, 4);
            _typeToEpisodeMapping.Add(typeof(MadMushroom).Name, 4);
            _typeToEpisodeMapping.Add(typeof(Berkeloid).Name, 4);
            _typeToEpisodeMapping.Add(typeof(SkyPest).Name, 4);
            _typeToEpisodeMapping.Add(typeof(Mimrock).Name, 4);
            _typeToEpisodeMapping.Add(typeof(BlueEagleEgg).Name, 4);
            _typeToEpisodeMapping.Add(typeof(BlueEagle).Name, 4);
            _typeToEpisodeMapping.Add(typeof(Dopefish).Name, 4);
            _typeToEpisodeMapping.Add(typeof(ThunderCloud).Name, 4);
            _typeToEpisodeMapping.Add(typeof(Arachnut).Name, 4);
            _typeToEpisodeMapping.Add(typeof(Keen4Sprite).Name, 4);
            _typeToEpisodeMapping.Add(typeof(GnosticeneAncient).Name, 4);
            //keen 5 enemies
            _typeToEpisodeMapping.Add(typeof(Sparky).Name, 5);
            _typeToEpisodeMapping.Add(typeof(LittleAmpton).Name, 5);
            _typeToEpisodeMapping.Add(typeof(DiagonalSlicestar).Name, 5);
            _typeToEpisodeMapping.Add(typeof(HorizontalSlicestar).Name, 5);
            _typeToEpisodeMapping.Add(typeof(VerticalSlicestar).Name, 5);
            _typeToEpisodeMapping.Add(typeof(Shikadi).Name, 5);
            _typeToEpisodeMapping.Add(typeof(Shockshund).Name, 5);
            _typeToEpisodeMapping.Add(typeof(ShikadiMine).Name, 5);
            _typeToEpisodeMapping.Add(typeof(ShikadiMaster).Name, 5);
            _typeToEpisodeMapping.Add(typeof(Spirogrip).Name, 5);
            _typeToEpisodeMapping.Add(typeof(Spindred).Name, 5);
            _typeToEpisodeMapping.Add(typeof(RoboRed).Name, 5);
            _typeToEpisodeMapping.Add(typeof(VolteFace).Name, 5);
            _typeToEpisodeMapping.Add(typeof(Sphereful).Name, 5);
            _typeToEpisodeMapping.Add(typeof(KorathInhabitant).Name, 5);
            _typeToEpisodeMapping.Add(typeof(Shelley).Name, 5);
            //keen 6 enemies
            _typeToEpisodeMapping.Add(typeof(Bloog).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Blooglet).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Blooguard).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Blorb).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Babobba).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Bobba).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Ceilick).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Gik).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Orbatrix).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Fleex).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Flect).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Nospike).Name, 6);
            _typeToEpisodeMapping.Add(typeof(BipShip).Name, 6);
            _typeToEpisodeMapping.Add(typeof(Bip).Name, 6);
        }
    }
}
