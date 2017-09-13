//using System;
//using System.Collections.Generic;
//using System.Data.SqlServerCe;
//using System.IO;
//using System.Linq;
//using System.Text;

//using ybzf.Storage.Dapper.Common;
//using ybzf.Storage.Dapper;
//using ybzf.Storage.DapperEx;
//using System.Data;
//using System.Data.SqlClient;
//using ybzf.Storage.Dapper.Rainbow;

//namespace ybzf.Storage.DapperTest
//{
//    class Program1
//    {

//        public const string ConnectionString = "Data Source=.;Initial Catalog=tempdb;Integrated Security=True";
//        public const string OleDbConnectionString = "Provider=SQLOLEDB;Data Source=.;Initial Catalog=tempdb;Integrated Security=SSPI";
//        static SqlConnection connection = Program.GetOpenConnection();
//        public static SqlConnection GetOpenConnection()
//        {
//            var connection = new SqlConnection(ConnectionString);
//            connection.Open();
//            return connection;
//        }
//        static void Main(string[] args)
//        {
//            //Insert();
//            //Update();
//            UpdateBatch2();
//            //Delete2();
//            //Page();

//            //MultiRSSqlCE();

//            //TestProcSupport();

//            Console.ReadLine();
//        }

//        public class PostCE
//        {
//            public int ID { get; set; }
//            public string Title { get; set; }
//            public string Body { get; set; }

//            public AuthorCE Author { get; set; }
//        }

//        public class AuthorCE
//        {
//            public int ID { get; set; }
//            public string Name { get; set; }
//        }

//        public static void MultiRSSqlCE()
//        {
//            if (File.Exists("Test.sdf"))
//                File.Delete("Test.sdf");

//            var cnnStr = "Data Source = Test.sdf";
//            var engine = new SqlCeEngine(cnnStr);
//            engine.CreateDatabase();

//            using (var cnn = new SqlCeConnection(cnnStr))
//            {
//                cnn.Open();

//                cnn.Execute("create table Posts (ID int, Title nvarchar(50), Body nvarchar(50), AuthorID int)");
//                cnn.Execute("create table Authors (ID int, Name nvarchar(50))");

//                cnn.Execute("insert Posts values (1,'title','body',1)");
//                cnn.Execute("insert Posts values(2,'title2','body2',2)");
//                cnn.Execute("insert Posts values(3,'title3','body3',3)");
//                cnn.Execute("insert Authors values(1,'sam')");

//                var data = cnn.Query<PostCE, AuthorCE, PostCE>(@"select * from Posts p left join Authors a on a.ID = p.AuthorID", (post, author) => { post.Author = author; return post; }).ToList();
//                var firstPost = data.First();
//                firstPost.Title.IsEqualTo("title");
//                firstPost.Author.Name.IsEqualTo("sam");
//                data[1].Author.IsNull();
//                cnn.Close();
//            }
//        }

//        public static void TestProcSupport()
//        {
//            var p = new DynamicParameters();
//            p.Add("a", 11);
//            p.Add("b", dbType: DbType.Int32, direction: ParameterDirection.Output);
//            p.Add("c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);


//            connection.Execute(@"create proc #TestProc 
//	                @a int,
//	                @b int output
//                as 
//                begin
//	                set @b = 999
//	                select 1111
//	                return @a
//                end");
//            connection.Query<int>("#TestProc", p, commandType: CommandType.StoredProcedure).First().IsEqualTo(1111);

//            int a = p.Get<int>("c");
//            int b = p.Get<int>("b");

//        }


//        public static string connectionName = "ConnString";
//        public static DbBase CreateDbBase()
//        {
//            return new DbBase(0);
//        }

//        // 写入数据
//        private static void Insert()
//        {
//            //using (var db = CreateDbBase())
//            //{
//            //    long i = db.Count<Account>();
//            //}

