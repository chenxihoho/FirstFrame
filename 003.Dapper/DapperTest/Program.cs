using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using FirstFrame.DapperTest;
using FirstFrame.DapperEx;
using Dapper;
using Newtonsoft.Json;
using FirstFrame.Helper.Log;

namespace DapperTest
{
    class Program
    {
        private static readonly DbBase _DbBase = new DbBase(new DBTools(), (int)DbIndex.DB_MALL);
        public const string ConnectionString = "Data Source=.;Initial Catalog=tempdb;Integrated Security=True";
        static readonly SqlConnection Connection = GetOpenConnection();
        public static string ConnectionName = "ConnString";
        private static object _objLock = new object();
        private static Dictionary<string, Type> _dynamicParamModelCache = new Dictionary<string, Type>();
        private static IList<DynamicPropertyModel> _param = new List<DynamicPropertyModel>();
        private static Dictionary<string, object> _paramValues = new Dictionary<string, object>();

        public static DbBase CreateDbBase()
        {
            return new DbBase(new DBTools(), 0);
        }
        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            //connection.Open();
            return connection;
        }
        #region 测试1
        public static string Account()
        {
            try
            {
                Account info = new Account();
                info.Name = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                info.Password = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                info.Email = "11@sohu.com";
                info.CreateTime = DateTime.Now;
                info.Age = 100;
                info.Flag = 1;
                _DbBase.Insert<Account>(info);


                List<Receipt> aList = new List<Receipt>();
                for (int i = 0; i < 1; i++)
                {
                    Receipt _Receipt = new Receipt()
                    {
                        PlatformID = "210110",
                        ReceiptID = "123",
                        PayType = string.Empty,
                        CreateTime = DateTime.Parse("1900-01-01")
                    };
                    aList.Add(_Receipt);
                }
                _DbBase.InsertBatch<Receipt>(aList);

                IDbTransaction trans = _DbBase.BeginTransaction(0);
                _DbBase.InsertBatch<Receipt>(aList, trans);
                _DbBase.Delete<Account>(SqlQuery<Account>.Builder(_DbBase).AndWhere(m => m.ID, OperationMethod.Equal, 1), trans);
                for (int i = 0; i < 1; i++)
                {
                    IDbTransaction trans1 = _DbBase.BeginTransaction(0);
                    _DbBase.Insert<Account>(info, trans1);
                    _DbBase.Commit(trans1);

                    _DbBase.Count<Account>(null, trans);
                    _DbBase.Max<Account>("id", null, trans);
                    _DbBase.Query<Account>(SqlQuery<Account>.Builder(_DbBase).AndWhere(m => m.ID, OperationMethod.Equal, i), trans);
                    _DbBase.Delete<Account>(SqlQuery<Account>.Builder(_DbBase).AndWhere(m => m.ID, OperationMethod.Equal, i), trans);
                }
                _DbBase.Commit(trans);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Account：{0}", ex.Message);
            }

            return string.Empty;
        }
        #endregion
        private static void AccountEx()
        {
            for (int i = 0; i < 5000000; i++)
            {
                Console.WriteLine("{0} ConnectionPoolSize： {1} {2} Thread Id : {3}", DateTime.Now, _DbBase.ConnectionPool.Count, _DbBase.GetHashCode(), Thread.CurrentThread.ManagedThreadId);
                Account();
                Thread.Sleep(100);
            }
        }
        #region 测试2 WriteGoodsTest
        private static void WriteGoodsTest()
        {
            try
            {
                IDbTransaction Trans = _DbBase.BeginTransaction();
                List<Goods> aList = new List<Goods>();
                for (int i = 0; i < 10; i++)
                {
                    Goods _Goods = new Goods()
                    {
                        GoodsID = Guid.NewGuid().ToString("N"),
                        yhProductId = string.Empty,
                        ProductCategoryId = "1",
                        SellerCategoryId = "1",
                        PlatformName = "test",
                        GoodsTitle = "test",
                        GoodsDescript = "test",
                        GoodsProperty = "",
                        GoodsImages = "",
                        GoodsNumber = 1,
                        SalesVolume = 1,
                        Supplier = "test",
                        OldPrice = 1,
                        GoodPrice = 1,
                        SalePrice = 1,
                        WholesalePrice = 1,
                        GoodsOuterId = "test",
                        ListTime = DateTime.Now,
                        //DelistTime = DateTime.Now,
                        StuffStatus = 100,
                        Province = "",
                        City = "",
                        PostageTempletID = "",
                        //ShelfNumber = "%7b%22ShelfNumber%22%3a%5b%7b%22GoodsAmount%22%3a%2297%22%2c%22Location%22%3a%22%e4%b8%80%e6%a5%bc%22%2c%22Name%22%3a%22%e7%94%b5%e5%95%86%e6%9e%b6%22%2c%22Number%22%3a%22ds00000001%22%7d%5d%7d",
                        ShelfNumber = "{\"ShelfNumber\":[{\"GoodsAmount\":\"97\",\"Location\":\"一楼\",\"Name\":\"电商架\",\"Number\":\"ds00000001\"}]}",
                        FreightPayer = 1,
                        HasInvoice = true,
                        HasWarranty = true,
                        ApproveStatus = 1,
                        Violation = true,
                        Visible = true,
                        Borrow = true,
                        Recommend = true,
                        PlatformID = "test"
                    };
                    aList.Add(_Goods);
                }
                _DbBase.InsertBatch<Goods>(aList, Trans);

                /*Goods _Goods = new Goods()
                {
                    GoodsID = Guid.NewGuid().ToString("N"),
                    yhProductId = string.Empty,
                    ProductCategoryId = "1",
                    SellerCategoryId = "1",
                    PlatformName = "test",
                    GoodsTitle = "test",
                    GoodsDescript = "test",
                    GoodsProperty = "",
                    GoodsImages = "",
                    GoodsNumber = 1,
                    SalesVolume = 1,
                    Supplier = "test",
                    OldPrice = 1,
                    GoodPrice = 1,
                    SalePrice = 1,
                    WholesalePrice = 1,
                    GoodsOuterId = "test",
                    ListTime = DateTime.Now,
                    //DelistTime = DateTime.Now,
                    StuffStatus = 100,
                    Province = "",
                    City = "",
                    PostageTempletID = "",
                    //ShelfNumber = "%7b%22ShelfNumber%22%3a%5b%7b%22GoodsAmount%22%3a%2297%22%2c%22Location%22%3a%22%e4%b8%80%e6%a5%bc%22%2c%22Name%22%3a%22%e7%94%b5%e5%95%86%e6%9e%b6%22%2c%22Number%22%3a%22ds00000001%22%7d%5d%7d",
                    ShelfNumber = "{\"ShelfNumber\":[{\"GoodsAmount\":\"97\",\"Location\":\"一楼\",\"Name\":\"电商架\",\"Number\":\"ds00000001\"}]}",
                    FreightPayer = 1,
                    HasInvoice = true,
                    HasWarranty = true,
                    ApproveStatus = 1,
                    Violation = true,
                    Visible = true,
                    Borrow = true,
                    Recommend = true,
                    PlatformID = "test"
                };
                string GoodsJson = JsonConvert.SerializeObject(_Goods);
                _Goods = JsonConvert.DeserializeObject<Goods>(GoodsJson);
                _DbBase.Insert<Goods>(_Goods);*/
            }
            catch(Exception e)
            {
                LogHelper.WriteString("WriteGoodsTest ", ref e);
            }
        }
        private static void LoadGoodsTest()
        {
            try
            {
                Goods _Goods = _DbBase.SingleOrDefault<Goods>(SqlQuery<Goods>.Builder(_DbBase).AndWhere(m => m.PlatformID, OperationMethod.Equal, "test")
                                                                                              .AndWhere(m => m.GoodsID, OperationMethod.Equal, "7fc1ffe9ac684bd6bf398d870c1f8185"));
                string GoodsJson = JsonConvert.SerializeObject(_Goods);
            }
            catch (Exception e)
            {
                LogHelper.WriteString("LoadGoodsTest ", ref e);
            }
        }
        #endregion
        #region UpdateReceipt
        private static void UpdateReceipt()
        {
            try
            {
                IDbTransaction Trans = _DbBase.BeginTransaction();

                Receipt _Receipt = _DbBase.SingleOrDefault<Receipt>(SqlQuery<Receipt>.Builder(_DbBase).AndWhere(m => m.PlatformID, OperationMethod.Equal, "B67C2C23685746C5BA101236BAC83DF4")
                                                                                                      .AndWhere(m => m.ReceiptID, OperationMethod.Equal, "6260986529376636928"));
                _Receipt.Processed = true;
                _Receipt.Data = DateTime.Now.ToString();
                //_Receipt.ProcessedTime = DateTime.Now;
                //_Receipt.CallbackTime = DateTime.Now;

                _DbBase.Update<Receipt>(_Receipt, null, Trans);
                _DbBase.Commit(Trans);
            }
            catch (Exception e)
            {
                LogHelper.WriteString("UpdateReceipt ", ref e);
            }
        }
        #endregion
        static void Main(string[] args)
        {
            for (int i = 0; i < 1; i++)
            {
                //Thread WorkThread = new Thread(new ThreadStart(WriteGoodsTest));
                //Thread WorkThread = new Thread(new ThreadStart(LoadGoodsTest));
                Thread WorkThread = new Thread(new ThreadStart(UpdateReceipt));
                WorkThread.Start();
            }
            //ProduceItemsInfo p = new ProduceItemsInfo();
            //p.iItemID = 1212121;
            //p.vcGeneralName = "【达纳康】银杏叶片";
            //p.vcApproval = "注册证号H20130307 123";
            //p.iPharmaFomulationID = 2;
            ////dbProduits.Insert<ProduceItemsInfo>(p);
            ////dbProduits.Update<ProduceItemsInfo>(p);
            //var list = dbProduits.Query<ProduceItemsInfo>().ToList();

            Console.ReadLine();
            //ICache cache = CacheManager.GetCache();
            //cache.Set("test", "1", new TimeSpan(1));

            //CreateDbBase();

            //int maxid = dbPower.Max<Account>("id");

            //try
            //{
            //    TransactionOptions options = new TransactionOptions();
            //    options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            //    options.Timeout = new TimeSpan(0, 2, 0);
            //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            //    //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 0, 0, 10, 250)))
            //    {
            //        Insert();
            //        Insert1();

            //        //scope.Complete();
            //    }
            //}
            //catch (Exception e)
            //{

            //}

            //Update();
            ////Delete2();
            //Page();
            ////MultiRSSqlCE();
            ////TestProcSupport();

            //QuerySql();
            //PageList();
            //QuerySql();
            //PageList();
            //Page();
            //Insert();
            ////InsertBatch();
            //Query();

            //string str = string.Empty;
            //DynamicObject
            //PropertyDescriptor
            //var ca = TypeDescriptor.GetAttributes(typeof(Foo)).OfType<CategoryAttribute>().FirstOrDefault();
            //Console.WriteLine(ca.Category); // <=== nice

            ////Attribute[] cate = new Attribute[2];
            ////cate[0] = new CategoryAttribute("naughty0");
            ////cate[1] = new TableAttribute("Table");
            ////TypeDescriptor.AddAttributes(typeof(Foo), new CategoryAttribute("naughty"));
            //TypeDescriptor.AddAttributes(typeof(Foo), new Attribute[2] { new TableAttribute("Table"), new DatabaseAttribute("Database") });
            //ca = TypeDescriptor.GetAttributes(typeof(Foo)).OfType<CategoryAttribute>().FirstOrDefault();            
            //var list1 = TypeDescriptor.GetAttributes(typeof(Foo)).OfType<Attribute>().ToList();
            //Console.WriteLine(ca.Category); // <=== naughty




            //var model = Common.GetModelDes<Foo>();
            //foreach (var item in model.Properties)
            //{
            //    var value = model.ClassType.GetProperty(item.Field).GetValue(t, null);
            //    this.ParamValues.Add(item.Field, value);
            //    var pmodel = new DynamicPropertyModel();
            //    pmodel.Name = item.Field;
            //    if (value != null)
            //        pmodel.PropertyType = value.GetType();
            //    else
            //        pmodel.PropertyType = typeof(System.String);
            //    this._Param.Add(pmodel);
            //}

            //var m = new DynamicPropertyModel();
            //m.Name = "ID";
            //m.PropertyType = 1.GetType();
            //_Param.Add(m);
            //m = new DynamicPropertyModel();
            //m.Name = "Name";
            //m.PropertyType = "gg".GetType();
            //_Param.Add(m);

            //string cacheKey = "Foodaf";            
            //Type modelType;
            //lock (objLock)
            //{
            //    DynamicParamModelCache.TryGetValue(cacheKey, out modelType);
            //    if (modelType == null)
            //    {
            //        var tyName = "CustomDynamicParamClass";
            //        modelType = CustomDynamicBuilder.DynamicCreateType(tyName, _Param);
            //        DynamicParamModelCache.Add(cacheKey, modelType);
            //    }
            //}
            //var model = Activator.CreateInstance(modelType);            
            ////IEnumerable<dynamic> obj = new List<dynamic>();
            //foreach (var item in ParamValues)
            //{
            //    modelType.GetProperty(item.Key).SetValue(model, item.Value, null);                
            //}
            ////var id = obj.ID;

            ////model.ID = "李四";
            ////model.Name = DateTime.Now;
            ////var result = dbPower.Update<model>(model);


            //QuerySql();

            //Console.ReadLine();
        }


