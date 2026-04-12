-- ═══════════════════════════════════════════════════════════════
--  SEED: 1 Agency, 10 Residential Complexes, 40 Listings
--  Database: ApartmentAzDb
-- ═══════════════════════════════════════════════════════════════

-- ── Variables ────────────────────────────────────────────────
DECLARE @UserId UNIQUEIDENTIFIER = '0CE89BD8-9457-4B7F-A818-08DE971095B0';

-- City IDs
DECLARE @Baku       UNIQUEIDENTIFIER = '4D479060-AAAA-4E02-BC6D-949E9535E97B';
DECLARE @Ganja      UNIQUEIDENTIFIER = '8632ECB5-033A-4647-A24C-2CCE54D2F89D';
DECLARE @Sumgait    UNIQUEIDENTIFIER = '12CAACA0-EC27-4357-A39A-823E14F4815C';
DECLARE @Lankaran   UNIQUEIDENTIFIER = 'BF091BD9-CB6C-4113-AC36-CCF3BBC52244';

-- Baku district IDs
DECLARE @Nasimi     UNIQUEIDENTIFIER = '2917E62C-781B-4EFF-9FB3-B2512FE610FF';
DECLARE @Narimanov  UNIQUEIDENTIFIER = 'E22F3B3A-7C3F-4929-BB64-9761AA1A6F64';
DECLARE @Yasamal    UNIQUEIDENTIFIER = '398BADDE-62E7-4D58-A885-EB90C3DE5C92';
DECLARE @Nizami     UNIQUEIDENTIFIER = '8BE7EADE-073F-4E4F-96BD-B65078136EC9';
DECLARE @Sabail     UNIQUEIDENTIFIER = 'CBA1B2B2-2225-4322-948C-47A26CD47A9D';
DECLARE @Khatai     UNIQUEIDENTIFIER = '2D492CD3-636F-450C-AF6D-5F5227BA4149';
DECLARE @Binagadi   UNIQUEIDENTIFIER = 'CE6ABA29-1A27-4E42-A076-95CD2FA1F026';
DECLARE @Sabunchu   UNIQUEIDENTIFIER = '17C335CE-70A7-47A7-A820-D0206E6451E7';
DECLARE @Surakhani  UNIQUEIDENTIFIER = 'C68D88C7-7430-4F33-A271-64EABAF8FC7E';

-- Metro IDs (Baku)
DECLARE @M28May     UNIQUEIDENTIFIER = '6108F3CB-5DA8-4887-B0C2-6108C21EA098';
DECLARE @MGanjlik   UNIQUEIDENTIFIER = 'AD458624-13BF-4F8B-AF39-B83DD16B756B';
DECLARE @MNarimanov UNIQUEIDENTIFIER = '4AB7E2DF-4957-431E-871D-51F570E8D1BB';
DECLARE @MKoroglu   UNIQUEIDENTIFIER = '7F62B923-30F9-47C6-A83B-D12EFFFD1CDC';
DECLARE @MElmler    UNIQUEIDENTIFIER = '27E48230-FFDC-4256-9A09-727029A0B82C';
DECLARE @MKhatai    UNIQUEIDENTIFIER = 'EAFD899D-6BE6-446A-9C01-61DC299FEBE8';
DECLARE @MSahil     UNIQUEIDENTIFIER = 'D17F0CBE-C51C-4F7C-A41A-9DDE7BE3097F';
DECLARE @MMemar     UNIQUEIDENTIFIER = '315F7304-29E3-476A-BD20-62D9E9E7C00E';
DECLARE @MIchari    UNIQUEIDENTIFIER = '0E9B4498-6BC1-4914-A374-5F7AEDE18645';
DECLARE @MAhmadli   UNIQUEIDENTIFIER = '470B8864-A416-4524-ADBD-B9518F50801D';

-- ═══════════════════════════════════════════════════════════════
--  1. AGENCY (1)
-- ═══════════════════════════════════════════════════════════════

DECLARE @Agency1 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Agencies (Id, [Name], Phone, UserId) VALUES
(@Agency1, N'Baku Premium Estate', N'+994 50 555 1234', @UserId);

-- ═══════════════════════════════════════════════════════════════
--  2. RESIDENTIAL COMPLEXES (10)
-- ═══════════════════════════════════════════════════════════════

