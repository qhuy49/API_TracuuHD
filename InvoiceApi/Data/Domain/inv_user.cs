using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InvoiceApi.Data.Domain
{
    public class inv_user
    {
        [Key]
        [Column("inv_user_id")]
        public Guid inv_user_id { get; set; }
        public string mst { get; set; }
        public string ma_dt { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        //public DateTime dateNew { get; set; }
        //public DateTime dateEdit { get; set; }
    }


    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã số thuế")]
        [Display(Name = "mst")]
        // [EmailAddress]
        public string mst { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        [Display(Name = "username")]
        // [EmailAddress]
        public string username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