        [Category("nice")]
        class Foo
        {
            public string ID { set; get; }
            public string Name { set; get; }
        }


        // 写入数据
        private static void Insert1()
        {
            //var model = new Account()
            //{
            //    Name = "张三1",
            //    Password = "123456",
            //    Email = "123@qq.com",
            //    CreateTime = DateTime.Now,
            //    Age = 15
            //};
            //var result = false;
            //try
            //{
            //    result = dbPower.Insert<Account>(model);
            //    //dbPower.Commit();
            //}
            //catch (Exception e)
            //{
            //    dbPower.Rollback();
            //}

            //if (result)
            //    Console.WriteLine("添加成功");
            //else
            //    Console.WriteLine("添加失败");
        }

        // 写入数据
        private static void Insert()
        {
            var model = new Account()
            {
                //ID = "1",
                Name = "张三1",
                Password = "123456",
                Email = "123@qq.com",
                //CreateTime = DateTime.Now,
                Age = 15
            };
            var result = false;
            try
            {
                result = _DbBase.Insert<Account>(model);
                string sql = _DbBase.SQL;
                //result = db.Insert<Account>(model, db.BeginTransaction);
                //result = db.Insert<Account>(model, db.BeginTransaction);

                //var info = db.SingleOrDefault<Account>(SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.Equal, "5"), db.BeginTransaction);
                //Account info = new Account();
                //info.ID = "5";
                //info.Name = "李四";
                //info.CreateTime = DateTime.Now;
                //result = db.Update<Account>(info, null, db.BeginTransaction);

                //var d = SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.Equal, "1");
                //result = db.Delete<Account>(d, db.BeginTransaction);

                //dbPower.Commit();
            }
            catch (Exception)
            {
                //dbPower.Rollback();
            }

            if (result)
                Console.WriteLine("添加成功");
            else
                Console.WriteLine("添加失败");
        }
        // 批量写入
        private static void InsertBatch()
        {
            for (int x = 0; x < int.MaxValue; x++)
            {
                var list = new List<Account>();
                for (int i = 0; i < 1; i++)
                {
                    var model = new Account()
                    {
                        ID = i.ToString(),
                        Name = "张三" + i.ToString(),
                        Password = "123456",
                        Email = "123@qq.com",
                        CreateTime = DateTime.Now,
                        Age = 15
                    };
                    list.Add(model);
                }
                using (var db = CreateDbBase())
                {
                    var result = db.InsertBatch<Account>(list);

                    //var result = db.InsertBatch<Account>(list, db.DbTransaction);

                    //if (result)
                    //    Console.WriteLine("添加成功: " + x);
                    //else
                    //    Console.WriteLine("添加失败");
                }

                Thread.Sleep(1000);
            }
        }