DECLARE @RC1  UNIQUEIDENTIFIER = NEWID();
DECLARE @RC2  UNIQUEIDENTIFIER = NEWID();
DECLARE @RC3  UNIQUEIDENTIFIER = NEWID();
DECLARE @RC4  UNIQUEIDENTIFIER = NEWID();
DECLARE @RC5  UNIQUEIDENTIFIER = NEWID();
DECLARE @RC6  UNIQUEIDENTIFIER = NEWID();
DECLARE @RC7  UNIQUEIDENTIFIER = NEWID();
DECLARE @RC8  UNIQUEIDENTIFIER = NEWID();
DECLARE @RC9  UNIQUEIDENTIFIER = NEWID();
DECLARE @RC10 UNIQUEIDENTIFIER = NEWID();

INSERT INTO ResidentialComplexes (Id) VALUES
(@RC1),(@RC2),(@RC3),(@RC4),(@RC5),(@RC6),(@RC7),(@RC8),(@RC9),(@RC10);

INSERT INTO ResidentialComplexTranslations (Id, [Name], LanguageCode, ResidentialComplexId) VALUES
-- RC1: Crescent Place
(NEWID(), N'Crescent Place',        N'az', @RC1),
(NEWID(), N'Crescent Place',        N'en', @RC1),
(NEWID(), N'Крещент Плейс',         N'ru', @RC1),
-- RC2: Port Baku Residence
(NEWID(), N'Port Baku Residence',   N'az', @RC2),
(NEWID(), N'Port Baku Residence',   N'en', @RC2),
(NEWID(), N'Порт Баку Резиденс',    N'ru', @RC2),
-- RC3: Alov Tower
(NEWID(), N'Alov Tower',            N'az', @RC3),
(NEWID(), N'Flame Tower',           N'en', @RC3),
(NEWID(), N'Башня Пламени',         N'ru', @RC3),
-- RC4: AG Residence
(NEWID(), N'AG Residence',          N'az', @RC4),
(NEWID(), N'AG Residence',          N'en', @RC4),
(NEWID(), N'АГ Резиденс',           N'ru', @RC4),
-- RC5: Yeni Hayat
(NEWID(), N'Yeni Həyat',            N'az', @RC5),
(NEWID(), N'New Life',              N'en', @RC5),
(NEWID(), N'Новая Жизнь',           N'ru', @RC5),
-- RC6: Azure
(NEWID(), N'Azure',                 N'az', @RC6),
(NEWID(), N'Azure',                 N'en', @RC6),
(NEWID(), N'Азур',                   N'ru', @RC6),
-- RC7: Caspian Residence
(NEWID(), N'Xəzər Residence',       N'az', @RC7),
(NEWID(), N'Caspian Residence',     N'en', @RC7),
(NEWID(), N'Каспий Резиденс',       N'ru', @RC7),
-- RC8: White City Residence
(NEWID(), N'Ağ Şəhər Residence',    N'az', @RC8),
(NEWID(), N'White City Residence',  N'en', @RC8),
(NEWID(), N'Белый Город Резиденс',  N'ru', @RC8),
-- RC9: Deniz Mall Residences
(NEWID(), N'Dəniz Mall Residences', N'az', @RC9),
(NEWID(), N'Deniz Mall Residences', N'en', @RC9),
(NEWID(), N'Дениз Молл Резиденсы',  N'ru', @RC9),
-- RC10: Socar Tower Apartments
(NEWID(), N'SOCAR Tower Apartments',N'az', @RC10),
(NEWID(), N'SOCAR Tower Apartments',N'en', @RC10),
(NEWID(), N'Апартаменты SOCAR Тауэр',N'ru', @RC10);

-- ═══════════════════════════════════════════════════════════════
--  3. LISTINGS (40) + Translations + Images
-- ═══════════════════════════════════════════════════════════════
-- ListingType:  1=Sale, 2=Rent
-- PropertyType: 1=NewBuilding, 2=OldBuilding, 3=House, 4=Office, 7=Commercial
-- SellerType:   1=Owner, 2=Agent
-- RepairStatus: 0=None, 1=Repaired, 2=NotRepaired
-- RentType:     1=Monthly, 2=Daily

