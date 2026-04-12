using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ApartmentAz.DAL.Data;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Cities.AnyAsync())
            return;

        // ══════════════════════════════════════════════════════════════════
        //  CITIES
        // ══════════════════════════════════════════════════════════════════

        var baku       = new City { Id = Guid.NewGuid() };
        var ganja      = new City { Id = Guid.NewGuid() };
        var sumgait    = new City { Id = Guid.NewGuid() };
        var mingachevir = new City { Id = Guid.NewGuid() };
        var lankaran   = new City { Id = Guid.NewGuid() };
        var sheki      = new City { Id = Guid.NewGuid() };
        var shirvan    = new City { Id = Guid.NewGuid() };
        var nakhchivan = new City { Id = Guid.NewGuid() };
        var absheron   = new City { Id = Guid.NewGuid() };
        var aghjabadi  = new City { Id = Guid.NewGuid() };
        var aghdam     = new City { Id = Guid.NewGuid() };
        var aghdash    = new City { Id = Guid.NewGuid() };
        var aghstafa   = new City { Id = Guid.NewGuid() };
        var aghsu      = new City { Id = Guid.NewGuid() };
        var astara     = new City { Id = Guid.NewGuid() };
        var balakan    = new City { Id = Guid.NewGuid() };
        var beylagan   = new City { Id = Guid.NewGuid() };
        var barda      = new City { Id = Guid.NewGuid() };
        var bilasuvar  = new City { Id = Guid.NewGuid() };
        var jabrayil   = new City { Id = Guid.NewGuid() };
        var jalilabad  = new City { Id = Guid.NewGuid() };
        var dashkasan  = new City { Id = Guid.NewGuid() };
        var fuzuli     = new City { Id = Guid.NewGuid() };
        var gadabay    = new City { Id = Guid.NewGuid() };
        var goranboy   = new City { Id = Guid.NewGuid() };
        var goychay    = new City { Id = Guid.NewGuid() };
        var goygol     = new City { Id = Guid.NewGuid() };
        var hajigabul  = new City { Id = Guid.NewGuid() };
        var imishli    = new City { Id = Guid.NewGuid() };
        var ismayilli  = new City { Id = Guid.NewGuid() };
        var kalbajat   = new City { Id = Guid.NewGuid() };
        var kurdamir   = new City { Id = Guid.NewGuid() };
        var lachin     = new City { Id = Guid.NewGuid() };
        var lerik      = new City { Id = Guid.NewGuid() };
        var masalli    = new City { Id = Guid.NewGuid() };
        var neftchala  = new City { Id = Guid.NewGuid() };
        var oghuz      = new City { Id = Guid.NewGuid() };
        var gakh       = new City { Id = Guid.NewGuid() };
        var gazakh     = new City { Id = Guid.NewGuid() };
        var gabala     = new City { Id = Guid.NewGuid() };
        var gobustan   = new City { Id = Guid.NewGuid() };
        var guba       = new City { Id = Guid.NewGuid() };
        var gubadli    = new City { Id = Guid.NewGuid() };
        var gusar      = new City { Id = Guid.NewGuid() };
        var saatli     = new City { Id = Guid.NewGuid() };
        var sabirabad  = new City { Id = Guid.NewGuid() };
        var shabran    = new City { Id = Guid.NewGuid() };
        var shahbuz    = new City { Id = Guid.NewGuid() };
        var shamakhi   = new City { Id = Guid.NewGuid() };
        var shamkir    = new City { Id = Guid.NewGuid() };
        var sharur     = new City { Id = Guid.NewGuid() };
        var siyazan    = new City { Id = Guid.NewGuid() };
        var tartar     = new City { Id = Guid.NewGuid() };
        var tovuz      = new City { Id = Guid.NewGuid() };
        var ujar       = new City { Id = Guid.NewGuid() };
        var yardimli   = new City { Id = Guid.NewGuid() };
        var yevlakh    = new City { Id = Guid.NewGuid() };
        var zaqatala   = new City { Id = Guid.NewGuid() };
        var zangilan   = new City { Id = Guid.NewGuid() };
        var zardab     = new City { Id = Guid.NewGuid() };

        var cities = new[]
        {
            baku, ganja, sumgait, mingachevir, lankaran, sheki, shirvan, nakhchivan,
            absheron, aghjabadi, aghdam, aghdash, aghstafa, aghsu, astara,
            balakan, beylagan, barda, bilasuvar, jabrayil, jalilabad,
            dashkasan, fuzuli, gadabay, goranboy, goychay, goygol,
            hajigabul, imishli, ismayilli, kalbajat, kurdamir,
            lachin, lerik, masalli, neftchala, oghuz, gakh, gazakh,
            gabala, gobustan, guba, gubadli, gusar, saatli, sabirabad,
            shabran, shahbuz, shamakhi, shamkir, sharur, siyazan,
            tartar, tovuz, ujar, yardimli, yevlakh, zaqatala, zangilan, zardab
        };

        await db.Cities.AddRangeAsync(cities);

        // ══════════════════════════════════════════════════════════════════
        //  CITY TRANSLATIONS  (az, en, ru)
        // ══════════════════════════════════════════════════════════════════

        var cityTranslations = new List<CityTranslation>();

        void AddCity(City c, string az, string en, string ru)
        {
            cityTranslations.Add(new CityTranslation { Id = Guid.NewGuid(), CityId = c.Id, Name = az, LanguageCode = "az" });
            cityTranslations.Add(new CityTranslation { Id = Guid.NewGuid(), CityId = c.Id, Name = en, LanguageCode = "en" });
            cityTranslations.Add(new CityTranslation { Id = Guid.NewGuid(), CityId = c.Id, Name = ru, LanguageCode = "ru" });
        }

        // Major cities
        AddCity(baku,        "Bakı",        "Baku",        "Баку");
        AddCity(ganja,       "Gəncə",       "Ganja",       "Гянджа");
        AddCity(sumgait,     "Sumqayıt",     "Sumgait",     "Сумгаит");
        AddCity(mingachevir, "Mingəçevir",   "Mingachevir", "Мингечевир");
        AddCity(lankaran,    "Lənkəran",     "Lankaran",    "Ленкорань");
        AddCity(sheki,       "Şəki",         "Sheki",       "Шеки");
        AddCity(shirvan,     "Şirvan",       "Shirvan",     "Ширван");
        AddCity(nakhchivan,  "Naxçıvan",     "Nakhchivan",  "Нахичевань");

        // Regions / rayons
        AddCity(absheron,   "Abşeron",     "Absheron",     "Абшерон");
        AddCity(aghjabadi,  "Ağcabədi",    "Aghjabadi",    "Агджабеди");
        AddCity(aghdam,     "Ağdam",       "Aghdam",       "Агдам");
        AddCity(aghdash,    "Ağdaş",       "Aghdash",      "Агдаш");
        AddCity(aghstafa,   "Ağstafa",     "Aghstafa",     "Агстафа");
        AddCity(aghsu,      "Ağsu",        "Aghsu",        "Агсу");
        AddCity(astara,     "Astara",      "Astara",       "Астара");
        AddCity(balakan,    "Balakən",     "Balakan",      "Балакен");
        AddCity(beylagan,   "Beyləqan",    "Beylagan",     "Бейлаган");
        AddCity(barda,      "Bərdə",       "Barda",        "Барда");
        AddCity(bilasuvar,  "Biləsuvar",   "Bilasuvar",    "Билясувар");
        AddCity(jabrayil,   "Cəbrayıl",    "Jabrayil",     "Джебраил");
        AddCity(jalilabad,  "Cəlilabad",   "Jalilabad",    "Джалилабад");
        AddCity(dashkasan,  "Daşkəsən",    "Dashkasan",    "Дашкесан");
        AddCity(fuzuli,     "Füzuli",      "Fuzuli",       "Физули");
        AddCity(gadabay,    "Gədəbəy",     "Gadabay",      "Гедабек");
        AddCity(goranboy,   "Goranboy",    "Goranboy",     "Горанбой");
        AddCity(goychay,    "Göyçay",      "Goychay",      "Гейчай");
        AddCity(goygol,     "Göygöl",      "Goygol",       "Гёйгёль");
        AddCity(hajigabul,  "Hacıqabul",   "Hajigabul",    "Гаджигабул");
        AddCity(imishli,    "İmişli",      "Imishli",      "Имишли");
        AddCity(ismayilli,  "İsmayıllı",   "Ismayilli",    "Исмаиллы");
        AddCity(kalbajat,   "Kəlbəcər",    "Kalbajar",     "Кельбаджар");
        AddCity(kurdamir,   "Kürdəmir",    "Kurdamir",     "Кюрдамир");
        AddCity(lachin,     "Laçın",       "Lachin",       "Лачин");
        AddCity(lerik,      "Lerik",       "Lerik",        "Лерик");
        AddCity(masalli,    "Masallı",     "Masalli",      "Масаллы");
        AddCity(neftchala,  "Neftçala",    "Neftchala",    "Нефтчала");
        AddCity(oghuz,      "Oğuz",        "Oghuz",        "Огуз");
        AddCity(gakh,       "Qax",         "Gakh",         "Гах");
        AddCity(gazakh,     "Qazax",       "Gazakh",       "Газах");
        AddCity(gabala,     "Qəbələ",      "Gabala",       "Габала");
        AddCity(gobustan,   "Qobustan",    "Gobustan",     "Гобустан");
        AddCity(guba,       "Quba",        "Guba",         "Губа");
        AddCity(gubadli,    "Qubadlı",     "Gubadli",      "Губадлы");
        AddCity(gusar,      "Qusar",       "Gusar",        "Гусар");
        AddCity(saatli,     "Saatlı",      "Saatli",       "Саатлы");
        AddCity(sabirabad,  "Sabirabad",   "Sabirabad",    "Сабирабад");
        AddCity(shabran,    "Şabran",      "Shabran",      "Шабран");
        AddCity(shahbuz,    "Şahbuz",      "Shahbuz",      "Шахбуз");
        AddCity(shamakhi,   "Şamaxı",      "Shamakhi",     "Шемаха");
        AddCity(shamkir,    "Şəmkir",      "Shamkir",      "Шамкир");
        AddCity(sharur,     "Şərur",       "Sharur",       "Шарур");
        AddCity(siyazan,    "Siyəzən",     "Siyazan",      "Сиазань");
        AddCity(tartar,     "Tərtər",      "Tartar",       "Тертер");
        AddCity(tovuz,      "Tovuz",       "Tovuz",        "Товуз");
        AddCity(ujar,       "Ucar",        "Ujar",         "Уджар");
        AddCity(yardimli,   "Yardımlı",    "Yardimli",     "Ярдымлы");
        AddCity(yevlakh,    "Yevlax",      "Yevlakh",      "Евлах");
        AddCity(zaqatala,   "Zaqatala",    "Zaqatala",     "Закатала");
        AddCity(zangilan,   "Zəngilan",    "Zangilan",     "Зангилан");
        AddCity(zardab,     "Zərdab",      "Zardab",       "Зардаб");

        await db.CityTranslations.AddRangeAsync(cityTranslations);

        // ══════════════════════════════════════════════════════════════════
        //  DISTRICTS  (Baku rayons — belong to Baku city)
        // ══════════════════════════════════════════════════════════════════

        var districtTranslations = new List<DistrictTranslation>();

        District MakeDistrict(City city, string az, string en, string ru)
        {
            var d = new District { Id = Guid.NewGuid(), CityId = city.Id };
            districtTranslations.Add(new DistrictTranslation { Id = Guid.NewGuid(), DistrictId = d.Id, Name = az, LanguageCode = "az" });
            districtTranslations.Add(new DistrictTranslation { Id = Guid.NewGuid(), DistrictId = d.Id, Name = en, LanguageCode = "en" });
            districtTranslations.Add(new DistrictTranslation { Id = Guid.NewGuid(), DistrictId = d.Id, Name = ru, LanguageCode = "ru" });
            return d;
        }

        // Baku districts
        var districts = new List<District>
        {
            MakeDistrict(baku, "Binəqədi",  "Binagadi",   "Бинагадинский"),
            MakeDistrict(baku, "Nəsimi",    "Nasimi",     "Насиминский"),
            MakeDistrict(baku, "Nərimanov",  "Narimanov",  "Наримановский"),
            MakeDistrict(baku, "Nizami",     "Nizami",     "Низаминский"),
            MakeDistrict(baku, "Sabunçu",    "Sabunchu",   "Сабунчинский"),
            MakeDistrict(baku, "Suraxanı",   "Surakhani",  "Сураханский"),
            MakeDistrict(baku, "Səbail",     "Sabail",     "Сабаильский"),
            MakeDistrict(baku, "Xətai",      "Khatai",     "Хатаинский"),
            MakeDistrict(baku, "Yasamal",    "Yasamal",    "Ясамальский"),
            MakeDistrict(baku, "Qaradağ",    "Garadagh",   "Гарадагский"),
            MakeDistrict(baku, "Pirallahı",  "Pirallahi",  "Пираллахинский"),
        };

        // Ganja districts
        districts.Add(MakeDistrict(ganja, "Kəpəz",    "Kapaz",    "Кяпазский"));
        districts.Add(MakeDistrict(ganja, "Nizami",    "Nizami",   "Низаминский"));

        // Sumgait mikrorayons
        districts.Add(MakeDistrict(sumgait, "1-ci mikrorayon", "Microdistrict 1", "1-й микрорайон"));
        districts.Add(MakeDistrict(sumgait, "2-ci mikrorayon", "Microdistrict 2", "2-й микрорайон"));
        districts.Add(MakeDistrict(sumgait, "3-cü mikrorayon", "Microdistrict 3", "3-й микрорайон"));
        districts.Add(MakeDistrict(sumgait, "4-cü mikrorayon", "Microdistrict 4", "4-й микрорайон"));
        districts.Add(MakeDistrict(sumgait, "5-ci mikrorayon", "Microdistrict 5", "5-й микрорайон"));
        districts.Add(MakeDistrict(sumgait, "6-cı mikrorayon", "Microdistrict 6", "6-й микрорайон"));

        // Mingachevir
        districts.Add(MakeDistrict(mingachevir, "Mingəçevir", "Mingachevir", "Мингечевир"));

        // Lankaran
        districts.Add(MakeDistrict(lankaran, "Lənkəran", "Lankaran", "Ленкорань"));

        // Sheki
        districts.Add(MakeDistrict(sheki, "Şəki", "Sheki", "Шеки"));

        // Shirvan
        districts.Add(MakeDistrict(shirvan, "Şirvan", "Shirvan", "Ширван"));

        // Nakhchivan
        districts.Add(MakeDistrict(nakhchivan, "Naxçıvan", "Nakhchivan", "Нахичевань"));

        await db.Districts.AddRangeAsync(districts);
        await db.DistrictTranslations.AddRangeAsync(districtTranslations);

        // ══════════════════════════════════════════════════════════════════
        //  METRO STATIONS  (Baku only)
        // ══════════════════════════════════════════════════════════════════

        var metroTranslations = new List<MetroTranslation>();

        Metro MakeMetro(string az, string en, string ru)
        {
            var m = new Metro { Id = Guid.NewGuid(), CityId = baku.Id };
            metroTranslations.Add(new MetroTranslation { Id = Guid.NewGuid(), MetroId = m.Id, Name = az, LanguageCode = "az" });
            metroTranslations.Add(new MetroTranslation { Id = Guid.NewGuid(), MetroId = m.Id, Name = en, LanguageCode = "en" });
            metroTranslations.Add(new MetroTranslation { Id = Guid.NewGuid(), MetroId = m.Id, Name = ru, LanguageCode = "ru" });
            return m;
        }

        var metros = new List<Metro>
        {
            // Green line (İçərişəhər → Həzi Aslanov)
            MakeMetro("İçərişəhər",           "Icherisheher",          "Ичеришехер"),
            MakeMetro("Sahil",                 "Sahil",                 "Сахил"),
            MakeMetro("28 May",                "28 May",                "28 Мая"),
            MakeMetro("Gənclik",               "Ganjlik",               "Гянджлик"),
            MakeMetro("Nərimanov",             "Narimanov",             "Нариманов"),
            MakeMetro("Bakmil",                "Bakmil",                "Бакмил"),
            MakeMetro("Ulduz",                 "Ulduz",                 "Улдуз"),
            MakeMetro("Koroğlu",               "Koroglu",               "Короглу"),
            MakeMetro("Qara Qarayev",          "Gara Garayev",          "Гара Гараев"),
            MakeMetro("Neftçilər",             "Neftchilar",            "Нефтчиляр"),
            MakeMetro("Xalqlar Dostluğu",      "Khalqlar Dostlugu",     "Халглар Достлугу"),
            MakeMetro("Əhmədli",               "Ahmadli",               "Ахмедлы"),
            MakeMetro("Həzi Aslanov",          "Hazi Aslanov",          "Гази Асланов"),

            // Red line (İçərişəhər → Dərnəgül)
            MakeMetro("Elmlər Akademiyası",    "Elmlar Akademiyasi",    "Эльмляр Академиясы"),
            MakeMetro("İnşaatçılar",           "Inshaatchilar",         "Иншаатчилар"),
            MakeMetro("20 Yanvar",             "20 January",            "20 Января"),
            MakeMetro("Memar Əcəmi",           "Memar Ajami",           "Мемар Аджеми"),
            MakeMetro("Avtovağzal",            "Avtovagzal",            "Автовокзал"),
            MakeMetro("8 Noyabr",              "8 November",            "8 Ноября"),
            MakeMetro("Dərnəgül",              "Darnagul",              "Дарнагюль"),

            // Purple line
            MakeMetro("Cəfər Cabbarlı",        "Jafar Jabbarly",        "Джафар Джаббарлы"),
            MakeMetro("Xətai",                 "Khatai",                "Хатаи"),
        };

        await db.Metros.AddRangeAsync(metros);
        await db.MetroTranslations.AddRangeAsync(metroTranslations);

        await db.SaveChangesAsync();
    }
}
