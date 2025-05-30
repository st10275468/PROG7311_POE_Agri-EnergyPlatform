﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Agri_Energy_Connect_Platform.Models
{
    public class Product
    {
        [Key]
        public int productID { get; set; }

        [Required]
        public string productName { get; set; }

        [Required]
        public string category {  get; set; }

        [Required]
        public string description { get; set; }

       [Required]
        public double price { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        public string? ImageUrl { get; set; }

        [ForeignKey("Farmer")]
        public int userID {  get; set; }

       public User? Farmer { get; set; }


    }
}