DECLARE @L TABLE (
    Idx INT IDENTITY(1,1),
    Id UNIQUEIDENTIFIER,
    ListingType INT, RentType INT, PropertyType INT, SellerType INT, RepairStatus INT,
    Price DECIMAL(18,2), RoomCount INT, Area FLOAT, [Floor] INT, TotalFloors INT,
    HasDocument BIT, HasMortgage BIT,
    CityId UNIQUEIDENTIFIER, DistrictId UNIQUEIDENTIFIER, MetroId UNIQUEIDENTIFIER,
    AgencyId UNIQUEIDENTIFIER, RCId UNIQUEIDENTIFIER,
    TitleAz NVARCHAR(200), TitleEn NVARCHAR(200), TitleRu NVARCHAR(200),
    DescAz NVARCHAR(500), DescEn NVARCHAR(500), DescRu NVARCHAR(500),
    ContactName NVARCHAR(100), ContactEmail NVARCHAR(100), ContactPhone NVARCHAR(50)
);

-- ── Sale listings (20) ──────────────────────────────────────────────
INSERT INTO @L VALUES
-- 1: Sale, NewBuilding, Nasimi, 28May metro, Crescent Place
(NEWID(),1,NULL,1,1,1, 185000,3,95,12,16,1,1, @Baku,@Nasimi,@M28May, @Agency1,@RC1,
 N'Nəsimidə 3 otaqlı yeni tikili',N'3-room new building in Nasimi',N'3-комн. новостройка в Насими',
 N'Tam təmirli, mərkəzdə',N'Fully renovated, city center',N'Полный ремонт, центр города',
 N'Əli Həsənov',N'ali@mail.az',N'+994 50 111 0001'),
-- 2: Sale, NewBuilding, Yasamal, Elmler metro
(NEWID(),1,NULL,1,2,1, 220000,4,130,8,20,1,1, @Baku,@Yasamal,@MElmler, NULL,@RC2,
 N'Yasamalda 4 otaqlı mənzil',N'4-room apartment in Yasamal',N'4-комн. квартира в Ясамале',
 N'Geniş mənzil, panoramik mənzərə',N'Spacious apartment, panoramic view',N'Просторная квартира, панорамный вид',
 N'Leyla Məmmədova',N'leyla@mail.az',N'+994 50 111 0002'),
-- 3: Sale, OldBuilding, Narimanov, Narimanov metro
(NEWID(),1,NULL,2,1,1, 95000,2,55,4,9,1,0, @Baku,@Narimanov,@MNarimanov, NULL,NULL,
 N'Nərimanovda 2 otaqlı köhnə tikili',N'2-room old building in Narimanov',N'2-комн. старый фонд в Нариманове',
 N'Təmirli, metroya yaxın',N'Renovated, near metro',N'С ремонтом, рядом с метро',
 N'Rəşad Əliyev',N'rashad@mail.az',N'+994 50 111 0003'),
-- 4: Sale, NewBuilding, Sabail, Sahil metro, Port Baku
(NEWID(),1,NULL,1,1,1, 450000,3,140,18,25,1,1, @Baku,@Sabail,@MSahil, @Agency1,@RC2,
 N'Port Bakuda lüks mənzil',N'Luxury apartment in Port Baku',N'Люкс квартира в Порт Баку',
 N'Dəniz mənzərəli premium mənzil',N'Premium apartment with sea view',N'Премиум квартира с видом на море',
 N'Kamran Hüseynov',N'kamran@mail.az',N'+994 50 111 0004'),
-- 5: Sale, NewBuilding, Khatai, Khatai metro
(NEWID(),1,NULL,1,1,1, 135000,2,75,6,14,1,1, @Baku,@Khatai,@MKhatai, NULL,@RC5,
 N'Xətaidə 2 otaqlı yeni tikili',N'2-room new building in Khatai',N'2-комн. новостройка в Хатаи',
 N'Tam təmirli kupçalı',N'Fully renovated with document',N'Полный ремонт с документами',
 N'Nigar Əhmədova',N'nigar@mail.az',N'+994 50 111 0005'),
-- 6: Sale, OldBuilding, Nizami
(NEWID(),1,NULL,2,1,2, 78000,2,50,3,5,1,0, @Baku,@Nizami,NULL, NULL,NULL,
 N'Nizamidə 2 otaqlı stalinka',N'2-room stalinka in Nizami',N'2-комн. сталинка в Низами',
 N'Yüksək tavanlar, geniş otaqlar',N'High ceilings, spacious rooms',N'Высокие потолки, просторные комнаты',
 N'Tural Babayev',N'tural@mail.az',N'+994 50 111 0006'),
