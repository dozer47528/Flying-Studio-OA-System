using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL
{
    [Serializable]
    public class UserRole
    {
        public int ID { get; set; }
        public UserRole FatherRole { get; set; }//上级角色
        public string Name { get; set; }//角色名
        public int RoleEnum { get; set; }//角色枚举
        public string Describe { get; set; }//描述
    }

/// <summary>
/// 角色枚举
/// </summary>
public enum UserRoleEnum
{
    执行站长 = 1,//1
    办公组组长 = 1 << 1,//10
    技术组组长 = 1 << 2,//100
    运营组组长 = 1 << 3,//1000
    组长 = 办公组组长 | 技术组组长 | 运营组组长,//1110
    组长和站长 = 组长 | 执行站长,//1111
    办公组成员 = 1 << 4,//10000
    技术组成员 = 1 << 5,//100000
    运营组成员 = 1 << 6,//1000000
    成员 = 办公组成员 | 技术组成员 | 运营组成员,//1110000
    全员 = 执行站长 | 组长 | 成员//1111111
}
}
