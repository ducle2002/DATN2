using Yootek.Core.Dto;
using Yootek.DataExporting.Excel.NPOI;
using Yootek.Storage;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using static Yootek.Common.Enum.UserFeedbackEnum;

namespace Yootek.Services.ExportData
{
    public interface ICitizenReflectExcelExporter
    {
        FileDto ExportToFile(List<CitizenReflectDto> citizenReflects);
    }
    public class CitizenReflectExcelExporter : NpoiExcelExporterBase, ICitizenReflectExcelExporter
    {
        public CitizenReflectExcelExporter(
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }
        public FileDto ExportToFile(List<CitizenReflectDto> citizenReflects)
        {
            return CreateExcelPackage(
                "CitizenReflect.xlsx",
                excelPackage =>
                {
                    ISheet sheet = excelPackage.CreateSheet("CitizenReflectList");

                    AddHeader(
                        sheet,
                        ("Tên phản ánh"),
                        ("Người phản ánh"),
                        ("Dự án"),
                        ("Tòa nhà"),
                        ("Mã căn hộ"),
                        ("Nội dung phản ánh"),
                        ("Phòng ban tiếp nhận"),
                        ("Người xử lý"),
                        ("Ngày phản ánh"),
                        ("Trạng thái"),
                        ("Thời gian hẹn xử lý"),
                        ("Thời gian hoàn thành"),
                        ("Đánh giá")
                    );
                    AddObjects(
                        sheet, citizenReflects,
                        _ => _.Name,
                        _ => _.FullName,
                        _ => _.UrbanName,
                        _ => _.BuildingName,
                        _ => _.ApartmentCode,
                        _ => _.Data,
                        _ => _.OrganizationUnitName,
                        _ => _.HandlerName,
                        _ => _.CreationTime,
                        _ => _.StateName,
                        _ => _.FinishTime,
                        _ => (_.State == 4 || _.State == 3) ? _.LastModificationTime : null,
                        _ => _.ReportName + '-' + _.ReflectReport
                        );

                    for (int i = 0; i < 15; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
