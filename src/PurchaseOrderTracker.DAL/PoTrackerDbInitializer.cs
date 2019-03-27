using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.DAL
{
    public static class PoTrackerDbInitializer
    {
        public static void Initialize(PoTrackerDbContext context)
        {
            // uncomment to delete database on every startup
            // context.Database.EnsureDeleted();
            var created = DbInitializerHelper.EnsureDatabaseCreated(context);

            if (created)
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

                // Techzon
                var tzappCategory = new ProductCategory("Appliances");
                var tzbinderCategory = new ProductCategory("Binders and Binder Accessories");
                var tzenvelopeCategory = new ProductCategory("Envelopes");
                var tzlabelCategory = new ProductCategory("Labels");
                var tzpaperCategory = new ProductCategory("Paper");
                var tzpensCategory = new ProductCategory("Pens & Art Supplies");
                var tzrubberBandCategory = new ProductCategory("Rubber Bands");
                var tzscissorsCategory = new ProductCategory("Scissors, Rulers and Trimmers");
                var tzstorageCategory = new ProductCategory("Storage & Organization");

                // Furniture Max
                var fmbookcaseCategory = new ProductCategory("Bookcases");
                var fmchairCategory = new ProductCategory("Chairs & Chairmats");
                var fmofficeFurnCategory = new ProductCategory("Office Furnishings");
                var fmtablesCategory = new ProductCategory("Tables");

                // Office A+
                var oappCategory = new ProductCategory("Appliances");
                var obinderCategory = new ProductCategory("Binders and Binder Accessories");
                var oenvelopeCategory = new ProductCategory("Envelopes");
                var olabelCategory = new ProductCategory("Labels");
                var opaperCategory = new ProductCategory("Paper");
                var opensCategory = new ProductCategory("Pens & Art Supplies");
                var orubberBandCategory = new ProductCategory("Rubber Bands");
                var oscissorsCategory = new ProductCategory("Scissors, Rulers and Trimmers");
                var ostorageCategory = new ProductCategory("Storage & Organization");

                var tzSupplier = new Supplier("Techzon");
                tzSupplier.AddCategorys(
                    new List<ProductCategory>
                    {
                        tzappCategory,
                        tzbinderCategory,
                        tzenvelopeCategory,
                        tzenvelopeCategory,
                        tzlabelCategory,
                        tzpaperCategory,
                        tzpensCategory,
                        tzrubberBandCategory,
                        tzscissorsCategory,
                        tzstorageCategory
                    }
                );

                var fmSupplier = new Supplier("Furniture Max");
                fmSupplier.AddCategorys(new List<ProductCategory>
                {
                    fmbookcaseCategory,
                    fmchairCategory,
                    fmofficeFurnCategory,
                    fmtablesCategory
                });

                var oSupplier = new Supplier("Office Supplies A+");
                oSupplier.AddCategorys(new List<ProductCategory>
                {
                    oappCategory,
                    obinderCategory,
                    oenvelopeCategory,
                    olabelCategory,
                    opaperCategory,
                    opensCategory,
                    orubberBandCategory,
                    oscissorsCategory,
                    ostorageCategory
                });

                foreach (var s in new[] {tzSupplier, fmSupplier, oSupplier})
                    context.Supplier.Add(s);
                context.SaveChanges();

                tzSupplier.AddProducts(
                    new List<Product>
                    {
                        new Product("10038", "Carina Double Wide Media Storage Towers in Natural & Black", tzstorageCategory, 80.98m),
                        new Product("10221", "Staples® General Use 3-Ring Binders", tzbinderCategory, 1.88m),
                        new Product("10222", "Xerox 1904", tzpaperCategory, 6.48m),
                        new Product("10224", "Xerox 217", tzpaperCategory, 6.48m),
                        new Product("10225", "Revere Boxed Rubber Bands by Revere", tzrubberBandCategory, 1.89m),
                        new Product("10384", "Acco Smartsocket™ Table Surge Protector, 6 Color-Coded Adapter Outlets", tzappCategory, 62.05m),
                        new Product("10386", "Tennsco Snap-Together Open Shelving Units, Starter Sets and Add-On Units", tzstorageCategory, 279.48m),
                        new Product("10809", "Xerox 1887", tzpaperCategory, 18.97m),
                        new Product("10810", "Xerox 1891", tzpaperCategory, 48.91m),
                        new Product("11100", "Avery 506", tzlabelCategory, 4.13m),
                        new Product("11402", "Staples Wirebound Steno Books, 6\" x 9\", 12/Pack", tzpaperCategory, 10.14m),
                        new Product("11882", "GBC2 Pre-Punched Binding Paper, Plastic, White, 8-1/2\" x 11\"", tzbinderCategory, 15.99m),
                        new Product("11998", "Newell 326", tzpensCategory, 1.76m),
                        new Product("12284", "Prismacolor Color Pencil Set", tzpensCategory, 19.84m),
                        new Product("12287", "Xerox Blank Computer Paper", tzpaperCategory, 19.98m),
                        new Product("12681", "Fellowes Recycled Storage Drawers", tzstorageCategory, 111.03m),
                        new Product("12753", "Satellite Sectional Post Binders", tzbinderCategory, 43.41m),
                        new Product("13050", "Avery 487", tzlabelCategory, 3.69m),
                        new Product("13565", "GBC Twin Loop™ Wire Binding Elements, 9/16\" Spine, Black", tzbinderCategory, 15.22m),
                        new Product("13596", "Hanging Personal Folder File2", tzstorageCategory, 15.7m),
                        new Product("14070", "Fellowes Black Plastic Comb Bindings2", tzbinderCategory, 5.81m),
                        new Product("14475", "Tennsco Lockers, Gray", tzstorageCategory, 20.98m),
                        new Product("14552", "Avery 4027 File Folder Labels for Dot Matrix Printers, 5000 Labels per Box, White", tzlabelCategory, 30.53m),
                        new Product("14553", "Newell 323", tzpensCategory, 1.68m),
                        new Product("14736", "Spiral Phone Message Books with Labels by Adams", tzpaperCategory, 4.48m),
                        new Product("15122", "Crate-A-Files™", tzstorageCategory, 10.9m),
                        new Product("15161", "Holmes 99% HEPA Air Purifier", tzappCategory, 21.66m),
                        new Product("15201", "Xerox 224", tzpaperCategory, 6.48m),
                        new Product("15615", "Xerox 1906", tzpaperCategory, 35.44m),
                        new Product("15723", "GBC Pre-Punched Binding Paper, Plastic, White, 8-1/2\" x 11\"", tzbinderCategory, 15.99m),
                        new Product("15724", "Xerox 188", tzpaperCategory, 11.34m),
                        new Product("15725", "Xerox 1932", tzpaperCategory, 35.44m),
                        new Product("16094", "GBC Linen Binding Covers", tzbinderCategory, 30.98m),
                        new Product("16095", "GBC Recycled Grain Textured Covers", tzbinderCategory, 34.54m),
                        new Product("16889", "Kensington 7 Outlet MasterPiece Power Center with Fax/Phone Line Protection", tzappCategory, 207.48m),
                        new Product("17222", "Newell 327", tzpensCategory, 2.21m),
                        new Product("17225", "Xerox 1893", tzpaperCategory, 40.99m),
                        new Product("18115", "Wirebound Message Books, 2 7/8\" x 5\", 3 Forms per Page", tzpaperCategory, 7.04m),
                        new Product("18439", "Multi-Use Personal File Cart and Caster Set, Three Stacking Bins", tzstorageCategory, 34.76m),
                        new Product("18704", "Xerox 20", tzpaperCategory, 6.48m),
                        new Product("18734", "Surelock™ Post Binders", tzbinderCategory, 30.56m),
                        new Product("18839", "Colorific® Watercolor Pencils", tzpensCategory, 5.16m),
                        new Product("18840", "*Staples* vLetter Openers, 2/Pack", tzscissorsCategory, 3.68m),
                        new Product("19023", "Prang Drawing Pencil Set", tzpensCategory, 2.78m),
                        new Product("19064", "Xerox 1982", tzpaperCategory, 22.84m),
                        new Product("19278", "Xerox 1924", tzpaperCategory, 5.78m),
                        new Product("19279", "Turquoise Lead Holder with Pocket Clip", tzpensCategory, 6.7m),
                        new Product("20209", "Xerox 19822", tzpaperCategory, 22.84m),
                        new Product("20633", "Xerox 1971", tzpaperCategory, 4.28m),
                        new Product("20634", "Eldon Portable Mobile Manager", tzstorageCategory, 28.28m),
                        new Product("20828", "Durable Pressboard Binders", tzbinderCategory, 3.8m),
                        new Product("21634", "Fellowes PB500 Electric Punch Plastic Comb Binding Machine with Manual Bind", tzbinderCategory, 1270.99m),
                        new Product("21715", "Staples 6 Outlet Surge", tzappCategory, 11.97m),
                        new Product("21781", "Recycled Eldon Regeneration Jumbo File", tzstorageCategory, 12.28m),
                        new Product("22414", "GBC Binding covers2", tzbinderCategory, 12.95m),
                        new Product("22775", "Newell 323_2", tzpensCategory, 1.68m),
                        new Product("22956", "Speediset Carbonless Redi-Letter® 7\" x 8 1/2\"", tzpaperCategory, 10.31m),
                        new Product("23074", "GBC Recycled Regency Composition Covers", tzbinderCategory, 59.78m),
                        new Product("24220", "Newell 323_3", tzpensCategory, 1.68m),
                        new Product("24221", "SANFORD Major Accent™ Highlighters", tzpensCategory, 7.08m),
                        new Product("24606", "Dixon Ticonderoga Core-Lock Colored Pencils, 48-Color Set", tzpensCategory, 36.55m),
                        new Product("24942", "Recycled Steel Personal File for Standard File Folders", tzstorageCategory, 55.29m),
                        new Product("25639", "#10- 4 1/8\" x 9 1/2\" Security-Tint Envelopes", tzenvelopeCategory, 7.64m),
                        new Product("25672", "GBC Standard Plastic Binding Systems Combs", tzbinderCategory, 8.85m),
                        new Product("26508", "Xerox 1930", tzpaperCategory, 6.48m),
                        new Product("26509", "Avery 474", tzlabelCategory, 2.88m),
                        new Product("27363", "Peel & Seel® Recycled Catalog Envelopes, Brown2", tzenvelopeCategory, 11.58m),
                        new Product("27364", "Avery 5062", tzlabelCategory, 4.13m),
                        new Product("27397", "Prang Drawing Pencil Set2", tzpensCategory, 2.78m),
                        new Product("27470", "Xerox 190", tzpaperCategory, 4.98m),
                        new Product("27508", "Standard Line™ “While You Were Out” Hardbound Telephone Message Book", tzpaperCategory, 21.98m),
                        new Product("29226", "Recycled Interoffice Envelopes with String and Button Closure, 10 x 13", tzenvelopeCategory, 23.99m),
                        new Product("29335", "GBC Standard Therm-A-Bind Covers", tzbinderCategory, 24.92m),
                        new Product("29372", "Xerox 1892", tzpaperCategory, 38.76m),
                        new Product("29664", "Staples Paper Clips", tzrubberBandCategory, 2.47m),
                        new Product("29765", "Computer Printout Paper with Letter-Trim Perforations", tzpaperCategory, 18.97m),
                        new Product("30058", "Xerox 1929", tzpaperCategory, 22.84m),
                        new Product("30059", "Home/Office Personal File Carts", tzstorageCategory, 34.76m),
                        new Product("30210", "Avery 507", tzlabelCategory, 2.88m),
                        new Product("30286", "3M Organizer Strips", tzlabelCategory, 5.4m),
                        new Product("30918", "Fellowes Staxonsteel® Drawer Files", tzstorageCategory, 193.17m),
                        new Product("31413", "Sauder Facets Collection Locker/File Cabinet, Sky Alder Finish", tzstorageCategory, 370.98m),
                        new Product("31418", "Memo Book, 100 Message Capacity, 5 3/8” x 11”", tzpaperCategory, 6.74m),
                        new Product("31553", "Belkin 6 Outlet Metallic Surge Strip", tzappCategory, 10.89m),
                        new Product("31554", "Xerox 21", tzpaperCategory, 6.48m),
                        new Product("31669", "GBC DocuBind TL200 Manual Binding Machine", tzbinderCategory, 223.98m),
                        new Product("31823", "Xerox 1940", tzpaperCategory, 54.96m),
                        new Product("31824", "Bagged Rubber Bands", tzrubberBandCategory, 1.26m),
                        new Product("31932", "Xerox 1897", tzpaperCategory, 4.98m),
                        new Product("31971", "Poly Designer Cover & Back2", tzbinderCategory, 18.99m),
                        new Product("32082", "Bionaire Personal Warm Mist Humidifier/Vaporizer", tzappCategory, 46.89m),
                        new Product("32083", "Acme® 8\" Straight Scissors", tzscissorsCategory, 12.98m),
                        new Product("32117", "Heavy-Duty E-Z-D® Binders", tzbinderCategory, 10.91m),
                        new Product("32252", "Hammermill CopyPlus Copy Paper (20Lb. and 84 Bright)", tzpaperCategory, 4.98m),
                        new Product("32478", "Deluxe Rollaway Locking File with Drawer", tzstorageCategory, 415.88m),
                        new Product("33105", "Prismacolor Color Pencil Set2", tzpensCategory, 19.84m),
                        new Product("33177", "Wilson Jones DublLock® D-Ring Binders", tzbinderCategory, 6.75m),
                        new Product("33405", "Binder Posts", tzbinderCategory, 5.74m),
                        new Product("33988", "Fellowes PB300 Plastic Comb Binding Machine", tzbinderCategory, 387.99m),
                        new Product("34063", "Avery 481", tzlabelCategory, 3.08m),
                        new Product("34964", "SAFCO Commercial Wire Shelving, Black", tzstorageCategory, 138.14m),
                        new Product("35293", "Avery Trapezoid Extra Heavy Duty 4\" Binders", tzbinderCategory, 41.94m),
                        new Product("35294", "Self-Adhesive Address Labels for Typewriters by Universal", tzlabelCategory, 7.31m),
                        new Product("35480", "Rediform Wirebound \"Phone Memo\" Message Book, 11 x 5-3/4", tzpaperCategory, 7.64m),
                        new Product("35765", "Deluxe Rollaway Locking File with Drawer2", tzstorageCategory, 415.88m),
                        new Product("35798", "*Staples* vLetter Openers, 3/Pack", tzscissorsCategory, 3.68m),
                        new Product("35903", "Staples 6 Outlet Surge2", tzappCategory, 11.97m),
                        new Product("35904", "Acco Perma® 2700 Stacking Storage Drawers", tzstorageCategory, 29.74m),
                        new Product("35905", "Fellowes Stor/Drawer® Steel Plus™ Storage Drawers", tzstorageCategory, 95.43m),
                        new Product("35987", "Honeywell Enviracaire Portable HEPA Air Cleaner for 17' x 22' Room", tzappCategory, 300.65m),
                        new Product("36055", "Lock-Up Easel 'Spel-Binder'", tzbinderCategory, 28.53m),
                        new Product("36130", "Adams Write n' Stick Phone Message Book, 11\" X 5 1/4\", 200 Messages", tzpaperCategory, 5.68m),
                        new Product("36131", "Tenex File Box, Personal Filing Tote with Lid, Black", tzstorageCategory, 15.51m),
                        new Product("36240", "Fellowes Black Plastic Comb Bindings", tzbinderCategory, 5.81m),
                        new Product("36241", "Assorted Color Push Pins", tzrubberBandCategory, 1.81m),
                        new Product("36717", "HP Office Paper (20Lb. and 87 Bright)", tzpaperCategory, 6.68m),
                        new Product("36718", "Staples Vinyl Coated Paper Clips", tzrubberBandCategory, 3.93m),
                        new Product("36762", "Newell 310", tzpensCategory, 1.76m),
                        new Product("37342", "Kensington 7 Outlet MasterPiece Power Center", tzappCategory, 177.98m),
                        new Product("38064", "Array® Memo Cubes", tzpaperCategory, 5.18m),
                        new Product("38065", "Fiskars 8\" Scissors, 2/Pack", tzscissorsCategory, 17.24m),
                        new Product("38135", "12 Colored Short Pencils", tzpensCategory, 2.6m),
                        new Product("38136", "Pizazz® Global Quick File™", tzstorageCategory, 14.97m),
                        new Product("38442", "Acco Perma® 2700 Stacking Storage Drawers2", tzstorageCategory, 29.74m),
                        new Product("38443", "Portable Personal File Box", tzstorageCategory, 12.21m),
                        new Product("38480", "Snap-A-Way® Black Print Carbonless Speed Message, No Reply Area, Duplicate", tzpaperCategory, 29.14m),
                        new Product("38651", "Ibico Covers for Plastic or Wire Binding Elements", tzbinderCategory, 11.5m),
                        new Product("38652", "Hanging Personal Folder File", tzstorageCategory, 15.7m),
                        new Product("38653", "Tennsco Double-Tier Lockers", tzstorageCategory, 225.02m),
                        new Product("38666", "Wirebound Message Books, Four 2 3/4\" x 5\" Forms per Page, 600 Sets per Book", tzpaperCategory, 9.27m),
                        new Product("38667", "Acme Design Line 8\" Stainless Steel Bent Scissors w/Champagne Handles, 3-1/8\" Cut", tzscissorsCategory, 6.84m),
                        new Product("38980", "Avery 494", tzlabelCategory, 2.61m),
                        new Product("38981", "Self-Adhesive Address Labels for Typewriters by Universal2", tzlabelCategory, 7.31m),
                        new Product("39027", "Wilson Jones 14 Line Acrylic Coated Pressboard Data Binders", tzbinderCategory, 5.34m),
                        new Product("39066", "Eureka Disposable Bags for Sanitaire® Vibra Groomer I® Upright Vac", tzappCategory, 4.06m),
                        new Product("39068", "Peel & Seel® Recycled Catalog Envelopes, Brown", tzenvelopeCategory, 11.58m),
                        new Product("39495", "Cardinal Poly Pocket Divider Pockets for Ring Binders", tzbinderCategory, 3.36m),
                        new Product("39976", "Sterling Rubber Bands by Alliance", tzrubberBandCategory, 4.71m),
                        new Product("40223", "Newell 312", tzpensCategory, 5.84m),
                        new Product("41049", "Avery Flip-Chart Easel Binder, Black", tzbinderCategory, 22.38m),
                        new Product("41050", "GBC Binding covers", tzbinderCategory, 12.95m),
                        new Product("41785", "Tennsco Industrial Shelving", tzstorageCategory, 48.91m),
                        new Product("41861", "Staples Bulldog Clip", tzrubberBandCategory, 3.78m),
                        new Product("42817", "Xerox 1882", tzpaperCategory, 55.98m),
                        new Product("42818", "Carina Double Wide Media Storage Towers in Natural & Black2", tzstorageCategory, 80.98m),
                        new Product("42920", "Portable Personal File Box2", tzstorageCategory, 12.21m),
                        new Product("43105", "Acme Hot Forged Carbon Steel Scissors with Nickel-Plated Handles, 3 7/8\" Cut, 8\"L", tzscissorsCategory, 13.9m),
                        new Product("44356", "Xerox 4200 Series MultiUse Premium Copy Paper (20Lb. and 84 Bright)", tzpaperCategory, 5.28m),
                        new Product("44924", "Fellowes PB500 Electric Punch Plastic Comb Binding Machine with Manual Bind2", tzbinderCategory, 1270.99m),
                        new Product("45281", "Poly Designer Cover & Back", tzbinderCategory, 18.99m)
                    });

                fmSupplier.AddProducts(
                    new List<Product>
                    {
                        new Product("41563", "Tensor \"Hersey Kiss\" Styled Floor Lamp", fmofficeFurnCategory, 12.99m),
                        new Product("46848", "Bretford CR8500 Series Meeting Room Furniture", fmtablesCategory, 400.98m),
                        new Product("49987", "Eldon Expressions Mahogany Wood Desk Collection", fmofficeFurnCategory, 6.24m),
                        new Product("52596", "Global Deluxe Stacking Chair, Gray", fmchairCategory, 50.98m),
                        new Product("54781", "Chromcraft Rectangular Conference Tables", fmtablesCategory, 236.97m),
                        new Product("58560", "SAFCO PlanMaster Heigh-Adjustable Drafting Table Base, 43won® Expressions™ Wood Desk Accessories, Oak x 30d x 30-37h, Black", fmtablesCategory, 349.45m),
                        new Product("59377", "Staples Plastic Wall Frames", fmofficeFurnCategory, 7.96m),
                        new Product("60602", "Telescoping Adjustable Floor Lamp", fmofficeFurnCategory, 19.99m),
                        new Product("63211", "Magna Visual Magnetic Picture Hangers", fmofficeFurnCategory, 4.82m),
                        new Product("65603", "Linden® 12\" Wall Clock With Oak Frame", fmofficeFurnCategory, 33.98m),
                        new Product("66858", "Eldon® Wave Desk Accessories", fmofficeFurnCategory, 2.08m),
                        new Product("67016", "DAX Solid Wood Frames", fmofficeFurnCategory, 9.77m),
                        new Product("1837", "Eldon Expressions™ Desk Accessory, Wood Pencil Holder, Oak", fmofficeFurnCategory, 9.65m),
                        new Product("3567", "Executive Impressions 14\" Contract Wall Clock with Quartz Movement", fmofficeFurnCategory, 22.23m),
                        new Product("4332", "Executive Impressions 12\" Wall Clock", fmofficeFurnCategory, 17.67m),
                        new Product("4556", "Artistic Insta-Plaque", fmofficeFurnCategory, 15.68m),
                        new Product("18001", "3M Hangers With Command Adhesive", fmofficeFurnCategory, 3.7m),
                        new Product("20459", "Office Star Flex Back Scooter Chair with White Frame", fmchairCategory, 110.98m),
                        new Product("23321", "Atlantic Metals Mobile 2-Shelf Bookcases, Custom Colors", fmbookcaseCategory, 240.98m),
                        new Product("40038", "Luxo Economy Swing Arm Lamp", fmofficeFurnCategory, 19.94m),
                        new Product("40088", "DAX Contemporary Wood Frame with Silver Metal Mat, Desktop, 11 x 14 Size", fmofficeFurnCategory, 20.24m),
                        new Product("42335", "Hon Multipurpose Stacking Arm Chairs", fmchairCategory, 216.6m),
                        new Product("46809", "Global Leather Task Chair, Black", fmchairCategory, 89.99m),
                        new Product("47396", "Office Star - Mid Back Dual function Ergonomic High Back Chair with 2-Way Adjustable Arms", fmchairCategory, 160.98m),
                        new Product("47468", "O'Sullivan Living Dimensions 3-Shelf Bookcases", fmbookcaseCategory, 200.98m),
                        new Product("57616", "Anderson Hickey Conga Table Tops & Accessories", fmtablesCategory, 15.23m),
                        new Product("60783", "Howard Miller 12-3/4 Diameter Accuwave DS ™ Wall Clock", fmofficeFurnCategory, 78.69m),
                        new Product("60784", "Bevis Rectangular Conference Tables", fmtablesCategory, 145.98m),
                        new Product("62439", "Office Star - Task Chair with Contemporary Loop Arms", fmchairCategory, 90.98m),
                        new Product("64499", "O'Sullivan Elevations Bookcase, Cherry Finish", fmbookcaseCategory, 130.98m),
                        new Product("64609", "Aluminum Document Frame", fmofficeFurnCategory, 12.22m),
                        new Product("11768", "Hon Pagoda™ Stacking Chairs", fmchairCategory, 320.98m),
                        new Product("11769", "Rubbermaid ClusterMat Chairmats, Mat Size- 66\" x 60\", Lip 20\" x 11\" -90 Degree Angle", fmofficeFurnCategory, 110.98m),
                        new Product("15162", "Tensor \"Hersey Kiss\" Styled Floor Lamp2", fmofficeFurnCategory, 12.99m),
                        new Product("23688", "DAX Copper Panel Documentn® 12\" Wall Clock With Oak Frame Frame, 5 x 7 Size", fmofficeFurnCategory, 12.58m),
                        new Product("42698", "Eldon Expressions Punched Metal & Wood Desk Accessories, Black & Cherry", fmofficeFurnCategory, 9.38m),
                        new Product("53804", "Chromcraft Bull-Nose Wood Round Conference Table Top, Wood Base", fmtablesCategory, 217.85m),
                        new Product("1550", "Bush Westfield Collection Bookcases, Fully Assembled", fmbookcaseCategory, 100.98m),
                        new Product("2525", "Hon 4070 Series Pagoda™ Round Back Stacking Chairs", fmchairCategory, 320.98m),
                        new Product("9357", "12-1/2 Diameter Round Wall Clock", fmofficeFurnCategory, 19.98m),
                        new Product("11103", "SAFCO Folding Chair Trolley", fmchairCategory, 128.24m),
                        new Product("13521", "Tenex Contemporary Contur Chairmats for Low and Medium Pile Carpet, Computer, 39\" x 49\"", fmofficeFurnCategory, 107.53m),
                        new Product("13916", "Bevis 36 x 72 Conference Tables", fmtablesCategory, 124.49m),
                        new Product("13964", "Global Leather Highback Executive Chair with Pneumatic Height Adjustment, Black", fmchairCategory, 200.98m),
                        new Product("15271", "Tenex Traditional Chairmats for Medium Pile Carpet, Standard Lip, 36\" x 48\"", fmofficeFurnCategory, 60.65m),
                        new Product("20315", "Seth Thomas 12\" Clock w/ Goldtone Case", fmofficeFurnCategory, 22.98m),
                        new Product("24683", "Howard Miller 12-3/4 Diameter Accuwave DS ™ Wall Clock2", fmofficeFurnCategory, 78.69m),
                        new Product("32432", "Electrix Halogen Magnifier Lamp", fmofficeFurnCategory, 194.3m),
                        new Product("32433", "Luxo Professional Fluorescent Magnifier Lamp with Clamp-Mount Base", fmofficeFurnCategory, 209.84m),
                        new Product("32471", "Master Caster Door Stop, Gray", fmofficeFurnCategory, 5.08m),
                        new Product("39018", "Coloredge Poster Frame", fmofficeFurnCategory, 14.2m),
                        new Product("42565", "Linden® 12\" Wall Clock With Oak Frame2", fmofficeFurnCategory, 33.98m),
                        new Product("51023", "Bush Advantage Collection® Racetrack Conference Table", fmtablesCategory, 424.21m),
                        new Product("51730", "Riverside Palais Royal Lawyers Bookcase, Royale Cherry Finish", fmbookcaseCategory, 880.98m),
                        new Product("57063", "Eldon Econocleat® Chair Mats for Low Pile Carpets", fmofficeFurnCategory, 41.47m),
                        new Product("58018", "Office Star - Ergonomic Mid Back Chair with 2-Way Adjustable Arms", fmchairCategory, 180.98m),
                        new Product("60459", "Career Cubicle Clock, 8 1/4\", Black", fmofficeFurnCategory, 20.28m),
                        new Product("61712", "Eldon® Wave Desk Accessories2", fmofficeFurnCategory, 2.08m),
                        new Product("65638", "Hon Metal Bookcases, Black", fmbookcaseCategory, 70.98m),
                        new Product("10772", "Executive Impressions 8-1/2\" Career Panel/Partition Cubicle Clock", fmofficeFurnCategory, 10.4m),
                        new Product("14039", "Eldon® Expressions™ Wood Desk Accessories, Oak", fmofficeFurnCategory, 7.38m),
                        new Product("20569", "Bretford CR4500 Series Slim Rectangular Table", fmtablesCategory, 348.21m),
                        new Product("33950", "Bevis Steel Folding Chairs", fmchairCategory, 95.95m),
                        new Product("60861", "Global Leather and Oak Executive Chair, Black", fmchairCategory, 300.98m),
                        new Product("524", "Eldon® Expressions™ Wood Desk Accessories, Oak2", fmofficeFurnCategory, 7.38m)
                    });

                oSupplier.AddProducts(
                    new List<Product>
                    {
                        new Product("15312", "Wirebound Voice Message Log Book", opaperCategory, 4.76m),
                        new Product("15494", "Eureka Hand Vacuum, Bagless", oappCategory, 49.43m),
                        new Product("15832", "Prang Colored Pencils", opensCategory, 2.94m),
                        new Product("18766", "3M Organizer Strips", obinderCategory, 5.4m),
                        new Product("21132", "Avery 05222 Permanent Self-Adhesive File Folder Labels for Typewriters, on Rolls, White, 250/Roll", olabelCategory, 4.13m),
                        new Product("22516", "GBC Prepunched Paper, 19-Hole, for Binding Systems, 24-lb", obinderCategory, 15.01m),
                        new Product("22517", "GBC Recycled Regency Composition Covers", obinderCategory, 59.78m),
                        new Product("24593", "SAFCO Boltless Steel Shelving", ostorageCategory, 113.64m),
                        new Product("27919", "Newell 310", opensCategory, 1.76m),
                        new Product("28359", "GBC Standard Plastic Binding Systems Combs", obinderCategory, 8.85m),
                        new Product("31344", "GBC Wire Binding Strips", obinderCategory, 31.74m),
                        new Product("31750", "Xerox 1952", opaperCategory, 4.98m),
                        new Product("32472", "Xerox 1927", opaperCategory, 4.28m),
                        new Product("32654", "Wilson Jones 1\" Hanging DublLock® Ring Binders", obinderCategory, 5.28m),
                        new Product("33516", "Xerox 1933", opaperCategory, 12.28m),
                        new Product("36535", "Xerox 1940", opaperCategory, 54.96m),
                        new Product("36536", "Pizazz® Global Quick File™", ostorageCategory, 14.97m),
                        new Product("38207", "EcoTones® Memo Sheets", opaperCategory, 4m),
                        new Product("39019", "Staples Colored Bar Computer Paper", opaperCategory, 35.44m),
                        new Product("41983", "Park Ridge™ Embossed Executive Business Envelopes", oenvelopeCategory, 15.57m),
                        new Product("43908", "Xerox 1949", opaperCategory, 4.98m),
                        new Product("47581", "Xerox 1927_2", opaperCategory, 4.28m),
                        new Product("51024", "Binder Clips by OIC", orubberBandCategory, 1.48m),
                        new Product("53186", "Xerox 1928", opaperCategory, 5.28m),
                        new Product("53797", "Xerox 1993", opaperCategory, 6.48m),
                        new Product("57062", "Avery Durable Poly Binders", obinderCategory, 5.53m),
                        new Product("57827", "Ibico Hi-Tech Manual Binding System", obinderCategory, 304.99m),
                        new Product("57872", "HP Office Paper (20Lb. and 87 Bright)", opaperCategory, 6.68m),
                        new Product("58743", "Office Impressions Heavy Duty Welded Shelving & Multimedia Storage Drawers", ostorageCategory, 167.27m),
                        new Product("59667", "Advantus Map Pennant Flags and Round Head Tacks", orubberBandCategory, 3.95m),
                        new Product("60359", "Xerox 21", opaperCategory, 6.48m),
                        new Product("60426", "Cardinal Poly Pocket Divider Pockets for Ring Binders", obinderCategory, 3.36m),
                        new Product("63644", "Xerox 190", opaperCategory, 4.98m),
                        new Product("64894", "Tennsco Regal Shelving Units", ostorageCategory, 101.41m),
                        new Product("3597", "Boston School Pro Electric Pencil Sharpener, 1670", opensCategory, 30.98m),
                        new Product("4182", "Self-Adhesive Removable Labels", olabelCategory, 3.15m),
                        new Product("7116", "Staples Vinyl Coated Paper Clips", orubberBandCategory, 3.93m),
                        new Product("10773", "Xerox 1962", opaperCategory, 4.28m),
                        new Product("14038", "Plastic Binding Combs", obinderCategory, 15.15m),
                        new Product("15764", "Tyvek® Side-Opening Peel & Seel® Expanding Envelopes", oenvelopeCategory, 90.48m),
                        new Product("28399", "Xerox 1891", opaperCategory, 48.91m),
                        new Product("29338", "DIXON Ticonderoga® Erasable Checking Pencils", opensCategory, 5.58m),
                        new Product("34055", "Rediform S.O.S. Phone Message Books", opaperCategory, 4.98m),
                        new Product("37154", "It's Hot Message Books with Stickers, 2 3/4\" x 5\"", opaperCategory, 7.4m),
                        new Product("37155", "Xerox 1984", opaperCategory, 6.48m),
                        new Product("38805", "*Staples* Highlighting Markers", opensCategory, 4.84m),
                        new Product("38806", "Super Decoflex Portable Personal File", ostorageCategory, 14.98m),
                        new Product("53471", "Newell 309", opensCategory, 11.55m),
                        new Product("59672", "GBC DocuBind TL200 Manual Binding Machine", obinderCategory, 223.98m),
                        new Product("60860", "Bravo II™ Megaboss® 12-Amp Hard Body Upright, Replacement Belts, 2 Belts per Pack", oappCategory, 3.25m),
                        new Product("61251", "Avery Arch Ring Binders", obinderCategory, 58.1m),
                        new Product("269", "Boston 1645 Deluxe Heavier-Duty Electric Pencil Sharpener", opensCategory, 43.98m),
                        new Product("742", "Xerox 1939", opaperCategory, 18.97m),
                        new Product("752", "Acme® Forged Steel Scissors with Black Enamel Handles", oscissorsCategory, 9.31m),
                        new Product("967", "Xerox 23", opaperCategory, 6.48m),
                        new Product("1034", "GBC Standard Therm-A-Bind Covers", obinderCategory, 24.92m),
                        new Product("1153", "OIC Colored Binder Clips, Assorted Sizes", orubberBandCategory, 3.58m),
                        new Product("2311", "Hoover Replacement Belts For Soft Guard™ & Commercial Ltweight Upright Vacs, 2/Pk", oappCategory, 3.95m),
                        new Product("2493", "Avery 494", olabelCategory, 2.61m),
                        new Product("2494", "OIC Bulk Pack Metal Binder Clips", orubberBandCategory, 3.49m),
                        new Product("3006", "*Staples* Letter Opener", oscissorsCategory, 2.18m),
                        new Product("3347", "Xerox 1908", opaperCategory, 55.98m),
                        new Product("3707", "Sterling Rubber Bands by Alliance", orubberBandCategory, 4.71m),
                        new Product("3739", "Durable Pressboard Binders", obinderCategory, 3.8m),
                        new Product("3850", "Deluxe Rollaway Locking File with Drawer", ostorageCategory, 415.88m),
                        new Product("4106", "Riverleaf Stik-Withit® Designer Note Cubes®", opaperCategory, 10.06m),
                        new Product("4107", "Newell 323", opensCategory, 1.68m),
                        new Product("4733", "Xerox 1948", opaperCategory, 9.99m),
                        new Product("5423", "Mead 1st Gear 2\" Zipper Binder, Asst. Colors", obinderCategory, 12.97m),
                        new Product("5424", "Iris® 3-Drawer Stacking Bin, Black", ostorageCategory, 20.89m),
                        new Product("6132", "Ampad® Evidence® Wirebond Steno Books, 6\" x 9\"", opaperCategory, 2.18m),
                        new Product("7335", "Xerox 1992", opaperCategory, 5.98m),
                        new Product("7509", "Acme Design Line 8\" Stainless Steel Bent Scissors w/Champagne Handles, 3-1/8\" Cut", oscissorsCategory, 6.84m),
                        new Product("7510", "Tennsco Industrial Shelving", ostorageCategory, 48.91m),
                        new Product("7688", "Wilson Jones Hanging View Binder, White, 1\"", obinderCategory, 7.1m),
                        new Product("9207", "Binding Machine Supplies", obinderCategory, 29.17m),
                        new Product("9388", "Staples 1 Part Blank Computer Paper", opaperCategory, 11.34m),
                        new Product("9616", "3M Organizer Strips2", obinderCategory, 5.4m),
                        new Product("9903", "Cardinal Poly Pocket Divider Pockets for Ring Binders2", obinderCategory, 3.36m),
                        new Product("9904", "\"While you Were Out\" Message Book, One Form per Page", opaperCategory, 3.71m),
                        new Product("10558", "Xerox 1930", opaperCategory, 6.48m),
                        new Product("10559", "Newell 339", opensCategory, 2.78m),
                        new Product("10987", "Bionaire Personal Warm Mist Humidifier/Vaporizer", oappCategory, 46.89m),
                        new Product("11660", "Xerox 19522", opaperCategory, 4.98m),
                        new Product("11809", "Acme Galleria® Hot Forged Steel Scissors with Colored Handles", oscissorsCategory, 15.73m),
                        new Product("11954", "Sanford 52201 APSCO Electric Pencil Sharpener", opensCategory, 40.97m),
                        new Product("13004", "Tenex Personal Self-Stacking Standard File Box, Black/Gray", ostorageCategory, 16.91m),
                        new Product("13477", "Xerox 1940_2", opaperCategory, 54.96m),
                        new Product("13478", "Staples SlimLine Pencil Sharpener", opensCategory, 11.97m),
                        new Product("13526", "Avery 48", olabelCategory, 6.3m),
                        new Product("13527", "Xerox 214", opaperCategory, 6.48m),
                        new Product("13956", "Bravo II™ Megaboss® 12-Amp Hard Body Upright, Replacement Belts, 2 Belts per Pack2", oappCategory, 3.25m)
                    });

                context.SaveChanges();

                var draftPo1 = new PurchaseOrder("POGH3261", fmSupplier);
                draftPo1.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(fmSupplier.Products.Single(p => p.ProdCode == "67016"), 20, 15),
                    new PurchaseOrderLine(fmSupplier.Products.Single(p => p.ProdCode == "1837"), 21, 15)
                });
                draftPo1.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var draftPo2 = new PurchaseOrder("PO961711", oSupplier);
                draftPo2.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(oSupplier.Products.Single(p => p.ProdCode == "269"), 20, 15),
                    new PurchaseOrderLine(oSupplier.Products.Single(p => p.ProdCode == "11660"), 21, 15)
                });
                draftPo2.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var draftPurchaseOrders = new[] { draftPo1, draftPo2 };

                var shipment1Po1 = new PurchaseOrder("PO2346751", tzSupplier);
                shipment1Po1.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "17225"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "39066"), 21, 15)
                });
                shipment1Po1.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment1Po2 = new PurchaseOrder("PO2346752", tzSupplier);
                shipment1Po2.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "41050"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "40223"), 21, 15)
                });
                shipment1Po2.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment1Po3 = new PurchaseOrder("PO2346753", tzSupplier);
                shipment1Po3.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "42817"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "41785"), 21, 15)
                });
                shipment1Po3.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment1Po4 = new PurchaseOrder("PO2346754", tzSupplier);
                shipment1Po4.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "44924"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "35480"), 21, 15)
                });
                shipment1Po4.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment1Po5 = new PurchaseOrder("PO2346755", tzSupplier);
                shipment1Po5.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "35294"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "24221"), 21, 15)
                });
                shipment1Po5.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);

                var shipment2Po1 = new PurchaseOrder("PO272747", tzSupplier);
                shipment2Po1.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "17225"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "39066"), 21, 15)
                });
                shipment2Po1.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment2Po2 = new PurchaseOrder("PO272748", tzSupplier);
                shipment2Po2.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "41050"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProdCode == "40223"), 21, 15)
                });
                shipment2Po2.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);

                foreach (var p in
                    new[]
                    {
                        shipment1Po1, shipment1Po2, shipment1Po3, shipment1Po4, shipment1Po5,
                        shipment2Po1, shipment2Po1
                    })
                    context.PurchaseOrder.Add(p);
                context.SaveChanges();

                var shipment1 = new Shipment("TRK#61683", "MSC", new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9, 0, 0), 3500, "1 George St, Sydney, 2000", null);
                shipment1.AddPurchaseOrders(new List<PurchaseOrder>(new[] {shipment1Po1, shipment1Po2, shipment1Po3, shipment1Po4, shipment1Po5}));
                shipment1.UpdateStatus(ShipmentStatus.Trigger.Shipped);

                var shipment2 = new Shipment("BRZ#71361", "HTL", new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9, 0, 0).AddDays(7), 17000, "1 Collins St, Melbourne, 3000", null);
                shipment2.AddPurchaseOrders(new List<PurchaseOrder>(new[] {shipment2Po1, shipment2Po2}));
                shipment2.UpdateStatus(ShipmentStatus.Trigger.Shipped);

                foreach (var s in new[] {shipment1, shipment2})
                    context.Shipment.Add(s);
                context.SaveChanges();
            }
        }
    }
}
