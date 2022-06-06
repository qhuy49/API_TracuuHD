﻿using System;
using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.Data.Domain
{
    public class inv_customer_banned
    {
        [Key]
        public Guid inv_customer_banned_id { get; set; }
        public string mst { get; set; }
        public string user_new { get; set; }
        public DateTime? date_new { get; set; }
        public string user_edit { get; set; }
        public DateTime? date_edit { get; set; }
        public string type { get; set; }
        public string link_web { get; set; }

        public bool? is_unblock { get; set; }
    }
}