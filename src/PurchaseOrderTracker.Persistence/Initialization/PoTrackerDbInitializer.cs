using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;

namespace PurchaseOrderTracker.Persistence.Initialization
{
    public static class PoTrackerDbInitializer
    {
        public static void Initialize(PoTrackerDbContext context)
        {
            // uncomment to delete database on every startup
            //context.Database.EnsureDeleted();
            var created = DbInitializerHelper.EnsureDatabaseCreated(context);

            if (created)
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

                // Techzon
                var tzappCategory = new ProductCategory(new ProductCategoryName("Appliances"));
                var tzbinderCategory = new ProductCategory(new ProductCategoryName("Binders and Binder Accessories"));
                var tzenvelopeCategory = new ProductCategory(new ProductCategoryName("Envelopes"));
                var tzlabelCategory = new ProductCategory(new ProductCategoryName("Labels"));
                var tzpaperCategory = new ProductCategory(new ProductCategoryName("Paper"));
                var tzpensCategory = new ProductCategory(new ProductCategoryName("Pens & Art Supplies"));
                var tzrubberBandCategory = new ProductCategory(new ProductCategoryName("Rubber Bands"));
                var tzscissorsCategory = new ProductCategory(new ProductCategoryName("Scissors, Rulers and Trimmers"));
                var tzstorageCategory = new ProductCategory(new ProductCategoryName("Storage & Organization"));

                // Furniture Max
                var fmbookcaseCategory = new ProductCategory(new ProductCategoryName("Bookcases"));
                var fmchairCategory = new ProductCategory(new ProductCategoryName("Chairs & Chairmats"));
                var fmofficeFurnCategory = new ProductCategory(new ProductCategoryName("Office Furnishings"));
                var fmtablesCategory = new ProductCategory(new ProductCategoryName("Tables"));

                // Office A+
                var oappCategory = new ProductCategory(new ProductCategoryName("Appliances"));
                var obinderCategory = new ProductCategory(new ProductCategoryName("Binders and Binder Accessories"));
                var oenvelopeCategory = new ProductCategory(new ProductCategoryName("Envelopes"));
                var olabelCategory = new ProductCategory(new ProductCategoryName("Labels"));
                var opaperCategory = new ProductCategory(new ProductCategoryName("Paper"));
                var opensCategory = new ProductCategory(new ProductCategoryName("Pens & Art Supplies"));
                var orubberBandCategory = new ProductCategory(new ProductCategoryName("Rubber Bands"));
                var oscissorsCategory = new ProductCategory(new ProductCategoryName("Scissors, Rulers and Trimmers"));
                var ostorageCategory = new ProductCategory(new ProductCategoryName("Storage & Organization"));

                var tzSupplier = new Supplier(new SupplierName("Techzon"));
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
                    });

                var fmSupplier = new Supplier(new SupplierName("Furniture Max"));
                fmSupplier.AddCategorys(new List<ProductCategory>
                {
                    fmbookcaseCategory,
                    fmchairCategory,
                    fmofficeFurnCategory,
                    fmtablesCategory
                });

                var oSupplier = new Supplier(new SupplierName("Office Supplies A+"));
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

                foreach (var s in new[] { tzSupplier, fmSupplier, oSupplier })
                {
                    context.Supplier.Add(s);
                }

                context.SaveChanges();