//            var model = new Account()
//            {
//                ID = "1",
//                Name = "张三1",
//                Password = "123456",
//                Email = "123@qq.com",
//                CreateTime = DateTime.Now,
//                Age = 15
//            };
//            using (var db = CreateDbBase())
//            {
//                var result = false;
//                try
//                {
//                    result = db.Insert<Account>(model, db.BeginTransaction);
//                    result = db.Insert<Account>(model, db.BeginTransaction);
//                    result = db.Insert<Account>(model, db.BeginTransaction);

//                    var info = db.SingleOrDefault<Account>(SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.Equal, "5"), db.BeginTransaction);
//                    //Account info = new Account();
//                    //info.ID = "5";
//                    info.Name = "李四";
//                    info.CreateTime = DateTime.Now;
//                    result = db.Update<Account>(info, null, db.BeginTransaction);

//                    //var d = SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.Equal, "1");
//                    //result = db.Delete<Account>(d, db.BeginTransaction);

//                    db.BeginTransaction.Commit();
//                }
//                catch (Exception e)
//                {
//                    db.BeginTransaction.Rollback();
//                }
//                finally
//                {
//                    if (db.DbConnecttion.State != ConnectionState.Closed)
//                    {
//                        db.DbConnecttion.Close();
//                    }
//                }

//                if (result)
//                    Console.WriteLine("添加成功");
//                else
//                    Console.WriteLine("添加失败");
//            }
//        }
//        // 批量写入
//        private static void InsertBatch()
//        {
//            for (int x = 0; x < int.MaxValue; x++)
//            {
//                var list = new List<Account>();
//                for (int i = 0; i < 1; i++)
//                {
//                    var model = new Account()
//                    {
//                        ID = i.ToString(),
//                        Name = "张三" + i.ToString(),
//                        Password = "123456",
//                        Email = "123@qq.com",
//                        CreateTime = DateTime.Now,
//                        Age = 15
//                    };
//                    list.Add(model);
//                }
//                using (var db = CreateDbBase())
//                {
//                    ////var result = db.InsertBatch<Account>(list);

//                    //var result = db.InsertBatch<Account>(list, db.DbTransaction);

//                    //if (result)
//                    //    Console.WriteLine("添加成功: " + x);
//                    //else
//                    //    Console.WriteLine("添加失败");
//                }

//                System.Threading.Thread.Sleep(1000);
//            }
//        }

//        // 更新数据
//        private static void Update()
//        {
//            using (var db = CreateDbBase())
//            {
//                var r = db.Query<Account>(SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.Equal, "5"));

//                var model = db.SingleOrDefault<Account>(SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.Equal, "5"));
//                model.Name = "李四";
//                model.CreateTime = DateTime.Now;
//                var result = db.Update<Account>(model);
//            }
//        }
//        // 批量更新
//        private static void UpdateBatch()
//        {
//            using (var db = CreateDbBase())
//            {
//                var model = db.SingleOrDefault<Account>(SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.Equal, "4"));
//                model.Name = "李四";
//                var result = db.Update<Account>(model, SqlQuery<Account>.Builder(db).AndWhere(m => m.Flag, OperationMethod.Greater, 110));
//            }
//        }

//        /// <summary>
//        /// 批量更新
//        /// </summary>
//        private static void UpdateBatch2()
//        {
//            var list = new List<Account>();
//            for (int i = 0; i < 5; i++)
//            {
//                var model = new Account()
//                {
//                    ID = i.ToString(),
//                    Name = "张三" + i.ToString(),
//                    Password = "123456",
//                    Email = "123@qq.com",
//                    CreateTime = DateTime.Now,
//                    Age = 15
//                };
//                list.Add(model);
//            }
//            using (var db = CreateDbBase())
//            {
//                var result = db.UpdateBatch<Account>(list, db.BeginTransaction);
//                db.BeginTransaction.Commit();

//                if (result)
//                    Console.WriteLine("更新成功 ");
//                else
//                    Console.WriteLine("更新失败");
//            }
//        }

