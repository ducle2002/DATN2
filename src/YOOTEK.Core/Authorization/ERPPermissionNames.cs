using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOOTEK.Authorization
{
    public static class ERPPermissionNames
    {
        public const string ERP = "ERP";
        #region Cấu hình hệ thống 
        public const string ERP_Admin = "ERP.Admin";
        public const string ERP_Account = "ERP.Account";
        public const string ERP_Admin_Store = "ERP.Admin.Store";
        public const string ERP_Admin_Branch = "ERP.Admin.Branch";
        #endregion

        #region Hóa đơn
        public const string ERP_Invoice = "ERP.Invoice";
        public const string ERP_Invoice_Order = "ERP.Invoice.Order";
        public const string ERP_Invoice_Pay = "ERP.Invoice.Pay";
        #endregion

        #region Mặt hàng
        public const string ERP_Items = "ERP.Items";
        public const string ERP_Items_List = "ERP.Items.List";
        public const string ERP_Items_Menu = "ERP.Items.Menu";
        public const string ERP_Items_Categories = "ERP.Items.Categories";
        public const string ERP_Items_Combo = "ERP.Items.Combo";
        public const string ERP_Items_Group = "ERP.Items.Group";
        #endregion

        #region Dịch vụ
        public const string ERP_Services = "ERP.Services";
        public const string ERP_Services_ServiceTypes = "ERP.Services.ServiceTypes";
        public const string ERP_Services_ServiceCatergories = "ERP.Services.ServiceCatergories";
        #endregion

        #region Nhân viên
        public const string ERP_Admin_Role = "ERP.Admin.Role";
        public const string ERP_Admin_Staff = "ERP.Admin.Staff";
        #endregion

        #region Khách hàng
        public const string ERP_Customer = "ERP.Customer";
        public const string ERP_Customer_List = "ERP.Customer.List";
        public const string ERP_Customer_Type = "ERP.Customer.Type";
        public const string ERP_Customer_Membership = "ERP.Customer.Membership";
        #endregion

        #region Khuyến mại
        public const string ERP_Promotion = "ERP.Promotion";
        #endregion

        #region Kho hàng
        public const string ERP_Stock_Note = "ERP.Stock.Note";
        public const string ERP_Stock_Note_Receipt = "ERP.Stock.Note.Receipt";
        public const string ERP_Stock_Note_Delivery = "ERP.Stock.Note.Delivery";
        public const string ERP_Stock_Note_Inventory = "ERP.Stock.Note.Inventory";
        #endregion

        #region Sổ quỹ
        public const string ERP_Cash_Books = "ERP.Cash.Books";
        public const string ERP_Cash_Books_Receipt = "ERP.Cash.Books.Receipt";
        public const string ERP_Cash_Books_Receipt_Type = "ERP.Cash.Books.Receipt.Type";
        public const string ERP_Cash_Books_Payment = "ERP.Cash.Books.Payment";
        public const string ERP_Cash_Books_Payment_Type = "ERP.Cash.Books.Payment_Type";
        #endregion

        #region Thiết lập nhà hàng FnB
        public const string ERP_FNB_Setup = "ERP.FNB.Setup";
        public const string ERP_FNB_Setup_Table = "ERP.FNB.Setup.Table";
        public const string ERP_FNB_Setup_Area = "ERP.FNB.Setup.Area";
        public const string ERP_FNB_Setup_Table_Type = "ERP.FNB.Setup.Table.Type";
        public const string ERP_FNB_Setup_Table_Style = "ERP.FNB.Setup.Table.Style";
        #endregion

        #region Bar/ Bếp FnB
        public const string ERP_FNB_Kitchen = "ERP.FNB.Kitchen";
        #endregion
    }
}