-- 7: Sale, NewBuilding, Binagadi, AG Residence
(NEWID(),1,NULL,1,2,1, 165000,3,105,10,18,1,1, @Baku,@Binagadi,NULL, @Agency1,@RC4,
 N'Binəqədidə 3 otaqlı',N'3-room apartment in Binagadi',N'3-комн. в Бинагади',
 N'AG Residence, tam təmirli',N'AG Residence, fully renovated',N'AG Резиденс, полный ремонт',
 N'Sənan Quliyev',N'senan@mail.az',N'+994 50 111 0007'),
-- 8: Sale, House, Sabunchu
(NEWID(),1,NULL,3,1,1, 250000,5,200,1,2,1,0, @Baku,@Sabunchu,NULL, NULL,NULL,
 N'Sabunçuda həyət evi',N'House in Sabunchu',N'Дом в Сабунчу',
 N'2 mərtəbəli ev, 3 sot torpaq',N'2-floor house, 3 hundred sq.m. land',N'2-этажный дом, 3 сотки земли',
 N'Orxan Həsənli',N'orxan@mail.az',N'+994 50 111 0008'),
-- 9: Sale, NewBuilding, Surakhani
(NEWID(),1,NULL,1,1,1, 72000,1,42,5,9,1,1, @Baku,@Surakhani,NULL, NULL,@RC5,
 N'Suraxanıda 1 otaqlı',N'1-room apartment in Surakhani',N'1-комн. в Сураханы',
 N'Yeni tikili, kupçalı',N'New building with document',N'Новостройка с купчей',
 N'Elvin Kazımov',N'elvin@mail.az',N'+994 50 111 0009'),
-- 10: Sale, NewBuilding, Nasimi, 28May metro, Flame Tower
(NEWID(),1,NULL,1,2,1, 580000,4,180,22,33,1,0, @Baku,@Nasimi,@M28May, @Agency1,@RC3,
 N'Alov Qülləsində penthouse',N'Penthouse in Flame Tower',N'Пентхаус в Башне Пламени',
 N'Premium sinif, şəhər mənzərəsi',N'Premium class, city panorama',N'Премиум класс, панорама города',
 N'Fərid Nəsirov',N'ferid@mail.az',N'+994 50 111 0010'),
-- 11: Sale, Ganja
(NEWID(),1,NULL,1,1,1, 65000,2,70,4,9,1,1, @Ganja,NULL,NULL, NULL,NULL,
 N'Gəncədə 2 otaqlı yeni tikili',N'2-room new building in Ganja',N'2-комн. новостройка в Гяндже',
 N'Tam təmirli, mərkəzdə',N'Fully renovated, center',N'Полный ремонт, центр',
 N'Vüsal Cəfərov',N'vusal@mail.az',N'+994 50 111 0011'),
-- 12: Sale, Sumgait
(NEWID(),1,NULL,1,1,1, 55000,2,65,7,12,1,1, @Sumgait,NULL,NULL, NULL,NULL,
 N'Sumqayıtda 2 otaqlı',N'2-room apartment in Sumgait',N'2-комн. в Сумгаите',
 N'Yeni tikili, sənədli',N'New building, documented',N'Новостройка, с документами',
 N'Cavid Məmmədov',N'cavid@mail.az',N'+994 50 111 0012'),
-- 13: Sale, NewBuilding, Yasamal, Elmler, Azure
(NEWID(),1,NULL,1,1,1, 290000,3,120,15,22,1,1, @Baku,@Yasamal,@MElmler, NULL,@RC6,
 N'Azure kompleksində 3 otaqlı',N'3-room in Azure complex',N'3-комн. в комплексе Азур',
 N'Lüks təmir, avadanlıqlı',N'Luxury renovation, furnished',N'Люкс ремонт, с мебелью',
 N'Aynur Sultanova',N'aynur@mail.az',N'+994 50 111 0013'),
