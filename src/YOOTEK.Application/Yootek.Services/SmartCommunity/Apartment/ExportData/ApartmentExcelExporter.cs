using Yootek.Core.Dto;
using Yootek.DataExporting.Excel.NPOI;
using Yootek.Storage;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Yootek.Services.ExportData
{
    public interface IApartmentExcelExporter
    {
        FileDto ExportToFile(List<ApartmentExportDto> apartments);
    }
    public class ApartmentExcelExporter : NpoiExcelExporterBase, IApartmentExcelExporter
    {
        public ApartmentExcelExporter(
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }
        public FileDto ExportToFile(List<ApartmentExportDto> apartments)
        {
            return CreateExcelPackage(
                "Apartments.xlsx",
                excelPackage =>
                {
                    ISheet sheet = excelPackage.CreateSheet(L("ApartmentList"));
                    AddHeader(
                        sheet,
                        ("Tên mặt bằng"),
                        ("Mã mặt bằng"),
                        ("Tên chủ hộ"),
                        ("Mô tả"),
                        ("Thuộc tòa nhà"),
                        ("Thuộc khu đô thị"),
                        ("Thuộc khối"),
                        ("Thuộc tầng"),
                        ("Diện tích"),
                        ("Trạng thái"),
                        ("Loại mặt bằng"),
                        ("Tỉnh thành"),
                        ("Quận/huyện"),
                        ("Phường/xã"),
                        ("Địa chỉ")
                    );
                    AddObjects(
                        sheet, apartments,
                        _ => _.Name,
                        _ => _.ApartmentCode,
                        _ => _.CustomerName,
                        _ => _.Description,
                        _ => _.BuildingCode,
                        _ => _.UrbanCode,
                        _ => _.BlockName,
                        _ => _.FloorName,
                        _ => _.Area,
                        _ => _.StatusName,
                        _ => _.TypeName,
                        _ => _.ProvinceName,
                        _ => _.DistrictName,
                        _ => _.WardName,
                        _ => _.Address
                        );

                    for (int i = 0; i < 15; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