        // 更新数据
        private static void Update()
        {
            //using (var db = CreateDbBase())
            {
                var r = _DbBase.Query<Account>(SqlQuery<Account>.Builder(_DbBase).AndWhere(m => m.ID, OperationMethod.Equal, "5"));

                var model = _DbBase.SingleOrDefault<Account>(SqlQuery<Account>.Builder(_DbBase).AndWhere(m => m.ID, OperationMethod.Equal, "5"));
                model.Name = "李四";
                model.CreateTime = DateTime.Now;
                var result = _DbBase.Update<Account>(model);
            }
        }
        // 批量更新
        private static void UpdateBatch()
        {
            using (var db = CreateDbBase())
            {
                var model = db.SingleOrDefault<Account>(SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.Equal, "4"));
                model.Name = "李四";
                var result = db.Update<Account>(model, SqlQuery<Account>.Builder(db).AndWhere(m => m.Flag, OperationMethod.Greater, 110));
            }
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        //private static void UpdateBatch2()
        //{
        //    var list = new List<Account>();
        //    for (int i = 0; i < 2; i++)
        //    {
        //        var model = new Account()
        //        {
        //            ID = i.ToString(),
        //            Name = "张三" + i.ToString(),
        //            Password = "123456",
        //            Email = "123@qq.com",
        //            CreateTime = DateTime.Now.AddDays(1),
        //            Age = 15
        //        };
        //        list.Add(model);
        //    }
        //    //using (var db = CreateDbBase())
        //    //{
        //    var db = dbPower;
        //    var result = db.UpdateBatch<Account>(list, db.BeginTransaction);
        //    db.Commit();

