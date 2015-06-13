using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShopDataSyncConsole
{
    class CategoryMappings
    {
        private static Dictionary<string, int> mappings = new Dictionary<string, int>
        {
            {@"BABY", 4},
            {@"BABY\BABY_CLEMMY", 23},
            {@"BABY\BABY_RAZNO", 24},
            {@"BABY\BABY_UNIKATOY", 22},
            {@"BALONI", 5},
            {@"DECKI", 6},
            {@"DECKI\DECKI_RAZNO", 26},
            {@"DECKI\DECKI_BOJEVNISKI", 25},
            {@"DEKLICE", 7},
            {@"DEKLICE\DEKLICE_RAZNO", 28},
            {@"DEKLICE\DEKLICE_DOJPUN", 27},
            {@"DRUZABNE", 8},
            {@"DRUZABNE\DRUZABNE_RAZNO", 34},
            {@"DRUZABNE\DRUZABNE_CLEMENT", 30},
            {@"DRUZABNE\DRUZABNE_KARTE", 33},
            {@"DRUZABNE\DRUZABNE_LOPARJI", 31},
            {@"DRUZABNE\DRUZABNE_UNIKA", 29},
            {@"DRUZABNE\DRUZABNE_PROSTEM", 32},
            {@"GLASBILA", 9},
            {@"MEHURCKI", 10},
            {@"PLIS", 11},
            {@"PLIS\PLIS_BATERIJE", 38},
            {@"PLIS\PLIS_RAZNO", 39},
            {@"PLIS\PLIS_VELIKI", 37},
            {@"PLIS\PLIS_MALI", 35},
            {@"PLIS\PLIS_SREDNJI", 36},
            {@"POLETNI", 12},
            {@"POLETNI\POLETNI_PESKOVNI", 42},
            {@"POLETNI\POLETNI_VODNI", 41},
            {@"POLETNI\POLETNI_RAZNO", 43},
            {@"POLETNI\POLETNI_SPRICANJ", 40},
            {@"PUSTNI", 13},
            {@"PUSTNI\PUSTNI_KOSTUMI", 44},
            {@"PUSTNI\PUSTNI_RAZNO", 46},
            {@"PUSTNI\PUSTNI_LASULJE", 45},
            {@"PUZZLE", 14},
            {@"RAZNO", 18},
            {@"RAZNO\RAZNO_OSTALO", 58},
            {@"RAZNO\RAZNO_IGRACE", 56},
            {@"RAZNO\RAZNO_ZIMA", 57},
            {@"UNIKATOY", 19},
            {@"VOZILA", 15},
            {@"VOZILA\VOZILA_RAZNO", 51},
            {@"VOZILA\VOZILA_DIECAST", 49},
            {@"VOZILA\VOZILA_PLASTIKA", 50},
            {@"VOZILA\VOZILA_BATERIJE", 48},
            {@"VOZILA\VOZILA_DALJINCI", 47},
            {@"ZIVALSKI", 16},
            {@"ZOGE", 17},
            {@"ZOGE\ZOGE_G2AIR", 54},
            {@"ZOGE\ZOGE_RAZNO", 55},
            {@"ZOGE\ZOGE_SPORTNE", 53},
            {@"ZOGE\ZOGE_OTROSKE", 52}
        };

        public static int getCategoryId(string category, string subCategory)
        {
            string key = category.Trim();
            if (subCategory != null)
            {
                key += @"\" + subCategory.Trim();
            }
            if (!mappings.ContainsKey(key))
            {
                throw new Exception("Category key mapping doesn't exist: " + key);
            }
            return mappings[key];
        }
    }
}