                tzSupplier.AddProducts(
                    new List<Product>
                    {
                        new Product(new ProductCode("10038"), new ProductName("Carina Double Wide Media Storage Towers in Natural & Black"), tzstorageCategory, 80.98m),
                        new Product(new ProductCode("10221"), new ProductName("Staples® General Use 3-Ring Binders"), tzbinderCategory, 1.88m),
                        new Product(new ProductCode("10222"), new ProductName("Xerox 1904"), tzpaperCategory, 6.48m),
                        new Product(new ProductCode("10224"), new ProductName("Xerox 217"), tzpaperCategory, 6.48m),
                        new Product(new ProductCode("10225"), new ProductName("Revere Boxed Rubber Bands by Revere"), tzrubberBandCategory, 1.89m),
                        new Product(new ProductCode("10384"), new ProductName("Acco Smartsocket™ Table Surge Protector, 6 Color-Coded Adapter Outlets"), tzappCategory, 62.05m),
                        new Product(new ProductCode("10386"), new ProductName("Tennsco Snap-Together Open Shelving Units, Starter Sets and Add-On Units"), tzstorageCategory, 279.48m),
                        new Product(new ProductCode("10809"), new ProductName("Xerox 1887"), tzpaperCategory, 18.97m),
                        new Product(new ProductCode("10810"), new ProductName("Xerox 1891"), tzpaperCategory, 48.91m),
                        new Product(new ProductCode("11100"), new ProductName("Avery 506"), tzlabelCategory, 4.13m),
                        new Product(new ProductCode("11402"), new ProductName("Staples Wirebound Steno Books, 6\" x 9\", 12/Pack"), tzpaperCategory, 10.14m),
                        new Product(new ProductCode("11882"), new ProductName("GBC2 Pre-Punched Binding Paper, Plastic, White, 8-1/2\" x 11\""), tzbinderCategory, 15.99m),
                        new Product(new ProductCode("11998"), new ProductName("Newell 326"), tzpensCategory, 1.76m),
                        new Product(new ProductCode("12284"), new ProductName("Prismacolor Color Pencil Set"), tzpensCategory, 19.84m),
                        new Product(new ProductCode("12287"), new ProductName("Xerox Blank Computer Paper"), tzpaperCategory, 19.98m),
                        new Product(new ProductCode("12681"), new ProductName("Fellowes Recycled Storage Drawers"), tzstorageCategory, 111.03m),
                        new Product(new ProductCode("12753"), new ProductName("Satellite Sectional Post Binders"), tzbinderCategory, 43.41m),
                        new Product(new ProductCode("13050"), new ProductName("Avery 487"), tzlabelCategory, 3.69m),
                        new Product(new ProductCode("13565"), new ProductName("GBC Twin Loop™ Wire Binding Elements, 9/16\" Spine, Black"), tzbinderCategory, 15.22m),
                        new Product(new ProductCode("13596"), new ProductName("Hanging Personal Folder File2"), tzstorageCategory, 15.7m),
                        new Product(new ProductCode("14070"), new ProductName("Fellowes Black Plastic Comb Bindings2"), tzbinderCategory, 5.81m),
                        new Product(new ProductCode("14475"), new ProductName("Tennsco Lockers, Gray"), tzstorageCategory, 20.98m),
                        new Product(new ProductCode("14552"), new ProductName("Avery 4027 File Folder Labels for Dot Matrix Printers, 5000 Labels per Box, White"), tzlabelCategory, 30.53m),
                        new Product(new ProductCode("14553"), new ProductName("Newell 323"), tzpensCategory, 1.68m),
                        new Product(new ProductCode("14736"), new ProductName("Spiral Phone Message Books with Labels by Adams"), tzpaperCategory, 4.48m),
                        new Product(new ProductCode("15122"), new ProductName("Crate-A-Files™"), tzstorageCategory, 10.9m),
                        new Product(new ProductCode("15161"), new ProductName("Holmes 99% HEPA Air Purifier"), tzappCategory, 21.66m),
                        new Product(new ProductCode("15201"), new ProductName("Xerox 224"), tzpaperCategory, 6.48m),
                        new Product(new ProductCode("15615"), new ProductName("Xerox 1906"), tzpaperCategory, 35.44m),
                        new Product(new ProductCode("15723"), new ProductName("GBC Pre-Punched Binding Paper, Plastic, White, 8-1/2\" x 11\""), tzbinderCategory, 15.99m),
                        new Product(new ProductCode("15724"), new ProductName("Xerox 188"), tzpaperCategory, 11.34m),
                        new Product(new ProductCode("15725"), new ProductName("Xerox 1932"), tzpaperCategory, 35.44m),
                        new Product(new ProductCode("16094"), new ProductName("GBC Linen Binding Covers"), tzbinderCategory, 30.98m),
                        new Product(new ProductCode("16095"), new ProductName("GBC Recycled Grain Textured Covers"), tzbinderCategory, 34.54m),
                        new Product(new ProductCode("16889"), new ProductName("Kensington 7 Outlet MasterPiece Power Center with Fax/Phone Line Protection"), tzappCategory, 207.48m),
                        new Product(new ProductCode("17222"), new ProductName("Newell 327"), tzpensCategory, 2.21m),
                        new Product(new ProductCode("17225"), new ProductName("Xerox 1893"), tzpaperCategory, 40.99m),
                        new Product(new ProductCode("18115"), new ProductName("Wirebound Message Books, 2 7/8\" x 5\", 3 Forms per Page"), tzpaperCategory, 7.04m),
                        new Product(new ProductCode("18439"), new ProductName("Multi-Use Personal File Cart and Caster Set, Three Stacking Bins"), tzstorageCategory, 34.76m),
                        new Product(new ProductCode("18704"), new ProductName("Xerox 20"), tzpaperCategory, 6.48m),
                        new Product(new ProductCode("18734"), new ProductName("Surelock™ Post Binders"), tzbinderCategory, 30.56m),
                        new Product(new ProductCode("18839"), new ProductName("Colorific® Watercolor Pencils"), tzpensCategory, 5.16m),
                        new Product(new ProductCode("18840"), new ProductName("*Staples* vLetter Openers, 2/Pack"), tzscissorsCategory, 3.68m),
                        new Product(new ProductCode("19023"), new ProductName("Prang Drawing Pencil Set"), tzpensCategory, 2.78m),
                        new Product(new ProductCode("19064"), new ProductName("Xerox 1982"), tzpaperCategory, 22.84m),
                        new Product(new ProductCode("19278"), new ProductName("Xerox 1924"), tzpaperCategory, 5.78m),
                        new Product(new ProductCode("19279"), new ProductName("Turquoise Lead Holder with Pocket Clip"), tzpensCategory, 6.7m),
                        new Product(new ProductCode("20209"), new ProductName("Xerox 19822"), tzpaperCategory, 22.84m),
                        new Product(new ProductCode("20633"), new ProductName("Xerox 1971"), tzpaperCategory, 4.28m),
                        new Product(new ProductCode("20634"), new ProductName("Eldon Portable Mobile Manager"), tzstorageCategory, 28.28m),
                        new Product(new ProductCode("20828"), new ProductName("Durable Pressboard Binders"), tzbinderCategory, 3.8m),
                        new Product(new ProductCode("21634"), new ProductName("Fellowes PB500 Electric Punch Plastic Comb Binding Machine with Manual Bind"), tzbinderCategory, 1270.99m),
                        new Product(new ProductCode("21715"), new ProductName("Staples 6 Outlet Surge"), tzappCategory, 11.97m),
                        new Product(new ProductCode("21781"), new ProductName("Recycled Eldon Regeneration Jumbo File"), tzstorageCategory, 12.28m),
                        new Product(new ProductCode("22414"), new ProductName("GBC Binding covers2"), tzbinderCategory, 12.95m),
                        new Product(new ProductCode("22775"), new ProductName("Newell 323_2"), tzpensCategory, 1.68m),
                        new Product(new ProductCode("22956"), new ProductName("Speediset Carbonless Redi-Letter® 7\" x 8 1/2\""), tzpaperCategory, 10.31m),
                        new Product(new ProductCode("23074"), new ProductName("GBC Recycled Regency Composition Covers"), tzbinderCategory, 59.78m),
                        new Product(new ProductCode("24220"), new ProductName("Newell 323_3"), tzpensCategory, 1.68m),
                        new Product(new ProductCode("24221"), new ProductName("SANFORD Major Accent™ Highlighters"), tzpensCategory, 7.08m),
                        new Product(new ProductCode("24606"), new ProductName("Dixon Ticonderoga Core-Lock Colored Pencils, 48-Color Set"), tzpensCategory, 36.55m),
                        new Product(new ProductCode("24942"), new ProductName("Recycled Steel Personal File for Standard File Folders"), tzstorageCategory, 55.29m),
                        new Product(new ProductCode("25639"), new ProductName("#10- 4 1/8\" x 9 1/2\" Security-Tint Envelopes"), tzenvelopeCategory, 7.64m),
                        new Product(new ProductCode("25672"), new ProductName("GBC Standard Plastic Binding Systems Combs"), tzbinderCategory, 8.85m),
                        new Product(new ProductCode("26508"), new ProductName("Xerox 1930"), tzpaperCategory, 6.48m),
                        new Product(new ProductCode("26509"), new ProductName("Avery 474"), tzlabelCategory, 2.88m),
                        new Product(new ProductCode("27363"), new ProductName("Peel & Seel® Recycled Catalog Envelopes, Brown2"), tzenvelopeCategory, 11.58m),
                        new Product(new ProductCode("27364"), new ProductName("Avery 5062"), tzlabelCategory, 4.13m),
                        new Product(new ProductCode("27397"), new ProductName("Prang Drawing Pencil Set2"), tzpensCategory, 2.78m),
                        new Product(new ProductCode("27470"), new ProductName("Xerox 190"), tzpaperCategory, 4.98m),
                        new Product(new ProductCode("27508"), new ProductName("Standard Line™ “While You Were Out” Hardbound Telephone Message Book"), tzpaperCategory, 21.98m),
                        new Product(new ProductCode("29226"), new ProductName("Recycled Interoffice Envelopes with String and Button Closure, 10 x 13"), tzenvelopeCategory, 23.99m),
                        new Product(new ProductCode("29335"), new ProductName("GBC Standard Therm-A-Bind Covers"), tzbinderCategory, 24.92m),
                        new Product(new ProductCode("29372"), new ProductName("Xerox 1892"), tzpaperCategory, 38.76m),
                        new Product(new ProductCode("29664"), new ProductName("Staples Paper Clips"), tzrubberBandCategory, 2.47m),
                        new Product(new ProductCode("29765"), new ProductName("Computer Printout Paper with Letter-Trim Perforations"), tzpaperCategory, 18.97m),
                        new Product(new ProductCode("30058"), new ProductName("Xerox 1929"), tzpaperCategory, 22.84m),
                        new Product(new ProductCode("30059"), new ProductName("Home/Office Personal File Carts"), tzstorageCategory, 34.76m),
                        new Product(new ProductCode("30210"), new ProductName("Avery 507"), tzlabelCategory, 2.88m),
                        new Product(new ProductCode("30286"), new ProductName("3M Organizer Strips"), tzlabelCategory, 5.4m),
                        new Product(new ProductCode("30918"), new ProductName("Fellowes Staxonsteel® Drawer Files"), tzstorageCategory, 193.17m),
                        new Product(new ProductCode("31413"), new ProductName("Sauder Facets Collection Locker/File Cabinet, Sky Alder Finish"), tzstorageCategory, 370.98m),
                        new Product(new ProductCode("31418"), new ProductName("Memo Book, 100 Message Capacity, 5 3/8” x 11”"), tzpaperCategory, 6.74m),
                        new Product(new ProductCode("31553"), new ProductName("Belkin 6 Outlet Metallic Surge Strip"), tzappCategory, 10.89m),
                        new Product(new ProductCode("31554"), new ProductName("Xerox 21"), tzpaperCategory, 6.48m),
                        new Product(new ProductCode("31669"), new ProductName("GBC DocuBind TL200 Manual Binding Machine"), tzbinderCategory, 223.98m),
                        new Product(new ProductCode("31823"), new ProductName("Xerox 1940"), tzpaperCategory, 54.96m),
                        new Product(new ProductCode("31824"), new ProductName("Bagged Rubber Bands"), tzrubberBandCategory, 1.26m),
                        new Product(new ProductCode("31932"), new ProductName("Xerox 1897"), tzpaperCategory, 4.98m),
                        new Product(new ProductCode("31971"), new ProductName("Poly Designer Cover & Back2"), tzbinderCategory, 18.99m),
                        new Product(new ProductCode("32082"), new ProductName("Bionaire Personal Warm Mist Humidifier/Vaporizer"), tzappCategory, 46.89m),
                        new Product(new ProductCode("32083"), new ProductName("Acme® 8\" Straight Scissors"), tzscissorsCategory, 12.98m),
                        new Product(new ProductCode("32117"), new ProductName("Heavy-Duty E-Z-D® Binders"), tzbinderCategory, 10.91m),
                        new Product(new ProductCode("32252"), new ProductName("Hammermill CopyPlus Copy Paper (20Lb. and 84 Bright)"), tzpaperCategory, 4.98m),
                        new Product(new ProductCode("32478"), new ProductName("Deluxe Rollaway Locking File with Drawer"), tzstorageCategory, 415.88m),
                        new Product(new ProductCode("33105"), new ProductName("Prismacolor Color Pencil Set2"), tzpensCategory, 19.84m),
                        new Product(new ProductCode("33177"), new ProductName("Wilson Jones DublLock® D-Ring Binders"), tzbinderCategory, 6.75m),
                        new Product(new ProductCode("33405"), new ProductName("Binder Posts"), tzbinderCategory, 5.74m),
                        new Product(new ProductCode("33988"), new ProductName("Fellowes PB300 Plastic Comb Binding Machine"), tzbinderCategory, 387.99m),
                        new Product(new ProductCode("34063"), new ProductName("Avery 481"), tzlabelCategory, 3.08m),
                        new Product(new ProductCode("34964"), new ProductName("SAFCO Commercial Wire Shelving, Black"), tzstorageCategory, 138.14m),
                        new Product(new ProductCode("35293"), new ProductName("Avery Trapezoid Extra Heavy Duty 4\" Binders"), tzbinderCategory, 41.94m),
                        new Product(new ProductCode("35294"), new ProductName("Self-Adhesive Address Labels for Typewriters by Universal"), tzlabelCategory, 7.31m),
                        new Product(new ProductCode("35480"), new ProductName("Rediform Wirebound \"Phone Memo\" Message Book, 11 x 5-3/4"), tzpaperCategory, 7.64m),
                        new Product(new ProductCode("35765"), new ProductName("Deluxe Rollaway Locking File with Drawer2"), tzstorageCategory, 415.88m),
                        new Product(new ProductCode("35798"), new ProductName("*Staples* vLetter Openers, 3/Pack"), tzscissorsCategory, 3.68m),
                        new Product(new ProductCode("35903"), new ProductName("Staples 6 Outlet Surge2"), tzappCategory, 11.97m),
                        new Product(new ProductCode("35904"), new ProductName("Acco Perma® 2700 Stacking Storage Drawers"), tzstorageCategory, 29.74m),
                        new Product(new ProductCode("35905"), new ProductName("Fellowes Stor/Drawer® Steel Plus™ Storage Drawers"), tzstorageCategory, 95.43m),
                        new Product(new ProductCode("35987"), new ProductName("Honeywell Enviracaire Portable HEPA Air Cleaner for 17' x 22' Room"), tzappCategory, 300.65m),
                        new Product(new ProductCode("36055"), new ProductName("Lock-Up Easel 'Spel-Binder'"), tzbinderCategory, 28.53m),
                        new Product(new ProductCode("36130"), new ProductName("Adams Write n' Stick Phone Message Book, 11\" X 5 1/4\", 200 Messages"), tzpaperCategory, 5.68m),
                        new Product(new ProductCode("36131"), new ProductName("Tenex File Box, Personal Filing Tote with Lid, Black"), tzstorageCategory, 15.51m),
                        new Product(new ProductCode("36240"), new ProductName("Fellowes Black Plastic Comb Bindings"), tzbinderCategory, 5.81m),
                        new Product(new ProductCode("36241"), new ProductName("Assorted Color Push Pins"), tzrubberBandCategory, 1.81m),
                        new Product(new ProductCode("36717"), new ProductName("HP Office Paper (20Lb. and 87 Bright)"), tzpaperCategory, 6.68m),
                        new Product(new ProductCode("36718"), new ProductName("Staples Vinyl Coated Paper Clips"), tzrubberBandCategory, 3.93m),
                        new Product(new ProductCode("36762"), new ProductName("Newell 310"), tzpensCategory, 1.76m),
                        new Product(new ProductCode("37342"), new ProductName("Kensington 7 Outlet MasterPiece Power Center"), tzappCategory, 177.98m),
                        new Product(new ProductCode("38064"), new ProductName("Array® Memo Cubes"), tzpaperCategory, 5.18m),
                        new Product(new ProductCode("38065"), new ProductName("Fiskars 8\" Scissors, 2/Pack"), tzscissorsCategory, 17.24m),
                        new Product(new ProductCode("38135"), new ProductName("12 Colored Short Pencils"), tzpensCategory, 2.6m),
                        new Product(new ProductCode("38136"), new ProductName("Pizazz® Global Quick File™"), tzstorageCategory, 14.97m),
                        new Product(new ProductCode("38442"), new ProductName("Acco Perma® 2700 Stacking Storage Drawers2"), tzstorageCategory, 29.74m),
                        new Product(new ProductCode("38443"), new ProductName("Portable Personal File Box"), tzstorageCategory, 12.21m),
                        new Product(new ProductCode("38480"), new ProductName("Snap-A-Way® Black Print Carbonless Speed Message, No Reply Area, Duplicate"), tzpaperCategory, 29.14m),
                        new Product(new ProductCode("38651"), new ProductName("Ibico Covers for Plastic or Wire Binding Elements"), tzbinderCategory, 11.5m),
                        new Product(new ProductCode("38652"), new ProductName("Hanging Personal Folder File"), tzstorageCategory, 15.7m),
                        new Product(new ProductCode("38653"), new ProductName("Tennsco Double-Tier Lockers"), tzstorageCategory, 225.02m),
                        new Product(new ProductCode("38666"), new ProductName("Wirebound Message Books, Four 2 3/4\" x 5\" Forms per Page, 600 Sets per Book"), tzpaperCategory, 9.27m),
                        new Product(new ProductCode("38667"), new ProductName("Acme Design Line 8\" Stainless Steel Bent Scissors w/Champagne Handles, 3-1/8\" Cut"), tzscissorsCategory, 6.84m),
                        new Product(new ProductCode("38980"), new ProductName("Avery 494"), tzlabelCategory, 2.61m),
                        new Product(new ProductCode("38981"), new ProductName("Self-Adhesive Address Labels for Typewriters by Universal2"), tzlabelCategory, 7.31m),
                        new Product(new ProductCode("39027"), new ProductName("Wilson Jones 14 Line Acrylic Coated Pressboard Data Binders"), tzbinderCategory, 5.34m),
                        new Product(new ProductCode("39066"), new ProductName("Eureka Disposable Bags for Sanitaire® Vibra Groomer I® Upright Vac"), tzappCategory, 4.06m),
                        new Product(new ProductCode("39068"), new ProductName("Peel & Seel® Recycled Catalog Envelopes, Brown"), tzenvelopeCategory, 11.58m),
                        new Product(new ProductCode("39495"), new ProductName("Cardinal Poly Pocket Divider Pockets for Ring Binders"), tzbinderCategory, 3.36m),
                        new Product(new ProductCode("39976"), new ProductName("Sterling Rubber Bands by Alliance"), tzrubberBandCategory, 4.71m),
                        new Product(new ProductCode("40223"), new ProductName("Newell 312"), tzpensCategory, 5.84m),
                        new Product(new ProductCode("41049"), new ProductName("Avery Flip-Chart Easel Binder, Black"), tzbinderCategory, 22.38m),
                        new Product(new ProductCode("41050"), new ProductName("GBC Binding covers"), tzbinderCategory, 12.95m),
                        new Product(new ProductCode("41785"), new ProductName("Tennsco Industrial Shelving"), tzstorageCategory, 48.91m),
                        new Product(new ProductCode("41861"), new ProductName("Staples Bulldog Clip"), tzrubberBandCategory, 3.78m),
                        new Product(new ProductCode("42817"), new ProductName("Xerox 1882"), tzpaperCategory, 55.98m),
                        new Product(new ProductCode("42818"), new ProductName("Carina Double Wide Media Storage Towers in Natural & Black2"), tzstorageCategory, 80.98m),
                        new Product(new ProductCode("42920"), new ProductName("Portable Personal File Box2"), tzstorageCategory, 12.21m),
                        new Product(new ProductCode("43105"), new ProductName("Acme Hot Forged Carbon Steel Scissors with Nickel-Plated Handles, 3 7/8\" Cut, 8\"L"), tzscissorsCategory, 13.9m),
                        new Product(new ProductCode("44356"), new ProductName("Xerox 4200 Series MultiUse Premium Copy Paper (20Lb. and 84 Bright)"), tzpaperCategory, 5.28m),
                        new Product(new ProductCode("44924"), new ProductName("Fellowes PB500 Electric Punch Plastic Comb Binding Machine with Manual Bind2"), tzbinderCategory, 1270.99m),
                        new Product(new ProductCode("45281"), new ProductName("Poly Designer Cover & Back"), tzbinderCategory, 18.99m)
                    });

