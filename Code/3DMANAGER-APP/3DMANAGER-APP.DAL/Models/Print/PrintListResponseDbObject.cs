using System.Data;

namespace _3DMANAGER_APP.DAL.Models.Print
{
    public class PrintListResponseDbObject
    {
        public int PrintId { get; set; }
        private const string PrintIdColumnName = "PRINT_ID";
        public string PrintName { get; set; }
        private const string PrintNameColumnName = "PRINT_NAME";
        public string PrintUserCreator { get; set; }
        private const string PrintUSerCreatorColumnName = "PRINT_USER";
        public DateTime PrintDate { get; set; }
        private const string PrintDateColumnName = "PRINT_DATE";
        public decimal PrintTime { get; set; }
        private const string PrintTimeColumnName = "PRINT_TIME";
        public decimal PrintFilamentConsumed { get; set; }
        private const string PrintFilamentConsumedColumnName = "PRINT_FILAMENT_USED";


        public PrintListResponseDbObject Create(DataRow row)
        {
            var obj = new PrintListResponseDbObject();

            obj.PrintId = row.Field<int>(PrintIdColumnName);
            obj.PrintName = row.Field<string>(PrintNameColumnName);
            obj.PrintUserCreator = row.Field<string>(PrintUSerCreatorColumnName);
            obj.PrintDate = row.Field<DateTime>(PrintDateColumnName);
            obj.PrintTime = row.Field<decimal>(PrintTimeColumnName);
            obj.PrintFilamentConsumed = row.Field<decimal>(PrintFilamentConsumedColumnName);

            return obj;
        }
    }
}
