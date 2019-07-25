using SugarCodeGeneration.Codes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SugarCodeGeneration
{


    /// <summary>
    /// F5直接运行生成项目
    /// </summary>
    /// <param name="args"></param>
    class Program
    {

        /***3个必填参数***/

        //如果你不需要自定义，直接配好数据库连接，F5运行项目
        const SqlSugar.DbType dbType = SqlSugar.DbType.SqlServer;
        /// <summary>
        /// 连接字符串
        /// </summary>
        const string connectionString = "server=.;uid=sa;pwd=radinfo;database=LuoKiPet";
        /// <summary>
        ///解决方案名称
        /// </summary>
        const string SolutionName = "LuoKiPet.Core";//如果修改解决方案名称，F5执行完成后会自动关闭项目，找到目录重新打开项目解决方案便可

        /***3个必填参数***/




        /// <summary>
        /// 执行生成
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            /***连接数据库***/
            var db = GetDB();


            /***生成实体***/ //CodeFirst可以注释生成实体

            //配置参数
            string classProjectName = SolutionName+".Model";//实体类项目名称
            string classPath = "LuoKiPetModels";//生成的目录
            string classNamespace = SolutionName + ".Model.LuoKiPetModels";//实体命名空间
            string classNamespaceAll = SolutionName + ".Model";//实体项目的全局空间
            var classDirectory = Methods.GetSlnPath + "\\" + classProjectName + "\\" + classPath.TrimStart('\\');
            //执行生成
            GenerationClass(classProjectName, classPath, classNamespace, classDirectory);
            Print("实体创建成功");

            /***生成一些自定义的实体类如PageModel等等***/
            //配置参数
            var CustomProjectName = SolutionName + ".Model";//所在的项目
            var savePath2 = Methods.GetSlnPath + "\\" + CustomProjectName ;//保存目录
            Dictionary<string, object> Models = new Dictionary<string, object>();
            Models.Add("PageModel", new {
                ClassNamespace= classProjectName
            });
            //执行生成
            CeneerationCustomModel(savePath2, Models);
            Print("自定义实体类创建成功");

            /***生成IBaseRepository***/
            //配置参数
            var IRepositoryProjectName = SolutionName + ".IRepository";//数据操作接口层项目名称
            var IRepositoryPath = "Base";//基类文件夹
            var IRepositoryBaseSavePath = Methods.GetSlnPath + "\\" + IRepositoryProjectName + "\\" + IRepositoryPath + "\\IBaseRepository.cs";//具体的文件名
            var IRepositoryBaseClassNamespace = SolutionName + ".IRepository.Base";//文件所在命名空间
            //执行生成
            CenerationIBaseRepository(IRepositoryProjectName, IRepositoryPath, IRepositoryBaseSavePath, IRepositoryBaseClassNamespace, classNamespaceAll);
            Print("IBaseRepository创建成功");

            /***生成每一个实体对应的IRepository***/
            //配置参数
            var IModelRepositoryProjectName = SolutionName + ".IRepository";//所在项目
            var IModelRepositoryPath = "IRepository";//所在文件夹
            var IModelRepositorySavePath = Methods.GetSlnPath + "\\" + IModelRepositoryProjectName + "\\" + IModelRepositoryPath;//保存的目录
            var IModelRepositoryTables = db.DbMaintenance.GetTableInfoList().Select(it => it.Name).ToList();
            var IModelRepositoryClassNamespace = SolutionName + ".IRepository";//每一个IModelRepository所在的命名空间
            CenerationIModelRepository(IModelRepositoryProjectName, IModelRepositoryPath, IModelRepositorySavePath, IModelRepositoryTables, IModelRepositoryClassNamespace, IRepositoryBaseClassNamespace, classNamespace);
            Print("IModelRepository创建成功");

            /***生成BaseRepository***/
            //配置参数
            var RepositoryProjectName = SolutionName + ".Repository";//数据操作接口层项目名称
            var RepositoryPath = "Base";//基类文件夹
            var RepositoryBaseSavePath = Methods.GetSlnPath + "\\" + RepositoryProjectName + "\\" + RepositoryPath + "\\BaseRepository.cs";//具体的文件名
            var RepositoryBaseClassNamespace = SolutionName + ".Repository.Base";//文件所在命名空间
            //执行生成
            CenerationBaseRepository(RepositoryProjectName, RepositoryPath, RepositoryBaseSavePath, RepositoryBaseClassNamespace, classNamespaceAll, IRepositoryBaseClassNamespace);
            Print("BaseRepository创建成功");

            /***在Repository生成DBContext***/
            var RepositoryDBContextSavePath= Methods.GetSlnPath + "\\" + RepositoryProjectName + "\\" + RepositoryPath + "\\DBContext.cs";//具体的文件名
            var RepositoryDBContextNamespace= SolutionName + ".Repository";
            CenerationRepositoryDBContext(RepositoryProjectName, RepositoryPath, RepositoryDBContextSavePath, RepositoryDBContextNamespace);
            Print("RepositoryDBContext创建成功");

            /***生成每一个实体对应的Repository***/
            //配置参数
            var ModelRepositoryProjectName = SolutionName + ".Repository";//所在项目
            var ModelRepositoryPath = "Repository";//所在文件夹
            var ModelRepositorySavePath = Methods.GetSlnPath + "\\" + ModelRepositoryProjectName + "\\" + ModelRepositoryPath;//保存的目录
            var ModelRepositoryTables = db.DbMaintenance.GetTableInfoList().Select(it => it.Name).ToList();
            var ModelRepositoryClassNamespace = SolutionName + ".Repository";//每一个IModelRepository所在的命名空间
            CenerationModelRepository(ModelRepositoryProjectName, ModelRepositoryPath, ModelRepositorySavePath, ModelRepositoryTables, ModelRepositoryClassNamespace, RepositoryBaseClassNamespace, classNamespace, IModelRepositoryClassNamespace);
            Print("ModelRepository创建成功");

            /***生成IBaseServices***/
            //配置参数
            var IServicesProjectName = SolutionName + ".IServices";//数据操作接口层项目名称
            var IServicesPath = "Base";//基类文件夹
            var IServicesBaseSavePath = Methods.GetSlnPath + "\\" + IServicesProjectName + "\\" + IServicesPath + "\\IBaseServices.cs";//具体的文件名
            var IServicesBaseClassNamespace = SolutionName + ".IServices.Base";//文件所在命名空间
            //执行生成
            CenerationIBaseServices(IServicesProjectName, IServicesPath, IServicesBaseSavePath, IServicesBaseClassNamespace, classNamespaceAll);
            Print("IBaseServices创建成功");

            /***生成每一个实体对应的IServices***/
            //配置参数
            var IModelServicesProjectName = SolutionName + ".IServices";//所在项目
            var IModelServicesPath = "IServices";//所在文件夹
            var IModelServicesSavePath = Methods.GetSlnPath + "\\" + IModelServicesProjectName + "\\" + IModelServicesPath;//保存的目录
            var IModelServicesTables = db.DbMaintenance.GetTableInfoList().Select(it => it.Name).ToList();
            var IModelServicesClassNamespace = SolutionName + ".IServices";//每一个IModelRepository所在的命名空间
            CenerationIModelServices(IModelServicesProjectName, IModelServicesPath, IModelServicesSavePath, IModelServicesTables, IModelServicesClassNamespace, IRepositoryBaseClassNamespace, classNamespace);
            Print("IModelServices创建成功");

            /***生成BaseServices***/
            //配置参数
            var ServicesProjectName = SolutionName + ".Services";//数据操作接口层项目名称
            var ServicesPath = "Base";//基类文件夹
            var ServicesBaseSavePath = Methods.GetSlnPath + "\\" + ServicesProjectName + "\\" + ServicesPath + "\\BaseServices.cs";//具体的文件名
            var ServicesBaseClassNamespace = SolutionName + ".Services.Base";//文件所在命名空间
            //执行生成
            CenerationBaseServices(ServicesProjectName, ServicesPath, ServicesBaseSavePath, ServicesBaseClassNamespace, classNamespaceAll, IRepositoryBaseClassNamespace, IServicesBaseClassNamespace);
            Print("BaseServices创建成功");

            /***生成每一个实体对应的Services***/
            //配置参数
            var ModelServicesProjectName = SolutionName + ".Services";//所在项目
            var ModelServicesPath = "Services";//所在文件夹
            var ModelServicesSavePath = Methods.GetSlnPath + "\\" + ModelServicesProjectName + "\\" + ModelServicesPath;//保存的目录
            var ModelServicesTables = db.DbMaintenance.GetTableInfoList().Select(it => it.Name).ToList();
            var ModelServicesClassNamespace = SolutionName + ".Services";//每一个ModelRepository所在的命名空间
            CenerationModelServices(ModelServicesProjectName, ModelServicesPath, ModelServicesSavePath, ModelServicesTables, ModelServicesClassNamespace, ServicesBaseClassNamespace, classNamespace, IModelRepositoryClassNamespace, IModelServicesClassNamespace);
            Print("ModelServices创建成功");

            /***生成DbContext***/

            //配置参数
            //var contextProjectName = SolutionName + ".BusinessCore";//DbContext所在项目
            //var contextPath = "DbCore";//dbcontext存储目录
            //var savePath = Methods.GetSlnPath + "\\" + contextProjectName + "\\" + contextPath + "\\DbContext.cs";//具体文件名
            //var tables = db.DbMaintenance.GetTableInfoList().Select(it => it.Name).ToList();//获取表名称
            ////执行生成
            //GenerationDContext(contextProjectName, contextPath, savePath, tables, classNamespace);
            //Print("DbContext创建成功");





            /***生成BLL***/

            //配置参数
            //var bllProjectName2 = SolutionName+".BusinessCore";//具体项目
            //var bllPath2 = "BaseCore";//文件目录
            //var savePath2 = Methods.GetSlnPath + "\\" + bllProjectName2 + "\\" + bllPath2;//保存目录
            //var tables2 = db.DbMaintenance.GetTableInfoList().Select(it => it.Name).ToList();
            //执行生成
            //GenerationBLL(bllProjectName2,bllPath2,savePath2, tables2, classNamespace);
            //Print("BLL创建成功");





            /***修改解决方案***/
            UpdateCsproj();
            Methods.RenameSln(SolutionName);
            Print("项目解决方案修改成功");


            /***添加项目引用***/
            //Methods.AddRef(bllProjectName2,classProjectName);
            Methods.AddRef(IRepositoryProjectName, classProjectName);//IRepository项目添加Model项目引用
            Methods.AddRef(RepositoryProjectName, IRepositoryProjectName);//Repository项目添加IRepository项目引用
            Methods.AddRef(RepositoryProjectName, classProjectName);//Repository项目添加Model项目引用
            Methods.AddRef(IServicesProjectName, classProjectName);//IServices项目添加Model项目引用
            Methods.AddRef(ServicesProjectName, classProjectName);//Services项目添加Model项目引用
            Methods.AddRef(ServicesProjectName, IRepositoryProjectName);//Services项目添加IRepository项目引用
            Methods.AddRef(ServicesProjectName, IServicesProjectName);//Services项目添加IService项目引用

            Print("引用添加成功");

            //如何使用创建好的业务类（注意 SchoolManager 不能是静态的）
            //SchoolManager sm = new SchoolManager();
            //sm.GetList();
            //sm.StudentDb.AsQueryable().Where(it => it.Id == 1).ToList();
            //sm.Db.Queryable<Student>().ToList();

        }

 
      /// <summary>
        /// 生成BLL
        /// </summary>
        private static void GenerationBLL(string bllProjectName, string bllPath, string savePath, List<string>tables,string classNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\Bll.txt";//bll模版地址
            //下面代码不动
            Methods.CreateBLL(templatePath, savePath, tables, classNamespace);
            AddTask(bllProjectName, bllPath);
        }
        /// <summary>
        /// 生成自定义实体类在Model项目中
        /// </summary>
        /// <param name="ProjectName"></param>
        /// <param name="Path"></param>
        /// <param name="SavePath"></param>
        /// <param name="Models"></param>
        /// <param name="ClassNamespace"></param>
        private static void CeneerationCustomModel(string SavePath, Dictionary<string, object> Models)
        {
            foreach (var item in Models)
            {
                var templatePath= Methods.GetCurrentProjectPath + "\\Template\\CustomModels\\"+item.Key+".txt";//自定义实体类模版地址
                Methods.CreateCustomModel(templatePath, SavePath, item.Value, item.Key);
            }
        }

        /// <summary>
        /// 生成DbContext
        /// </summary>
        private static void GenerationDContext(string contextProjectName, string contextPath, string savePath,List<string> tables,string classNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\DbContext.txt";//dbcontexts模版文件
            //下面代码不动
            var model = new DbContextParameter{
                ConnectionString = connectionString,
                DbType = dbType,
                Tables = tables,
                ClassNamespace= classNamespace
            };
            Methods.CreateDbContext(templatePath,savePath,model);
            AddTask(contextProjectName,contextPath);
        }

        /// <summary>
        /// 生成IBaseRepository
        /// </summary>
        /// <param name="IRepositoryProjectName"></param>
        /// <param name="IRepositoryPath"></param>
        /// <param name="IRepositorySavePath"></param>
        /// <param name="classNamespace"></param>
        public static void CenerationIBaseRepository(string IRepositoryProjectName, string IRepositoryPath, string IRepositorySavePath, string classNamespace,string ModelsClassNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\IBaseRepository.txt";
            //下面代码不动
            var model = new IBaseRepositoryParameter
            {
                ModelsNamespace= ModelsClassNamespace,
                ClassNamespace = classNamespace
            };
            Methods.CreateIBaseRepository(templatePath, IRepositorySavePath, model);
            AddTask(IRepositoryProjectName, IRepositoryPath);
        }
        /// <summary>
        /// 生成每一个Model对应的IRepository
        /// </summary>
        /// <param name="IModelRepositoryProjectName"></param>
        /// <param name="IModelRepositoryPath"></param>
        /// <param name="savePath"></param>
        /// <param name="tables"></param>
        /// <param name="classNamespace"></param>
        /// <param name="IBaseRepositoryNamespace"></param>
        /// <param name="ModelsNamespace"></param>
        public static void CenerationIModelRepository(string IModelRepositoryProjectName, string IModelRepositoryPath, string savePath, List<string> tables, string classNamespace, string IBaseRepositoryNamespace, string ModelsNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\IModelRepository.txt";//IModelRepository模版地址
            //下面代码不动
            Methods.CreateIModelRepository(templatePath, savePath, tables, classNamespace, IBaseRepositoryNamespace, ModelsNamespace);
            AddTask(IModelRepositoryProjectName, IModelRepositoryPath);
        }
        /// <summary>
        /// 生成BaseRepository
        /// </summary>
        /// <param name="IRepositoryProjectName"></param>
        /// <param name="IRepositoryPath"></param>
        /// <param name="IRepositorySavePath"></param>
        /// <param name="classNamespace"></param>
        /// <param name="ModelsClassNamespace"></param>
        public static void CenerationBaseRepository(string RepositoryProjectName, string RepositoryPath, string RepositorySavePath, string classNamespace, string ModelsClassNamespace,string IBaseRepositoryNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\BaseRepository.txt";//dbcontexts模版文件
            //下面代码不动
            var model = new BaseRepositoryParameter
            {
                ModelsNamespace = ModelsClassNamespace,
                ClassNamespace = classNamespace,
                IBaseRepositoryNamespace= IBaseRepositoryNamespace
            };
            Methods.CreateBaseRepository(templatePath, RepositorySavePath, model);
            //AddTask(RepositoryProjectName, RepositoryPath);
        }
        /// <summary>
        /// 生成RepositoryDBContext
        /// </summary>
        /// <param name="RepositoryDBContextProjectName"></param>
        /// <param name="RepositoryDBContextPath"></param>
        /// <param name="RepositorySavePath"></param>
        /// <param name="classNamespace"></param>
        public static void CenerationRepositoryDBContext(string RepositoryDBContextProjectName, string RepositoryDBContextPath, string RepositorySavePath, string classNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\RepositoryDBContext.txt";//dbcontexts模版文件
            //下面代码不动
            var model = new RepositoryDBContextParameter
            {
                ClassNamespace = classNamespace
            };
            Methods.CreateRepositoryDBContext(templatePath, RepositorySavePath, model);
            AddTask(RepositoryDBContextProjectName, RepositoryDBContextPath);
        }
        /// <summary>
        /// 生成每一个Model对应的Repository
        /// </summary>
        /// <param name="ModelRepositoryProjectName"></param>
        /// <param name="ModelRepositoryPath"></param>
        /// <param name="savePath"></param>
        /// <param name="tables"></param>
        /// <param name="classNamespace"></param>
        /// <param name="BaseRepositoryNamespace"></param>
        /// <param name="ModelsNamespace"></param>
        /// <param name="IRepositoryNamespace"></param>
        public static void CenerationModelRepository(string ModelRepositoryProjectName, string ModelRepositoryPath, string savePath, List<string> tables, string classNamespace, string BaseRepositoryNamespace, string ModelsNamespace,string IRepositoryNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\ModelRepository.txt";//IModelRepository模版地址
            //下面代码不动
            Methods.CreateModelRepository(templatePath, savePath, tables, classNamespace, BaseRepositoryNamespace, ModelsNamespace, IRepositoryNamespace);
            AddTask(ModelRepositoryProjectName, ModelRepositoryPath);
        }
        /// <summary>
        /// 生成IBaseServices
        /// </summary>
        /// <param name="IServicesProjectName"></param>
        /// <param name="IServicesPath"></param>
        /// <param name="IServicesSavePath"></param>
        /// <param name="classNamespace"></param>
        /// <param name="ModelsClassNamespace"></param>
        public static void CenerationIBaseServices(string IServicesProjectName, string IServicesPath, string IServicesSavePath, string classNamespace, string ModelsClassNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\IBaseServices.txt";//dbcontexts模版文件
            //下面代码不动
            var model = new IBaseServicesParameter
            {
                ModelsNamespace = ModelsClassNamespace,
                ClassNamespace = classNamespace
            };
            Methods.CreateIBaseServices(templatePath, IServicesSavePath, model);
            AddTask(IServicesProjectName, IServicesPath);
        }
        /// <summary>
        /// 生成每一个Model对应的IServices
        /// </summary>
        /// <param name="IModellServicesProjectName"></param>
        /// <param name="IModellServicesPath"></param>
        /// <param name="savePath"></param>
        /// <param name="tables"></param>
        /// <param name="classNamespace"></param>
        /// <param name="IBaselServicesNamespace"></param>
        /// <param name="ModelsNamespace"></param>
        public static void CenerationIModelServices(string IModellServicesProjectName, string IModellServicesPath, string savePath, List<string> tables, string classNamespace, string IBaselServicesNamespace, string ModelsNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\IModelServices.txt";//IModelRepository模版地址
            //下面代码不动
            Methods.CreateIModelServices(templatePath, savePath, tables, classNamespace, IBaselServicesNamespace, ModelsNamespace);
            AddTask(IModellServicesProjectName, IModellServicesPath);
        }
        /// <summary>
        /// 生成BaseSservices
        /// </summary>
        /// <param name="ServicesProjectName"></param>
        /// <param name="ServicesPath"></param>
        /// <param name="ServicesSavePath"></param>
        /// <param name="classNamespace"></param>
        /// <param name="ModelsClassNamespace"></param>
        /// <param name="IBaseRepositoryNamespace"></param>
        /// <param name="IBaseServicesNamespace"></param>
        public static void CenerationBaseServices(string ServicesProjectName, string ServicesPath, string ServicesSavePath, string classNamespace, string ModelsClassNamespace,string IBaseRepositoryNamespace,string IBaseServicesNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\BaseServices.txt";//dbcontexts模版文件
            //下面代码不动
            var model = new BaseServicesParameter
            {
                ModelsNamespace = ModelsClassNamespace,
                ClassNamespace = classNamespace,
                IBaseServicesNamespace= IBaseServicesNamespace,
                IBaseRepositoryNamespace= IBaseRepositoryNamespace
            };
            Methods.CreateBaseServices(templatePath, ServicesSavePath, model);
            AddTask(ServicesProjectName, ServicesPath);
        }

        public static void CenerationModelServices(string ModelServicesProjectName, string ModelServicesPath, string savePath, List<string> tables, string classNamespace, string BaseServicesNamespace, string ModelsNamespace,string IRepositoryNamespace,string IServicesNamespace)
        {
            var templatePath = Methods.GetCurrentProjectPath + "\\Template\\ModelServices.txt";//IModelRepository模版地址
            //下面代码不动
            Methods.CreateModelServices(templatePath, savePath, tables, classNamespace, BaseServicesNamespace, ModelsNamespace, IRepositoryNamespace, IServicesNamespace);
            AddTask(ModelServicesProjectName, ModelServicesPath);
        }
        /// <summary>
        /// 生成实体类
        /// </summary>
        private static void GenerationClass(string classProjectName,string classPath,string classNamespace,string classDirectory)
        {
            //连接数据库
            var db = GetDB();
            //下面代码不动
            db.DbFirst.IsCreateAttribute().CreateClassFile(classDirectory, classNamespace);
            AddTask(classProjectName,classPath);
        }

        #region 辅助方法
        /// <summary>
        ///  修改解决方案
        /// </summary>
        private static void UpdateCsproj()
        {
            foreach (var item in CsprojList)
            {
                item.Start();
                item.Wait();
            }
        }
        private static void Print(string message)
        {
            Console.WriteLine("");
            Console.WriteLine(message);
            Console.WriteLine("");
        }

        private static void AddTask(string bllProjectName, string bllPath)
        {
            var task = new Task(() =>
            {
                Methods.AddCsproj(bllPath, bllProjectName);
            });
            CsprojList.Add(task);
        }
        static List<Task> CsprojList = new List<Task>();
        static SqlSugar.SqlSugarClient GetDB()
        {

            return new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
            {
                DbType = dbType,
                ConnectionString = connectionString,
                IsAutoCloseConnection = true,
                InitKeyType = SqlSugar.InitKeyType.Attribute
            });
        }
        #endregion
    }
}
