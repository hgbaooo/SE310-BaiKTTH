﻿namespace SE310.P12_BaiKTTH_BE.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string PassWord { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
}