-- 14: Sale, OldBuilding, Nasimi
(NEWID(),1,NULL,2,1,1, 110000,3,80,2,5,1,0, @Baku,@Nasimi,@M28May, NULL,NULL,
 N'28 Maya yaxın 3 otaqlı',N'3-room near 28 May',N'3-комн. рядом 28 Мая',
 N'Təmirli, metroya 2 dəq',N'Renovated, 2 min to metro',N'С ремонтом, 2 мин до метро',
 N'Murad Əsgərov',N'murad@mail.az',N'+994 50 111 0014'),
-- 15: Sale, NewBuilding, Khatai, White City
(NEWID(),1,NULL,1,1,1, 195000,3,100,9,16,1,1, @Baku,@Khatai,@MKhatai, @Agency1,@RC8,
 N'Ağ Şəhərdə 3 otaqlı',N'3-room in White City',N'3-комн. в Белом Городе',
 N'Müasir dizayn, kupçalı',N'Modern design, documented',N'Современный дизайн, с купчей',
 N'Xəyal Sadıqov',N'xeyal@mail.az',N'+994 50 111 0015'),
-- 16: Sale, Office, Nasimi, Ichari metro
(NEWID(),1,NULL,4,2,1, 320000,0,150,3,6,1,0, @Baku,@Sabail,@MIchari, @Agency1,NULL,
 N'İçərişəhərdə ofis',N'Office in Old City',N'Офис в Ичеришехер',
 N'Tarixi binada premium ofis',N'Premium office in historic building',N'Премиум офис в историческом здании',
 N'Ramin Abbasov',N'ramin@mail.az',N'+994 50 111 0016'),
-- 17: Sale, NewBuilding, Narimanov, Ganjlik, Deniz Mall
(NEWID(),1,NULL,1,1,1, 175000,2,85,11,20,1,1, @Baku,@Narimanov,@MGanjlik, NULL,@RC9,
 N'Dəniz Mall yanında 2 otaqlı',N'2-room near Deniz Mall',N'2-комн. рядом Дениз Молл',
 N'Yeni tikili, mənzərəli',N'New building, with view',N'Новостройка, с видом',
 N'Əli Hüseynov',N'ali.h@mail.az',N'+994 50 111 0017'),
-- 18: Sale, NewBuilding, Sabail, SOCAR Tower
(NEWID(),1,NULL,1,1,1, 650000,3,160,28,42,1,0, @Baku,@Sabail,@MSahil, @Agency1,@RC10,
 N'SOCAR Tower-da 3 otaqlı',N'3-room in SOCAR Tower',N'3-комн. в SOCAR Tower',
 N'Ultra premium, dəniz mənzərəsi',N'Ultra premium, sea view',N'Ультра премиум, вид на море',
 N'Zaur Məmmədov',N'zaur@mail.az',N'+994 50 111 0018'),
-- 19: Sale, Lankaran
(NEWID(),1,NULL,3,1,1, 85000,4,180,1,2,1,0, @Lankaran,NULL,NULL, NULL,NULL,
 N'Lənkəranda həyət evi',N'House in Lankaran',N'Дом в Ленкорани',
 N'Bağ evi, 5 sot torpaq',N'Garden house, 5 hundred sq.m. land',N'Дом с садом, 5 соток земли',
 N'Rəhman Əlizadə',N'rehman@mail.az',N'+994 50 111 0019'),
-- 20: Sale, NewBuilding, Binagadi, Caspian Res
(NEWID(),1,NULL,1,1,1, 145000,2,80,7,14,1,1, @Baku,@Binagadi,NULL, NULL,@RC7,
 N'Xəzər Residence-da 2 otaqlı',N'2-room in Caspian Residence',N'2-комн. в Каспий Резиденс',
 N'Yeni tikili, tam təmirli',N'New building, fully renovated',N'Новостройка, полный ремонт',
 N'Samir Həsənov',N'samir@mail.az',N'+994 50 111 0020');

-- ── Rent listings (20) ──────────────────────────────────────────────
INSERT INTO @L VALUES
-- 21: Rent monthly, NewBuilding, Nasimi
(NEWID(),2,1,1,1,1, 1200,2,75,8,16,1,0, @Baku,@Nasimi,@M28May, NULL,@RC1,
 N'Nəsimidə kirayə 2 otaqlı',N'2-room for rent in Nasimi',N'2-комн. в аренду в Насими',
 N'Tam təmirli, əşyalı',N'Fully renovated, furnished',N'Полный ремонт, с мебелью',
 N'Gülnarə İsmayılova',N'gulnare@mail.az',N'+994 50 222 0001'),