                fmSupplier.AddProducts(
                    new List<Product>
                    {
                        new Product(new ProductCode("41563"), new ProductName("Tensor \"Hersey Kiss\" Styled Floor Lamp"), fmofficeFurnCategory, 12.99m),
                        new Product(new ProductCode("46848"), new ProductName("Bretford CR8500 Series Meeting Room Furniture"), fmtablesCategory, 400.98m),
                        new Product(new ProductCode("49987"), new ProductName("Eldon Expressions Mahogany Wood Desk Collection"), fmofficeFurnCategory, 6.24m),
                        new Product(new ProductCode("52596"), new ProductName("Global Deluxe Stacking Chair, Gray"), fmchairCategory, 50.98m),
                        new Product(new ProductCode("54781"), new ProductName("Chromcraft Rectangular Conference Tables"), fmtablesCategory, 236.97m),
                        new Product(new ProductCode("58560"), new ProductName("SAFCO PlanMaster Heigh-Adjustable Drafting Table Base, 43won® Expressions™ Wood Desk Accessories, Oak x 30d x 30-37h, Black"), fmtablesCategory, 349.45m),
                        new Product(new ProductCode("59377"), new ProductName("Staples Plastic Wall Frames"), fmofficeFurnCategory, 7.96m),
                        new Product(new ProductCode("60602"), new ProductName("Telescoping Adjustable Floor Lamp"), fmofficeFurnCategory, 19.99m),
                        new Product(new ProductCode("63211"), new ProductName("Magna Visual Magnetic Picture Hangers"), fmofficeFurnCategory, 4.82m),
                        new Product(new ProductCode("65603"), new ProductName("Linden® 12\" Wall Clock With Oak Frame"), fmofficeFurnCategory, 33.98m),
                        new Product(new ProductCode("66858"), new ProductName("Eldon® Wave Desk Accessories"), fmofficeFurnCategory, 2.08m),
                        new Product(new ProductCode("67016"), new ProductName("DAX Solid Wood Frames"), fmofficeFurnCategory, 9.77m),
                        new Product(new ProductCode("1837"), new ProductName("Eldon Expressions™ Desk Accessory, Wood Pencil Holder, Oak"), fmofficeFurnCategory, 9.65m),
                        new Product(new ProductCode("3567"), new ProductName("Executive Impressions 14\" Contract Wall Clock with Quartz Movement"), fmofficeFurnCategory, 22.23m),
                        new Product(new ProductCode("4332"), new ProductName("Executive Impressions 12\" Wall Clock"), fmofficeFurnCategory, 17.67m),
                        new Product(new ProductCode("4556"), new ProductName("Artistic Insta-Plaque"), fmofficeFurnCategory, 15.68m),
                        new Product(new ProductCode("18001"), new ProductName("3M Hangers With Command Adhesive"), fmofficeFurnCategory, 3.7m),
                        new Product(new ProductCode("20459"), new ProductName("Office Star Flex Back Scooter Chair with White Frame"), fmchairCategory, 110.98m),
                        new Product(new ProductCode("23321"), new ProductName("Atlantic Metals Mobile 2-Shelf Bookcases, Custom Colors"), fmbookcaseCategory, 240.98m),
                        new Product(new ProductCode("40038"), new ProductName("Luxo Economy Swing Arm Lamp"), fmofficeFurnCategory, 19.94m),
                        new Product(new ProductCode("40088"), new ProductName("DAX Contemporary Wood Frame with Silver Metal Mat, Desktop, 11 x 14 Size"), fmofficeFurnCategory, 20.24m),
                        new Product(new ProductCode("42335"), new ProductName("Hon Multipurpose Stacking Arm Chairs"), fmchairCategory, 216.6m),
                        new Product(new ProductCode("46809"), new ProductName("Global Leather Task Chair, Black"), fmchairCategory, 89.99m),
                        new Product(new ProductCode("47396"), new ProductName("Office Star - Mid Back Dual function Ergonomic High Back Chair with 2-Way Adjustable Arms"), fmchairCategory, 160.98m),
                        new Product(new ProductCode("47468"), new ProductName("O'Sullivan Living Dimensions 3-Shelf Bookcases"), fmbookcaseCategory, 200.98m),
                        new Product(new ProductCode("57616"), new ProductName("Anderson Hickey Conga Table Tops & Accessories"), fmtablesCategory, 15.23m),
                        new Product(new ProductCode("60783"), new ProductName("Howard Miller 12-3/4 Diameter Accuwave DS ™ Wall Clock"), fmofficeFurnCategory, 78.69m),
                        new Product(new ProductCode("60784"), new ProductName("Bevis Rectangular Conference Tables"), fmtablesCategory, 145.98m),
                        new Product(new ProductCode("62439"), new ProductName("Office Star - Task Chair with Contemporary Loop Arms"), fmchairCategory, 90.98m),
                        new Product(new ProductCode("64499"), new ProductName("O'Sullivan Elevations Bookcase, Cherry Finish"), fmbookcaseCategory, 130.98m),
                        new Product(new ProductCode("64609"), new ProductName("Aluminum Document Frame"), fmofficeFurnCategory, 12.22m),
                        new Product(new ProductCode("11768"), new ProductName("Hon Pagoda™ Stacking Chairs"), fmchairCategory, 320.98m),
                        new Product(new ProductCode("11769"), new ProductName("Rubbermaid ClusterMat Chairmats, Mat Size- 66\" x 60\", Lip 20\" x 11\" -90 Degree Angle"), fmofficeFurnCategory, 110.98m),
                        new Product(new ProductCode("15162"), new ProductName("Tensor \"Hersey Kiss\" Styled Floor Lamp2"), fmofficeFurnCategory, 12.99m),
                        new Product(new ProductCode("23688"), new ProductName("DAX Copper Panel Documentn® 12\" Wall Clock With Oak Frame Frame, 5 x 7 Size"), fmofficeFurnCategory, 12.58m),
                        new Product(new ProductCode("42698"), new ProductName("Eldon Expressions Punched Metal & Wood Desk Accessories, Black & Cherry"), fmofficeFurnCategory, 9.38m),
                        new Product(new ProductCode("53804"), new ProductName("Chromcraft Bull-Nose Wood Round Conference Table Top, Wood Base"), fmtablesCategory, 217.85m),
                        new Product(new ProductCode("1550"), new ProductName("Bush Westfield Collection Bookcases, Fully Assembled"), fmbookcaseCategory, 100.98m),
                        new Product(new ProductCode("2525"), new ProductName("Hon 4070 Series Pagoda™ Round Back Stacking Chairs"), fmchairCategory, 320.98m),
                        new Product(new ProductCode("9357"), new ProductName("12-1/2 Diameter Round Wall Clock"), fmofficeFurnCategory, 19.98m),
                        new Product(new ProductCode("11103"), new ProductName("SAFCO Folding Chair Trolley"), fmchairCategory, 128.24m),
                        new Product(new ProductCode("13521"), new ProductName("Tenex Contemporary Contur Chairmats for Low and Medium Pile Carpet, Computer, 39\" x 49\""), fmofficeFurnCategory, 107.53m),
                        new Product(new ProductCode("13916"), new ProductName("Bevis 36 x 72 Conference Tables"), fmtablesCategory, 124.49m),
                        new Product(new ProductCode("13964"), new ProductName("Global Leather Highback Executive Chair with Pneumatic Height Adjustment, Black"), fmchairCategory, 200.98m),
                        new Product(new ProductCode("15271"), new ProductName("Tenex Traditional Chairmats for Medium Pile Carpet, Standard Lip, 36\" x 48\""), fmofficeFurnCategory, 60.65m),
                        new Product(new ProductCode("20315"), new ProductName("Seth Thomas 12\" Clock w/ Goldtone Case"), fmofficeFurnCategory, 22.98m),
                        new Product(new ProductCode("24683"), new ProductName("Howard Miller 12-3/4 Diameter Accuwave DS ™ Wall Clock2"), fmofficeFurnCategory, 78.69m),
                        new Product(new ProductCode("32432"), new ProductName("Electrix Halogen Magnifier Lamp"), fmofficeFurnCategory, 194.3m),
                        new Product(new ProductCode("32433"), new ProductName("Luxo Professional Fluorescent Magnifier Lamp with Clamp-Mount Base"), fmofficeFurnCategory, 209.84m),
                        new Product(new ProductCode("32471"), new ProductName("Master Caster Door Stop, Gray"), fmofficeFurnCategory, 5.08m),
                        new Product(new ProductCode("39018"), new ProductName("Coloredge Poster Frame"), fmofficeFurnCategory, 14.2m),
                        new Product(new ProductCode("42565"), new ProductName("Linden® 12\" Wall Clock With Oak Frame2"), fmofficeFurnCategory, 33.98m),
                        new Product(new ProductCode("51023"), new ProductName("Bush Advantage Collection® Racetrack Conference Table"), fmtablesCategory, 424.21m),
                        new Product(new ProductCode("51730"), new ProductName("Riverside Palais Royal Lawyers Bookcase, Royale Cherry Finish"), fmbookcaseCategory, 880.98m),
                        new Product(new ProductCode("57063"), new ProductName("Eldon Econocleat® Chair Mats for Low Pile Carpets"), fmofficeFurnCategory, 41.47m),
                        new Product(new ProductCode("58018"), new ProductName("Office Star - Ergonomic Mid Back Chair with 2-Way Adjustable Arms"), fmchairCategory, 180.98m),
                        new Product(new ProductCode("60459"), new ProductName("Career Cubicle Clock, 8 1/4\", Black"), fmofficeFurnCategory, 20.28m),
                        new Product(new ProductCode("61712"), new ProductName("Eldon® Wave Desk Accessories2"), fmofficeFurnCategory, 2.08m),
                        new Product(new ProductCode("65638"), new ProductName("Hon Metal Bookcases, Black"), fmbookcaseCategory, 70.98m),
                        new Product(new ProductCode("10772"), new ProductName("Executive Impressions 8-1/2\" Career Panel/Partition Cubicle Clock"), fmofficeFurnCategory, 10.4m),
                        new Product(new ProductCode("14039"), new ProductName("Eldon® Expressions™ Wood Desk Accessories, Oak"), fmofficeFurnCategory, 7.38m),
                        new Product(new ProductCode("20569"), new ProductName("Bretford CR4500 Series Slim Rectangular Table"), fmtablesCategory, 348.21m),
                        new Product(new ProductCode("33950"), new ProductName("Bevis Steel Folding Chairs"), fmchairCategory, 95.95m),
                        new Product(new ProductCode("60861"), new ProductName("Global Leather and Oak Executive Chair, Black"), fmchairCategory, 300.98m),
                        new Product(new ProductCode("524"), new ProductName("Eldon® Expressions™ Wood Desk Accessories, Oak2"), fmofficeFurnCategory, 7.38m)
                    });

