using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FirstFrame.DapperEx
{
    /// <summary>
    /// Common
    /// </summary>
    public static class DapperCommon
    {
        private static object objLock = new object();

        /// <summary>
        /// 用于缓存对象转换实体
        /// </summary>
        private static Dictionary<string, ModelDes> _ModelDesCache = new Dictionary<string, ModelDes>();

        /// <summary>
        /// 确定是否已经存在缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static ModelDes ExistModelDesCache(string key)
        {
            return null;

            //禁用缓存
            //ModelDes value;
            //_ModelDesCache.TryGetValue(key, out value);
            //return value;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="des"></param>
        private static void Add(string key, ModelDes des)
        {
            lock (objLock)
            {
                if ((!_ModelDesCache.ContainsKey(key)) && des != null)
                {
                    _ModelDesCache.Add(key, des);
                }
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static ModelDes GetModelDesCache(string key)
        {
            ModelDes value;
            _ModelDesCache.TryGetValue(key, out value);
            if (value != null) return value;
            throw new Exception("缓存中没存在此数据");
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static ModelDes UpdateModelDesCache<T>(T t)
        {
            var type = typeof(T);
            var cacheValue = ExistModelDesCache(type.FullName);
            if (cacheValue == null)
            {
                var model = new ModelDes();
                #region 表描述
                model.ClassType = type;
                model.ClassName = type.Name;
                var AttrObj = type.GetCustomAttributes(true);
                if (AttrObj != null)
                {
                    DatabaseAttribute dbName = null;
                    OwnerAttribute ownerName = null;
                    TableAttribute tableName = null;
                    foreach (var o in AttrObj)
                    {
                        if (dbName == null) dbName = o as DatabaseAttribute;
                        if (ownerName == null) ownerName = o as OwnerAttribute;
                        if (tableName == null) tableName = o as TableAttribute;
                    }
                    if (tableName != null && !string.IsNullOrEmpty(tableName.Name))
                    {
                        string dName = dbName != null ? "[" + dbName.Name + "]" + "." : "";
                        string oName = ownerName != null ? "[" + ownerName.Name + "]" + "." : "";
                        if (!string.IsNullOrEmpty(dName) && string.IsNullOrEmpty(oName)) oName = "[dbo].";
                        string tName = tableName != null ? "[" + tableName.Name + "]" : "";
                        model.TableName = dName + oName + tName;
                    }
                    else
                    {
                        model.TableName = model.ClassName;
                    }
                }
                else
                {
                    model.TableName = model.ClassName;
                }
                #endregion

                #region 属性描述
                foreach (var propertyInfo in type.GetProperties())
                {
                    var pty = new PropertyDes();
                    pty.Field = propertyInfo.Name;
                    var arri = propertyInfo.GetCustomAttributes(typeof(BaseAttribute), true).FirstOrDefault();
                    if (arri is IgnoreAttribute)
                    {
                        continue;
                    }
                    else if (arri is IDAttribute)
                    {
                        pty.CusAttribute = arri as IDAttribute;
                    }
                    else if (arri is ColumnAttribute)
                    {
                        pty.CusAttribute = arri as ColumnAttribute;
                    }

                    if (t != null) //如果传入了实体，则只为有值的字段创建语法模板
                    {
                        if (PropertieHasValue(t, propertyInfo.Name)) model.Properties.Add(pty);
                    }
                    else
                    {
                        model.Properties.Add(pty);
                    }
                }
                #endregion

                Add(type.FullName, model);
                cacheValue = model;
            }
            return cacheValue;
        }
        #region 判断实体中的指定字段是否有值
        private static bool PropertieHasValue<T>(T t, string FieldName)
        {
            var type = typeof(T);
            var cacheValue = ExistModelDesCache(type.FullName);
            if (cacheValue == null)
            {
                var model = new ModelDes();
                #region 表描述
                model.ClassType = type;
                model.ClassName = type.Name;
                var AttrObj = type.GetCustomAttributes(true);
                if (AttrObj != null)
                {
                    DatabaseAttribute dbName = null;
                    OwnerAttribute ownerName = null;
                    TableAttribute tableName = null;
                    foreach (var o in AttrObj)
                    {
                        if (dbName == null) dbName = o as DatabaseAttribute;
                        if (ownerName == null) ownerName = o as OwnerAttribute;
                        if (tableName == null) tableName = o as TableAttribute;
                    }
                    if (tableName != null && !string.IsNullOrEmpty(tableName.Name))
                    {
                        string dName = dbName != null ? "[" + dbName.Name + "]" + "." : "";
                        string oName = ownerName != null ? "[" + ownerName.Name + "]" + "." : "";
                        if (!string.IsNullOrEmpty(dName) && string.IsNullOrEmpty(oName)) oName = "[dbo].";
                        string tName = tableName != null ? "[" + tableName.Name + "]" : "";
                        model.TableName = dName + oName + tName;
                    }
                    else
                    {
                        model.TableName = model.ClassName;
                    }
                }
                else
                {
                    model.TableName = model.ClassName;
                }
                #endregion

                #region 属性描述
                foreach (var propertyInfo in type.GetProperties())
                {
                    var pty = new PropertyDes();
                    pty.Field = propertyInfo.Name;
                    var arri = propertyInfo.GetCustomAttributes(typeof(BaseAttribute), true).FirstOrDefault();
                    if (arri is IgnoreAttribute)
                    {
                        continue;
                    }
                    else if (arri is IDAttribute)
                    {
                        pty.CusAttribute = arri as IDAttribute;
                    }
                    else if (arri is ColumnAttribute)
                    {
                        pty.CusAttribute = arri as ColumnAttribute;
                    }

                    model.Properties.Add(pty);
                }
                #endregion

                Add(type.FullName, model);
                cacheValue = model;
            }

            foreach (var item in cacheValue.Properties)
            {
                if (item.Field != FieldName) continue;

                var value = cacheValue.ClassType.GetProperty(item.Field).GetValue(t, null);
                if (value == null) return false;
                if (string.IsNullOrEmpty(value.ToString())) return false;

                return true;
            }
            return false;
        }
        #endregion
        /// <summary>
        /// 获取转换实体对象描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static ModelDes GetModelDes<T>(T t)
        {
            return UpdateModelDesCache<T>(t);
        }

        /// <summary>
        /// 获取要执行SQL时的列,添加和修改数据时
        /// </summary>
        /// <param name="des"></param>
        /// <param name="add">是否是添加</param>
        /// <returns></returns>
        internal static IList<ParamColumnModel> GetExecColumns(ModelDes des, bool add = true)
        {
            var columns = new List<ParamColumnModel>();
            if (des != null && des.Properties != null)
            {
                foreach (var item in des.Properties)
                {
                    if ((!add) && item.CusAttribute is IDAttribute)
                    {
                        continue;
                    }
                    else if ((item.CusAttribute is IDAttribute) && ((item.CusAttribute as IDAttribute).CheckAutoId))
                    {
                        continue;
                    }
                    else if ((item.CusAttribute is ColumnAttribute) && ((item.CusAttribute as ColumnAttribute).AutoIncrement))
                    {
                        continue;
                    }
                    columns.Add(new ParamColumnModel() { ColumnName = item.Column, FieldName = item.Field });
                }
            }
            return columns;
        }

        /// <summary>
        /// 获取对象的主键标识列和属性
        /// </summary>
        /// <param name="des"></param>
        /// <returns></returns>
        internal static IList<PropertyDes> GetPrimary(ModelDes des)
        {
            //var p = des.Properties.Where(m => m.CusAttribute is IDAttribute).FirstOrDefault();
            var p = des.Properties.Where(m => m.CusAttribute is IDAttribute).ToList();
            if (p == null)
            {
                throw new Exception("没有任何列标记为主键特性");
            }
            //return p as PropertyDes;
            return p;
        }

        /// <summary>
        /// 通过表达式树获取属性名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="des"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        internal static PropertyDes GetPropertyByExpress<T>(ModelDes des, Expression<Func<T, object>> expr) where T : class
        {
            var pname = "";
            if (expr.Body is UnaryExpression)
            {
                var uy = expr.Body as UnaryExpression;
                pname = (uy.Operand as MemberExpression).Member.Name;
            }
            else
            {
                pname = (expr.Body as MemberExpression).Member.Name;
            }
            var p = des.Properties.Where(m => m.Column == pname).FirstOrDefault();
            if (p == null)
            {
                throw new Exception(string.Format("{0}不是映射列，不能进行SQL处理", pname));
            }
            return p;
        }

    }
}
