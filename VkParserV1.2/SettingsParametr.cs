using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace VkParserV1._2
{
    class SettingsParametr
    {
    }

    #region Контекст сбора заданий на выполнение
    public class Setting
    {
        public int Id { get; set; }
        public string TypeSetting { get; set; }
        public bool ComplitedResult { get; set; }
        //резервные поля
        public string Data1 { get; set; }
        public string Data2 { get; set; }
    }

    class ContextSetting : DbContext
    {
        public ContextSetting() : base("DbConnection") { }
        public DbSet<Setting> Settings { get; set; }
    }
    #endregion

    #region Контекст обработки параметров для работы с группами

    public class GroupSetting
    {
        public int Id { get; set; }
        public int Group_id { get; set; }
        public string Group_classification_Content { get; set; }
        //Резервные поля
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public string Data3 { get; set; }
    }

    class ContextGroupSetting : DbContext
    {
        public ContextGroupSetting() : base("DbConnection") { }
        public DbSet<GroupSetting> GroupSettings { get; set; }
    }
    #endregion

    #region контекст для работы с классификацией груп и тарам пам пам

    public class ClassificationParametr
    {
        public int Id { get; set; }
        public string CLassification { get; set; }
    }

    class ContextClassificationParametr : DbContext
    {
        public ContextClassificationParametr() : base("DbConnection") { }
        public DbSet<ClassificationParametr> ClassificationParametrs { get; set; }
    }
    #endregion
}