                oSupplier.AddProducts(
                    new List<Product>
                    {
                        new Product(new ProductCode("15312"), new ProductName("Wirebound Voice Message Log Book"), opaperCategory, 4.76m),
                        new Product(new ProductCode("15494"), new ProductName("Eureka Hand Vacuum, Bagless"), oappCategory, 49.43m),
                        new Product(new ProductCode("15832"), new ProductName("Prang Colored Pencils"), opensCategory, 2.94m),
                        new Product(new ProductCode("18766"), new ProductName("3M Organizer Strips"), obinderCategory, 5.4m),
                        new Product(new ProductCode("21132"), new ProductName("Avery 05222 Permanent Self-Adhesive File Folder Labels for Typewriters, on Rolls, White, 250/Roll"), olabelCategory, 4.13m),
                        new Product(new ProductCode("22516"), new ProductName("GBC Prepunched Paper, 19-Hole, for Binding Systems, 24-lb"), obinderCategory, 15.01m),
                        new Product(new ProductCode("22517"), new ProductName("GBC Recycled Regency Composition Covers"), obinderCategory, 59.78m),
                        new Product(new ProductCode("24593"), new ProductName("SAFCO Boltless Steel Shelving"), ostorageCategory, 113.64m),
                        new Product(new ProductCode("27919"), new ProductName("Newell 310"), opensCategory, 1.76m),
                        new Product(new ProductCode("28359"), new ProductName("GBC Standard Plastic Binding Systems Combs"), obinderCategory, 8.85m),
                        new Product(new ProductCode("31344"), new ProductName("GBC Wire Binding Strips"), obinderCategory, 31.74m),
                        new Product(new ProductCode("31750"), new ProductName("Xerox 1952"), opaperCategory, 4.98m),
                        new Product(new ProductCode("32472"), new ProductName("Xerox 1927"), opaperCategory, 4.28m),
                        new Product(new ProductCode("32654"), new ProductName("Wilson Jones 1\" Hanging DublLock® Ring Binders"), obinderCategory, 5.28m),
                        new Product(new ProductCode("33516"), new ProductName("Xerox 1933"), opaperCategory, 12.28m),
                        new Product(new ProductCode("36535"), new ProductName("Xerox 1940"), opaperCategory, 54.96m),
                        new Product(new ProductCode("36536"), new ProductName("Pizazz® Global Quick File™"), ostorageCategory, 14.97m),
                        new Product(new ProductCode("38207"), new ProductName("EcoTones® Memo Sheets"), opaperCategory, 4m),
                        new Product(new ProductCode("39019"), new ProductName("Staples Colored Bar Computer Paper"), opaperCategory, 35.44m),
                        new Product(new ProductCode("41983"), new ProductName("Park Ridge™ Embossed Executive Business Envelopes"), oenvelopeCategory, 15.57m),
                        new Product(new ProductCode("43908"), new ProductName("Xerox 1949"), opaperCategory, 4.98m),
                        new Product(new ProductCode("47581"), new ProductName("Xerox 1927_2"), opaperCategory, 4.28m),
                        new Product(new ProductCode("51024"), new ProductName("Binder Clips by OIC"), orubberBandCategory, 1.48m),
                        new Product(new ProductCode("53186"), new ProductName("Xerox 1928"), opaperCategory, 5.28m),
                        new Product(new ProductCode("53797"), new ProductName("Xerox 1993"), opaperCategory, 6.48m),
                        new Product(new ProductCode("57062"), new ProductName("Avery Durable Poly Binders"), obinderCategory, 5.53m),
                        new Product(new ProductCode("57827"), new ProductName("Ibico Hi-Tech Manual Binding System"), obinderCategory, 304.99m),
                        new Product(new ProductCode("57872"), new ProductName("HP Office Paper (20Lb. and 87 Bright)"), opaperCategory, 6.68m),
                        new Product(new ProductCode("58743"), new ProductName("Office Impressions Heavy Duty Welded Shelving & Multimedia Storage Drawers"), ostorageCategory, 167.27m),
                        new Product(new ProductCode("59667"), new ProductName("Advantus Map Pennant Flags and Round Head Tacks"), orubberBandCategory, 3.95m),
                        new Product(new ProductCode("60359"), new ProductName("Xerox 21"), opaperCategory, 6.48m),
                        new Product(new ProductCode("60426"), new ProductName("Cardinal Poly Pocket Divider Pockets for Ring Binders"), obinderCategory, 3.36m),
                        new Product(new ProductCode("63644"), new ProductName("Xerox 190"), opaperCategory, 4.98m),
                        new Product(new ProductCode("64894"), new ProductName("Tennsco Regal Shelving Units"), ostorageCategory, 101.41m),
                        new Product(new ProductCode("3597"), new ProductName("Boston School Pro Electric Pencil Sharpener, 1670"), opensCategory, 30.98m),
                        new Product(new ProductCode("4182"), new ProductName("Self-Adhesive Removable Labels"), olabelCategory, 3.15m),
                        new Product(new ProductCode("7116"), new ProductName("Staples Vinyl Coated Paper Clips"), orubberBandCategory, 3.93m),
                        new Product(new ProductCode("10773"), new ProductName("Xerox 1962"), opaperCategory, 4.28m),
                        new Product(new ProductCode("14038"), new ProductName("Plastic Binding Combs"), obinderCategory, 15.15m),
                        new Product(new ProductCode("15764"), new ProductName("Tyvek® Side-Opening Peel & Seel® Expanding Envelopes"), oenvelopeCategory, 90.48m),
                        new Product(new ProductCode("28399"), new ProductName("Xerox 1891"), opaperCategory, 48.91m),
                        new Product(new ProductCode("29338"), new ProductName("DIXON Ticonderoga® Erasable Checking Pencils"), opensCategory, 5.58m),
                        new Product(new ProductCode("34055"), new ProductName("Rediform S.O.S. Phone Message Books"), opaperCategory, 4.98m),
                        new Product(new ProductCode("37154"), new ProductName("It's Hot Message Books with Stickers, 2 3/4\" x 5\""), opaperCategory, 7.4m),
                        new Product(new ProductCode("37155"), new ProductName("Xerox 1984"), opaperCategory, 6.48m),
                        new Product(new ProductCode("38805"), new ProductName("*Staples* Highlighting Markers"), opensCategory, 4.84m),
                        new Product(new ProductCode("38806"), new ProductName("Super Decoflex Portable Personal File"), ostorageCategory, 14.98m),
                        new Product(new ProductCode("53471"), new ProductName("Newell 309"), opensCategory, 11.55m),
                        new Product(new ProductCode("59672"), new ProductName("GBC DocuBind TL200 Manual Binding Machine"), obinderCategory, 223.98m),
                        new Product(new ProductCode("60860"), new ProductName("Bravo II™ Megaboss® 12-Amp Hard Body Upright, Replacement Belts, 2 Belts per Pack"), oappCategory, 3.25m),
                        new Product(new ProductCode("61251"), new ProductName("Avery Arch Ring Binders"), obinderCategory, 58.1m),
                        new Product(new ProductCode("269"), new ProductName("Boston 1645 Deluxe Heavier-Duty Electric Pencil Sharpener"), opensCategory, 43.98m),
                        new Product(new ProductCode("742"), new ProductName("Xerox 1939"), opaperCategory, 18.97m),
                        new Product(new ProductCode("752"), new ProductName("Acme® Forged Steel Scissors with Black Enamel Handles"), oscissorsCategory, 9.31m),
                        new Product(new ProductCode("967"), new ProductName("Xerox 23"), opaperCategory, 6.48m),
                        new Product(new ProductCode("1034"), new ProductName("GBC Standard Therm-A-Bind Covers"), obinderCategory, 24.92m),
                        new Product(new ProductCode("1153"), new ProductName("OIC Colored Binder Clips, Assorted Sizes"), orubberBandCategory, 3.58m),
                        new Product(new ProductCode("2311"), new ProductName("Hoover Replacement Belts For Soft Guard™ & Commercial Ltweight Upright Vacs, 2/Pk"), oappCategory, 3.95m),
                        new Product(new ProductCode("2493"), new ProductName("Avery 494"), olabelCategory, 2.61m),
                        new Product(new ProductCode("2494"), new ProductName("OIC Bulk Pack Metal Binder Clips"), orubberBandCategory, 3.49m),
                        new Product(new ProductCode("3006"), new ProductName("*Staples* Letter Opener"), oscissorsCategory, 2.18m),
                        new Product(new ProductCode("3347"), new ProductName("Xerox 1908"), opaperCategory, 55.98m),
                        new Product(new ProductCode("3707"), new ProductName("Sterling Rubber Bands by Alliance"), orubberBandCategory, 4.71m),
                        new Product(new ProductCode("3739"), new ProductName("Durable Pressboard Binders"), obinderCategory, 3.8m),
                        new Product(new ProductCode("3850"), new ProductName("Deluxe Rollaway Locking File with Drawer"), ostorageCategory, 415.88m),
                        new Product(new ProductCode("4106"), new ProductName("Riverleaf Stik-Withit® Designer Note Cubes®"), opaperCategory, 10.06m),
                        new Product(new ProductCode("4107"), new ProductName("Newell 323"), opensCategory, 1.68m),
                        new Product(new ProductCode("4733"), new ProductName("Xerox 1948"), opaperCategory, 9.99m),
                        new Product(new ProductCode("5423"), new ProductName("Mead 1st Gear 2\" Zipper Binder, Asst. Colors"), obinderCategory, 12.97m),
                        new Product(new ProductCode("5424"), new ProductName("Iris® 3-Drawer Stacking Bin, Black"), ostorageCategory, 20.89m),
                        new Product(new ProductCode("6132"), new ProductName("Ampad® Evidence® Wirebond Steno Books, 6\" x 9\""), opaperCategory, 2.18m),
                        new Product(new ProductCode("7335"), new ProductName("Xerox 1992"), opaperCategory, 5.98m),
                        new Product(new ProductCode("7509"), new ProductName("Acme Design Line 8\" Stainless Steel Bent Scissors w/Champagne Handles, 3-1/8\" Cut"), oscissorsCategory, 6.84m),
                        new Product(new ProductCode("7510"), new ProductName("Tennsco Industrial Shelving"), ostorageCategory, 48.91m),
                        new Product(new ProductCode("7688"), new ProductName("Wilson Jones Hanging View Binder, White, 1\""), obinderCategory, 7.1m),
                        new Product(new ProductCode("9207"), new ProductName("Binding Machine Supplies"), obinderCategory, 29.17m),
                        new Product(new ProductCode("9388"), new ProductName("Staples 1 Part Blank Computer Paper"), opaperCategory, 11.34m),
                        new Product(new ProductCode("9616"), new ProductName("3M Organizer Strips2"), obinderCategory, 5.4m),
                        new Product(new ProductCode("9903"), new ProductName("Cardinal Poly Pocket Divider Pockets for Ring Binders2"), obinderCategory, 3.36m),
                        new Product(new ProductCode("9904"), new ProductName("\"While you Were Out\" Message Book, One Form per Page"), opaperCategory, 3.71m),
                        new Product(new ProductCode("10558"), new ProductName("Xerox 1930"), opaperCategory, 6.48m),
                        new Product(new ProductCode("10559"), new ProductName("Newell 339"), opensCategory, 2.78m),
                        new Product(new ProductCode("10987"), new ProductName("Bionaire Personal Warm Mist Humidifier/Vaporizer"), oappCategory, 46.89m),
                        new Product(new ProductCode("11660"), new ProductName("Xerox 19522"), opaperCategory, 4.98m),
                        new Product(new ProductCode("11809"), new ProductName("Acme Galleria® Hot Forged Steel Scissors with Colored Handles"), oscissorsCategory, 15.73m),
                        new Product(new ProductCode("11954"), new ProductName("Sanford 52201 APSCO Electric Pencil Sharpener"), opensCategory, 40.97m),
                        new Product(new ProductCode("13004"), new ProductName("Tenex Personal Self-Stacking Standard File Box, Black/Gray"), ostorageCategory, 16.91m),
                        new Product(new ProductCode("13477"), new ProductName("Xerox 1940_2"), opaperCategory, 54.96m),
                        new Product(new ProductCode("13478"), new ProductName("Staples SlimLine Pencil Sharpener"), opensCategory, 11.97m),
                        new Product(new ProductCode("13526"), new ProductName("Avery 48"), olabelCategory, 6.3m),
                        new Product(new ProductCode("13527"), new ProductName("Xerox 214"), opaperCategory, 6.48m),
                        new Product(new ProductCode("13956"), new ProductName("Bravo II™ Megaboss® 12-Amp Hard Body Upright, Replacement Belts, 2 Belts per Pack2"), oappCategory, 3.25m)
                    });