-- 22: Rent monthly, Yasamal, Elmler
(NEWID(),2,1,1,2,1, 1500,3,110,10,18,1,0, @Baku,@Yasamal,@MElmler, @Agency1,NULL,
 N'Yasamalda kirayə 3 otaqlı',N'3-room for rent in Yasamal',N'3-комн. в аренду в Ясамале',
 N'Lüks təmir, avadanlıqlı',N'Luxury renovation, equipped',N'Люкс ремонт, оснащённая',
 N'Pərviz Qasımov',N'perviz@mail.az',N'+994 50 222 0002'),
-- 23: Rent monthly, Narimanov, Ganjlik
(NEWID(),2,1,1,1,1, 800,1,45,5,12,1,0, @Baku,@Narimanov,@MGanjlik, NULL,NULL,
 N'Nərimanovda 1 otaqlı kirayə',N'1-room for rent in Narimanov',N'1-комн. в аренду в Нариманове',
 N'Studiya tipli, təmirli',N'Studio type, renovated',N'Студия, с ремонтом',
 N'Aysel Əliyeva',N'aysel@mail.az',N'+994 50 222 0003'),
-- 24: Rent daily, Sabail, Ichari
(NEWID(),2,2,2,1,1, 80,1,40,2,4,1,0, @Baku,@Sabail,@MIchari, NULL,NULL,
 N'İçərişəhərdə günlük kirayə',N'Daily rent in Old City',N'Посуточная аренда в Ичеришехер',
 N'Turistlər üçün ideal',N'Ideal for tourists',N'Идеально для туристов',
 N'Samirə Məmmədli',N'samire@mail.az',N'+994 50 222 0004'),
-- 25: Rent monthly, Khatai
(NEWID(),2,1,1,1,1, 900,2,65,6,14,1,0, @Baku,@Khatai,@MKhatai, NULL,@RC8,
 N'Xətaidə 2 otaqlı kirayə',N'2-room for rent in Khatai',N'2-комн. в аренду в Хатаи',
 N'Ağ Şəhər, tam təmirli',N'White City, fully renovated',N'Белый Город, полный ремонт',
 N'Rəna Hüseynova',N'rena@mail.az',N'+994 50 222 0005'),
-- 26: Rent monthly, Nizami
(NEWID(),2,1,2,1,1, 650,2,55,3,5,1,0, @Baku,@Nizami,NULL, NULL,NULL,
 N'Nizamidə 2 otaqlı kirayə',N'2-room for rent in Nizami',N'2-комн. в аренду в Низами',
 N'Metroya yaxın, əşyalı',N'Near metro, furnished',N'Рядом с метро, с мебелью',
 N'Elçin Vəliyev',N'elchin@mail.az',N'+994 50 222 0006'),
-- 27: Rent monthly, Binagadi
(NEWID(),2,1,1,1,1, 1100,3,95,9,16,1,0, @Baku,@Binagadi,NULL, @Agency1,@RC4,
 N'Binəqədidə 3 otaqlı kirayə',N'3-room for rent in Binagadi',N'3-комн. в аренду в Бинагади',
 N'AG Residence, avadanlıqlı',N'AG Residence, equipped',N'АГ Резиденс, оснащённая',
 N'Könül Rzayeva',N'konul@mail.az',N'+994 50 222 0007'),
-- 28: Rent daily, Nasimi, 28May
(NEWID(),2,2,1,1,1, 120,2,70,14,20,1,0, @Baku,@Nasimi,@M28May, NULL,NULL,
 N'28 May metroda günlük kirayə',N'Daily rent near 28 May metro',N'Посуточная аренда у метро 28 Мая',
 N'Tam təmirli, mərkəz',N'Fully renovated, center',N'Полный ремонт, центр',
 N'Anar Məmmədov',N'anar@mail.az',N'+994 50 222 0008'),
-- 29: Rent monthly, Sabunchu
(NEWID(),2,1,1,1,2, 500,2,60,4,5,1,0, @Baku,@Sabunchu,NULL, NULL,NULL,
 N'Sabunçuda 2 otaqlı kirayə',N'2-room for rent in Sabunchu',N'2-комн. в аренду в Сабунчу',
 N'Təmirsiz, ucuz qiymət',N'No renovation, cheap',N'Без ремонта, дешево',
 N'Tofiq Əsgərov',N'tofiq@mail.az',N'+994 50 222 0009'),
