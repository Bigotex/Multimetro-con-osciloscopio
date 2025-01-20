using OfficeOpenXml;

namespace Multimetro1_0_2.Model
{
    internal class Excel
    {
        public static void CreateTable(string[,] data, string filename)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                // Crea una hoja de cálculo
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                for (int i = 1; i <= data.GetLength(0); i++)
                {
                    for (int j = 1; j <= data.GetLength(1); j++)
                    {
                        worksheet.Cells[i, j].Value = data[i - 1, j - 1];
                        worksheet.Cells[i, j].Value = data[i - 1, j - 1];
                    }
                }
                string filePath = Path.Combine(FileSystem.AppDataDirectory, filename + ".xlsx");
                FileInfo fi = new FileInfo(filePath);
                package.SaveAs(fi);
            }



        }





    }
}