                context.SaveChanges();

                var draftPo1 = new PurchaseOrder(new OrderNo("POGH3261"), fmSupplier);
                draftPo1.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(fmSupplier.Products.Single(p => p.ProductCode.Value == "67016"), 20, 15),
                    new PurchaseOrderLine(fmSupplier.Products.Single(p => p.ProductCode.Value == "1837"), 21, 15)
                });
                draftPo1.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var draftPo2 = new PurchaseOrder(new OrderNo("PO961711"), oSupplier);
                draftPo2.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(oSupplier.Products.Single(p => p.ProductCode.Value == "269"), 20, 15),
                    new PurchaseOrderLine(oSupplier.Products.Single(p => p.ProductCode.Value == "11660"), 21, 15)
                });
                draftPo2.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                foreach (var po in new[] { draftPo1, draftPo2 })
                {
                    context.PurchaseOrder.Add(po);
                }

                context.SaveChanges();

                var shipment1Po1 = new PurchaseOrder(new OrderNo("PO2346751"), tzSupplier);
                shipment1Po1.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "17225"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "39066"), 21, 15)
                });
                shipment1Po1.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment1Po2 = new PurchaseOrder(new OrderNo("PO2346752"), tzSupplier);
                shipment1Po2.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "41050"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "40223"), 21, 15)
                });
                shipment1Po2.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment1Po3 = new PurchaseOrder(new OrderNo("PO2346753"), tzSupplier);
                shipment1Po3.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "42817"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "41785"), 21, 15)
                });
                shipment1Po3.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment1Po4 = new PurchaseOrder(new OrderNo("PO2346754"), tzSupplier);
                shipment1Po4.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "44924"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "35480"), 21, 15)
                });
                shipment1Po4.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment1Po5 = new PurchaseOrder(new OrderNo("PO2346755"), tzSupplier);
                shipment1Po5.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "35294"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "24221"), 21, 15)
                });
                shipment1Po5.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);

                var shipment2Po1 = new PurchaseOrder(new OrderNo("PO272747"), tzSupplier);
                shipment2Po1.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "17225"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "39066"), 21, 15)
                });
                shipment2Po1.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment2Po2 = new PurchaseOrder(new OrderNo("PO272748"), tzSupplier);
                shipment2Po2.AddLineItems(new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "41050"), 20, 15),
                    new PurchaseOrderLine(tzSupplier.Products.Single(p => p.ProductCode.Value == "40223"), 21, 15)
                });
                shipment2Po2.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);

                foreach (var p in
                    new[]
                    {
                        shipment1Po1, shipment1Po2, shipment1Po3, shipment1Po4, shipment1Po5,
                        shipment2Po1, shipment2Po1
                    })
                {
                    context.PurchaseOrder.Add(p);
                }

                context.SaveChanges();

                var shipment1 = new Shipment("TRK#61683", "MSC", new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9, 0, 0), 3500, "1 George St, Sydney, 2000", null);
                shipment1.AddPurchaseOrders(new List<PurchaseOrder>(new[] { shipment1Po1, shipment1Po2, shipment1Po3, shipment1Po4, shipment1Po5 }));
                shipment1.UpdateStatus(ShipmentStatus.Trigger.Shipped);

                var shipment2 = new Shipment("BRZ#71361", "HTL", new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9, 0, 0).AddDays(7), 17000, "1 Collins St, Melbourne, 3000", null);
                shipment2.AddPurchaseOrders(new List<PurchaseOrder>(new[] { shipment2Po1, shipment2Po2 }));
                shipment2.UpdateStatus(ShipmentStatus.Trigger.Shipped);

                foreach (var s in new[] { shipment1, shipment2 })
                {
                    context.Shipment.Add(s);
                }

                context.SaveChanges();
            }
        }
    }
}