-- 30: Rent monthly, Surakhani
(NEWID(),2,1,1,1,1, 550,1,35,2,9,1,0, @Baku,@Surakhani,NULL, NULL,NULL,
 N'Suraxanıda 1 otaqlı kirayə',N'1-room for rent in Surakhani',N'1-комн. в аренду в Сураханы',
 N'Kiçik amma rahat',N'Small but cozy',N'Маленькая, но уютная',
 N'Əminə Sadıqova',N'emine@mail.az',N'+994 50 222 0010'),
-- 31: Rent monthly, Ganja
(NEWID(),2,1,1,1,1, 400,2,65,3,9,1,0, @Ganja,NULL,NULL, NULL,NULL,
 N'Gəncədə 2 otaqlı kirayə',N'2-room for rent in Ganja',N'2-комн. в аренду в Гяндже',
 N'Təmirli, əşyalı',N'Renovated, furnished',N'С ремонтом, с мебелью',
 N'Fərhad Nəsibov',N'ferhad@mail.az',N'+994 50 222 0011'),
-- 32: Rent monthly, Port Baku, Sabail
(NEWID(),2,1,1,1,1, 3500,3,140,20,25,1,0, @Baku,@Sabail,@MSahil, @Agency1,@RC2,
 N'Port Bakuda kirayə',N'Rent in Port Baku',N'Аренда в Порт Баку',
 N'Premium, tam avadanlıqlı',N'Premium, fully equipped',N'Премиум, полностью оснащена',
 N'Zaur Babayev',N'zaur.b@mail.az',N'+994 50 222 0012'),
-- 33: Rent monthly, Narimanov, Koroglu
(NEWID(),2,1,1,1,1, 750,2,60,7,12,1,0, @Baku,@Narimanov,@MKoroglu, NULL,NULL,
 N'Koroğlu metroda 2 otaqlı',N'2-room near Koroglu metro',N'2-комн. у метро Короглу',
 N'Təmirli, rahat',N'Renovated, comfortable',N'С ремонтом, комфортная',
 N'Nəzrin Əliyeva',N'nezrin@mail.az',N'+994 50 222 0013'),
-- 34: Rent daily, Yasamal
(NEWID(),2,2,1,1,1, 100,2,70,6,12,1,0, @Baku,@Yasamal,@MElmler, NULL,NULL,
 N'Yasamalda günlük kirayə',N'Daily rent in Yasamal',N'Посуточная аренда в Ясамале',
 N'Tam avadanlıqlı',N'Fully equipped',N'Полностью оснащена',
 N'Lamiyə Vəliyeva',N'lamiye@mail.az',N'+994 50 222 0014'),
-- 35: Rent monthly, Flame Tower
(NEWID(),2,1,1,1,1, 5000,4,200,30,33,1,0, @Baku,@Nasimi,@M28May, @Agency1,@RC3,
 N'Alov Qülləsində kirayə',N'Rent in Flame Tower',N'Аренда в Башне Пламени',
 N'Ultra lüks, xidmətçi mənzil',N'Ultra luxury, serviced apt',N'Ультра люкс, обслуживаемая',
 N'Vüqar Əhmədov',N'vuqar@mail.az',N'+994 50 222 0015'),
-- 36: Rent monthly, Sumgait
(NEWID(),2,1,1,1,1, 350,2,60,5,9,1,0, @Sumgait,NULL,NULL, NULL,NULL,
 N'Sumqayıtda 2 otaqlı kirayə',N'2-room for rent in Sumgait',N'2-комн. в аренду в Сумгаите',
 N'Təmirli, sənədli',N'Renovated, documented',N'С ремонтом, с документами',
 N'Elnur Həsənov',N'elnur@mail.az',N'+994 50 222 0016'),
-- 37: Rent, Office, Nasimi
(NEWID(),2,1,4,2,1, 2000,0,120,5,12,1,0, @Baku,@Nasimi,@M28May, @Agency1,NULL,
 N'Nəsimidə ofis kirayə',N'Office for rent in Nasimi',N'Офис в аренду в Насими',
 N'Hazır ofis, avadanlıqlı',N'Ready office, equipped',N'Готовый офис, оснащённый',
 N'Bəhruz Quliyev',N'behruz@mail.az',N'+994 50 222 0017'),
