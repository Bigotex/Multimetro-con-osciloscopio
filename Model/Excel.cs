using Microsoft.VisualBasic;
using OfficeOpenXml;
using System.Collections;

namespace Multimetro1_0_2.Model
{
    internal class Excel
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filename"></param>
        /// <returns> falso: cuando ocurrió un error al crear la tabla Verdadero:Creación de tabla exitosa</returns>
        public static bool CreateTable(dynamic data, string filename,string path)
        {
            if (data is not IEnumerable)
            {
                return false;
            }
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
                string filePath = Path.Combine(path, filename + ".xlsx");
                FileInfo fi = new FileInfo(filePath);
                package.SaveAs(fi);
                return true;
            }



        }





    }
}
