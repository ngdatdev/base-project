using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper;


//public static class ExcelUtil
//{
//    public static byte[] CreateExcelFromList<T>(List<T> data, string sheetName = "Sheet1")
//    {
//        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//        using var package = new ExcelPackage();
//        var sheet = package.Workbook.Worksheets.Add(sheetName);
//        sheet.Cells[1, 1].LoadFromCollection(data, true);
//        return package.GetAsByteArray();
//    }
//}