-- 38: Rent monthly, Narimanov, Deniz Mall
(NEWID(),2,1,1,1,1, 1300,2,80,13,20,1,0, @Baku,@Narimanov,@MGanjlik, NULL,@RC9,
 N'Dəniz Mall yanı 2 otaqlı kirayə',N'2-room rent near Deniz Mall',N'2-комн. аренда у Дениз Молл',
 N'Mənzərəli, tam təmirli',N'With view, fully renovated',N'С видом, полный ремонт',
 N'Günel Məmmədova',N'gunel@mail.az',N'+994 50 222 0018'),
-- 39: Rent monthly, Khatai, Ahmadli
(NEWID(),2,1,1,1,1, 600,2,55,3,9,1,0, @Baku,@Khatai,@MAhmadli, NULL,NULL,
 N'Əhmədlidə 2 otaqlı kirayə',N'2-room for rent in Ahmadli',N'2-комн. в аренду в Ахмедлы',
 N'Metroya yaxın, əşyalı',N'Near metro, furnished',N'Рядом с метро, с мебелью',
 N'Səbinə Əliyeva',N'sebine@mail.az',N'+994 50 222 0019'),
-- 40: Rent daily, Sabail
(NEWID(),2,2,1,1,1, 150,2,80,10,16,1,0, @Baku,@Sabail,@MSahil, NULL,@RC10,
 N'Sahildə günlük lüks mənzil',N'Daily luxury apt in Sahil',N'Посуточная люкс квартира на Сахиле',
 N'Dəniz mənzərəli, premium',N'Sea view, premium',N'Вид на море, премиум',
 N'Nihad Əsgərov',N'nihad@mail.az',N'+994 50 222 0020');

-- ── Insert into Listings table ──────────────────────────────────────
INSERT INTO Listings (Id, UserId, ListingType, RentType, PropertyType, SellerType, RepairStatus,
    Price, RoomCount, Area, [Floor], TotalFloors, HasDocument, HasMortgage,
    [Name], Email, Phone, CityId, DistrictId, MetroId, AgencyId, ResidentialComplexId)
SELECT Id, @UserId, ListingType, RentType, PropertyType, SellerType, RepairStatus,
    Price, RoomCount, Area, [Floor], TotalFloors, HasDocument, HasMortgage,
    ContactName, ContactEmail, ContactPhone, CityId, DistrictId, MetroId, AgencyId, RCId
FROM @L;

-- ── Insert translations ─────────────────────────────────────────────
INSERT INTO ListingTranslations (Id, Title, [Description], LanguageCode, ListingId)
SELECT NEWID(), TitleAz, DescAz, N'az', Id FROM @L
UNION ALL
SELECT NEWID(), TitleEn, DescEn, N'en', Id FROM @L
UNION ALL
SELECT NEWID(), TitleRu, DescRu, N'ru', Id FROM @L;

-- ── Insert placeholder images (using picsum.photos) ─────────────────
-- Each listing gets 2 images
INSERT INTO ListingImages (Id, ImageUrl, ListingId)
SELECT NEWID(),
    CONCAT('https://picsum.photos/seed/', CAST(l.Id AS VARCHAR(36)), '/800/600'),
    l.Id
FROM @L l
UNION ALL
SELECT NEWID(),
    CONCAT('https://picsum.photos/seed/', CAST(l.Id AS VARCHAR(36)), 'b/800/600'),
    l.Id
FROM @L l;

-- ── Verify ──────────────────────────────────────────────────────────
SELECT 'Agencies' AS [Table], COUNT(*) AS [Count] FROM Agencies
UNION ALL SELECT 'ResidentialComplexes', COUNT(*) FROM ResidentialComplexes
UNION ALL SELECT 'RC Translations', COUNT(*) FROM ResidentialComplexTranslations
UNION ALL SELECT 'Listings', COUNT(*) FROM Listings
UNION ALL SELECT 'ListingTranslations', COUNT(*) FROM ListingTranslations
UNION ALL SELECT 'ListingImages', COUNT(*) FROM ListingImages;
