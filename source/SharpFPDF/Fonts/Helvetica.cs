﻿using System.Text;

namespace SharpFPDF.Fonts
{
    public class Helvetica : PdfFont
    {
        public Helvetica()
        {
            Type = "Core";
            Name = "Helvetica";
            UnderlinePosition = -100;
            UnderlineThickness = 50;
            Encoding = Encoding.GetEncoding(1252); // cp1252
            
            #region CharacterWidths
            CharacterWidths.Add((char)0, 278);
            CharacterWidths.Add((char)1, 278);
            CharacterWidths.Add((char)2, 278);
            CharacterWidths.Add((char)3, 278);
            CharacterWidths.Add((char)4, 278);
            CharacterWidths.Add((char)5, 278);
            CharacterWidths.Add((char)6, 278);
            CharacterWidths.Add((char)7, 278);
            CharacterWidths.Add((char)8, 278);
            CharacterWidths.Add((char)9, 278);
            CharacterWidths.Add((char)10, 278);
            CharacterWidths.Add((char)11, 278);
            CharacterWidths.Add((char)12, 278);
            CharacterWidths.Add((char)13, 278);
            CharacterWidths.Add((char)14, 278);
            CharacterWidths.Add((char)15, 278);
            CharacterWidths.Add((char)16, 278);
            CharacterWidths.Add((char)17, 278);
            CharacterWidths.Add((char)18, 278);
            CharacterWidths.Add((char)19, 278);
            CharacterWidths.Add((char)20, 278);
            CharacterWidths.Add((char)21, 278);
            CharacterWidths.Add((char)22, 278);
            CharacterWidths.Add((char)23, 278);
            CharacterWidths.Add((char)24, 278);
            CharacterWidths.Add((char)25, 278);
            CharacterWidths.Add((char)26, 278);
            CharacterWidths.Add((char)27, 278);
            CharacterWidths.Add((char)28, 278);
            CharacterWidths.Add((char)29, 278);
            CharacterWidths.Add((char)30, 278);
            CharacterWidths.Add((char)31, 278);
            CharacterWidths.Add((char)32, 278);
            CharacterWidths.Add((char)33, 278);
            CharacterWidths.Add((char)34, 355);
            CharacterWidths.Add((char)35, 556);
            CharacterWidths.Add((char)36, 556);
            CharacterWidths.Add((char)37, 889);
            CharacterWidths.Add((char)38, 667);
            CharacterWidths.Add((char)39, 191);
            CharacterWidths.Add((char)40, 333);
            CharacterWidths.Add((char)41, 333);
            CharacterWidths.Add((char)42, 389);
            CharacterWidths.Add((char)43, 584);
            CharacterWidths.Add((char)44, 278);
            CharacterWidths.Add((char)45, 333);
            CharacterWidths.Add((char)46, 278);
            CharacterWidths.Add((char)47, 278);
            CharacterWidths.Add((char)48, 556);
            CharacterWidths.Add((char)49, 556);
            CharacterWidths.Add((char)50, 556);
            CharacterWidths.Add((char)51, 556);
            CharacterWidths.Add((char)52, 556);
            CharacterWidths.Add((char)53, 556);
            CharacterWidths.Add((char)54, 556);
            CharacterWidths.Add((char)55, 556);
            CharacterWidths.Add((char)56, 556);
            CharacterWidths.Add((char)57, 556);
            CharacterWidths.Add((char)58, 278);
            CharacterWidths.Add((char)59, 278);
            CharacterWidths.Add((char)60, 584);
            CharacterWidths.Add((char)61, 584);
            CharacterWidths.Add((char)62, 584);
            CharacterWidths.Add((char)63, 556);
            CharacterWidths.Add((char)64, 1015);
            CharacterWidths.Add((char)65, 667);
            CharacterWidths.Add((char)66, 667);
            CharacterWidths.Add((char)67, 722);
            CharacterWidths.Add((char)68, 722);
            CharacterWidths.Add((char)69, 667);
            CharacterWidths.Add((char)70, 611);
            CharacterWidths.Add((char)71, 778);
            CharacterWidths.Add((char)72, 722);
            CharacterWidths.Add((char)73, 278);
            CharacterWidths.Add((char)74, 500);
            CharacterWidths.Add((char)75, 667);
            CharacterWidths.Add((char)76, 556);
            CharacterWidths.Add((char)77, 833);
            CharacterWidths.Add((char)78, 722);
            CharacterWidths.Add((char)79, 778);
            CharacterWidths.Add((char)80, 667);
            CharacterWidths.Add((char)81, 778);
            CharacterWidths.Add((char)82, 722);
            CharacterWidths.Add((char)83, 667);
            CharacterWidths.Add((char)84, 611);
            CharacterWidths.Add((char)85, 722);
            CharacterWidths.Add((char)86, 667);
            CharacterWidths.Add((char)87, 944);
            CharacterWidths.Add((char)88, 667);
            CharacterWidths.Add((char)89, 667);
            CharacterWidths.Add((char)90, 611);
            CharacterWidths.Add((char)91, 278);
            CharacterWidths.Add((char)92, 278);
            CharacterWidths.Add((char)93, 278);
            CharacterWidths.Add((char)94, 469);
            CharacterWidths.Add((char)95, 556);
            CharacterWidths.Add((char)96, 333);
            CharacterWidths.Add((char)97, 556);
            CharacterWidths.Add((char)98, 556);
            CharacterWidths.Add((char)99, 500);
            CharacterWidths.Add((char)100, 556);
            CharacterWidths.Add((char)101, 556);
            CharacterWidths.Add((char)102, 278);
            CharacterWidths.Add((char)103, 556);
            CharacterWidths.Add((char)104, 556);
            CharacterWidths.Add((char)105, 222);
            CharacterWidths.Add((char)106, 222);
            CharacterWidths.Add((char)107, 500);
            CharacterWidths.Add((char)108, 222);
            CharacterWidths.Add((char)109, 833);
            CharacterWidths.Add((char)110, 556);
            CharacterWidths.Add((char)111, 556);
            CharacterWidths.Add((char)112, 556);
            CharacterWidths.Add((char)113, 556);
            CharacterWidths.Add((char)114, 333);
            CharacterWidths.Add((char)115, 500);
            CharacterWidths.Add((char)116, 278);
            CharacterWidths.Add((char)117, 556);
            CharacterWidths.Add((char)118, 500);
            CharacterWidths.Add((char)119, 722);
            CharacterWidths.Add((char)120, 500);
            CharacterWidths.Add((char)121, 500);
            CharacterWidths.Add((char)122, 500);
            CharacterWidths.Add((char)123, 334);
            CharacterWidths.Add((char)124, 260);
            CharacterWidths.Add((char)125, 334);
            CharacterWidths.Add((char)126, 584);
            CharacterWidths.Add((char)127, 350);
            CharacterWidths.Add((char)128, 556);
            CharacterWidths.Add((char)129, 350);
            CharacterWidths.Add((char)130, 222);
            CharacterWidths.Add((char)131, 556);
            CharacterWidths.Add((char)132, 333);
            CharacterWidths.Add((char)133, 1000);
            CharacterWidths.Add((char)134, 556);
            CharacterWidths.Add((char)135, 556);
            CharacterWidths.Add((char)136, 333);
            CharacterWidths.Add((char)137, 1000);
            CharacterWidths.Add((char)138, 667);
            CharacterWidths.Add((char)139, 333);
            CharacterWidths.Add((char)140, 1000);
            CharacterWidths.Add((char)141, 350);
            CharacterWidths.Add((char)142, 611);
            CharacterWidths.Add((char)143, 350);
            CharacterWidths.Add((char)144, 350);
            CharacterWidths.Add((char)145, 222);
            CharacterWidths.Add((char)146, 222);
            CharacterWidths.Add((char)147, 333);
            CharacterWidths.Add((char)148, 333);
            CharacterWidths.Add((char)149, 350);
            CharacterWidths.Add((char)150, 556);
            CharacterWidths.Add((char)151, 1000);
            CharacterWidths.Add((char)152, 333);
            CharacterWidths.Add((char)153, 1000);
            CharacterWidths.Add((char)154, 500);
            CharacterWidths.Add((char)155, 333);
            CharacterWidths.Add((char)156, 944);
            CharacterWidths.Add((char)157, 350);
            CharacterWidths.Add((char)158, 500);
            CharacterWidths.Add((char)159, 667);
            CharacterWidths.Add((char)160, 278);
            CharacterWidths.Add((char)161, 333);
            CharacterWidths.Add((char)162, 556);
            CharacterWidths.Add((char)163, 556);
            CharacterWidths.Add((char)164, 556);
            CharacterWidths.Add((char)165, 556);
            CharacterWidths.Add((char)166, 260);
            CharacterWidths.Add((char)167, 556);
            CharacterWidths.Add((char)168, 333);
            CharacterWidths.Add((char)169, 737);
            CharacterWidths.Add((char)170, 370);
            CharacterWidths.Add((char)171, 556);
            CharacterWidths.Add((char)172, 584);
            CharacterWidths.Add((char)173, 333);
            CharacterWidths.Add((char)174, 737);
            CharacterWidths.Add((char)175, 333);
            CharacterWidths.Add((char)176, 400);
            CharacterWidths.Add((char)177, 584);
            CharacterWidths.Add((char)178, 333);
            CharacterWidths.Add((char)179, 333);
            CharacterWidths.Add((char)180, 333);
            CharacterWidths.Add((char)181, 556);
            CharacterWidths.Add((char)182, 537);
            CharacterWidths.Add((char)183, 278);
            CharacterWidths.Add((char)184, 333);
            CharacterWidths.Add((char)185, 333);
            CharacterWidths.Add((char)186, 365);
            CharacterWidths.Add((char)187, 556);
            CharacterWidths.Add((char)188, 834);
            CharacterWidths.Add((char)189, 834);
            CharacterWidths.Add((char)190, 834);
            CharacterWidths.Add((char)191, 611);
            CharacterWidths.Add((char)192, 667);
            CharacterWidths.Add((char)193, 667);
            CharacterWidths.Add((char)194, 667);
            CharacterWidths.Add((char)195, 667);
            CharacterWidths.Add((char)196, 667);
            CharacterWidths.Add((char)197, 667);
            CharacterWidths.Add((char)198, 1000);
            CharacterWidths.Add((char)199, 722);
            CharacterWidths.Add((char)200, 667);
            CharacterWidths.Add((char)201, 667);
            CharacterWidths.Add((char)202, 667);
            CharacterWidths.Add((char)203, 667);
            CharacterWidths.Add((char)204, 278);
            CharacterWidths.Add((char)205, 278);
            CharacterWidths.Add((char)206, 278);
            CharacterWidths.Add((char)207, 278);
            CharacterWidths.Add((char)208, 722);
            CharacterWidths.Add((char)209, 722);
            CharacterWidths.Add((char)210, 778);
            CharacterWidths.Add((char)211, 778);
            CharacterWidths.Add((char)212, 778);
            CharacterWidths.Add((char)213, 778);
            CharacterWidths.Add((char)214, 778);
            CharacterWidths.Add((char)215, 584);
            CharacterWidths.Add((char)216, 778);
            CharacterWidths.Add((char)217, 722);
            CharacterWidths.Add((char)218, 722);
            CharacterWidths.Add((char)219, 722);
            CharacterWidths.Add((char)220, 722);
            CharacterWidths.Add((char)221, 667);
            CharacterWidths.Add((char)222, 667);
            CharacterWidths.Add((char)223, 611);
            CharacterWidths.Add((char)224, 556);
            CharacterWidths.Add((char)225, 556);
            CharacterWidths.Add((char)226, 556);
            CharacterWidths.Add((char)227, 556);
            CharacterWidths.Add((char)228, 556);
            CharacterWidths.Add((char)229, 556);
            CharacterWidths.Add((char)230, 889);
            CharacterWidths.Add((char)231, 500);
            CharacterWidths.Add((char)232, 556);
            CharacterWidths.Add((char)233, 556);
            CharacterWidths.Add((char)234, 556);
            CharacterWidths.Add((char)235, 556);
            CharacterWidths.Add((char)236, 278);
            CharacterWidths.Add((char)237, 278);
            CharacterWidths.Add((char)238, 278);
            CharacterWidths.Add((char)239, 278);
            CharacterWidths.Add((char)240, 556);
            CharacterWidths.Add((char)241, 556);
            CharacterWidths.Add((char)242, 556);
            CharacterWidths.Add((char)243, 556);
            CharacterWidths.Add((char)244, 556);
            CharacterWidths.Add((char)245, 556);
            CharacterWidths.Add((char)246, 556);
            CharacterWidths.Add((char)247, 584);
            CharacterWidths.Add((char)248, 611);
            CharacterWidths.Add((char)249, 556);
            CharacterWidths.Add((char)250, 556);
            CharacterWidths.Add((char)251, 556);
            CharacterWidths.Add((char)252, 556);
            CharacterWidths.Add((char)253, 500);
            CharacterWidths.Add((char)254, 556);
            CharacterWidths.Add((char)255, 500);
            #endregion

            #region Uw
            Uw.Add(0, new int[] { 0, 128 });
            Uw.Add(128, new int[] { 8364 });
            Uw.Add(130, new int[] { 8218 });
            Uw.Add(131, new int[] { 402 });
            Uw.Add(132, new int[] { 8222 });
            Uw.Add(133, new int[] { 8230 });
            Uw.Add(134, new int[] { 8224, 2 });
            Uw.Add(136, new int[] { 710 });
            Uw.Add(137, new int[] { 8240 });
            Uw.Add(138, new int[] { 352 });
            Uw.Add(139, new int[] { 8249 });
            Uw.Add(140, new int[] { 338 });
            Uw.Add(142, new int[] { 381 });
            Uw.Add(145, new int[] { 8216, 2 });
            Uw.Add(147, new int[] { 8220, 2 });
            Uw.Add(149, new int[] { 8226 });
            Uw.Add(150, new int[] { 8211, 2 });
            Uw.Add(152, new int[] { 732 });
            Uw.Add(153, new int[] { 8482 });
            Uw.Add(154, new int[] { 353 });
            Uw.Add(155, new int[] { 8250 });
            Uw.Add(156, new int[] { 339 });
            Uw.Add(158, new int[] { 382 });
            Uw.Add(159, new int[] { 376 });
            Uw.Add(160, new int[] { 160, 96 });
            #endregion
        }
    }
}