using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using FA_Accounting.Common;
namespace RecloserAcq
{
    /*
     * Ghi chu PermX 
     * perm8 : 0,1,2 0: khong cho xem nhom 1 (trong Table DeviceGroup: GroupDevName = "Nhom 1", GroupId = 1) , 1 cho xem ,2 
     * perm9: 0,1,2 0: khong cho xem nhom 2 (trong Table DeviceGroup: GroupDevName = "Nhom 1", GroupId = 1) , 1 cho xem ,2 
     * ....
     * perm26: nhom 17
     */
    public enum ePermission
    {
        Perm1 = 0,
        Perm2 = 1,
        Perm3 = 2,
    }
    public class UserObj
    {
        public int id;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        private string _name;
        public string fullname;
        public string role;
        /// <summary>
        /// "Đồng bộ thời gian"
        /// </summary>
        public UInt16 permsyntime;
        /// <summary>
        /// "Xem lịch sử"
        /// </summary>
        public UInt16 permseeHistory;
        /// <summary>
        /// "Đóng Recloser"
        /// </summary>

        public UInt16 permCloseRecloser;
        /// <summary>
        /// "Mở Recloser"
        /// </summary>
        public UInt16 permopenRecloser;
        /// <summary>
        /// "Đóng mở tụ bù"
        /// </summary>
            public UInt16 permoperateTubu;
        /// <summary>
        /// "Quản lý người dùng"
        /// </summary>
         public UInt16 permUser;
        /// <summary>
        /// "Cấu hình thiết bị"
        /// </summary>
        public UInt16 permConfigCommon;
        public UInt16 permNhom1;
        public UInt16 permNhom2;
        public UInt16 permNhom3;
        public UInt16 permNhom4;
        public UInt16 permNhom5;
        public UInt16 permNhom6;
        public UInt16 permNhom7;
        public UInt16 permNhom8;
        public UInt16 permNhom9;
        public UInt16 permNhom10;
        public UInt16 permNhom11;
        public UInt16 permNhom12;
        public UInt16 permNhom13;
        public UInt16 permNhom14;
        public UInt16 permNhom15;
        public UInt16 permNhom16;
        public UInt16 permNhom17;
        public UInt16 permNhom18;
        public UInt16 permNhom19;
        

        public UInt16 permConfigDatabase;
        //0  Không cho phép
        // 1 Chỉ đọc
        // 2 Đầy đủ quyền (Edit, Insert , Delete
        // 3 Chỉ Edit (không delete, không insert)
        // 4 Edit, Insert , không delete
        public string computer;
        public string password;
        public DateTime lastlogin;
        public string RoleName
        {
            get { return _rolename; }
            set { _rolename = value; }
        }
        private string _rolename;
        public int RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }
        private int _roleId = 0;
        public static bool IsUserInPermission(ePermission permission)
        {
            
            //return (user != null) ? user.UserPermissions.IndexOf(permission.GetHashCode().ToString()) >= 0 : false;
            return true;
        }
    }
}