        //    //result = db.UpdateBatch<Account>(list, db.BeginTransaction);
        //    //db.BeginTransaction.Commit();
        //    //db.Commit();
        //    if (result)
        //        Console.WriteLine("更新成功 ");
        //    else
        //        Console.WriteLine("更新失败");
        //    //}
        //}

        //删除单条
        public static void Delete()
        {
            using (var db = CreateDbBase())
            {
                var d = SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.Equal, "1");
                var result = db.Delete<Account>(d);
            }
        }
        //删除多条
        public static void Delete2()
        {
            using (var db = CreateDbBase())
            {
                var d = SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.In, new List<string>() { "2", "3" });
                var result = db.Delete<Account>(d);
            }
        }

        //分页查询
        public static void Page()
        {
            using (var db = CreateDbBase())
            {
                var d = SqlQuery<Account>.Builder(db).AndWhere(m => m.Age, OperationMethod.Less, 20)
                     .LeftInclude()
                     .AndWhere(m => m.CreateTime, OperationMethod.Greater, DateTime.Now.AddDays(-5))
                     .AndWhere(m => m.Name, OperationMethod.FullIndex, "李")
                     .RightInclude();
                long dc = 0;
                var result = db.Page<Account>(1, 20, out dc, d);
                string sql = db.SQL;
                Console.WriteLine("查询出数据条数:" + result.Count);
            }
        }

        public static void PageList()
        {
            //var p = new DynamicParameters();
            //p.Add("TableName", "account");
            //p.Add("FieldName", "Id, Name");            
            //p.Add("FieldSort", "id");
            //p.Add("Condition", "1=1");
            //p.Add("PageSize", 20);
            //p.Add("Page",0);
            //p.Add("RtnCurpage", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //p.Add("RtnPageCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //p.Add("RtnCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //p.Add("RtnMsg", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //p.Add("Return Value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            //var list = dbPower.Query<int>("USP_Get_Page_List", p, commandType: CommandType.StoredProcedure).ToList();
            //int a = p.Get<int>("RtnCurpage");
            //int b = p.Get<int>("RtnPageCount");
            //int c = p.Get<int>("RtnCount");
            //int d = p.Get<int>("RtnMsg");
            //int e = p.Get<int>("Return Value");



            //var p = new DynamicParameters();
            //p.Add("TableName", "dbo.T_Produits_Items as a left join dbo.T_Produits_Items_Images as b on a.iItemID = b.iItemID");
            ////p.Add("FieldName", "a.iID,a.iItemID,a.iGeneralTypeID");
            //p.Add("FieldName", "a.iID,a.iItemID,a.iGeneralTypeID,a.vcApproval,a.vcEasyName,a.vcGeneralName,a.vcShortName,a.vcPYName"
            //                    +",a.iFactoryID,a.iMarkID,a.iPharmaTypeID,a.iPharmacodeID,a.iPharmaFomulationID,a.vcMarkName,a.ncPharmaCode"
            //                    +",a.iPharmaBranchID,a.iSeasonPeriodID,a.iTimePeriodID,a.iStatus,vcReason,a.vcOperUserName,a.vcOperTime"
            //                    +",a.bIsDelete,a.dCreateTime"
            //                    + ",b.iImagesID,b.iImgType,b.vcImgUrl,b.vcIntro,b.iSort,b.bIsDefault");
            //p.Add("FieldSort", "dCreateTime asc");
            //string condition = " 1=1 ";
            //p.Add("Condition", condition);
            //p.Add("RtnCurpage", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //p.Add("RtnPageCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //p.Add("RtnCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //p.Add("RtnMsg", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //p.Add("Return Value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            ////var list = dbPower.Query<obj>("USP_Get_Page_List", p, commandType: CommandType.StoredProcedure);
            //SqlMapper.GridReader red = dbPower.QueryMultiple("USP_Get_Page_List", p, commandType: CommandType.StoredProcedure);
            //var obj = red.Read().ToList();
            //int a = p.Get<int>("RtnCurpage");
            //int b = p.Get<int>("RtnPageCount");
            //int c = p.Get<int>("RtnCount");
            //int d = p.Get<int>("RtnMsg");
            //int e = p.Get<int>("Return Value");


            var p = new DynamicParameters();
            p.Add("TableName", "DB_Produits.dbo.T_Produits_Items as a left join DB_Produits.dbo.T_Produits_Items_Images as b on a.iItemID = b.iItemID");
            p.Add("FieldName", "a.*");
            p.Add("FieldSort", "dCreateTime asc");
            string condition = " 1=1 ";
            p.Add("Condition", condition);
            p.Add("RtnCurpage", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("RtnPageCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("RtnCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("RtnMsg", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Return Value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var reader = _DbBase.QueryMultiple("DB_System.dbo.USP_Get_Page_List", p, commandType: CommandType.StoredProcedure);
            if (reader != null)
            {
                int a = p.Get<int>("RtnCurpage");
                int b = p.Get<int>("RtnPageCount");
                int c = p.Get<int>("RtnCount");
                int d = p.Get<int>("RtnMsg");
                int e = p.Get<int>("Return Value");
            }
        }

        public static void Query()
        {
            //            var ResultList = dbSystem.Query<ADInfo>
            //                    (@"SELECT A.*,B.vcTypeName AS iStatusName FROM DB_System.dbo.T_AD A 
            //                        LEFT JOIN DB_System.dbo.T_AD_Type B ON A.iSysAdTypeID=B.iSysAdTypeID
            //                    WHERE A.iSysAdTypeID=@iSysAdTypeID AND A.vcTitle LIKE '%@vcTitle%'  ", new { iSysAdTypeID = iSysAdTypeID, vcTitle = vcTitle }).ToList();
            //            string ss = dbSystem.SQL;
            //            return JsonConvert.SerializeObject(ResultList);

            var list = _DbBase.Query(@"SELECT * FROM Account A LEFT JOIN Account B ON A.Id=B.Id WHERE charindex(@Name,a.name) > 0", new { Id = 1, Name = "李" }).ToList();
            if (list.Count > 0)
            {

            }
            string ss = _DbBase.SQL;
        }


        public static void QuerySql()
        {
            //string sql = "select count(*) from T_Power where cast(cast(iShopID as varchar) + cast(iRoleID as varchar) + cast(iMenuID as varchar) + cast(iActionID as varchar) as varchar) = '01200'";
            //var list = dbPower.Query<int>("select count(*) from T_Power where cast(cast(iShopID as varchar) + cast(iRoleID as varchar) + cast(iMenuID as varchar) + cast(iActionID as varchar) as varchar) = @Module", new { Module = "01200" });

            var ResultList = _DbBase.Query("select count(*) from T_Power where cast(iShopID as varchar) + cast(iRoleID as varchar) + cast(iMenuID as varchar) + cast(iActionID as varchar) = @Module", new { Module = "01200" }).ToList();

            string s = _DbBase.SQL;


        }

    }
}
