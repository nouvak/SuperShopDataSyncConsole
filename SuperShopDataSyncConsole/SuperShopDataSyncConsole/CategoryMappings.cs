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
            {@"BABY", 3},
            {@"BABY\BABY_CLEMMY", 21},
            {@"BABY\BABY_RAZNO", 22},
            {@"BABY\BABY_UNIKATOY", 20},
            {@"BALONI", 4},
            {@"DECKI", 5},
            {@"DECKI\DECKI_RAZNO", 24},
            {@"DECKI\DECKI_BOJEVNISKI", 23},
            {@"DEKLICE", 6},
            {@"DEKLICE\DEKLICE_RAZNO", 26},
            {@"DEKLICE\DEKLICE_DOJPUN", 25},
            {@"DRUZABNE", 7},
            {@"DRUZABNE\DRUZABNE_RAZNO", 32},
            {@"DRUZABNE\DRUZABNE_CLEMENT", 28},
            {@"DRUZABNE\DRUZABNE_KARTE", 31},
            {@"DRUZABNE\DRUZABNE_LOPARJI", 29},
            {@"DRUZABNE\DRUZABNE_UNIKA", 27},
            {@"DRUZABNE\DRUZABNE_PROSTEM", 30},
            {@"GLASBILA", 8},
            {@"MEHURCKI", 9},
            {@"PLIS", 10},
            {@"PLIS\PLIS_BATERIJE", 36},
            {@"PLIS\PLIS_RAZNO", 37},
            {@"PLIS\PLIS_VELIKI", 35},
            {@"PLIS\PLIS_MALI", 33},
            {@"PLIS\PLIS_SREDNJI", 34},
            {@"POLETNI", 11},
            {@"POLETNI\POLETNI_PESKOVNI", 40},
            {@"POLETNI\POLETNI_VODNI", 39},
            {@"POLETNI\POLETNI_RAZNO", 41},
            {@"POLETNI\POLETNI_SPRICANJ", 38},
            {@"PUSTNI", 12},
            {@"PUSTNI\PUSTNI_KOSTUMI", 42},
            {@"PUSTNI\PUSTNI_RAZNO", 44},
            {@"PUSTNI\PUSTNI_LASULJE", 43},
            {@"PUZZLE", 13},
            {@"RAZNO", 17},
            {@"RAZNO\RAZNO_OSTALO", 56},
            {@"RAZNO\RAZNO_IGRACE", 54},
            {@"RAZNO\RAZNO_ZIMA", 55},
            {@"UNIKATOY", 18},
            {@"VOZILA", 14},
            {@"VOZILA\VOZILA_RAZNO", 49},
            {@"VOZILA\VOZILA_DIECAST", 47},
            {@"VOZILA\VOZILA_PLASTIKA", 48},
            {@"VOZILA\VOZILA_BATERIJE", 46},
            {@"VOZILA\VOZILA_DALJINCI", 45},
            {@"ZIVALSKI", 15},
            {@"ZOGE", 16},
            {@"ZOGE\ZOGE_G2AIR", 52},
            {@"ZOGE\ZOGE_RAZNO", 53},
            {@"ZOGE\ZOGE_SPORTNE", 51},
            {@"ZOGE\ZOGE_OTROSKE", 50}
        };

        public static int getCategoryId(string category, string subCategory)
        {
            string key = category.Trim();
            if (subCategory != null && !subCategory.Trim().Equals(""))
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
