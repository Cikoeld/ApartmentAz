namespace ApartmentAz.CLIENT.Helpers;

public class UILocalizer
{
    private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
    {
        // ── Navigation ──────────────────────────────────────────────
        ["Home"] = new() { ["az"] = "Ana səhifə", ["en"] = "Home", ["ru"] = "Главная" },
        ["CreateListing"] = new() { ["az"] = "Elan yarat", ["en"] = "Create Listing", ["ru"] = "Создать объявление" },
        ["Favorites"] = new() { ["az"] = "Sevimlilər", ["en"] = "Favorites", ["ru"] = "Избранное" },
        ["Logout"] = new() { ["az"] = "Çıxış", ["en"] = "Logout", ["ru"] = "Выход" },
        ["Login"] = new() { ["az"] = "Giriş", ["en"] = "Login", ["ru"] = "Вход" },
        ["Register"] = new() { ["az"] = "Qeydiyyat", ["en"] = "Register", ["ru"] = "Регистрация" },
        ["AdminPanel"] = new() { ["az"] = "Admin Panel", ["en"] = "Admin Panel", ["ru"] = "Админ панель" },
        ["WelcomeBack"] = new() { ["az"] = "Xoş gəlmisiniz", ["en"] = "Welcome back", ["ru"] = "С возвращением" },
        ["SignInToYourAccount"] = new() { ["az"] = "ApartmentAz hesabınıza daxil olun", ["en"] = "Sign in to your ApartmentAz account", ["ru"] = "Войдите в свой аккаунт ApartmentAz" },
        ["Password"] = new() { ["az"] = "Şifrə", ["en"] = "Password", ["ru"] = "Пароль" },
        ["RememberMe"] = new() { ["az"] = "Məni xatırla", ["en"] = "Remember me", ["ru"] = "Запомнить меня" },
        ["SignIn"] = new() { ["az"] = "Daxil ol", ["en"] = "Sign In", ["ru"] = "Войти" },
        ["DontHaveAnAccount"] = new() { ["az"] = "Hesabınız yoxdur?", ["en"] = "Don't have an account?", ["ru"] = "Нет аккаунта?" },
        ["CreateOne"] = new() { ["az"] = "Yaradın", ["en"] = "Create one", ["ru"] = "Создать" },

        // ── Listing results ─────────────────────────────────────────
        ["NoListingsFound"] = new() { ["az"] = "Elan tapılmadı", ["en"] = "No listings found", ["ru"] = "Объявления не найдены" },
        ["TryDifferentFilters"] = new() { ["az"] = "Fərqli filterlər sınayın", ["en"] = "Try different filters", ["ru"] = "Попробуйте другие фильтры" },
        ["ShowingResults"] = new() { ["az"] = "Nəticələr", ["en"] = "Results", ["ru"] = "Результатов" },
        ["Saving"] = new() { ["az"] = "Saxlanılır", ["en"] = "Saving", ["ru"] = "Сохранение" },

        // ── Filter labels ───────────────────────────────────────────
        ["City"] = new() { ["az"] = "Şəhər", ["en"] = "City", ["ru"] = "Город" },
        ["District"] = new() { ["az"] = "Rayon", ["en"] = "District", ["ru"] = "Район" },
        ["ListingType"] = new() { ["az"] = "Elan növü", ["en"] = "Listing Type", ["ru"] = "Тип объявления" },
        ["PropertyType"] = new() { ["az"] = "Əmlak növü", ["en"] = "Property Type", ["ru"] = "Тип недвижимости" },
        ["RepairStatus"] = new() { ["az"] = "Təmir", ["en"] = "Repair Status", ["ru"] = "Ремонт" },
        ["Rooms"] = new() { ["az"] = "Otaqlar", ["en"] = "Rooms", ["ru"] = "Комнаты" },
        ["MinPrice"] = new() { ["az"] = "Min qiymət", ["en"] = "Min Price", ["ru"] = "Мин. цена" },
        ["MaxPrice"] = new() { ["az"] = "Maks qiymət", ["en"] = "Max Price", ["ru"] = "Макс. цена" },
        ["MinArea"] = new() { ["az"] = "Min sahə (m²)", ["en"] = "Min Area (m²)", ["ru"] = "Мин. площадь (м²)" },
        ["MaxArea"] = new() { ["az"] = "Maks sahə (m²)", ["en"] = "Max Area (m²)", ["ru"] = "Макс. площадь (м²)" },
        ["SortBy"] = new() { ["az"] = "Sıralama", ["en"] = "Sort By", ["ru"] = "Сортировка" },
        ["All"] = new() { ["az"] = "Hamısı", ["en"] = "All", ["ru"] = "Все" },

        // ── Listing type values ─────────────────────────────────────
        ["Sale"] = new() { ["az"] = "Satış", ["en"] = "Sale", ["ru"] = "Продажа" },
        ["Rent"] = new() { ["az"] = "Kirayə", ["en"] = "Rent", ["ru"] = "Аренда" },

        // ── Property type values ────────────────────────────────────
        ["NewBuilding"] = new() { ["az"] = "Yeni tikili", ["en"] = "New Building", ["ru"] = "Новостройка" },
        ["OldBuilding"] = new() { ["az"] = "Köhnə tikili", ["en"] = "Old Building", ["ru"] = "Вторичка" },
        ["House"] = new() { ["az"] = "Ev", ["en"] = "House", ["ru"] = "Дом" },
        ["Office"] = new() { ["az"] = "Ofis", ["en"] = "Office", ["ru"] = "Офис" },
        ["Garage"] = new() { ["az"] = "Qaraj", ["en"] = "Garage", ["ru"] = "Гараж" },
        ["Land"] = new() { ["az"] = "Torpaq", ["en"] = "Land", ["ru"] = "Земля" },
        ["Commercial"] = new() { ["az"] = "Kommersiya", ["en"] = "Commercial", ["ru"] = "Коммерция" },

        // ── Repair status values ────────────────────────────────────
        ["None"] = new() { ["az"] = "Yoxdur", ["en"] = "None", ["ru"] = "Нет" },
        ["Repaired"] = new() { ["az"] = "Təmirli", ["en"] = "Repaired", ["ru"] = "С ремонтом" },
        ["NotRepaired"] = new() { ["az"] = "Təmirsiz", ["en"] = "Not Repaired", ["ru"] = "Без ремонта" },

        // ── Sort options ────────────────────────────────────────────
        ["Newest"] = new() { ["az"] = "Ən yeni", ["en"] = "Newest", ["ru"] = "Новые" },
        ["Oldest"] = new() { ["az"] = "Ən köhnə", ["en"] = "Oldest", ["ru"] = "Старые" },
        ["PriceAsc"] = new() { ["az"] = "Qiymət ↑", ["en"] = "Price ↑", ["ru"] = "Цена ↑" },
        ["PriceDesc"] = new() { ["az"] = "Qiymət ↓", ["en"] = "Price ↓", ["ru"] = "Цена ↓" },
        ["AreaAsc"] = new() { ["az"] = "Sahə ↑", ["en"] = "Area ↑", ["ru"] = "Площадь ↑" },
        ["AreaDesc"] = new() { ["az"] = "Sahə ↓", ["en"] = "Area ↓", ["ru"] = "Площадь ↓" },

        // ── Details page ────────────────────────────────────────────
        ["Details"] = new() { ["az"] = "Detallar", ["en"] = "Details", ["ru"] = "Подробности" },
        ["Price"] = new() { ["az"] = "Qiymət", ["en"] = "Price", ["ru"] = "Цена" },
        ["Area"] = new() { ["az"] = "Sahə", ["en"] = "Area", ["ru"] = "Площадь" },
        ["Floor"] = new() { ["az"] = "Mərtəbə", ["en"] = "Floor", ["ru"] = "Этаж" },
        ["Document"] = new() { ["az"] = "Sənəd", ["en"] = "Document", ["ru"] = "Документ" },
        ["Mortgage"] = new() { ["az"] = "İpoteka", ["en"] = "Mortgage", ["ru"] = "Ипотека" },
        ["Yes"] = new() { ["az"] = "Bəli", ["en"] = "Yes", ["ru"] = "Да" },
        ["No"] = new() { ["az"] = "Xeyr", ["en"] = "No", ["ru"] = "Нет" },
        ["Location"] = new() { ["az"] = "Məkan", ["en"] = "Location", ["ru"] = "Местоположение" },
        ["Metro"] = new() { ["az"] = "Metro", ["en"] = "Metro", ["ru"] = "Метро" },
        ["Contact"] = new() { ["az"] = "Əlaqə", ["en"] = "Contact", ["ru"] = "Контакт" },
        ["Agency"] = new() { ["az"] = "Agentlik", ["en"] = "Agency", ["ru"] = "Агентство" },
        ["ResidentialComplex"] = new() { ["az"] = "Yaşayış kompleksi", ["en"] = "Residential Complex", ["ru"] = "Жилой комплекс" },
        ["BackToListings"] = new() { ["az"] = "Elanlara qayıt", ["en"] = "Back to Listings", ["ru"] = "Назад к объявлениям" },
        ["ViewDetails"] = new() { ["az"] = "Ətraflı bax", ["en"] = "View Details", ["ru"] = "Подробнее" },
        ["SaveToFavorites"] = new() { ["az"] = "Sevimlilərə əlavə et", ["en"] = "Save to Favorites", ["ru"] = "В избранное" },
        ["RemoveFromFavorites"] = new() { ["az"] = "Sevimlilərdən sil", ["en"] = "Remove from Favorites", ["ru"] = "Удалить из избранного" },
        ["DeleteListing"] = new() { ["az"] = "Elanı sil", ["en"] = "Delete Listing", ["ru"] = "Удалить объявление" },
        ["ConfirmDelete"] = new() { ["az"] = "Bu elanı silmək istəyirsiniz?", ["en"] = "Delete this listing?", ["ru"] = "Удалить это объявление?" },
        ["CommercialDetails"] = new() { ["az"] = "Kommersiya detalları", ["en"] = "Commercial Details", ["ru"] = "Коммерческие детали" },
        ["Parking"] = new() { ["az"] = "Parkinq", ["en"] = "Parking", ["ru"] = "Парковка" },
        ["Heating"] = new() { ["az"] = "İstilik", ["en"] = "Heating", ["ru"] = "Отопление" },
        ["AirConditioner"] = new() { ["az"] = "Kondisioner", ["en"] = "Air Conditioner", ["ru"] = "Кондиционер" },
        ["NoListingsFound"] = new() { ["az"] = "Elan tapılmadı.", ["en"] = "No listings found.", ["ru"] = "Объявления не найдены." },
        ["Room"] = new() { ["az"] = "otaq", ["en"] = "room", ["ru"] = "ком." },

        // ── Create page ─────────────────────────────────────────────
        ["CreateNewListing"] = new() { ["az"] = "Yeni elan yarat", ["en"] = "Create New Listing", ["ru"] = "Создать новое объявление" },
        ["Translations"] = new() { ["az"] = "Tərcümələr", ["en"] = "Translations", ["ru"] = "Переводы" },
        ["Title"] = new() { ["az"] = "Başlıq", ["en"] = "Title", ["ru"] = "Заголовок" },
        ["DescriptionLabel"] = new() { ["az"] = "Təsvir", ["en"] = "Description", ["ru"] = "Описание" },
        ["AutoTranslateHint"] = new() { ["az"] = "Digər dillərə avtomatik tərcümə olunacaq.", ["en"] = "Will be auto-translated to other languages.", ["ru"] = "Будет автоматически переведено на другие языки." },
        ["PropertyInfo"] = new() { ["az"] = "Əmlak məlumatı", ["en"] = "Property Info", ["ru"] = "Информация об объекте" },
        ["PriceAZN"] = new() { ["az"] = "Qiymət (₼)", ["en"] = "Price (₼)", ["ru"] = "Цена (₼)" },
        ["RoomCount"] = new() { ["az"] = "Otaq sayı", ["en"] = "Room Count", ["ru"] = "Количество комнат" },
        ["AreaM2"] = new() { ["az"] = "Sahə (m²)", ["en"] = "Area (m²)", ["ru"] = "Площадь (м²)" },
        ["TotalFloors"] = new() { ["az"] = "Ümumi mərtəbə", ["en"] = "Total Floors", ["ru"] = "Всего этажей" },
        ["ContactInfo"] = new() { ["az"] = "Əlaqə məlumatı", ["en"] = "Contact Info", ["ru"] = "Контактная информация" },
        ["Name"] = new() { ["az"] = "Ad", ["en"] = "Name", ["ru"] = "Имя" },
        ["Email"] = new() { ["az"] = "E-poçt", ["en"] = "Email", ["ru"] = "Электронная почта" },
        ["Phone"] = new() { ["az"] = "Telefon", ["en"] = "Phone", ["ru"] = "Телефон" },
        ["Images"] = new() { ["az"] = "Şəkillər", ["en"] = "Images", ["ru"] = "Изображения" },
        ["Type"] = new() { ["az"] = "Növ", ["en"] = "Type", ["ru"] = "Тип" },
        ["RentType"] = new() { ["az"] = "Kirayə növü", ["en"] = "Rent Type", ["ru"] = "Тип аренды" },
        ["Monthly"] = new() { ["az"] = "Aylıq", ["en"] = "Monthly", ["ru"] = "Помесячно" },
        ["Daily"] = new() { ["az"] = "Günlük", ["en"] = "Daily", ["ru"] = "Посуточно" },
        ["SellerType"] = new() { ["az"] = "Satıcı növü", ["en"] = "Seller Type", ["ru"] = "Тип продавца" },
        ["Owner"] = new() { ["az"] = "Mülkiyyətçi", ["en"] = "Owner", ["ru"] = "Собственник" },
        ["Agent"] = new() { ["az"] = "Agent", ["en"] = "Agent", ["ru"] = "Агент" },
        ["SelectCity"] = new() { ["az"] = "Şəhər seçin", ["en"] = "Select City", ["ru"] = "Выберите город" },
        ["SelectDistrict"] = new() { ["az"] = "Rayon seçin", ["en"] = "Select District", ["ru"] = "Выберите район" },
        ["SelectMetro"] = new() { ["az"] = "Metro seçin (ixtiyari)", ["en"] = "Select Metro (optional)", ["ru"] = "Выберите метро (необязательно)" },
        ["Optional"] = new() { ["az"] = "İxtiyari", ["en"] = "Optional", ["ru"] = "Дополнительно" },

        // ── Favorites page ──────────────────────────────────────────
        ["MyFavorites"] = new() { ["az"] = "Sevimlilərim", ["en"] = "My Favorites", ["ru"] = "Моё избранное" },
        ["Listing"] = new() { ["az"] = "elan", ["en"] = "listing", ["ru"] = "объявление" },
        ["Listings"] = new() { ["az"] = "elan", ["en"] = "listings", ["ru"] = "объявлений" },
        ["NoSavedListings"] = new() { ["az"] = "Hələ saxlanmış elan yoxdur", ["en"] = "No saved listings yet", ["ru"] = "Пока нет сохранённых объявлений" },
        ["BrowseListingsHint"] = new() { ["az"] = "Elanlara baxın və ♡ işarəsinə basaraq burada saxlayın.", ["en"] = "Browse listings and tap the ♡ to save them here.", ["ru"] = "Просматривайте объявления и нажмите ♡, чтобы сохранить их здесь." },
        ["BrowseListings"] = new() { ["az"] = "Elanlara bax", ["en"] = "Browse Listings", ["ru"] = "Просмотреть объявления" },

        // ── Common ──────────────────────────────────────────────────
        ["Listings_Title"] = new() { ["az"] = "Elanlar", ["en"] = "Listings", ["ru"] = "Объявления" },
    };

    public string this[string key, string lang]
    {
        get
        {
            if (Translations.TryGetValue(key, out var langMap)
                && langMap.TryGetValue(lang, out var value))
                return value;

            // Fallback: try English, then return key itself
            if (Translations.TryGetValue(key, out var fallback)
                && fallback.TryGetValue("en", out var enValue))
                return enValue;

            return key;
        }
    }
}