//        //删除单条
//        public static void Delete()
//        {
//            using (var db = CreateDbBase())
//            {
//                var d = SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.Equal, "1");
//                var result = db.Delete<Account>(d);
//            }
//        }
//        //删除多条
//        public static void Delete2()
//        {
//            using (var db = CreateDbBase())
//            {
//                var d = SqlQuery<Account>.Builder(db).AndWhere(m => m.ID, OperationMethod.In, new List<string>() { "2", "3" });
//                var result = db.Delete<Account>(d);
//            }
//        }

//        //分页查询
//        public static void Page()
//        {
//            using (var db = CreateDbBase())
//            {
//                var d = SqlQuery<Account>.Builder(db).AndWhere(m => m.Age, OperationMethod.Less, 20)
//                     .LeftInclude()
//                     .AndWhere(m => m.CreateTime, OperationMethod.Greater, DateTime.Now.AddDays(-5))
//                     .AndWhere(m => m.Name, OperationMethod.Contains, "张")
//                     .RightInclude();
//                long dc = 0;
//                var result = db.Page<Account>(1, 20, out dc, d);
//                Console.WriteLine("查询出数据条数:" + result.Count);
//            }
//        }

//    }

//    [Database(Name = "DB_Log")]
//    [Owner(Name = "dbo")]
//    [Table(Name = "Account")]
//    public class Account
//    {
//        [ID]
//        public virtual string ID { get; set; }
//        [ID]
//        public virtual string Name { get; set; }
//        public virtual string Password { get; set; }
//        public virtual string Email { get; set; }
//        public virtual DateTime CreateTime { get; set; }
//        public virtual int Age { get; set; }
//        [Column(true)]
//        public virtual int Flag { get; set; }
//        [Ignore]
//        public virtual string AgeStr
//        {
//            get
//            {
//                return "年龄：" + Age;
//            }
//        }
//    }


//    //class Product
//    //{
//    //    public int Id { get; set; }
//    //    public string Name { get; set; }
//    //    public string Description { get; set; }
//    //    public DateTime? LastPurchase { get; set; }
//    //}

//    //// container with all the tables
//    //class MyDatabase : Database<MyDatabase>
//    //{
//    //    public Table<Product> Products { get; set; }
//    //}

//    //        static void Main(string[] args)
//    //        {
//    //            var cnn = new SqlConnection("Data Source=.;Initial Catalog=tempdb;Integrated Security=True");
//    //            cnn.Open();

//    //            var db = MyDatabase.Init(cnn, commandTimeout: 2);

//    //            try
//    //            {
//    //                db.Execute("waitfor delay '00:00:03'");
//    //            }
//    //            catch (Exception)
//    //            {
//    //                Console.WriteLine("yeah ... it timed out");
//    //            }


//    //            db.Execute("if object_id('Products') is not null drop table Products");
//    //            db.Execute(@"create table Products (
//    //                                Id int identity(1,1) primary key,
//    //                                Name varchar(20),
//    //                                Description varchar(max),
//    //                                LastPurchase datetime)");

//    //            int? productId = db.Products.Insert(new { Name = "Hello", Description = "Nothing" });
//    //            var product = db.Products.Get((int)productId);

//    //            product.Description = "untracked change";

//    //            // snapshotter tracks which fields change on the object
//    //            var s = Snapshotter.Start(product);
//    //            product.LastPurchase = DateTime.UtcNow;
//    //            product.Name += " World";

//    //            // run: update Products set LastPurchase = @utcNow, Name = @name where Id = @id
//    //            // note, this does not touch untracked columns
//    //            db.Products.Update(product.Id, s.Diff());

//    //            // reload
//    //            product = db.Products.Get(product.Id);


//    //            Console.WriteLine("id: {0} name: {1} desc: {2} last {3}", product.Id, product.Name, product.Description, product.LastPurchase);
//    //            // id: 1 name: Hello World desc: Nothing last 12/01/2012 5:49:34 AM

//    //            Console.WriteLine("deleted: {0}", db.Products.Delete(product.Id));
//    //            // deleted: True

//    //            //// F：front（前台），B：后台
//    //            //// 1：ShopID（商家ID号）
//    //            //string userID = TextHelper.GetGenerateRandom("F", 1);
//    //            Console.ReadKey();
//    //        }
//    //}
//}
