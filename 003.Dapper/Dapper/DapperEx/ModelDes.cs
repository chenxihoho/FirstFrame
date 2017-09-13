﻿using System;
using System.Collections.Generic;

namespace FirstFrame.DapperEx
{
    /// <summary>
    /// 转换实体对象描述
    /// </summary>
    internal class ModelDes
    {
        public ModelDes()
        {
            Properties = new List<PropertyDes>();
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Database { get; set; }

        //private string _Owner = "dbo";
        /// <summary>
        /// 所有者
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 对象的所有属性,只包含有映射关系的属性
        /// </summary>
        public IList<PropertyDes> Properties { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Type ClassType { get; set; }
    }

    /// <summary>
    /// 转换实体属性描述,只包含有映射关系的属性
    /// </summary>
    internal class PropertyDes
    {
        /// <summary>
        /// 表列名
        /// </summary>
        public string Column
        {
            get
            {
                return Field;
            }
        }

        /// <summary>
        /// 属性字段名
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 映射特性
        /// </summary>
        public BaseAttribute CusAttribute { get; set; }
    }
}
