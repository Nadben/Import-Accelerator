using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PostgreSQLCopyHelper;

namespace Accelerator.Commercetools.Importer.Shared.Helper;

public static class SqlCopyHelper
{
    public static PostgreSQLCopyHelper<T> CreateHelper<T>(string schemaName, string tableName)
    {
        var helper = new PostgreSQLCopyHelper<T>("dbo", "\"" + tableName + "\"");
        var properties = typeof(T).GetProperties();
        foreach (var prop in properties)
        {
            var type = prop.PropertyType;
            if (Attribute.IsDefined(prop, typeof(KeyAttribute)) ||
                Attribute.IsDefined(prop, typeof(ForeignKeyAttribute)))
                continue;
            switch (type)
            {
                case Type intType when intType == typeof(int) || intType == typeof(int?):
                {
                    helper = helper.MapInteger("\"" + prop.Name + "\"",
                        x => (int?)typeof(T).GetProperty(prop.Name).GetValue(x, null));
                    break;
                }
                case Type stringType when stringType == typeof(string):
                {
                    helper = helper.MapText("\"" + prop.Name + "\"",
                        x => (string)typeof(T).GetProperty(prop.Name).GetValue(x, null));
                    break;
                }
                case Type dateType when dateType == typeof(DateTime) || dateType == typeof(DateTime?):
                {
                    helper = helper.MapTimeStamp("\"" + prop.Name + "\"",
                        x => (DateTime?)typeof(T).GetProperty(prop.Name).GetValue(x, null));
                    break;
                }
                case Type decimalType when decimalType == typeof(decimal) || decimalType == typeof(decimal?):
                {
                    helper = helper.MapMoney("\"" + prop.Name + "\"",
                        x => (decimal?)typeof(T).GetProperty(prop.Name).GetValue(x, null));
                    break;
                }
                case Type doubleType when doubleType == typeof(double) || doubleType == typeof(double?):
                {
                    helper = helper.MapDouble("\"" + prop.Name + "\"",
                        x => (double?)typeof(T).GetProperty(prop.Name).GetValue(x, null));
                    break;
                }
                case Type floatType when floatType == typeof(float) || floatType == typeof(float?):
                {
                    helper = helper.MapReal("\"" + prop.Name + "\"",
                        x => (float?)typeof(T).GetProperty(prop.Name).GetValue(x, null));
                    break;
                }
                case Type guidType when guidType == typeof(Guid):
                {
                    helper = helper.MapUUID("\"" + prop.Name + "\"",
                        x => (Guid)typeof(T).GetProperty(prop.Name).GetValue(x, null));
                    break;
                }
            }
        }

        return helper;
    }
}