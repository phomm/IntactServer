using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace Intact.API.Extensions
{
    // https://stackoverflow.com/a/75473564
    public static class MigrationExtensions
    {
        public enum MigrationDirection
        {
            Up,
            Down
        }

        /// <summary>
        /// Execute a .sql file on the a EF Migration
        /// </summary>
        /// <param name="migrationBuilder"></param>
        /// <param name="direction">Optional parameter, it add a .Up or a .Down at the end of the file name Ex.: "20221227004545_Initial.Up.sql"</param>
        /// <param name="fileName">Optional parameter, if not informed get the name of the caller class as name of the Sql file.</param>
        /// <param name="filesPath">Change the relative path where the sql files will be looked for</param>
        /// <param name="onWrongFilesPathThrowException">true: throw a exception if the file is not found, false: try to find sql file by the file name</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static OperationBuilder<SqlOperation> ExecuteSqlFile(
            this MigrationBuilder migrationBuilder,
                 MigrationDirection? direction = null,
                 string? fileName = null,
                 string filesPath = "Migrations/SqlFiles",
                 bool onWrongFilesPathThrowException = true)
        {
            if (fileName == null)
            {
                //Get stack to get the name of the calling Migration
                var frame = new StackFrame(1);
                string className = frame.GetMethod()!.DeclaringType!.Name;
                fileName = $"{className}{(direction != null ? $".{direction}" : "")}.sql";
            }
            else if (!Path.HasExtension(fileName))
            {
                fileName = $"{fileName}.sql";
            }

            string fileFullPath = Path.Combine(AppContext.BaseDirectory, filesPath, fileName);
            if (!File.Exists(fileFullPath))
            {
                string? alternativePath = Directory.EnumerateFiles(path: AppContext.BaseDirectory, fileName, SearchOption.AllDirectories).FirstOrDefault();
                if (alternativePath != null)
                {
                    fileFullPath = onWrongFilesPathThrowException
                        ? throw new FileNotFoundException($"\"{Path.Combine(filesPath, fileName)}\" does not exists. There a file with the same name in {alternativePath.Replace(AppContext.BaseDirectory, "")}", fileFullPath)
                        : alternativePath;
                }
                else
                    throw new FileNotFoundException($"\"{fileName}\" was not found in any directory.", fileFullPath);
            }

            return migrationBuilder.Sql(File.ReadAllText(fileFullPath));
        }
    }
}