using System.Collections.Generic;
using SqlSugar;

namespace SugarCodeGeneration
{
    public class DbContextParameter
    {
        public string ConnectionString { get; set; }
        public DbType DbType { get; set; }
        public List<string> Tables { get; set; }
        public string ClassNamespace { get;  set; }
    }
    public class IBaseRepositoryParameter
    {
        public string ModelsNamespace { get; set; }
        public string ClassNamespace { get; set; }
    }
    public class IModelRepositoryParameter
    {
        public string Name { get; set; }
        public string IBaseRepositoryNamespace { get; set; }
        public string ModelsNamespace { get; set; }
        public string ClassNamespace { get; set; }
    }
    public class BaseRepositoryParameter
    {
        public string ModelsNamespace { get; set; }
        public string ClassNamespace { get; set; }
        public string IBaseRepositoryNamespace { get; set; }
    }
    public class RepositoryDBContextParameter
    {
        public string ClassNamespace { get; set; }
    }
    public class ModelRepositoryParameter
    {
        public string Name { get; set; }
        public string BaseRepositoryNamespace { get; set; }
        public string ModelsNamespace { get; set; }
        public string ClassNamespace { get; set; }
        public string IRepositoryNamespace { get; set; }
    }
    public class IBaseServicesParameter
    {
        public string ModelsNamespace { get; set; }
        public string ClassNamespace { get; set; }
    }
    public class IModelServicesParameter
    {
        public string Name { get; set; }
        public string IBaseServicesNamespace { get; set; }
        public string ModelsNamespace { get; set; }
        public string ClassNamespace { get; set; }
    }
    public class BaseServicesParameter
    {
        public string ModelsNamespace { get; set; }
        public string ClassNamespace { get; set; }
        public string IBaseRepositoryNamespace { get; set; }
        public string IBaseServicesNamespace { get; set; }
    }
    public class ModelServicesParameter
    {
        public string Name { get; set; }
        public string BaseServicesNamespace { get; set; }
        public string ModelsNamespace { get; set; }
        public string ClassNamespace { get; set; }
        public string IRepositoryNamespace { get; set; }
        public string IServicesNamespace { get; set; }
    }
}